'===================================================================================================
'
'	Filename:			PS4000AImports.vb
'
'	Description: 
'	This file defines enumerations, functions and structures from the ps4000aApi.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'===================================================================================================

Module PS4000AImports

    ' Constant values
    ' ===============

    ' Define any constant values here as required.

    Public Const PS4000A_EXT_MAX_VALUE = 32767
    Public Const PS4000A_EXT_MIN_VALUE = -32767

    Public Const MAX_PULSE_WIDTH_QUALIFIER_COUNT = 16777215
    Public Const MAX_DELAY_COUNT = 8388607

    Public Const PS4000A_MIN_SIG_GEN_BUFFER_SIZE = 10
    Public Const MIN_DWELL_COUNT = 3
    Public Const PS4000A_MAX_SWEEPS_SHOTS = ((1 << 30) - 1)
    Public Const AWG_DAC_FREQUENCY = 80000000.0
    Public Const AWG_PHASE_ACCUMULATOR = 4294967296.0


    Public Const PS4000A_MAX_ANALOGUE_OFFSET_50MV_200MV = 0.25F
    Public Const PS4000A_MIN_ANALOGUE_OFFSET_50MV_200MV = -0.25F
    Public Const PS4000A_MAX_ANALOGUE_OFFSET_500MV_2V = 2.5F
    Public Const PS4000A_MIN_ANALOGUE_OFFSET_500MV_2V = -2.5F
    Public Const PS4000A_MAX_ANALOGUE_OFFSET_5V_20V = 20.0F
    Public Const PS4000A_MIN_ANALOGUE_OFFSET_5V_20V = -20.0F

    ' Enumerations
    ' ============

    Enum DeviceResolution
        PS4000A_DR_8BIT
        PS4000A_DR_12BIT
        PS4000A_DR_14BIT
        PS4000A_DR_15BIT
        PS4000A_DR_16BIT
    End Enum

    Enum ExtraOperations
        PS4000A_ES_OFF
        PS4000A_WHITENOISE
        PS4000A_PRBS 'Pseudo-Random Bit Stream 
    End Enum

    Enum BandwidthLimiterFlags
        PS4000A_BW_FULL_FLAG = (1 << 0)
        PS4000A_BW_20KHZ_FLAG = (1 << 1)
        PS4000A_BW_100KHZ_FLAG = (1 << 2) '( Default When current clamp detected: can be changed)
        PS4000A_BW_1MHZ_FLAG = (1 << 3) '( default for 14 bits: can be changed)
    End Enum

    Enum BandwidthLimiter
        PS4000A_BW_FULL
        PS4000A_BW_20KHZ
        PS4000A_BW_100KHZ '( Default When current clamp detected: can be changed)
        PS4000A_BW_1MHZ
    End Enum

    Enum CouplingMode
        PS4000A_AC
        PS4000A_DC
    End Enum

    Enum Channel
        PS4000A_CHANNEL_A
        PS4000A_CHANNEL_B
        PS4000A_CHANNEL_C
        PS4000A_CHANNEL_D
        PS4000A_MAX_4_CHANNELS
        PS4000A_CHANNEL_E = PS4000A_MAX_4_CHANNELS
        PS4000A_CHANNEL_F
        PS4000A_CHANNEL_G
        PS4000A_CHANNEL_H
        PS4000A_EXTERNAL
        PS4000A_MAX_CHANNELS = PS4000A_EXTERNAL
        PS4000A_TRIGGER_AUX
        PS4000A_MAX_TRIGGER_SOURCES
        PS4000A_PULSE_WIDTH_SOURCE = &H10000000
    End Enum

    Enum ChannelBufferIndex
        PS4000A_CHANNEL_A_MAX
        PS4000A_CHANNEL_A_MIN
        PS4000A_CHANNEL_B_MAX
        PS4000A_CHANNEL_B_MIN
        PS4000A_CHANNEL_C_MAX
        PS4000A_CHANNEL_C_MIN
        PS4000A_CHANNEL_D_MAX
        PS4000A_CHANNEL_D_MIN
        PS4000A_CHANNEL_E_MAX
        PS4000A_CHANNEL_E_MIN
        PS4000A_CHANNEL_F_MAX
        PS4000A_CHANNEL_F_MIN
        PS4000A_CHANNEL_G_MAX
        PS4000A_CHANNEL_G_MIN
        PS4000A_CHANNEL_H_MAX
        PS4000A_CHANNEL_H_MIN
        PS4000A_MAX_CHANNEL_BUFFERS
    End Enum

    Enum VoltageRange
        PS4000A_10MV
        PS4000A_20MV
        PS4000A_50MV
        PS4000A_100MV
        PS4000A_200MV
        PS4000A_500MV
        PS4000A_1V
        PS4000A_2V
        PS4000A_5V
        PS4000A_10V
        PS4000A_20V
        PS4000A_50V
        PS4000A_100V
        PS4000A_200V
        PS4000A_MAX_RANGES
    End Enum

    Enum ResistanceRange
        PS4000A_RESISTANCE_315K = &H200
        PS4000A_RESISTANCE_1100K
        PS4000A_RESISTANCE_10M
        PS4000A_MAX_RESISTANCE_RANGES = (PS4000A_RESISTANCE_10M + 1) - PS4000A_RESISTANCE_315K
        PS4000A_RESISTANCE_ADCV = &H10000000
    End Enum

    Enum EtsMode
        PS4000A_ETS_OFF             ' ETS disabled
        PS4000A_ETS_FAST            ' Return ready As soon As requested no Of interleaves Is available
        PS4000A_ETS_SLOW            ' Return ready every time a New Set Of no_of_cycles Is collected
        PS4000A_ETS_MODES_MAX
    End Enum

    Enum TimeUnits
        PS4000A_FS
        PS4000A_PS
        PS4000A_NS
        PS4000A_US
        PS4000A_MS
        PS4000A_S
        PS4000A_MAX_TIME_UNITS
    End Enum

    Enum SweepType
        PS4000A_UP
        PS4000A_DOWN
        PS4000A_UPDOWN
        PS4000A_DOWNUP
        PS4000A_MAX_SWEEP_TYPES
    End Enum

    Enum WaveType
        PS4000A_SINE
        PS4000A_SQUARE
        PS4000A_TRIANGLE
        PS4000A_RAMP_UP
        PS4000A_RAMP_DOWN
        PS4000A_SINC
        PS4000A_GAUSSIAN
        PS4000A_HALF_SINE
        PS4000A_DC_VOLTAGE
        PS4000A_WHITE_NOISE
    End Enum

    Enum PinStates
        PS4000A_CAL_PINS_OFF
        PS4000A_GND_SIGNAL
        PS4000A_SIGNAL_SIGNAL
    End Enum

    Enum ChannelLed
        PS4000A_CHANNEL_LED_OFF
        PS4000A_CHANNEL_LED_RED
        PS4000A_CHANNEL_LED_GREEN
    End Enum

    Enum SigGenTrigType
        PS4000A_SIGGEN_RISING
        PS4000A_SIGGEN_FALLING
        PS4000A_SIGGEN_GATE_HIGH
        PS4000A_SIGGEN_GATE_LOW
    End Enum

    Enum SigGenTrigSource
        PS4000A_SIGGEN_NONE
        PS4000A_SIGGEN_SCOPE_TRIG
        PS4000A_SIGGEN_AUX_IN
        PS4000A_SIGGEN_EXT_IN
        PS4000A_SIGGEN_SOFT_TRIG
    End Enum

    Enum IndexMode
        PS4000A_SINGLE
        PS4000A_DUAL
        PS4000A_QUAD
        PS4000A_MAX_INDEX_MODES
    End Enum

    Enum ThresholdMode
        PS4000A_LEVEL
        PS4000A_WINDOW
    End Enum

    Enum ThresholdDirection
        PS4000A_ABOVE 'Using upper threshold
        PS4000A_BELOW 'Using upper threshold
        PS4000A_RISING ' Using upper threshold
        PS4000A_FALLING ' Using upper threshold
        PS4000A_RISING_OR_FALLING ' Using both threshold
        PS4000A_ABOVE_LOWER ' Using lower threshold
        PS4000A_BELOW_LOWER ' Using lower threshold
        PS4000A_RISING_LOWER             ' Using lower threshold
        PS4000A_FALLING_LOWER        ' Using lower threshold

        ' Windowing using both thresholds
        PS4000A_INSIDE = PS4000A_ABOVE
        PS4000A_OUTSIDE = PS4000A_BELOW
        PS4000A_ENTER = PS4000A_RISING
        PS4000A_EXIT = PS4000A_FALLING
        PS4000A_ENTER_OR_EXIT = PS4000A_RISING_OR_FALLING
        PS4000A_POSITIVE_RUNT = 9
        PS4000A_NEGATIVE_RUNT

        ' No trigger set
        PS4000A_NONE = PS4000A_RISING 
    End Enum

    Enum TriggerState
        PS4000A_CONDITION_DONT_CARE
        PS4000A_CONDITION_TRUE
        PS4000A_CONDITION_FALSE
        PS4000A_CONDITION_MAX
    End Enum

    Enum ConditionsInfo
        PS4000A_CLEAR = &H1
        PS4000A_ADD = &H2
    End Enum

    Enum RatioMode
        PS4000A_RATIO_MODE_NONE = 0
        PS4000A_RATIO_MODE_AGGREGATE = 1
        PS4000A_RATIO_MODE_DECIMATE = 2
        PS4000A_RATIO_MODE_AVERAGE = 4
        PS4000A_RATIO_MODE_DISTRIBUTION = 8
    End Enum

    Enum PulseWidthType
        PS4000A_PW_TYPE_NONE
        PS4000A_PW_TYPE_LESS_THAN
        PS4000A_PW_TYPE_GREATER_THAN
        PS4000A_PW_TYPE_IN_RANGE
        PS4000A_PW_TYPE_OUT_OF_RANGE
    End Enum

    Enum ChannelInfo
        PS4000A_CI_RANGES
        PS4000A_CI_RESISTANCES
    End Enum

    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps4000aOpenUnit Lib "ps4000a.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Function ps4000aOpenUnitWithResolution Lib "ps4000a.dll" (ByRef handle As Short, ByVal serial As String, ByVal resolution As DeviceResolution) As UInteger
    Declare Function ps4000aCloseUnit Lib "ps4000a.dll" (ByVal handle As Short) As UInteger
    Declare Function ps4000aGetUnitInfo Lib "ps4000a.dll" (ByVal handle As Short, ByVal str As String, ByVal lth As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger
    Declare Function ps4000aPingUnit Lib "ps4000a.dll" (ByVal handle As Short) As UInteger

    Declare Function ps4000aSetChannel Lib "ps4000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As CouplingMode, ByVal range As PicoConnectProbeRange,
                                                          ByVal analogOffset As Single) As UInteger

    Declare Function ps4000aChangePowerSource Lib "ps4000a.dll" (ByVal handle As Short, ByVal powerstate As UInteger) As UInteger
    Declare Function ps4000aCurrentPowerSource Lib "ps4000a.dll" (ByVal handle As Short) As UInteger

    Declare Function ps4000aGetDeviceResolution Lib "ps4000a.dll" (ByVal handle As Short, ByRef resolution As DeviceResolution) As UInteger
    Declare Function ps4000aSetDeviceResolution Lib "ps4000a.dll" (ByVal handle As Short, ByVal resolution As DeviceResolution) As UInteger

    Declare Function ps4000aMaximumValue Lib "ps4000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger
    Declare Function ps4000aMinimumValue Lib "ps4000a.dll" (ByVal handle As Short, ByRef value As Short) As UInteger

    ' Trigger Functions
    ' -----------------

    Declare Function ps4000aSetSimpleTrigger Lib "ps4000a.dll" (ByVal handle As Short, ByVal enable As Short, ByVal channel As Channel, ByVal threshold As Short, ByVal direction As ThresholdDirection,
                                                                    ByVal delay As UInteger, ByVal autoTriggerMs As Short) As UInteger

    ' Functions relevant to all Data Capture Modes
    ' --------------------------------------------

    Declare Function ps4000aSetDataBuffer Lib "ps4000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef buffer As Short, ByVal length As Integer, ByVal segmentIndex As UInteger,
                                                                ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps4000aSetDataBuffers Lib "ps4000a.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef bufferMax As Short, ByRef bufferMin As Short, ByVal length As Integer,
                                                                ByVal segmentIndex As UInteger, ByVal downSampleRatioMode As RatioMode) As UInteger
    Declare Function ps4000aStop Lib "ps4000a.dll" (ByVal handle As Short) As UInteger

    ' Block functions
    ' ---------------

    Declare Function ps4000aGetValues Lib "ps4000a.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger,
                                                            ByVal downSampleRatioMode As RatioMode, ByVal segmentIndex As UInteger, ByRef overflow As Short) As UInteger
    Declare Function ps4000aGetTimebase2 Lib "ps4000a.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As Integer, ByRef timeIntervalNs As Single,
                                                                ByRef maxSamples As Integer, ByVal segmentIndex As UInteger) As UInteger
    Declare Function ps4000aRunBlock Lib "ps4000a.dll" (ByVal handle As Short, ByVal noOfPreTriggerSamples As Integer, ByVal noOfPostTriggerSamples As Integer, ByVal timebase As UInteger,
                                                            ByRef timeIndisposedMs As Integer, ByVal segmentIndex As UInteger, ByVal lpps4000aBlockReady As ps4000aBlockReady, ByVal pParam As IntPtr) As UInteger

    ' Rapid Block functions
    ' ---------------------

    Declare Function ps4000aGetMaxSegments Lib "ps4000a.dll" (ByVal handle As Short, ByRef maxSegments As UInteger) As UInteger
    Declare Function ps4000aGetNoOfCaptures Lib "ps4000a.dll" (ByVal handle As Short, ByRef nCaptures As UInteger) As UInteger
    Declare Function ps4000aGetValuesBulk Lib "ps4000a.dll" (ByVal handle As Short, ByRef numSamples As UInteger, ByVal fromSegmentIndex As UInteger, ByVal toSegmentIndex As UInteger,
                                                                ByVal downSampleRatio As UInteger, ByVal downSampleRatioMode As RatioMode, ByRef overflow As Short) As UInteger
    Declare Function ps4000aMemorySegments Lib "ps4000a.dll" (ByVal handle As Short, ByVal nSegments As UInteger, ByRef nMaxSamples As Integer) As UInteger
    Declare Function ps4000aSetNoOfCaptures Lib "ps4000a.dll" (ByVal handle As Short, ByVal nCaptures As UInteger) As UInteger

    ' Signal Generator Functions
    ' --------------------------

    Declare Function ps4000aSetSigGenBuiltIn Lib "ps4000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal waveType As WaveType, ByVal startFreq As Double,
                                                                ByVal stopFreq As Double, ByVal increment As Double, ByVal dwellTime As Double, ByVal sweepType As SweepType,
                                                                ByVal operation As ExtraOperations, ByVal shot As UInteger, ByVal sweeps As UInteger, ByRef triggerType As SigGenTrigType,
                                                                ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    Declare Function ps4000aSetSigGenArbitrary Lib "ps4000a.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal startDeltaPhase As UInteger,
                                                                  ByVal stopDeltaPhase As UInteger, ByVal deltaPhaseIncrement As UInteger, ByVal dwellCount As UInteger, ByRef arbitraryWaveform As Short,
                                                                  ByVal arbitaryWaveformSize As Integer, ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal indexMode As IndexMode,
                                                                  ByVal shots As UInteger, ByVal sweeps As UInteger, ByRef triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource,
                                                                  ByVal extInThreshold As Short) As UInteger

    ' Delegate declarations
    ' =====================

    ' Block mode
    ' ----------

    Public Delegate Sub ps4000aBlockReady(handle As Short, status As UInteger, pVoid As IntPtr)

End Module
