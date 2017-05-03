'===================================================================================================
'
'	Filename:			PS2000Imports.vb
'
'	Description: 
'	    This file defines enumerations, functions and structures from the ps2000.h and ps2000Wrap.h 
'       C header files.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Module PS2000Imports

    ' Constant values
    ' ===============

    ' Although the PS2000 uses an 8-bit ADC it is usually possible to
    ' oversample (collect multiple readings at each time) by up to 256.
    ' the results are therefore ALWAYS scaled up to 16-bits even if
    ' oversampling is not used.
    ' The maximum and minimum values returned are therefore as follows:

    Public Const PS2000_MAX_VALUE = 32767
    Public Const PS2000_LOST_DATA = -32768


    ' Structures
    ' ==========

    Structure TRIGGER_CHANNEL_PROPERTIES
        Dim thresholdMajor As Short
        Dim thresholdMinor As Short
        Dim hysteresis As UShort
        Dim channel As Channel
        Dim thresholdMode As ThresholdMode
    End Structure

    Structure TRIGGER_CONDITIONS
        Dim channelA As TriggerState
        Dim channelB As TriggerState
        Dim channelC As TriggerState
        Dim channelD As TriggerState
        Dim external As TriggerState
        Dim pulseWidthQualifier As TriggerState
    End Structure

    Structure PWQ_CONDITIONS
        Dim channelA As TriggerState
        Dim channelB As TriggerState
        Dim channelC As TriggerState
        Dim channelD As TriggerState
        Dim external As TriggerState
    End Structure

    ' Enumerations
    ' ============

    Enum Channel
        PS2000_CHANNEL_A
        PS2000_CHANNEL_B
        PS2000_CHANNEL_C
        PS2000_CHANNEL_D
        PS2000_EXTERNAL
        PS2000_MAX_CHANNELS = PS2000_EXTERNAL
        PS2000_NONE
    End Enum

    Enum VoltageRange
        PS2000_10MV
        PS2000_20MV
        PS2000_50MV
        PS2000_100MV
        PS2000_200MV
        PS2000_500MV
        PS2000_1V
        PS2000_2V
        PS2000_5V
        PS2000_10V
        PS2000_20V
        PS2000_50V
        PS2000_MAX_RANGES
    End Enum

    Enum TimeUnits
        PS2000_FS
        PS2000_PS
        PS2000_NS
        PS2000_US
        PS2000_MS
        PS2000_S
        PS2000_MAX_TIME_UNITS
    End Enum


    Enum PS2000Error

        PS2000_OK
        PS2000_MAX_UNITS_OPENED  ' more than PS2000_MAX_UNITS
        PS2000_MEM_FAIL          ' not enough RAM on host machine
        PS2000_NOT_FOUND         ' cannot find device
        PS2000_FW_FAIL           ' unable to download firmware
        PS2000_NOT_RESPONDING
        PS2000_CONFIG_FAIL       'missing or corrupted configuration settings
        PS2000_OS_NOT_SUPPORTED  'need to use win98SE (or later) or win2k (or later)
        PS2000_PICOPP_TOO_OLD
    End Enum

    Enum Info
        PS2000_DRIVER_VERSION
        PS2000_USB_VERSION
        PS2000_HARDWARE_VERSION
        PS2000_VARIANT_INFO
        PS2000_BATCH_AND_SERIAL
        PS2000_CAL_DATE
        PS2000_ERROR_CODE
        PS2000_KERNEL_DRIVER_VERSION
    End Enum

    Enum TriggerDirection
        PS2000_RISING
        PS2000_FALLING
        PS2000_MAX_DIRS
    End Enum

    Enum OpenProgress
        PS2000_OPEN_PROGRESS_FAIL = -1
        PS2000_OPEN_PROGRESS_PENDING = 0
        PS2000_OPEN_PROGRESS_COMPLETE = 1
    End Enum

    Enum EtsMode
        PS2000_ETS_OFF   ' ETS disabled
        PS2000_ETS_FAST  ' Return ready as soon as requested no of interleaves is available
        PS2000_ETS_SLOW  ' Return ready every time a new set of no_of_cycles is collected
        PS2000_ETS_MODES_MAX
    End Enum

    Enum ButtonState
        PS2000_NO_PRESS
        PS2000_SHORT_PRESS
        PS2000_LONG_PRESS
    End Enum

    Enum SweepType
        PS2000_UP
        PS2000_DOWN
        PS2000_UPDOWN
        PS2000_DOWNUP
        MAX_SWEEP_TYPES
    End Enum

    Enum WaveType
        PS2000_SINE
        PS2000_SQUARE
        PS2000_TRIANGLE
        PS2000_RAMPUP
        PS2000_RAMPDOWN
        PS2000_DC_VOLTAGE
        PS2000_GAUSSIAN
        PS2000_SINC
        PS2000_HALF_SINE
    End Enum

    Enum ThresholdDirection
        PS2000_ABOVE
        PS2000_BELOW
        PS2000_ADV_RISING
        PS2000_ADV_FALLING
        PS2000_RISING_OR_FALLING
        PS2000_INSIDE = PS2000_ABOVE
        PS2000_OUTSIDE = PS2000_BELOW
        PS2000_ENTER = PS2000_ADV_RISING
        PS2000_EXIT = PS2000_ADV_FALLING
        PS2000_ENTER_OR_EXIT = PS2000_RISING_OR_FALLING
        PS2000_ADV_NONE = PS2000_ADV_RISING
    End Enum

    Enum ThresholdMode
        PS2000_LEVEL
        PS2000_WINDOW
    End Enum

    Enum TriggerState
        PS2000_CONDITION_DONT_CARE
        PS2000_CONDITION_TRUE
        PS2000_CONDITION_FALSE
        PS2000_CONDITION_MAX
    End Enum

    Enum PulseWidthType
        PS2000_PW_TYPE_NONE
        PS2000_PW_TYPE_LESS_THAN
        PS2000_PW_TYPE_GREATER_THAN
        PS2000_PW_TYPE_IN_RANGE
        PS2000_PW_TYPE_OUT_OF_RANGE
    End Enum

    ' Functions 
    ' =========

    Declare Function ps2000_open_unit Lib "ps2000.dll" () As Short
    Declare Function ps2000_flash_led Lib "ps2000.dll" (ByVal handle As Short) As Short
    Declare Sub ps2000_close_unit Lib "ps2000.dll" (ByVal handle As Short)

    Declare Function ps2000_get_unit_info Lib "ps2000.dll" (ByVal handle As Short, ByVal str As String, ByVal lth As Short, ByVal line_no As Info) As Short

    Declare Function ps2000_set_channel Lib "ps2000.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As Short, ByVal range As VoltageRange) As Short

    Declare Function ps2000_set_trigger2 Lib "ps2000.dll" (ByVal handle As Short, ByVal source As Channel, ByVal threshold As Short, ByVal direction As ThresholdDirection, _
                                                           ByVal delay As Single, ByVal auto_trigger_ms As Short) As Short

    Declare Function ps2000_get_timebase Lib "ps2000.dll" (ByVal handle As Short, ByVal timebase As Short, ByVal numSamples As Integer, ByRef timeInterval As Integer, ByRef timeUnits As Short, _
                                                           ByVal oversample As Short, ByRef maxSamples As Integer) As Short

    Declare Function ps2000_run_block Lib "ps2000.dll" (ByVal handle As Short, ByVal no_of_values As Integer, ByVal timebase As Short, ByVal oversample As Short, ByRef timeIndisposedMs As Integer) As Short
    Declare Function ps2000_ready Lib "ps2000.dll" (ByVal handle As Short) As Short

    Declare Function ps2000_get_values Lib "ps2000.dll" (ByVal handle As Short, ByRef buffer_a As Short, ByRef buffer_b As Short, ByRef buffer_c As Short, ByRef buffer_d As Short, _
                                                         ByRef overflow As Short, ByVal no_of_values As Integer) As Integer

    Declare Function ps2000_get_times_and_values Lib "ps2000.dll" (ByVal handle As Short, ByRef times As Integer, ByRef buffer_a As Short, ByRef buffer_b As Short, ByRef buffer_c As Short, _
                                                                   ByRef buffer_d As Short, ByRef overflow As Short, ByVal timeUnits As Short, ByVal numSamples As Integer) As Integer

    Declare Function ps2000_stop Lib "ps2000.dll" (ByVal handle As Short) As Short

    Declare Function ps2000_run_streaming Lib "ps2000.dll" (ByVal handle As Short, ByVal sample_interval_ms As Short, ByVal maxSamples As Integer, ByVal windowed As Short) As Short
    Declare Function ps2000_run_streaming_ns Lib "ps2000.dll" (ByVal handle As Short, ByVal sample_interval As UInteger, ByVal timeUnits As TimeUnits, ByVal maxSamples As UInteger, _
                                                               ByVal auto_stop As Short, ByVal noOfSamplesPerAggregate As UInteger, ByVal overview_buffer_size As UInteger) As Short

    Declare Function ps2000_get_streaming_values_no_aggregation Lib "ps2000.dll" (ByVal handle As Short, ByRef start_time As Double, ByRef pBuffer_a As Short, ByRef pBuffer_b As Short, _
                                                                                  ByRef pBuffer_c As Short, ByRef pBuffer_d As Short, ByRef overflow As Short, ByRef triggerAt As UInteger, _
                                                                                  ByRef trigger As Short, ByVal numValues As UInteger) As UInteger

    Declare Function ps2000_set_ets Lib "ps2000.dll" (ByVal handle As Short, ByVal mode As Short, ByVal ets_cycles As Short, ByVal ets_interleave As Short) As Integer

    Declare Function ps2000_set_sig_gen_built_in Lib "ps2000.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal waveType As WaveType, _
                                                                   ByVal startFrequency As Single, ByVal stopFrequency As Single, ByVal increment As Single, ByVal dwellTime As Single, _
                                                                   ByVal sweepType As SweepType, ByVal sweeps As UInteger) As Short

    ' Wrapper Functions
    Declare Function PollFastStreaming Lib "ps2000Wrap.dll" (ByVal handle As Integer) As Short
    Declare Sub SetBuffer Lib "ps2000Wrap.dll" (ByVal handle As Integer, ByVal channel As Channel, ByRef buffer As Integer, ByVal bufferSize As UInteger)
    Declare Sub SetAggregateBuffer Lib "ps2000Wrap.dll" (ByVal handle As Integer, ByVal channel As Channel, ByRef bufferMax As Integer, ByRef bufferMin As Integer, ByVal bufferSize As UInteger)
    Declare Function FastStreamingReady Lib "ps2000Wrap.dll" (ByVal handle As Short) As Short

    Declare Function GetFastStreamingDetails Lib "ps2000Wrap.dll" (ByVal handle As Short, ByRef overflow As Short, ByRef triggeredAt As UInteger, ByRef triggered As Short, ByRef auto_stop As Short, _
                                                                   ByRef appBufferFull As Short, ByRef startIndex As UInteger) As UInteger

    Declare Sub setEnabledChannels Lib "ps2000Wrap.dll" (ByVal handle As Short, ByRef enabledChannels As Short)
    Declare Sub clearFastStreamingParameters Lib "ps2000Wrap.dll" (ByVal handle As Short)

    Declare Function setCollectionInfo Lib "ps2000Wrap.dll" (ByVal handle As Short, ByVal collectionSize As UInteger, ByVal overviewBufferSize As UInteger) As Short
    
    Declare Sub Sleep Lib "kernel32.dll" (ByVal time As Integer)

End Module
