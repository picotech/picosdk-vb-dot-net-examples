'===================================================================================================
'
'	Filename:			PS2000AImports.vb
'
'	Description: 
'	This file defines enumerations, functions and structures from the ps2000aApi.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'===================================================================================================
Module PS2000AImports

    ' Constant values
    ' ===============

    ' Define any constant values here as required.


    ' Enumerations
    ' ============

    Enum Channel
        PS2000A_CHANNEL_A
        PS2000A_CHANNEL_B
        PS2000A_CHANNEL_C
        PS2000A_CHANNEL_D
        PS2000A_EXTERNAL ' PicoScope 2206/7/8 only
    End Enum

    Enum ChannelBufferIndex
        PS2000A_CHANNEL_A_MAX
        PS2000A_CHANNEL_A_MIN
        PS2000A_CHANNEL_B_MAX
        PS2000A_CHANNEL_B_MIN
        PS2000A_CHANNEL_C_MAX
        PS2000A_CHANNEL_C_MIN
        PS2000A_CHANNEL_D_MAX
        PS2000A_CHANNEL_D_MIN
    End Enum

    Enum CouplingMode
        PS2000A_AC
        PS2000A_DC
    End Enum

    Enum DigitalPort
        PS2000A_DIGITAL_PORT0 = 128     ' digital channel 0 - 7
        PS2000A_DIGITAL_PORT1           ' digital channel 8 - 15
        PS2000A_DIGITAL_PORT2           ' digital channel 16 - 23
        PS2000A_DIGITAL_PORT3           ' digital channel 24 - 31
        PS2000A_MAX_DIGITAL_PORTS = (PS2000A_DIGITAL_PORT3 - PS2000A_DIGITAL_PORT0) + 1
    End Enum

    Enum EtsMode
        PS2000A_ETS_OFF     'ETS disabled
        PS2000A_ETS_FAST
        PS2000A_ETS_SLOW
        PS2000A_ETS_MODES_MAX
    End Enum

    Enum ExtraOperations
        PS2000A_ES_OFF
        PS2000A_WHITENOISE
        PS2000A_PRBS 'Pseudo-Random Bit Stream 
    End Enum

    Enum IndexMode
        PS2000A_SINGLE
        PS2000A_DUAL
        PS2000A_QUAD
        PS2000A_MAX_INDEX_MODES
    End Enum

    Enum PulseWidthType
        PS2000A_PW_TYPE_NONE
        PS2000A_PW_TYPE_LESS_THAN
        PS2000A_PW_TYPE_GREATER_THAN
        PS2000A_PW_TYPE_IN_RANGE
        PS2000A_PW_TYPE_OUT_OF_RANGE
    End Enum

    Enum RatioMode
        PS2000A_RATIO_MODE_NONE = 0
        PS2000A_RATIO_MODE_AGGREGATE = 1
        PS2000A_RATIO_MODE_DECIMATE = 2
        PS2000A_RATIO_MODE_AVERAGE = 4
    End Enum

    Enum SigGenTrigSource
        PS2000A_SIGGEN_NONE
        PS2000A_SIGGEN_SCOPE_TRIG
        PS2000A_SIGGEN_AUX_IN
        PS2000A_SIGGEN_EXT_IN
        PS2000A_SIGGEN_SOFT_TRIG
    End Enum

    Enum SigGenTrigType
        PS2000A_SIGGEN_RISING
        PS2000A_SIGGEN_FALLING
        PS2000A_SIGGEN_GATE_HIGH
        PS2000A_SIGGEN_GATE_LOW
    End Enum

    Enum SweepType
        PS2000A_UP
        PS2000A_DOWN
        PS2000A_UPDOWN
        PS2000A_DOWNUP
        PS2000A_MAX_SWEEP_TYPES
    End Enum

    Enum ThresholdDirection
        'Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,
        PS2000A_ABOVE                   ' using upper threshold
        PS2000A_BELOW                   ' using upper threshold
        PS2000A_RISING                  ' using upper threshold
        PS2000A_NONE = PS2000A_RISING   ' no trigger set
        PS2000A_FALLING                 ' using upper threshold
        PS2000A_RISING_OR_FALLING       ' using both threshold
        PS2000A_ABOVE_LOWER             ' using lower threshold
        PS2000A_BELOW_LOWER             ' using lower threshold
        PS2000A_RISING_LOWER            ' using lower threshold
        PS2000A_FALLING_LOWER           ' using lower threshold
        ' Windowing using both thresholds
        PS2000A_INSIDE = PS2000A_ABOVE
        PS2000A_OUTSIDE = PS2000A_BELOW
        PS2000A_ENTER = PS2000A_RISING
        PS2000A_EXIT = PS2000A_FALLING
        PS2000A_ENTER_OR_EXIT = PS2000A_RISING_OR_FALLING
        PS2000A_POSITIVE_RUNT = 9
        PS2000A_NEGATIVE_RUNT
    End Enum

    Enum ThresholdMode
        LEVEL
        WINDOW
    End Enum

    Enum TimeUnits
        PS2000A_FS
        PS2000A_PS
        PS2000A_NS
        PS2000A_US
        PS2000A_MS
        PS2000A_S
    End Enum

    Enum TriggerState
        CONDITION_DONT_CARE
        CONDITION_TRUE
        CONDITION_FALSE
        CONDITION_MAX
    End Enum

    Enum VoltageRange
        PS2000A_10MV
        PS2000A_20MV
        PS2000A_50MV
        PS2000A_100MV
        PS2000A_200MV
        PS2000A_500MV
        PS2000A_1V
        PS2000A_2V
        PS2000A_5V
        PS2000A_10V
        PS2000A_20V
        PS2000A_50V
    End Enum

    Enum WaveType
        PS2000A_SINE
        PS2000A_SQUARE
        PS2000A_TRIANGLE
        PS2000A_RAMP_UP
        PS2000A_RAMP_DOWN
        PS2000A_SINC
        PS2000A_GAUSSIAN
        PS2000A_HALF_SINE
        PS2000A_DC_VOLTAGE
        PS2000A_WHITE_NOISE
        PS2000A_MAX_WAVE_TYPES
    End Enum


    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps2000aOpenUnit Lib "ps2000a.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Sub ps2000aCloseUnit Lib "ps2000a.dll" (ByVal handle As Short)

    Declare Function ps2000aGetUnitInfo Lib "ps2000a.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger
    Declare Function ps2000aSetChannel Lib "ps2000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As CouplingMode, ByVal range As VoltageRange,
                                                          ByVal analogueOffset As Single) As UInteger

    Declare Function ps2000aSetDigitalPort Lib "ps2000a.dll" (ByVal handle As Short, ByVal port As DigitalPort, ByVal enabled As Short, ByVal logicLevel As Short) As UInteger
    Declare Function ps2000aMaximumValue Lib "ps2000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger

    ' Block functions
    ' ---------------

    Declare Function ps2000aGetTimebase2 Lib "ps2000a.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As Integer, ByRef timeIntervalNs As Single, ByVal oversample As Short,
                                                            ByRef maxSamples As Integer, ByVal segmentIndex As UInteger) As UInteger

    Declare Function ps2000aGetValues Lib "ps2000a.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger,
                                                         ByVal downSampleRatioMode As RatioMode, ByVal segmentIndex As UInteger, ByRef overflow As Short) As UInteger

    Declare Function ps2000aIsReady Lib "ps2000a.dll" (ByVal handle As Short, ByRef ready As Short) As UInteger

    Declare Function ps2000aRunBlock Lib "ps2000a.dll" (ByVal handle As Short, ByVal noOfPreTriggerSamples As Integer, ByVal noOfPostTriggerSamples As Integer, ByVal timebase As UInteger,
                                                        ByVal oversample As Short, ByRef timeIndisposedMs As Integer, ByVal segmentIndex As UInteger, ByVal lpps2000aBlockReady As ps2000aBlockReady,
                                                        ByVal pParam As IntPtr) As UInteger

    ' Trigger Functions
    ' -----------------

    Declare Function ps2000aSetSimpleTrigger Lib "ps2000a.dll" (ByVal handle As Short, ByVal enable As Short, ByVal channel As Channel, ByVal threshold As Short, ByVal direction As ThresholdDirection,
                                                                ByVal delay As UInteger, ByVal autoTriggerMs As Short) As UInteger


    ' Functions relevant to all Data Capture Modes
    ' --------------------------------------------

    Declare Function ps2000aSetDataBuffer Lib "ps2000a.dll" (ByVal handle As Short, ByVal channelOrPort As Integer, ByRef buffer As Short, ByVal length As Integer, ByVal segmentIndex As UInteger,
                                                             ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps2000aStop Lib "ps2000a.dll" (ByVal handle As Short) As UInteger

    ' Signal Generator Functions
    ' --------------------------

    Declare Function ps2000aSetSigGenArbitrary Lib "ps2000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal peakToPeak As UInteger, ByVal startDeltaPhase As UInteger,
                                                                  ByVal stopDeltaPhase As UInteger, ByVal deltaPhaseIncrement As UInteger, ByVal dwellCount As UInteger, ByRef arbitraryWaveform As Short,
                                                                  ByVal arbitraryWaveformSize As Integer, ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal indexMode As IndexMode,
                                                                  ByVal shots As UInteger, ByVal sweeps As UInteger, ByVal triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource,
                                                                  ByVal extInThreshold As Short) As UInteger

    Declare Function ps2000aSetSigGenBuiltInV2 Lib "ps2000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal peakToPeak As UInteger, ByVal waveType As WaveType,
                                                                  ByVal startFrequency As Double, ByVal stopFrequency As Double, ByVal increment As Double, ByVal dwellTime As Double,
                                                                  ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal shots As UInteger, ByVal sweeps As UInteger,
                                                                  ByVal triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    Declare Function ps2000aSigGenArbitraryMinMaxValues Lib "ps2000a.dll" (ByVal handle As Short, ByRef minArbWaveformValue As Short, ByRef maxArbWaveformValue As Short,
                                                                           ByRef minArbWaveformSize As UInteger, ByRef maxArbWaveformSize As UInteger) As UInteger

    Declare Function ps2000aSigGenFrequencyToPhase Lib "ps2000a.dll" (ByVal handle As Short, ByVal frequency As Double, ByVal indexMode As IndexMode, ByVal bufferLength As UInteger,
                                                                      ByRef phase As UInteger) As UInteger

    Declare Function ps2000aSigGenSoftwareControl Lib "ps2000a.dll" (ByVal handle As Short, ByVal state As Short) As UInteger


    ' Delegate declarations
    ' =====================

    ' Block mode
    ' ----------
    Public Delegate Sub ps2000aBlockReady(handle As Short, status As UInteger, pVoid As IntPtr)


    ' Other Functions
    ' ===============


End Module
