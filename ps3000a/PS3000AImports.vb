'====================================================================================================
'
'	Filename:			PS3000AImports.vb
'
'	Description: 
'	    This file defines enumerations, functions and structures from the ps3000aApi.h C header file.
'
'   Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'====================================================================================================

Module PS3000AImports

    ' Constant values
    ' ===============

    ' Define any constant values here as required.


    ' Enumerations
    ' ============

    Enum Channel
        PS3000A_CHANNEL_A
        PS3000A_CHANNEL_B
        PS3000A_CHANNEL_C
        PS3000A_CHANNEL_D
        PS3000A_EXTERNAL
    End Enum

    Enum CouplingMode
        PS3000A_AC
        PS3000A_DC
    End Enum

    Enum DigitalPort
        PS3000A_DIGITAL_PORT0 = 128     ' digital channel 0 - 7
        PS3000A_DIGITAL_PORT1           ' digital channel 8 - 15
        PS3000A_DIGITAL_PORT2           ' digital channel 16 - 23
        PS3000A_DIGITAL_PORT3           ' digital channel 24 - 31
        PS3000A_MAX_DIGITAL_PORTS = (PS3000A_DIGITAL_PORT3 - PS3000A_DIGITAL_PORT0) + 1
    End Enum

    Enum EtsMode
        PS3000A_ETS_OFF     'ETS disabled
        PS3000A_ETS_FAST
        PS3000A_ETS_SLOW
        PS3000A_ETS_MODES_MAX
    End Enum

    Enum ExtraOperations
        PS3000A_ES_OFF
        PS3000A_WHITENOISE
        PS3000A_PRBS 'Pseudo-Random Bit Stream 
    End Enum

    Enum IndexMode
        PS3000A_SINGLE
        PS3000A_DUAL
        PS3000A_QUAD
        PS3000A_MAX_INDEX_MODES
    End Enum

    Enum PulseWidthType
        PS3000A_PW_TYPE_NONE
        PS3000A_PW_TYPE_LESS_THAN
        PS3000A_PW_TYPE_GREATER_THAN
        PS3000A_PW_TYPE_IN_RANGE
        PS3000A_PW_TYPE_OUT_OF_RANGE
    End Enum

    Enum RatioMode
        PS3000A_RATIO_MODE_NONE = 0
        PS3000A_RATIO_MODE_AGGREGATE = 1
        PS3000A_RATIO_MODE_DECIMATE = 2
        PS3000A_RATIO_MODE_AVERAGE = 4
    End Enum

    Enum SigGenTrigSource
        PS3000A_SIGGEN_NONE
        PS3000A_SIGGEN_SCOPE_TRIG
        PS3000A_SIGGEN_AUX_IN
        PS3000A_SIGGEN_EXT_IN
        PS3000A_SIGGEN_SOFT_TRIG
    End Enum

    Enum SigGenTrigType
        PS3000A_SIGGEN_RISING
        PS3000A_SIGGEN_FALLING
        PS3000A_SIGGEN_GATE_HIGH
        PS3000A_SIGGEN_GATE_LOW
    End Enum

    Enum SweepType
        PS3000A_UP
        PS3000A_DOWN
        PS3000A_UPDOWN
        PS3000A_DOWNUP
        PS3000A_MAX_SWEEP_TYPES
    End Enum

    Enum ThresholdDirection
        'Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,
        PS3000A_ABOVE                   ' using upper threshold
        PS3000A_BELOW                   ' using upper threshold
        PS3000A_RISING                  ' using upper threshold
        PS3000A_NONE = PS3000A_RISING   ' no trigger set
        PS3000A_FALLING                 ' using upper threshold
        PS3000A_RISING_OR_FALLING       ' using both threshold
        PS3000A_ABOVE_LOWER             ' using lower threshold
        PS3000A_BELOW_LOWER             ' using lower threshold
        PS3000A_RISING_LOWER            ' using lower threshold
        PS3000A_FALLING_LOWER           ' using lower threshold
        ' Windowing using both thresholds
        PS3000A_INSIDE = PS3000A_ABOVE
        PS3000A_OUTSIDE = PS3000A_BELOW
        PS3000A_ENTER = PS3000A_RISING
        PS3000A_EXIT = PS3000A_FALLING
        PS3000A_ENTER_OR_EXIT = PS3000A_RISING_OR_FALLING
        PS3000A_POSITIVE_RUNT = 9
        PS3000A_NEGATIVE_RUNT
    End Enum

    Enum ThresholdMode
        LEVEL
        WINDOW
    End Enum

    Enum TimeUnits
        PS3000A_FS
        PS3000A_PS
        PS3000A_NS
        PS3000A_US
        PS3000A_MS
        PS3000A_S
    End Enum

    Enum TriggerState
        CONDITION_DONT_CARE
        CONDITION_TRUE
        CONDITION_FALSE
        CONDITION_MAX
    End Enum

    Enum VoltageRange
        PS3000A_10MV
        PS3000A_20MV
        PS3000A_50MV
        PS3000A_100MV
        PS3000A_200MV
        PS3000A_500MV
        PS3000A_1V
        PS3000A_2V
        PS3000A_5V
        PS3000A_10V
        PS3000A_20V
        PS3000A_50V
    End Enum

    Enum WaveType
        PS3000A_SINE
        PS3000A_SQUARE
        PS3000A_TRIANGLE
        PS3000A_RAMP_UP
        PS3000A_RAMP_DOWN
        PS3000A_SINC
        PS3000A_GAUSSIAN
        PS3000A_HALF_SINE
        PS3000A_DC_VOLTAGE
        PS3000A_WHITE_NOISE
        PS3000A_MAX_WAVE_TYPES
    End Enum

    ' Structures
    ' ==========

    Public Structure TRIGGER_CONDITION
        Public channelA As UInteger
        Public channelB As UInteger
        Public channelC As UInteger
        Public channelD As UInteger
        Public external As UInteger
        Public aux As UInteger
        Public pulseWidthQualifier As UInteger
    End Structure

    Public Structure DIGITAL_DIRECTION
        Public channel As UInteger
        Public direction As UInteger
    End Structure


    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps3000aOpenUnit Lib "ps3000a.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Sub ps3000aCloseUnit Lib "ps3000a.dll" (ByVal handle As Short)

    Declare Function ps3000aGetUnitInfo Lib "ps3000a.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger
    Declare Function ps3000aChangePowerSource Lib "ps3000a.dll" (ByVal handle As Short, ByVal powerState As UInteger) As UInteger

    Declare Function ps3000aSetChannel Lib "ps3000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As CouplingMode, ByVal range As VoltageRange, ByVal analogueOffset As Single) As UInteger
    Declare Function ps3000aSetDigitalPort Lib "ps3000a.dll" (ByVal handle As Short, ByVal port As DigitalPort, ByVal enabled As Short, ByVal logicLevel As Short) As UInteger

    Declare Function ps3000aGetTimebase2 Lib "ps3000a.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As Integer, ByRef timeIntervalNs As Single, ByVal oversample As Short, ByRef maxSamples As Integer, ByVal segmentIndex As UInteger) As UInteger
    Declare Function ps3000aMaximumValue Lib "ps3000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger

    ' Block functions
    ' ---------------

    Declare Function ps3000aRunBlock Lib "ps3000a.dll" (ByVal handle As Short, ByVal noOfPreTriggerSamples As Integer,
                                                        ByVal noOfPostTriggerSamples As Integer, ByVal timebase As UInteger,
                                                        ByVal oversample As Short, ByRef timeIndisposedMs As Integer,
                                                        ByVal segmentIndex As UInteger, ByVal lpps3000aBlockReady As ps3000aBlockReady,
                                                        ByVal pParam As IntPtr) As UInteger

    Declare Function ps3000aIsReady Lib "ps3000a.dll" (ByVal handle As Short, ByRef ready As Short) As UInteger

    ' Trigger Functions
    ' -----------------

    Declare Function ps3000aSetSimpleTrigger Lib "ps3000a.dll" (ByVal handle As Short, ByVal enable As Short, ByVal channel As Channel, ByVal threshold As Short, ByVal direction As ThresholdDirection,
                                                                ByVal delay As UInteger, ByVal autoTriggerMs As Short) As UInteger


    ' Functions relevant to all Data Capture Modes
    ' --------------------------------------------

    Declare Function ps3000aSetDataBuffer Lib "ps3000a.dll" (ByVal handle As Short, ByVal channel As UInteger, ByRef buffer As Short, ByVal length As Integer, ByVal segmentIndex As UInteger, ByVal downSampleRatioMode As UInteger) As UInteger
    Declare Function ps3000aGetValues Lib "ps3000a.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger, ByVal downSampleRatioMode As UInteger, ByVal segmentIndex As UInteger, ByRef overflow As Short) As UInteger
    Declare Function ps3000aStop Lib "ps3000a.dll" (ByVal handle As Short) As UInteger

    ' Signal Generator Functions
    ' --------------------------

    Declare Function ps3000aSetSigGenBuiltInV2 Lib "ps3000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal peakToPeak As UInteger, ByVal waveType As WaveType,
                                                                  ByVal startFrequency As Double, ByVal stopFrequency As Double, ByVal increment As Double, ByVal dwellTime As Double,
                                                                  ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal shots As UInteger, ByVal sweeps As UInteger,
                                                                  ByVal triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    ' Delegate declaration
    ' --------------------

    ' Block mode
    Public Delegate Sub ps3000aBlockReady(handle As Short, status As UInteger, pVoid As IntPtr)


    ' Other Functions
    ' ===============

    Declare Sub Sleep Lib "kernel32.dll" (ByVal millseconds As Integer)

End Module
