'===================================================================================================
'
'	Filename:			PicoHRDLImports.vb
'
'	Description: 
'	This file defines enumerations functions and structures from the HRDL.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'===================================================================================================

Module USBTC08Imports

    ' Constants
    ' =========

    Public Const HRDL_MAX_PICO_UNITS As UInteger = 64
    Public Const HRDL_MAX_UNITS As UInteger = 20
    Public Const INVALID_HRDL_VALUE = &HF0000000

    ' Enumerations
    ' ============

    Enum HRDLInputs

        HRDL_DIGITAL_CHANNELS
        HRDL_ANALOG_IN_CHANNEL_1
        HRDL_ANALOG_IN_CHANNEL_2
        HRDL_ANALOG_IN_CHANNEL_3
        HRDL_ANALOG_IN_CHANNEL_4
        HRDL_ANALOG_IN_CHANNEL_5
        HRDL_ANALOG_IN_CHANNEL_6
        HRDL_ANALOG_IN_CHANNEL_7
        HRDL_ANALOG_IN_CHANNEL_8
        HRDL_ANALOG_IN_CHANNEL_9
        HRDL_ANALOG_IN_CHANNEL_10
        HRDL_ANALOG_IN_CHANNEL_11
        HRDL_ANALOG_IN_CHANNEL_12
        HRDL_ANALOG_IN_CHANNEL_13
        HRDL_ANALOG_IN_CHANNEL_14
        HRDL_ANALOG_IN_CHANNEL_15
        HRDL_ANALOG_IN_CHANNEL_16
        HRDL_MAX_ANALOG_CHANNELS = HRDL_ANALOG_IN_CHANNEL_16

    End Enum

    Enum HRDLDigitalIOChannel

        HRDL_DIGITAL_IO_CHANNEL_1 = &H1
        HRDL_DIGITAL_IO_CHANNEL_2 = &H2
        HRDL_DIGITAL_IO_CHANNEL_3 = &H4
        HRDL_DIGITAL_IO_CHANNEL_4 = &H8
        HRDL_MAX_DIGITAL_CHANNELS = 4

    End Enum

    Enum HRDLRange

        HRDL_2500_MV
        HRDL_1250_MV
        HRDL_625_MV
        HRDL_313_MV
        HRDL_156_MV
        HRDL_78_MV
        HRDL_39_MV
        HRDL_MAX_RANGES

    End Enum

    Enum HRDLConversionTime

        HRDL_60MS
        HRDL_100MS
        HRDL_180MS
        HRDL_340MS
        HRDL_660MS
        HRDL_MAX_CONVERSION_TIMES

    End Enum

    Enum HRDLInfo

        HRDL_DRIVER_VERSION
        HRDL_USB_VERSION
        HRDL_HARDWARE_VERSION
        HRDL_VARIANT_INFO
        HRDL_BATCH_AND_SERIAL
        HRDL_CAL_DATE
        HRDL_KERNEL_DRIVER_VERSION
        HRDL_ERROR
        HRDL_SETTINGS

    End Enum

    Enum HRDLErrorCode

        HRDL_OK
        HRDL_KERNEL_DRIVER
        HRDL_NOT_FOUND
        HRDL_CONFIG_FAIL
        HRDL_ERROR_OS_NOT_SUPPORTED
        HRDL_MAX_DEVICES

    End Enum

    Enum SettingsError

        SE_CONVERSION_TIME_OUT_OF_RANGE
        SE_SAMPLEINTERVAL_OUT_OF_RANGE
        SE_CONVERSION_TIME_TOO_SLOW
        SE_CHANNEL_NOT_AVAILABLE
        SE_INVALID_CHANNEL
        SE_INVALID_VOLTAGE_RANGE
        SE_INVALID_PARAMETER
        SE_CONVERSION_IN_PROGRESS
        SE_COMMUNICATION_FAILED
        SE_OK
        SE_MAX = SE_OK

    End Enum

    Enum HRDLOpenProgress

        HRDL_OPEN_PROGRESS_FAIL = -1
        HRDL_OPEN_PROGRESS_PENDING = 0
        HRDL_OPEN_PROGRESS_COMPLETE = 1

    End Enum

    Enum BlockMethod

        HRDL_BM_BLOCK
        HRDL_BM_WINDOW
        HRDL_BM_STREAM

    End Enum

    Enum MainsRejection

        HRDL_FIFTY_HERTZ
        HRDL_SIXTY_HERTZ

    End Enum

    ' Functions
    ' =========

    Declare Function HRDLOpenUnit Lib "picohrdl.dll" () As Short
    Declare Function HRDLOpenUnitAsync Lib "picohrdl.dll" () As Short
    Declare Function HRDLOpenUnitProgress Lib "picohrdl.dll" (ByRef handle As Short, ByRef progress As Short) As Short
    Declare Function HRDLGetUnitInfo Lib "picohrdl.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByVal info As HRDLInfo) As Short
    Declare Function HRDLCloseUnit Lib "picohrdl.dll" (ByVal handle As Short) As Short

    Declare Function HRDLGetMinMaxAdcCounts Lib "picohrdl.dll" (ByVal handle As Short, ByRef minAdc As Integer, ByRef maxAdc As Integer, ByVal channel As HRDLInputs) As Short
    Declare Function HRDLSetAnalogInChannel Lib "picohrdl.dll" (ByVal handle As Short, ByVal channel As HRDLInputs, ByVal enabled As Short, ByVal range As HRDLRange,
                                                                ByVal singleEnded As Short) As Short

    Declare Function HRDLSetDigitalIOChannel Lib "picohrdl.dll" (ByVal handle As Short, ByVal directionOut As Short, ByVal digitalOutPinState As Short, ByVal enabledDigitalIn As Short) As Short
    Declare Function HRDLSetInterval Lib "picohrdl.dll" (ByVal handle As Short, ByVal sampleInterval_ms As Integer, ByVal conversionTime As HRDLConversionTime) As Short

    Declare Function HRDLRun Lib "picohrdl.dll" (ByVal handle As Short, ByVal nValues As Integer, ByVal method As BlockMethod) As Short
    Declare Function HRDLReady Lib "picohrdl.dll" (ByVal handle As Short) As Short
    Declare Function HRDLGetValues Lib "picohrdl.dll" (ByVal handle As Short, ByRef values As Integer, ByRef overflow As Short, ByVal noOfValues As Integer) As Integer
    Declare Function HRDLGetTimesAndValues Lib "picohrdl.dll" (ByVal handle As Short, ByRef times As Integer, ByRef values As Integer, ByRef overflow As Short, ByVal noOfValues As Integer) As Integer
    Declare Function HRDLGetSingleValue Lib "picohrdl.dll" (ByVal handle As Short, ByVal channel As HRDLInputs, ByVal range As HRDLRange, ByVal conversionTime As HRDLConversionTime,
                                                            ByVal singleEnded As Short, ByRef overflow As Short, ByRef value As Integer) As Short

    Declare Function HRDLCollectSingleValueAsync Lib "picohrdl.dll" (ByVal handle As Short, ByVal channel As HRDLInputs, ByVal range As HRDLRange, ByVal conversionTime As HRDLConversionTime,
                                                                     ByVal singleEnded As Short) As Short
    Declare Function HRDLGetSingleValueAsync Lib "picohrdl.dll" (ByVal handle As Short, ByRef value As Integer, ByRef overflow As Short) As Short

    Declare Function HRDLSetMains Lib "picohrdl.dll" (ByVal handle As Short, ByVal sixtyHertz As MainsRejection) As Short
    Declare Function HRDLGetNumberOfEnabledChannels Lib "picohrdl.dll" (ByVal handle As Short, ByRef nEnabledChannels As Short) As Short

    Declare Sub HRDLStop Lib "picohrdl.dll" (ByVal handle As Short)

End Module
