'========================================================================================
'	
'   Filename: PS4000VBCon.vb
'
'   Description:
'	This file demonstrates how to use VB .NET on the PicoScope 4000 Series Oscilloscopes 
'   using the ps4000 driver API functions.
'   It shows the use of many API call functions for block data collection and triggering 
'   from more than 1 source
'
'   Copyright © 2011-2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Imports System.IO
Imports PS4000VBCon.PS4000Imports

Module PS4000VBCon

    Dim mvConvert = True        ' flag to show if values get converted to mV or stay as ADC counts
    Dim HasRunOnce = False      ' Flag to show if 'Run once only' section has been run

    Public ps4000BlockCallback As ps4000BlockReady

    ' Define variables

    Public deviceReady As Boolean

    ' ******************************************************************************************************************************************************************
    ' TrigMenu -  User interface to enable / disable trigger channels, set trigger levels
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '              TrigSettings - A TRIG_SETTINGS structure where trigger settings information will be stored
    ' *******************************************************************************************************************************************************************
    Sub TrigMenu(ByRef UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim chan As Short
        Dim ckey As ConsoleKeyInfo
        Dim Str As String
        Dim StrValue As Short

        TrigSettings.NumberTrigs = 0    ' reset the trigger count

        ' Keep showing options and getting user input until user enters 'X' to quit the menu
        Do
            Console.WriteLine(vbNewLine & "Toggle Channel Triggers")

            ' show the enabled / disabled state of each of the scopes channels (A & B on a 2 channel scope, A,B,C & D on a 4 channel scope)
            For chan = 0 To UnitModel.channelCount - 1

                If UnitModel.channelSettings(chan).enabled Then

                    Console.Write(Chr(65 + chan) & ":  Channel " & Chr(65 + chan) & " Trigger: ") 'Chr(65 + chan) will print ASCII 'A', 'B' etc

                    If TrigSettings.ChanTrig(chan) Then
                        Console.WriteLine("Enabled")
                    Else
                        Console.WriteLine("Disabled")
                    End If

                End If

            Next

            Console.WriteLine("V:  Trigger Voltage  : " & TrigSettings.TrigVoltage & "mV")
            Console.WriteLine("H:  Hysteresis level : " & TrigSettings.TrigHist & "mV")
            Console.WriteLine("X:  Return to previous menu")


            ckey = Console.ReadKey(True)

            ' If a channel key is pressed (A & B on a 2 channel scope, A,B,C & D on a 4 channel scope), toggle the trigger setting between true 7 false
            For chan = 0 To UnitModel.channelCount - 1

                If ckey.Key = ConsoleKey.A + chan Then
                    TrigSettings.ChanTrig(chan) = Not TrigSettings.ChanTrig(chan)
                End If

            Next

            If ckey.Key = ConsoleKey.V Then

                Console.WriteLine("Enter Trigger Voltage Level")

                Try
                    Str = Console.ReadLine()
                    StrValue = Str - "0"                        ' Convert the string to a number
                Catch ex As Exception
                    StrValue = TrigSettings.TrigVoltage         ' if an invalid value gets entered, use the current value as the new value (i.e. don't change the stored value)
                    Console.WriteLine("Invalid Input")
                End Try

                TrigSettings.TrigVoltage = StrValue             ' set the trigger voltage parameter to the new value 

            End If

            If ckey.Key = ConsoleKey.H Then

                Console.WriteLine("Enter Hysteresis Level")

                Try
                    Str = Console.ReadLine()
                    StrValue = Str - "0"                         ' Convert the string to a number
                Catch ex As Exception
                    StrValue = TrigSettings.TrigHist             ' if an invalid value gets entered, use the current value as the new value (i.e. don't change the stored value)
                    Console.WriteLine("Invalid Input")
                End Try

                TrigSettings.TrigHist = StrValue                 ' set the trigger hysteresis parameter to the new value 

            End If

        Loop While (ckey.Key <> ConsoleKey.X)                   ' keep looping until the user hits 'X' to quit this section

        For chan = 0 To UnitModel.channelCount - 1              ' before exiting, count how many channels are marked as trigger enabled....

            If TrigSettings.ChanTrig(chan) Then
                TrigSettings.NumberTrigs = TrigSettings.NumberTrigs + 1 ' .... and store this number in the trigger settings structure
            End If

        Next

    End Sub

    ' ******************************************************************************************************************************************************************
    ' SetDefaults - used to set default settings at the start of the application, and to set channel settings that might have changed before each capture routing
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '              TrigSettings - A TRIG_SETTINGS structure where trigger settings information will be stored
    ' *******************************************************************************************************************************************************************
    Sub SetDefaults(ByRef UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim channel As Short
        Dim status As UInteger

        If HasRunOnce = False Then              ' Only do this part once

            ' Load some initial channel settings into the UnitModel structure
            For channel = 0 To UnitModel.channelCount - 1 Step 1
                UnitModel.channelSettings(channel).enabled = True
                UnitModel.channelSettings(channel).DCCoupled = True
                UnitModel.channelSettings(channel).range = VoltageRange.PS4000_5V
            Next

            ' Load some intial trigger settings into the TrigSettings structure
            TrigSettings.ChanTrig(0) = True     ' start with Chan A as the trigger
            TrigSettings.NumberTrigs = 1        ' Reset the trigger count
            TrigSettings.TrigVoltage = 1000     ' default trigger level
            TrigSettings.TrigHist = 10          ' default trigger Hysteresis

            HasRunOnce = True                   ' change flag so this section won't be run again

        End If

        ' Set the channel settings with the values loaded into the UnitModel structure.
        ' These might have been changed by the user in other meuns in the application
        For channel = 0 To UnitModel.channelCount - 1 Step 1

            status = ps4000SetChannel(UnitModel.handle, channel, UnitModel.channelSettings(channel).enabled, UnitModel.channelSettings(channel).DCCoupled, UnitModel.channelSettings(channel).range)       ' unit, channel, enabled, coupling (0=DC, 1=AC), Voltage range 

            If status <> PS4000Imports.PICO_OK Then
                Console.WriteLine("Status Error $" & Hex(status))
                Exit Sub
            End If

        Next

    End Sub

    ' ******************************************************************************************************************************************************************
    ' SetVoltages - User interfce to set voltage ranges for each channel, or turn channel off
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    ' *******************************************************************************************************************************************************************
    Sub SetVoltages(ByVal UnitModel As UNIT_MODEL)

        Dim i As Integer
        Dim ch As Integer
        Dim voltagerange As Short
        Dim str As String
        Dim enabledCount = 0

        Console.WriteLine(vbNewLine)

        For i = UnitModel.firstRange To UnitModel.lastRange                     ' show list of available voltage ranges
            Console.WriteLine(i & vbTab & " -> " & vbTab & inputRanges(i) & "mV")
        Next

        Do
            For ch = 0 To UnitModel.channelCount - 1                            ' for each channel (a & B on 2 channel scopes, A to D on 4 channel scopes)
                Do
                    Console.WriteLine(vbNewLine & "Channel " & Chr(65 + ch))
                    Console.WriteLine("Specify Voltage Range (" & UnitModel.firstRange & ".." & UnitModel.lastRange & ")")
                    Console.WriteLine("99 - Switches channel off")

                    Try
                        str = Console.ReadLine()                            ' get the user input
                        voltagerange = str - "0"                            ' conver string value to a numeric value
                    Catch ex As Exception                                   ' if the user entered a non numeric value....
                        voltagerange = -1                                   ' catch the error and set a value that will cause the loop to re-run
                    End Try

                Loop While ((voltagerange < UnitModel.firstRange) Or (voltagerange > UnitModel.lastRange) And (voltagerange <> 99)) ' stay in the loop if user selection is out of range

                UnitModel.channelSettings(ch).range = voltagerange          ' set the range of each channel in the unit model structure

                If UnitModel.channelSettings(ch).range <> 99 Then           ' If channel is not disabled

                    Console.WriteLine("-> " & inputRanges(UnitModel.channelSettings(ch).range) & "mV") ' show the set value
                    UnitModel.channelSettings(ch).enabled = True            ' mark the channel as enabled
                    enabledCount = enabledCount + 1                         ' keep a count of how many enabled channels we've got

                Else

                    Console.WriteLine("Channel " & Chr(65 + ch) & " switched off")  ' If channel switched off
                    UnitModel.channelSettings(ch).enabled = False           ' mark the channel as disabled

                End If

            Next

            If enabledCount = 0 Then

                Console.WriteLine(vbNewLine & "Error: At least 1 channel must be enabled")

            End If

        Loop While (enabledCount = 0)                                       ' restart the loop in no channels are enabled

    End Sub

    ' ******************************************************************************************************************************************************************
    ' GetDeviceInfo - Reads and displays the scopes device information. Fills out the UnitModel Structure depending upon device type.
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure where device information will be written
    ' *******************************************************************************************************************************************************************
    Sub GetDeviceInfo(ByRef UnitModel As UNIT_MODEL)

        Dim infoText(8) As String
        Dim str As String
        Dim status As UInteger

        Dim requiredSize As Short
        Dim i As UInteger
        Dim scopetype As Integer
        Dim chrs(5) As Char

        infoText(0) = "Driver Ver:       "
        infoText(1) = "USB Ver:          "
        infoText(2) = "Hardware Ver:     "
        infoText(3) = "Variant:          "
        infoText(4) = "Batch / Serial:   "
        infoText(5) = "Cal Date:         "
        infoText(6) = "Kernel Driver Ver:"

        For i = 0 To 6
            str = "            "
            status = ps4000GetUnitInfo(UnitModel.handle, str, CShort(str.Length), requiredSize, i)
            Console.Write(infoText(i) & vbTab)
            Console.WriteLine(str)

            If i = 3 Then                   ' Get the 4 digit model number as an integer
                str.CopyTo(0, chrs, 0, 4)
                scopetype = Decimal.Parse(chrs)

            End If
        Next i

        ' Set the maximum ADC value for the device, if the device is a PicoScope 4262 this will be corrected below
        UnitModel.maxADCValue = PS4000_MAX_VALUE

        ' Populate the UnitModel Structure with information about the Scope type
        If scopetype = MODEL_TYPE.MODEL_PS4223 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4223
            UnitModel.SigGen = False
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_100V
            UnitModel.channelCount = 2

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4224 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4224
            UnitModel.SigGen = False
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_20V
            UnitModel.channelCount = 2

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4423 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4423
            UnitModel.SigGen = False
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_100V
            UnitModel.channelCount = 4

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4424 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4424
            UnitModel.SigGen = False
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_20V
            UnitModel.channelCount = 4

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4226 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4226
            UnitModel.SigGen = True
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_20V
            UnitModel.channelCount = 2

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4227 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4227
            UnitModel.SigGen = True
            UnitModel.firstRange = VoltageRange.PS4000_50MV
            UnitModel.lastRange = VoltageRange.PS4000_20V
            UnitModel.channelCount = 2

        ElseIf scopetype = MODEL_TYPE.MODEL_PS4262 Then

            UnitModel.model = MODEL_TYPE.MODEL_PS4262
            UnitModel.SigGen = True
            UnitModel.firstRange = VoltageRange.PS4000_10MV
            UnitModel.lastRange = VoltageRange.PS4000_20V
            UnitModel.channelCount = 2
            UnitModel.maxADCValue = PS4262_MAX_VALUE

        End If

    End Sub

    ' ******************************************************************************************************************************************************************
    ' adc_to_mv - Converts from raw ADC values to mV values. The mV value returned depends upon the ADC count, and the voltage range set for the channel. 
    '
    ' Parameters - raw    - An integer holding the ADC count to be converted to mV
    '            - range  - A value indicating where in the 'inputRanges' array the range value can be found
    '            - maxValue - The maximum ADC count value for the device
    '
    ' Returns    - value converted into mV
    ' *******************************************************************************************************************************************************************

    Function adc_to_mv(ByVal raw As Integer, ByVal range As Short, ByVal maxValue As Short)

        Dim mVVal As Single        ' Use this variable to force data to be returned as a float

        mVVal = (raw / maxValue) * inputRanges(range)

        Return mVVal

    End Function


    ' ******************************************************************************************************************************************************************
    ' mv_to_adc - Converts from mV into ADC value. The ADC count returned depends upon the mV value, and the voltage range set for the channel. 
    '
    ' Parameters - mv     - An short holding the mv value to be converted to the ADC count
    '            - range  - A value indicating where in the 'inputRanges' array the range value can be found
    '            - maxValue - The maximum ADC count value for the device
    '
    ' Returns    - value converted into an ADC count
    ' *******************************************************************************************************************************************************************
    Function mv_to_adc(ByVal mv As Short, ByVal range As Short, ByVal maxValue As Short)

        Return (mv / inputRanges(range)) * maxValue

    End Function

    ' ******************************************************************************************************************************************************************
    ' BlockDataHandler - This is where the work of collecting data occurs, using Block mode.
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    ' *******************************************************************************************************************************************************************

    Sub BlockDataHandler(ByRef UnitModel As UNIT_MODEL)
        Dim status As UInteger
        'Dim buffers(3, 9999) As Short                           ' Two dimensional array, 4 x 10,000  (0 to 3 x 0 to 9,999)
        Dim buffers As Short()() = New Short(3)() {}
        Dim numPreTriggerSamples As Integer
        Dim numPostTriggerSamples As Integer
        Dim totalSamples As Integer
        Dim totalSamplesCollected As UInteger
        Dim i As Integer
        Dim n As Integer
        Dim max_samples As Integer
        Dim time_interval As Integer
        Dim timebase As UInteger
        Dim keypress As Boolean
        Dim chan As Integer
        Dim timeIndisposed As Integer

        keypress = False
        deviceReady = False

        numPreTriggerSamples = 0
        numPostTriggerSamples = 5000
        totalSamples = numPreTriggerSamples + numPostTriggerSamples

        Dim pinned As PinnedArray.PinnedArray(Of Short)() = New PinnedArray.PinnedArray(Of Short)(3) {}

        For chan = 0 To UnitModel.channelCount - 1              ' Set up buffers for each channel and pin them

            buffers(chan) = New Short(totalSamples - 1) {}
            pinned(chan) = New PinnedArray.PinnedArray(Of Short)(buffers(chan))

            status = ps4000SetDataBuffer(UnitModel.handle, chan, buffers(chan), totalSamples)
        Next

        timebase = 65

        'increase timebase value if the timebase is not valid using the while loop below
        While (ps4000GetTimebase(UnitModel.handle, timebase, totalSamples, time_interval, 1, max_samples, 0) <> PICO_OK)

            timebase = timebase + 1

        End While

        Console.WriteLine(vbNewLine & "Timebase: " & timebase & vbTab & "Sampleinterval: " & time_interval & " ns" & vbNewLine)

        ' Create instance of delegate for block callback
        ps4000BlockCallback = New ps4000BlockReady(AddressOf BlockCallback)


        ' Start the Block-mode capture
        ' Parameters: handle, preTrigSamples, postTrigSamples, timebase, oversample, segmentIndex, lpReady, pParameter
        status = ps4000RunBlock(UnitModel.handle, numPreTriggerSamples, numPostTriggerSamples, timebase, CShort(1), timeIndisposed, CUShort(0), ps4000BlockCallback, IntPtr.Zero)

        ' Check the state of the deviceReady variable to verify that valid data is available to collect
        While ((deviceReady = False) And (keypress = False))

            System.Threading.Thread.Sleep(10)

            If Console.KeyAvailable Then                ' Check if the user has hit a key to indicate they want to stop waiting for data collection
                keypress = True
            End If

        End While

        ' To get here, either user aborted by hitting a key, or the ready flag is set, showing the data has been captured and is ready for download using get values 
        If keypress = False Then                       ' Data must have been collected if keypress is False

            totalSamplesCollected = totalSamples
            Dim overflow As Short = 0

            status = ps4000GetValues(UnitModel.handle, 0, totalSamplesCollected, 1, 0, 0, overflow) ' handle, startIndex, *NoSamples, downSampleRatio, 0=RATIO_MODE_NONE, segmentIndex, overflow

            Console.WriteLine("Retrieved " & totalSamplesCollected & " values." & vbNewLine)
            Console.WriteLine("First 100 values:" & vbNewLine)

            If mvConvert = True Then                                        ' Convert values to mV

                For i = 0 To 100 Step i + 1                                 ' show first 100 values

                    For chan = 0 To UnitModel.channelCount - 1              ' On all channels

                        If UnitModel.channelSettings(chan).enabled Then
                            Console.Write(adc_to_mv(CInt(pinned(chan).Target(i)), UnitModel.channelSettings(chan).range, UnitModel.maxADCValue) & vbTab)
                        End If

                    Next chan

                    Console.Write(vbNewLine)

                Next i

            Else                                                            ' Leave as ADC values

                For i = 0 To 100 Step i + 1                                 ' show first 100 values

                    For chan = 0 To UnitModel.channelCount - 1              ' On all channels

                        If UnitModel.channelSettings(chan).enabled Then
                            Console.Write(pinned(chan).Target(i) & vbTab)
                        End If

                    Next chan

                    Console.Write(vbNewLine)

                Next i

            End If

        Else                                                                ' If keypress = True, user aborted collection

            Console.WriteLine("Data collection aborted")
            Console.ReadKey(True)

        End If

        ' Output to file
        ' Output will be Channel A, Channel B on each line (unless the channel is not enabled)
        Using outfile As New StreamWriter("Block.txt")

            For n = 0 To totalSamplesCollected - 1

                For chan = 0 To UnitModel.channelCount - 1              ' On all channels

                    If UnitModel.channelSettings(chan).enabled Then


                        If mvConvert = True Then    ' Convert values to mV

                            outfile.Write(CShort(adc_to_mv(buffers(chan)(n), UnitModel.channelSettings(chan).range, UnitModel.maxADCValue)).ToString & vbTab)

                        Else

                            outfile.Write(buffers(chan)(n).ToString & vbTab)

                        End If


                    End If

                Next chan

                outfile.WriteLine()

            Next n

        End Using

        ' Reset deviceReady flag if we need to collect another block of data
        deviceReady = False

        Call ps4000Stop(UnitModel.handle)

    End Sub

    ' ******************************************************************************************************************************************************************
    ' TriggerPropertiesSetup -  Sets up the triggerChannelPropertiesArray structure with settings derived from reading the TrigSettings structure
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '            - TrigSettings - A TRIG_SETTINGS structure where trigger settings information is stored
    '
    ' Returns    - status      - status value returned from the SetTriggerProperties call
    ' *******************************************************************************************************************************************************************
    Function TriggerPropertiesSetup(ByRef UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim trigADC As Short
        Dim hystADC As Short
        Dim status As Short
        Dim Chan As Short
        Dim i As Short
        Dim j As Short


        ' Get the trigger level in ADC counts
        trigADC = mv_to_adc(TrigSettings.TrigVoltage, UnitModel.channelSettings(Channel.PS4000_CHANNEL_A).range, UnitModel.maxADCValue)
        hystADC = mv_to_adc(TrigSettings.TrigHist, UnitModel.channelSettings(Channel.PS4000_CHANNEL_A).range, UnitModel.maxADCValue)


        Console.WriteLine("Trigger when Input rises past " & TrigSettings.TrigVoltage & "mV")

        Dim triggerChannelPropertiesArray((TrigSettings.NumberTrigs * 6) - 1) As TRIGGER_CHANNEL_PROPERTIES


        Chan = 0
        j = 0

        For i = 0 To TrigSettings.NumberTrigs - 1 Step 1

            triggerChannelPropertiesArray(j).thresholdUpper = trigADC            ' Upper Threshold
            triggerChannelPropertiesArray(j).thresholdUpperHysteresis = hystADC  ' Upper Hysteresis
            triggerChannelPropertiesArray(j).thresholdLower = 0                  ' Lower Threshold
            triggerChannelPropertiesArray(j).thresholdLowerHysteresis = 0        ' Lower Hysteresis

            ' Find the next channel with a trigger set, and get its channel number 
            While TrigSettings.ChanTrig(Chan) = False
                Chan = Chan + 1
            End While

            triggerChannelPropertiesArray(j).Channel = Chan     ' channel

            triggerChannelPropertiesArray(j).thresholdMode = ThresholdMode.LEVEL   ' thresholdMode (Level=0, Window=1)


            Chan = Chan + 1                                                 ' increment the next channel to check for a trigger set
            j = j + 1                                                       ' increment the array position
        Next

        status = ps4000SetTriggerChannelProperties(UnitModel.handle, triggerChannelPropertiesArray(0), TrigSettings.NumberTrigs, CShort(0), CInt(0))

        Return status

    End Function

    ' ******************************************************************************************************************************************************************
    ' TriggerConditionsSetup -  Sets up the triggerConditions Array structure with settings derived from reading the TrigSettings structure
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '              TrigSettings - A TRIG_SETTINGS structure where trigger settings information is stored
    '
    ' Returns    - status      - status value returned from the SetTriggerConditions call
    ' *******************************************************************************************************************************************************************
    Function TriggerConditionsSetup(ByVal UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim status As UInteger
        Dim Chan As Short
        Dim j As Short

        Dim triggerConditions(TrigSettings.NumberTrigs - 1) As TRIGGER_CONDITIONS

        ' start with all in the 'CONDITION_DONT_CARE ' state
        For Chan = 0 To TrigSettings.NumberTrigs - 1 Step 1

            triggerConditions(Chan).channelA = TriggerState.CONDITION_DONT_CARE    ' channel A
            triggerConditions(Chan).channelB = TriggerState.CONDITION_DONT_CARE    ' channel B
            triggerConditions(Chan).channelC = TriggerState.CONDITION_DONT_CARE    ' channel C
            triggerConditions(Chan).channelD = TriggerState.CONDITION_DONT_CARE    ' channel D
            triggerConditions(Chan).external = TriggerState.CONDITION_DONT_CARE    ' external
            triggerConditions(Chan).aux = TriggerState.CONDITION_DONT_CARE         ' aux
            triggerConditions(Chan).pwq = TriggerState.CONDITION_DONT_CARE         ' pulse width qualifier

        Next


        ' see which channels have a trigger selected and set them
        j = 0

        For Chan = 0 To UnitModel.channelCount - 1 Step 1

            If TrigSettings.ChanTrig(Chan) Then

                Select Case Chan

                    Case Channel.PS4000_CHANNEL_A

                        triggerConditions(Chan).channelA = TriggerState.CONDITION_TRUE

                    Case Channel.PS4000_CHANNEL_B

                        triggerConditions(Chan).channelB = TriggerState.CONDITION_TRUE

                    Case Channel.PS4000_CHANNEL_C

                        triggerConditions(Chan).channelC = TriggerState.CONDITION_TRUE

                    Case Channel.PS4000_CHANNEL_D

                        triggerConditions(Chan).channelD = TriggerState.CONDITION_TRUE

                End Select


                j = j + 1

            End If

        Next

        'Set the trigger using the wrapper function

        status = ps4000SetTriggerChannelConditions(UnitModel.handle, triggerConditions(0), TrigSettings.NumberTrigs)


        Return status
    End Function


    ' ******************************************************************************************************************************************************************
    ' TriggerDirectionsSetup -  Calls the ps4000SetTriggerChannelDirections API function
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '
    ' Returns    - status      - status value returned from the ps4000SetTriggerChannelDirections call
    ' *******************************************************************************************************************************************************************

    Function TriggerDirectionsSetup(ByVal UnitModel As UNIT_MODEL)

        Dim status As UInteger

        ' LEVEL Triggers ( Used with ps4000SetTriggerChannelDirections if thresholdMode=0)
        ' Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,

        ' WINDOW Triggers ( Used with ps4000SetTriggerChannelDirections if thresholdMode=1)
        ' Inside=0, Outside=1, Enter=2, Exit=3, Enter_Or_Exit=4, Positive_Runt=9, Negative_Runt=10

        status = ps4000SetTriggerChannelDirections(UnitModel.handle, LevelTrig.RISING, LevelTrig.RISING_OR_FALLING, LevelTrig.RISING_OR_FALLING,
                                                   LevelTrig.RISING_OR_FALLING, LevelTrig.NONE, LevelTrig.NONE)    ' Handle, TrigCHA, TrigCHB, TrigCHC, TrigCHD, Trigext, Trigaux

        Return status

    End Function

    ' ******************************************************************************************************************************************************************
    ' CollectBlockTriggered -  Sets up Triggering, then calls the BlockDataHandler routine
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '              TrigSettings - A TRIG_SETTINGS structure where trigger settings information is stored
    '
    ' *******************************************************************************************************************************************************************
    Sub CollectBlockTriggered(ByRef UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim chan As Short
        Dim i As Short

        i = 1

        If TrigSettings.NumberTrigs < 1 Then                ' If no Triggers are set, return without doing anything

            Console.WriteLine(vbNewLine & "Error. No Trigger Channels set.")

        Else                                                ' show user which channels will be used for triggering

            Console.Write(vbNewLine & "Triggering from Channel ")

            For chan = 0 To UnitModel.channelCount - 1

                If TrigSettings.ChanTrig(chan) Then

                    Console.Write(Chr(65 + chan))

                    If i < TrigSettings.NumberTrigs Then

                        Console.Write(" *** OR *** ")
                        i = i + 1

                    End If

                End If

            Next

            SetDefaults(UnitModel, TrigSettings)                        ' Set the channels with selected parameters

            Console.Write(vbNewLine)

            ' Set up triggering
            If TriggerPropertiesSetup(UnitModel, TrigSettings) Then

                MsgBox("SetTriggerProperties Error", vbOKOnly, "Error Message")
                Exit Sub

            End If


            If TriggerConditionsSetup(UnitModel, TrigSettings) Then

                MsgBox("SetTriggerProperties Error", vbOKOnly, "Error Message")
                Exit Sub

            End If


            If TriggerDirectionsSetup(UnitModel) <> PICO_OK Then

                MsgBox("SetTriggerChannelDirections Error", vbOKOnly, "Error Message")
                Exit Sub

            End If

            ' Do the BlockMode data collection work
            BlockDataHandler(UnitModel)

        End If

    End Sub

    ' ******************************************************************************************************************************************************************
    ' CollectBlockImmediate -   Disables Triggering, then calls the BlockDataHandler routine
    '
    ' Parameters - UnitModel    - A UNIT_MODEL structure containing information about the opened unit
    '              TrigSettings - A TRIG_SETTINGS structure where trigger settings information is stored
    '
    ' *******************************************************************************************************************************************************************
    Sub CollectBlockImmediate(ByVal UnitModel As UNIT_MODEL, ByRef TrigSettings As TRIG_SETTINGS)

        Dim status As UInteger

        SetDefaults(UnitModel, TrigSettings) ' Set the channels with selected parameters

        ' Disable the trigger
        status = ps4000SetSimpleTrigger(UnitModel.handle, 1, Channel.PS4000_CHANNEL_A, 0, LevelTrig.NONE, 0, 0)

        ' Do the BlockMode data collection work
        BlockDataHandler(UnitModel)

    End Sub


    ' ******************************************************************************************************************************************************************
    ' Main -        Entry point to the application
    '
    '
    ' *******************************************************************************************************************************************************************

    Sub Main()

        Dim status As UInteger
        Dim ch As ConsoleKeyInfo
        Dim UnitModel As UNIT_MODEL
        Dim TrigSettings As TRIG_SETTINGS


        ReDim UnitModel.channelSettings(3)  ' for up to 4 channels
        ReDim TrigSettings.ChanTrig(3)      ' for up to 4 channels


        Console.WriteLine("PicoScope 4000 Series (ps4000) VB .NET Example Program" & vbNewLine _
                          & "======================================================" & vbNewLine)
        status = ps4000OpenUnit(UnitModel.handle)

        If UnitModel.handle = 0 Then

            Console.WriteLine("Unit not opened")
            ch = Console.ReadKey(True)
            Exit Sub

        End If

        GetDeviceInfo(UnitModel)                    'Read and display the device information

        SetDefaults(UnitModel, TrigSettings)        ' Set defaults, including those in 'HasRunOnce = False' section

        ' Stay in this loop until user hits 'X' to quit
        Do

            Console.WriteLine(vbNewLine)
            Console.WriteLine("B: Immediate Block")
            Console.WriteLine("T: Triggered Block")

            If mvConvert Then
                Console.WriteLine("A: Readings will be scaled in mV")
            Else
                Console.WriteLine("A: Readings will be scaled in ADC counts")
            End If

            Console.WriteLine("V: Voltage Settings/mV")
            Console.WriteLine("G: Trigger Menu")
            Console.WriteLine("X: Exit")

            ' Read user input and run relevant section
            ch = Console.ReadKey(True)

            If ch.Key = ConsoleKey.T Then
                CollectBlockTriggered(UnitModel, TrigSettings)

            ElseIf ch.Key = ConsoleKey.B Then
                CollectBlockImmediate(UnitModel, TrigSettings)

            ElseIf ch.Key = ConsoleKey.A Then
                mvConvert = Not mvConvert

            ElseIf ch.Key = ConsoleKey.V Then
                SetVoltages(UnitModel)

            ElseIf ch.Key = ConsoleKey.G Then
                TrigMenu(UnitModel, TrigSettings)
            End If


        Loop While ch.Key <> ConsoleKey.X

        ' close the unit at the end
        ps4000CloseUnit(UnitModel.handle)
    End Sub

    ' Block callback function

    Public Sub BlockCallback(handle As Short, status As UInteger, pVoid As IntPtr)
        ' flag to say done reading data
        If status <> &H3A Then
            deviceReady = True
        End If
    End Sub

End Module

