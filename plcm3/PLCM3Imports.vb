'===================================================================================================
'
'	Filename:			PLCM3Imports.vb
'
'	Description: 
'	This file defines enumerations, functions and constants from the plcm3Api.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Module PLCM3Imports

#Region "Constants"

    Public Const PLCM3_MIN_WIRES As Short = 2
    Public Const PLCM3_MAX_WIRES As Short = 4

#End Region

#Region "Driver Enums"

    'PLCM3_CHANNELS
    Public Enum PLCM3Channels As UInteger
        PLCM3_CHANNEL_1 = 1
        PLCM3_CHANNEL_2
        PLCM3_CHANNEL_3
        PLCM3_MAX_CHANNELS = PLCM3_CHANNEL_3
    End Enum

    'PLCM3_DATA_TYPES
    Public Enum PLCM3DataType As UInteger
        PLCM3_OFF
        PLCM3_1_MILLIVOLT
        PLCM3_10_MILLIVOLTS
        PLCM3_100_MILLIVOLTS
        PLCM3_VOLTAGE
        PLCM3_MAX_DATA_TYPES
    End Enum

    'PLCM3_IP_DETAILS_TYPE
    Public Enum IpDetailsType As UInteger
        IDT_GET
        IDT_SET
    End Enum

    'PLCM3_COMMUNICATION_TYPE
    Public Enum CommunicationType As UInteger
        CT_USB = &H1
        CT_ETHERNET = &H2
        CT_ALL = &HFFFFFFFFUI
    End Enum

#End Region

#Region "Driver Imports"

    Declare Function PLCM3Enumerate Lib "plcm3.dll" (ByVal details As String, ByRef length As UInteger, ByVal type As CommunicationType) As UInteger
    Declare Function PLCM3OpenUnit Lib "plcm3.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Function PLCM3OpenUnitViaIp Lib "plcm3.dll" (ByRef handle As Short, ByVal serial As String, ByVal ipAddress As String) As UInteger
    Declare Function PLCM3CloseUnit Lib "plcm3.dll" (ByVal handle As Short) As UInteger

    Declare Function PLCM3GetUnitInfo Lib "plcm3.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger

    Declare Function PLCM3SetChannel Lib "plcm3.dll" (ByVal handle As Short, ByVal channel As PLCM3Channels, ByVal measurementType As PLCM3DataType) As UInteger
    Declare Function PLCM3SetMains Lib "plcm3.dll" (ByVal handle As Short, ByVal sixty_hertz As UShort) As UInteger
    Declare Function PLCM3GetValue Lib "plcm3.dll" (ByVal handle As Short, ByVal channel As PLCM3Channels, ByRef value As Integer) As UInteger

    Declare Function PLCM3IpDetails Lib "plcm3.dll" (ByVal handle As Short, ByRef enabled As Short, ByVal ipAddress As String, ByRef length As UShort, ByRef listeningPort As UShort,
                                                           ByVal type As IpDetailsType) As UInteger

    Declare Sub Sleep Lib "kernel32.dll" (ByVal time As Integer)

#End Region


End Module
