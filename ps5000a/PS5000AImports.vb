'===================================================================================================
'	
'   Filename: PS5000AImports.vb
'
'	Description: 
'	This file defines enumerations, functions and structures from the ps5000aApi.h C header file.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Module PS5000AImports

    ' Constant values
    ' ===============

    ' Define any constant values here as required.

    ' Structures
    ' ==========

    Structure TRIGGER_CHANNEL_PROPERTIES
        Dim thresholdUpper As Short
        Dim thresholdUpperHysteresis As UShort
        Dim thresholdLower As Short
        Dim thresholdLowerHysteresis As UShort
        Dim Channel As Channel
        Dim thresholdMode As ThresholdMode
    End Structure


    ' Enumerations
    ' ============

    Enum BandwidthLimiter
        PS5000A_BW_FULL
        PS5000A_BW_20MHZ
    End Enum

    Enum Channel
        PS5000A_CHANNEL_A
        PS5000A_CHANNEL_B
        PS5000A_CHANNEL_C
        PS5000A_CHANNEL_D
        PS5000A_EXTERNAL
    End Enum

    Enum ChannelBufferIndex
        PS5000A_CHANNEL_A_MAX
        PS5000A_CHANNEL_A_MIN
        PS5000A_CHANNEL_B_MAX
        PS5000A_CHANNEL_B_MIN
        PS5000A_CHANNEL_C_MAX
        PS5000A_CHANNEL_C_MIN
        PS5000A_CHANNEL_D_MAX
        PS5000A_CHANNEL_D_MIN
    End Enum

    Enum CouplingMode
        PS5000A_AC
        PS5000A_DC
    End Enum

    Enum DeviceResolution
        PS5000A_DR_8BIT
        PS5000A_DR_12BIT
        PS5000A_DR_14BIT
        PS5000A_DR_15BIT
        PS5000A_DR_16BIT
    End Enum

    Enum ExtraOperations
        PS5000A_ES_OFF
        PS5000A_WHITENOISE
        PS5000A_PRBS 'Pseudo-Random Bit Stream 
    End Enum

    Enum IndexMode
        PS5000A_SINGLE
        PS5000A_DUAL
        PS5000A_QUAD
        PS5000A_MAX_INDEX_MODES
    End Enum

    Enum PulseWidthType
        PS5000A_PW_TYPE_NONE
        PS5000A_PW_TYPE_LESS_THAN
        PS5000A_PW_TYPE_GREATER_THAN
        PS5000A_PW_TYPE_IN_RANGE
        PS5000A_PW_TYPE_OUT_OF_RANGE
    End Enum

    Enum RatioMode
        PS5000A_RATIO_MODE_NONE = 0
        PS5000A_RATIO_MODE_AGGREGATE = 1
        PS5000A_RATIO_MODE_DECIMATE = 2
        PS5000A_RATIO_MODE_AVERAGE = 4
    End Enum

    Enum SigGenTrigSource
        PS5000A_SIGGEN_NONE
        PS5000A_SIGGEN_SCOPE_TRIG
        PS5000A_SIGGEN_AUX_IN
        PS5000A_SIGGEN_EXT_IN
        PS5000A_SIGGEN_SOFT_TRIG
    End Enum

    Enum SigGenTrigType
        PS5000A_SIGGEN_RISING
        PS5000A_SIGGEN_FALLING
        PS5000A_SIGGEN_GATE_HIGH
        PS5000A_SIGGEN_GATE_LOW
    End Enum

    Enum SweepType
        PS5000A_UP
        PS5000A_DOWN
        PS5000A_UPDOWN
        PS5000A_DOWNUP
    End Enum

    Enum ThresholdDirection
        'Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,
        PS5000A_ABOVE
        PS5000A_BELOW
        PS5000A_RISING
        PS5000A_NONE = PS5000A_RISING ' no trigger set
        PS5000A_FALLING
        PS5000A_RISING_OR_FALLING
        PS5000A_ABOVE_LOWER
        PS5000A_BELOW_LOWER
        PS5000A_RISING_LOWER
        PS5000A_FALLING_LOWER
        PS5000A_INSIDE = PS5000A_ABOVE
        PS5000A_OUTSIDE = PS5000A_BELOW
        PS5000A_ENTER = PS5000A_RISING
        PS5000A_EXIT = PS5000A_FALLING
        PS5000A_ENTER_OR_EXIT = PS5000A_RISING_OR_FALLING
        PS5000A_POSITIVE_RUNT = 9
        PS5000A_NEGATIVE_RUNT
    End Enum

    Enum ThresholdMode
        LEVEL
        WINDOW
    End Enum

    Enum TimeUnits
        PS5000A_FS
        PS5000A_PS
        PS5000A_NS
        PS5000A_US
        PS5000A_MS
        PS5000A_S
    End Enum

    Enum TriggerState
        CONDITION_DONT_CARE
        CONDITION_TRUE
        CONDITION_FALSE
        CONDITION_MAX
    End Enum

    Enum VoltageRange
        PS5000A_10MV
        PS5000A_20MV
        PS5000A_50MV
        PS5000A_100MV
        PS5000A_200MV
        PS5000A_500MV
        PS5000A_1V
        PS5000A_2V
        PS5000A_5V
        PS5000A_10V
        PS5000A_20V
        PS5000A_50V
        PS5000A_100V
    End Enum

    Enum WaveType
        PS5000A_SINE
        PS5000A_SQUARE
        PS5000A_TRIANGLE
        PS5000A_RAMP_UP
        PS5000A_RAMP_DOWN
        PS5000A_SINC
        PS5000A_GAUSSIAN
        PS5000A_HALF_SINE
        PS5000A_DC_VOLTAGE
        PS5000A_WHITE_NOISE
        PS5000A_MAX_WAVE_TYPES
    End Enum


    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps5000aOpenUnit Lib "ps5000a.dll" (ByRef handle As Short, ByVal serial As String, ByVal resolution As DeviceResolution) As UInteger
    Declare Function ps5000aCloseUnit Lib "ps5000a.dll" (ByVal handle As Short) As UInteger
    Declare Function ps5000aGetUnitInfo Lib "ps5000a.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger
    Declare Function ps5000aPingUnit Lib "ps5000a.dll" (ByVal handle As Short) As UInteger

    Declare Function ps5000aSetChannel Lib "ps5000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As CouplingMode, ByVal range As VoltageRange, ByVal analogueOffset As Single) As UInteger

    Declare Function ps5000aChangePowerSource Lib "ps5000a.dll" (ByVal handle As Short, ByVal powerState As UInteger) As UInteger
    Declare Function ps5000aCurrentPowerSource Lib "ps5000a.dll" (ByVal handle As Short) As UInteger

    Declare Function ps5000aGetDeviceResolution Lib "ps5000a.dll" (ByVal handle As Short, ByRef resolution As DeviceResolution) As UInteger
    Declare Function ps5000aSetDeviceResolution Lib "ps5000a.dll" (ByVal handle As Short, ByVal resolution As DeviceResolution) As UInteger

    Declare Function ps5000aMaximumValue Lib "ps5000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger
    Declare Function ps5000aMinimumValue Lib "ps5000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger


    ' Trigger Functions
    ' -----------------

    Declare Function ps5000aSetSimpleTrigger Lib "ps5000a.dll" (ByVal handle As Short, ByVal enable As Short, ByVal channel As Channel, ByVal threshold As Short, ByVal direction As ThresholdDirection, ByVal delay As UInteger, ByVal autoTriggerMs As Short) As UInteger


    ' Functions relevant to all Data Capture Modes
    ' --------------------------------------------

    Declare Function ps5000aSetDataBuffer Lib "ps5000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef buffer As Short, ByVal length As Integer, ByVal segmentIndex As UInteger, ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps5000aSetDataBuffers Lib "ps5000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef bufferMax As Short, ByRef bufferMin As Short, ByVal length As Integer, ByVal segmentIndex As UInteger, ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps5000aStop Lib "ps5000a.dll" (ByVal handle As Short) As UInteger

    ' Block functions
    ' ---------------

    Declare Function ps5000aGetValues Lib "ps5000a.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger, ByVal downSampleRatioMode As RatioMode, ByVal segmentIndex As UInteger, ByRef overflow As Short) As UInteger
    Declare Function ps5000aGetTimebase2 Lib "ps5000a.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As Integer, ByRef timeIntervalNs As Single, ByRef maxSamples As Integer, ByVal segment As UInteger) As UInteger
    Declare Function ps5000aRunBlock Lib "ps5000a.dll" (ByVal handle As Short, noOfPreTriggerSamples As Integer, noOfPostTriggerSamples As Integer, timebase As UInteger, ByRef timeIndisposedMs As Integer, ByVal segmentIndex As UInteger, ByVal lpps5000aBlockReady As ps5000aBlockReady, pParam As IntPtr) As UInteger


    ' Rapid Block functions
    ' ---------------------

    Declare Function ps5000aGetMaxSegments Lib "ps5000a.dll" (ByVal handle As Short, ByRef maxSegments As UInteger) As UInteger
    Declare Function ps5000aGetNoOfCaptures Lib "ps5000a.dll" (ByVal handle As Short, ByRef nCaptures As UInteger) As UInteger
    Declare Function ps5000aGetValuesBulk Lib "ps5000a.dll" (ByVal handle As Short, ByRef numSamples As UInteger, ByVal fromSegmentIndex As UInteger, ByVal toSegmentIndex As UInteger, ByVal downSampleRatio As UInteger, ByVal downSampleRatioMode As RatioMode, ByRef overflow As Short) As UInteger
    Declare Function ps5000aMemorySegments Lib "ps5000a.dll" (ByVal handle As Short, ByVal nSegments As UInteger, ByRef nMaxSamples As Integer) As UInteger
    Declare Function ps5000aSetNoOfCaptures Lib "ps5000a.dll" (ByVal handle As Short, ByVal nCaptures As UInteger) As UInteger


    ' Streaming Functions
    ' -------------------




    ' Signal Generator Functions
    ' --------------------------

    Declare Function ps5000aSetSigGenArbitrary Lib "ps5000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal peakToPeak As UInteger, ByVal startDeltaPhase As UInteger, ByVal stopDeltaPhase As UInteger,
                                                                  ByVal deltaPhaseIncrement As UInteger, ByVal dwellCount As UInteger, ByRef arbitraryWaveform As Short, ByVal arbitraryWaveformSize As Integer,
                                                                  ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal indexMode As IndexMode, ByVal shots As UInteger, ByVal sweeps As UInteger,
                                                                  ByVal triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    Declare Function ps5000aSetSigGenBuiltInV2 Lib "ps5000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal peakToPeak As UInteger, ByVal waveType As WaveType, ByVal startFrequency As Double,
                                                                  ByVal stopFrequency As Double, ByVal increment As Double, ByVal dwellTime As Double, ByVal sweepType As SweepType, ByVal operation As ExtraOperations,
                                                                  ByVal shots As UInteger, ByVal sweeps As UInteger, ByVal triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    Declare Function ps5000aSigGenArbitraryMinMaxValues Lib "ps5000a.dll" (ByVal handle As Short, ByRef minArbWaveformValue As Short, ByRef maxArbWaveformValue As Short,
                                                                           ByRef minArbWaveformSize As UInteger, ByRef maxArbWaveformSize As UInteger) As UInteger

    Declare Function ps5000aSigGenFrequencyToPhase Lib "ps5000a.dll" (ByVal handle As Short, ByVal frequency As Double, ByVal indexMode As IndexMode, ByVal bufferLength As UInteger, ByRef phase As UInteger) As UInteger

    Declare Function ps5000aSigGenSoftwareControl Lib "ps5000a.dll" (ByVal handle As Short, ByVal state As Short) As UInteger


    ' Other Functions
    ' ---------------

    Declare Sub Sleep Lib "kernel32.dll" (ByVal milliseconds As Integer)


    ' Delegate declarations
    ' =====================

    ' Block mode
    ' ----------

    Public Delegate Sub ps5000aBlockReady(handle As Short, status As UInteger, pVoid As IntPtr)

End Module
