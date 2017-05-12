'========================================================================================
'	
'   Filename: PLCM3Form.vb
'
'   Description:
'	An example Windows Form application demonstrating how to use VB .NET with the 
'   PicoLog CM3 Current Data Logger.
'   It shows how to call the API functions to:
'
'       Open a connection to a data logger via USB or Ethernet connection, 
'       display device information and capture some data.
'
'   Copyright (C) 2011 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Public Class PLCM3Form

    Dim plcm3Handle As Short
    Dim status As UInteger
    Dim opened As Boolean

    Dim dataType(3) As PLCM3DataType

    Dim combobox As List(Of ComboBox)
    Dim progressbar As List(Of ProgressBar)
    Dim textbox As List(Of TextBox)
    Dim unitslabel As List(Of Label)

    ' *******************************************************************************
    ' Form1_Load
    '
    ' Initialisation code
    ' *******************************************************************************
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        combobox = New List(Of ComboBox)
        progressbar = New List(Of ProgressBar)
        textbox = New List(Of TextBox)
        unitslabel = New List(Of Label)


        progressbar.Add(ProgressBar1)
        progressbar.Add(ProgressBar2)
        progressbar.Add(ProgressBar3)

        textbox.Add(TextBox1)
        textbox.Add(TextBox2)
        textbox.Add(TextBox3)

        unitslabel.Add(UnitsLabel1)
        unitslabel.Add(UnitsLabel2)
        unitslabel.Add(UnitsLabel3)

        combobox.Add(ComboBox1)
        combobox.Add(ComboBox2)
        combobox.Add(ComboBox3)

        opened = False
        Button1.Text = "Open Unit"
        UsbButton.Checked = True

        For i = 0 To progressbar.Count - 1 Step 1
            combobox.Item(i).Text = "Voltage"
            dataType(i) = PLCM3DataType.PLCM3_VOLTAGE ' 4 = Voltage
            unitslabel.Item(i).Text = "mV"
        Next

    End Sub

    ' *******************************************************************************
    ' GetInfo
    '
    ' Get the unit information and display each value
    ' *******************************************************************************
    Private Sub GetInfo()

        Dim infoText(8) As String
        Dim infoStr As String
        Dim requiredSize As Integer


        infoText(0) = "Driver Ver:       "
        infoText(1) = "USB Ver:          "
        infoText(2) = "Hardware Ver:     "
        infoText(3) = "Variant:          "
        infoText(4) = "Batch / Serial:   "
        infoText(5) = "Cal Date:         "
        infoText(6) = "Kernel Driver Ver:"

        For i = 0 To 6

            infoStr = ".................."
            PLCM3GetUnitInfo(plcm3Handle, infoStr, 255, requiredSize, i)
            UnitInfoTextBox.Text &= infoText(i) & vbTab
            UnitInfoTextBox.Text &= infoStr
            UnitInfoTextBox.Text &= Environment.NewLine

        Next i

    End Sub
    ' *******************************************************************************
    ' CheckIPAddr
    '
    ' Check that the IP Adress entered is a valid IP Address format
    ' *******************************************************************************
    Private Function CheckIPAddr(ByRef IPAddr) As Boolean

        Dim IPAddrPart(0 To 3) As String
        Dim RetVal As Boolean

        RetVal = True

        IPAddrPart = IPAddr.Split(".")

        If (IPAddrPart.Length <> 4) Then

            MsgBox("Use IP Address format xxx.xxx.xxx.xxx ", vbOKOnly, "Error Message")
            RetVal = False

        Else

            For Part = 0 To 3 Step 1

                Try

                    If (IPAddrPart(Part) < 0 Or IPAddrPart(Part) > 255) Then

                        MsgBox("IP Values out of range ", vbOKOnly, "Error Message")
                        RetVal = False
                        Exit For

                    End If

                Catch ex As Exception

                    MsgBox("IP Address contains invalid characters ", vbOKOnly, "Error Message")
                    RetVal = False
                    Exit For

                End Try

            Next Part

        End If

        Return RetVal

    End Function

    ' *******************************************************************************
    ' OpenUnit
    '
    ' Open Unit Via USB or Ethernet, depending upon which button is selected
    ' *******************************************************************************
    Private Function OpenUnit()

        Dim serial As String
        Dim ipaddr As String

        ipaddr = vbNullString
        serial = vbNullString

        status = PicoStatus.PICO_OK

        If EthernetButton.Checked = True Then

            ipaddr = IPAddrTextBox.Text

            If CheckIPAddr(ipaddr) Then

                status = PLCM3OpenUnitViaIp(plcm3Handle, serial, ipaddr)

                If status <> PicoStatus.PICO_OK Then

                    MsgBox("Unit not opened status = " & status, vbOKOnly, "Error Message")

                Else

                    opened = True
                    Button1.Text = "Close Unit"     ' Chenge text on button

                End If

            End If

        Else

            status = PLCM3OpenUnit(plcm3Handle, serial)

            If status <> PicoStatus.PICO_OK Then

                MsgBox("Unit not opened status = " & status, vbOKOnly, "Error Message")

            Else

                opened = True
                Button1.Text = "Close Unit"     ' Chenge text on button

            End If

        End If

        Return status

    End Function

    ' *******************************************************************************
    ' CloseUnit
    '
    ' Close the unit, and clear message boxes
    ' *******************************************************************************
    Private Sub CloseUnit()

        status = PLCM3CloseUnit(plcm3Handle)
        opened = False
        Button1.Text = "Open Unit"  ' Change text ready for next opening

        UnitInfoTextBox.Clear()     ' Clear the unit information box
        TextBox1.Clear()            ' Clear Units boxes (mV mA A)
        TextBox2.Clear()
        TextBox3.Clear()
        ProgressBar1.Value = 0      ' Clear progress bars
        ProgressBar2.Value = 0
        ProgressBar3.Value = 0

    End Sub

    ' *******************************************************************************
    ' ShowValues
    '
    ' Display values read from each channel. Show on progress bar, 
    ' and in text box. Show units used.
    ' *******************************************************************************

    Private Sub ShowValues()

        Dim Val As Integer
        Dim ProgBarVal As Integer
        Dim chan As Integer

        For i = 0 To progressbar.Count - 1 Step 1

            chan = i + 1 ' Channels are 1, 2 & 3

            If dataType(i) > 0 Then

                PLCM3GetValue(plcm3Handle, chan, Val)  ' channel 1,2 & 3

                If dataType(i) < PLCM3DataType.PLCM3_VOLTAGE Then               ' 1 mA/V 10 mA/V or 100 mA/V

                    If dataType(i) = PLCM3DataType.PLCM3_1_MILLIVOLT Then       ' 1mV/A

                        ProgBarVal = Val

                    ElseIf dataType(i) = PLCM3DataType.PLCM3_10_MILLIVOLTS Then ' 10mV/A

                        ProgBarVal = Val * 10

                    ElseIf dataType(i) = PLCM3DataType.PLCM3_100_MILLIVOLTS Then ' 100mV/A

                        ProgBarVal = Val * 100

                    End If


                    If Val < 10000 Then

                        unitslabel.Item(i).Text = "mA"

                    Else

                        Val = Val / 1000
                        unitslabel.Item(i).Text = "A"

                    End If


                ElseIf dataType(i) = PLCM3DataType.PLCM3_VOLTAGE Then ' Voltage

                    ProgBarVal = Val
                    Val = Val / 1000.0
                    unitslabel.Item(i).Text = "mV"

                Else                                ' should never get here....

                    ProgBarVal = Val
                    Val = 0
                    unitslabel.Item(i).Text = "units"

                End If

                If ProgBarVal > ProgressBar1.Maximum Then     ' If input voltage is overrange, show as max range

                    ProgBarVal = ProgressBar1.Maximum

                End If

                progressbar.Item(i).Value = ProgBarVal  ' show progress bar
                textbox.Item(i).Text = Convert.ToString(Val) ' show value


            Else ' If dataType = 0

                progressbar.Item(i).Value = 0 ' Channel is off
                textbox.Item(i).Text = Convert.ToString(0)

            End If

        Next

    End Sub

    ' *******************************************************************************
    ' Button1_Click
    '
    ' If unit is not opened, calls the OpenUnit function.
    ' If unit is opened, calls the CloseUint function
    ' *******************************************************************************

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim status As UInteger

        If opened = False Then

            status = OpenUnit()

            If status = 0 Then ' Unit opened ok

                GetInfo()

                For i = 0 To 3 Step 1

                    status = PLCM3SetChannel(plcm3Handle, i + 1, dataType(i))

                Next

            End If

        Else

            CloseUnit()

        End If

    End Sub

    ' *******************************************************************************
    ' Ch1ComboBox_SelectedIndexChanged
    '
    ' Selects the input range / type for channel 1
    ' *******************************************************************************
    Private Sub Ch1ComboBox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ComboBox1.Text = "Voltage" Then

            dataType(0) = PLCM3DataType.PLCM3_VOLTAGE

        ElseIf ComboBox1.Text = "100 mV/A" Then

            dataType(0) = PLCM3DataType.PLCM3_100_MILLIVOLTS

        ElseIf ComboBox1.Text = "10 mV/A" Then

            dataType(0) = PLCM3DataType.PLCM3_10_MILLIVOLTS

        ElseIf ComboBox1.Text = "1 mV/A" Then

            dataType(0) = PLCM3DataType.PLCM3_1_MILLIVOLT

        Else

            dataType(0) = PLCM3DataType.PLCM3_OFF

        End If

        status = PLCM3SetChannel(plcm3Handle, 1, dataType(0)) ' Re-initialise channel settings
    End Sub


    ' *******************************************************************************
    ' Ch2ComboBox_SelectedIndexChanged
    '
    ' Selects the input range / type for channel 2
    ' *******************************************************************************
    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

        If ComboBox2.Text = "Voltage" Then

            dataType(1) = PLCM3DataType.PLCM3_VOLTAGE

        ElseIf ComboBox2.Text = "100 mV/A" Then

            dataType(1) = PLCM3DataType.PLCM3_100_MILLIVOLTS

        ElseIf ComboBox2.Text = "10 mV/A" Then

            dataType(1) = PLCM3DataType.PLCM3_10_MILLIVOLTS

        ElseIf ComboBox2.Text = "1 mV/A" Then

            dataType(1) = PLCM3DataType.PLCM3_1_MILLIVOLT

        Else

            dataType(1) = PLCM3DataType.PLCM3_OFF

        End If

        status = PLCM3SetChannel(plcm3Handle, 2, dataType(1)) ' Re-initialise channel settings

    End Sub

    ' *******************************************************************************
    ' Ch3ComboBox_SelectedIndexChanged
    '
    ' Selects the input range / type for channel 3
    ' *******************************************************************************
    Private Sub ComboBox3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged

        If ComboBox3.Text = "Voltage" Then

            dataType(2) = PLCM3DataType.PLCM3_VOLTAGE

        ElseIf ComboBox3.Text = "100 mV/A" Then

            dataType(2) = PLCM3DataType.PLCM3_100_MILLIVOLTS

        ElseIf ComboBox3.Text = "10 mV/A" Then

            dataType(2) = PLCM3DataType.PLCM3_10_MILLIVOLTS

        ElseIf ComboBox3.Text = "1 mV/A" Then

            dataType(2) = PLCM3DataType.PLCM3_1_MILLIVOLT

        Else

            dataType(2) = PLCM3DataType.PLCM3_OFF

        End If

        status = PLCM3SetChannel(plcm3Handle, 3, dataType(2)) ' Re-initialise channel settings

    End Sub


    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

        If opened = True Then

            ShowValues()

        End If

    End Sub

End Class