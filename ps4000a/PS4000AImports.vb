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

    Public Const AWG_DAC_FREQUENCY = 80000000.0
    Public Const AWG_PHASE_ACCUMULATOR = 4294967296.0
    Public Const MIN_DWELL_COUNT = 3

    ' Enumerations
    ' ============

    Enum SigGenTrigSource
        ps4000A_SIGGEN_NONE
        ps4000A_SIGGEN_SCOPE_TRIG
        ps4000A_SIGGEN_AUX_IN
        ps4000A_SIGGEN_EXT_IN
        ps4000A_SIGGEN_SOFT_TRIG
    End Enum

    Enum SigGenTrigType
        ps4000A_SIGGEN_RISING
        ps4000A_SIGGEN_FALLING
        ps4000A_SIGGEN_GATE_HIGH
        ps4000A_SIGGEN_GATE_LOW
    End Enum

    Enum ExtraOperations
        ps4000A_ES_OFF
        ps4000A_WHITENOISE
        ps4000A_PRBS 'Pseudo-Random Bit Stream 
    End Enum

    Enum SweepType
        ps4000A_UP
        ps4000A_DOWN
        ps4000A_UPDOWN
        ps4000A_DOWNUP
    End Enum

    Enum WaveType
        ps4000A_SINE
        ps4000A_SQUARE
        ps4000A_TRIANGLE
        ps4000A_RAMP_UP
        ps4000A_RAMP_DOWN
        ps4000A_SINC
        ps4000A_GAUSSIAN
        ps4000A_HALF_SINE
        ps4000A_DC_VOLTAGE
        ps4000A_WHITE_NOISE
    End Enum

    Enum IndexMode
        ps4000A_SINGLE
        ps4000A_DUAL
        ps4000A_QUAD
        ps4000A_MAX_INDEX_MODES
    End Enum

    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function ps4000aGetUnitInfo Lib "ps4000A.dll" (ByVal handle As Short, ByVal str As String, ByVal lth As Short, ByRef requiredSize As Short, ByVal info As Short) As UInteger
    Declare Function ps4000aOpenUnit Lib "ps4000A.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Function ps4000aCloseUnit Lib "ps4000A.dll" (ByVal handle As Short) As UInteger
    Declare Function ps4000aChangePowerSource Lib "ps4000A.dll" (ByVal handle As Short, ByVal source As Short) As UInteger


    ' Signal Generator Functions
    ' --------------------------

    Declare Function ps4000aSetSigGenBuiltIn Lib "ps4000A.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal waveType As WaveType, ByVal startFreq As Double, _
                                                                ByVal stopFreq As Double, ByVal increment As Double, ByVal dwellTime As Double, ByVal sweepType As SweepType, _
                                                                ByVal operation As ExtraOperations, ByVal shot As UInteger, ByVal sweeps As UInteger, ByRef triggerType As SigGenTrigType, _
                                                                ByVal triggerSource As SigGenTrigSource, ByVal extInThreshold As Short) As UInteger

    Declare Function ps4000aSetSigGenArbitrary Lib "ps4000A.dll" (ByVal handle As Short, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal startDeltaPhase As UInteger, _
                                                                  ByVal stopDeltaPhase As UInteger, ByVal deltaPhaseIncrement As UInteger, ByVal dwellCount As UInteger, ByRef arbitraryWaveform As Short, _
                                                                  ByVal arbitaryWaveformSize As Integer, ByVal sweepType As SweepType, ByVal operation As ExtraOperations, ByVal indexMode As IndexMode, _
                                                                  ByVal shots As UInteger, ByVal sweeps As UInteger, ByRef triggerType As SigGenTrigType, ByVal triggerSource As SigGenTrigSource, _
                                                                  ByVal extInThreshold As Short) As UInteger


End Module
