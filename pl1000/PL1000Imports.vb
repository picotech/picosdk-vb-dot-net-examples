'===================================================================================================
'	
'   Filename: PL1000Imports.vb
'
'	Description: 
'	This file defines enumerations and functions from the pl1000Api.h C header file.
'
'   Copyright (C) 2018 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Module PL1000Imports

    ' Constant values
    ' ===============

    Public Const PL1000_MIN_PERIOD = 100
    Public Const PL1000_MAX_PERIOD = 1800

    ' Enumerations
    ' ============

    Enum PL1000Inputs
        PL1000_CHANNEL_1 = 1
        PL1000_CHANNEL_2
        PL1000_CHANNEL_3
        PL1000_CHANNEL_4
        PL1000_CHANNEL_5
        PL1000_CHANNEL_6
        PL1000_CHANNEL_7
        PL1000_CHANNEL_8
        PL1000_CHANNEL_9
        PL1000_CHANNEL_10
        PL1000_CHANNEL_11
        PL1000_CHANNEL_12
        PL1000_CHANNEL_13
        PL1000_CHANNEL_14
        PL1000_CHANNEL_15
        PL1000_CHANNEL_16
        PL1000_MAX_CHANNELS = PL1000_CHANNEL_16
    End Enum

    Enum PL1000DOChannel
        PL1000_DO_CHANNEL_0
        PL1000_DO_CHANNEL_1
        PL1000_DO_CHANNEL_2
        PL1000_DO_CHANNEL_3
        PL1000_DO_CHANNEL_MAX
    End Enum

    Enum PL1000OpenProgress
        PL1000_OPEN_PROGRESS_FAIL = -1
        PL1000_OPEN_PROGRESS_PENDING = 0
        PL1000_OPEN_PROGRESS_COMPLETE = 1
    End Enum

    Enum BlockMethod
        BM_SINGLE
        BM_WINDOW
        BM_STREAM
    End Enum

    ' Function Declarations
    ' =====================

    ' Device Connection and Setup Functions
    ' -------------------------------------

    Declare Function pl1000OpenUnit Lib "pl1000.dll" (ByRef handle As Short) As UInteger
    Declare Function pl1000CloseUnit Lib "pl1000.dll" (ByVal handle As Short) As UInteger
    Declare Function pl1000GetUnitInfo Lib "pl1000.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger
    Declare Function pl1000SetInterval Lib "pl1000.dll" (ByVal handle As Short, ByRef us_for_block As UInteger, ByVal ideal_no_of_samples As UInteger, ByRef channels As Short, ByVal no_of_channels As Short) As UInteger
    Declare Function pl1000SetDO Lib "pl1000.dll" (ByVal handle As Short, ByVal doValue As Short, ByVal doNo As Short) As UInteger

    ' Data Collection Functions
    ' -------------------------

    Declare Function pl1000Run Lib "pl1000.dll" (ByVal handle As Short, ByVal numValuesPerChannel As UInteger, ByVal method As BlockMethod) As UInteger
    Declare Function pl1000GetValues Lib "pl1000.dll" (ByVal handle As Short, ByRef values As UShort, ByRef numValuesPerChannel As UInteger, ByRef overflow As UShort, ByRef triggerIndex As UInteger) As UInteger
    Declare Function pl1000Ready Lib "pl1000.dll" (ByVal handle As Short, ByRef ready As Short) As UInteger
    Declare Function pl1000Stop Lib "pl1000.dll" (ByVal handle As Short) As UInteger
    Declare Function pl1000GetSingle Lib "pl1000.dll" (ByVal handle As Short, ByVal channel As PL1000Inputs, ByRef value As UShort) As UInteger

    ' Other Functions
    ' ---------------

    Declare Function pl1000MaxValue Lib "pl1000.dll" (ByVal handle As Short, ByRef maxValues As UShort) As UInteger
    Declare Function pl1000PingUnit Lib "pl1000.dll" (ByVal handle As Short) As UInteger
    Declare Function pl1000SetPulseWidth Lib "pl1000.dll" (ByVal handle As Short, ByVal period As UShort, ByVal cycle As Byte) As UInteger

    Declare Function pl1000SetTrigger Lib "pl1000.dll" (ByVal handle As Short, ByVal enabled As UShort, ByVal autoTrigger As UShort,
                                                        ByVal autoMs As UShort, ByVal channel As UShort, ByVal direction As UShort,
                                                        ByVal threshold As UShort, ByVal hysteresis As UShort, ByVal delay As Single) As UInteger

End Module