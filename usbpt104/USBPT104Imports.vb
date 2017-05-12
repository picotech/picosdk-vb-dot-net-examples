'===================================================================================================
'
'	Filename:			USBPT104Imports.vb
'
'	Description: 
'	This file defines enumerations and constants from the usbPT104Api.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Module USBPT104Imports

#Region "Constants"

    Public Const USBPT104_OFF As Short = 1
    Public Const USBPT104_MIN_WIRES As Short = 2
    Public Const USBPT104_3_WIRES As Short = 3
    Public Const USBPT104_MAX_WIRES As Short = 4

#End Region

#Region "Driver Enums"

    ' USBPT104_CHANNELS
    Public Enum UsbPt104Channels As UInteger
        USBPT104_CHANNEL_1 = 1
        USBPT104_CHANNEL_2
        USBPT104_CHANNEL_3
        USBPT104_CHANNEL_4
        USBPT104_CHANNEL_5
        USBPT104_CHANNEL_6
        USBPT104_CHANNEL_7
        USBPT104_CHANNEL_8
        USBPT104_MAX_CHANNELS = USBPT104_CHANNEL_8
    End Enum

    'USBPT104_DATA_TYPES;
    Public Enum UsbPt104DataType As UInteger
        USBPT104_OFF
        USBPT104_PT100
        USBPT104_PT1000
        USBPT104_RESISTANCE_TO_375R
        USBPT104_RESISTANCE_TO_10K
        USBPT104_DIFFERENTIAL_TO_115MV
        USBPT104_DIFFERENTIAL_TO_2500MV
        USBPT104_SINGLE_ENDED_TO_115MV
        USBPT104_SINGLE_ENDED_TO_2500MV
        USBPT104_MAX_DATA_TYPES

    End Enum

    'IP_DETAILS_TYPE;
    Public Enum IpDetailsType As UInteger
        IDT_GET
        IDT_SET
    End Enum

    'COMMUNICATION_TYPE;
    Public Enum CommunicationType As UInteger
        CT_USB = &H1
        CT_ETHERNET = &H2
        CT_ALL = &HFFFFFFFFUI
    End Enum

#End Region

#Region "Driver Imports"

    Declare Function UsbPt104OpenUnit Lib "usbpt104.dll" (ByRef handle As Short, ByVal serial As String) As UInteger
    Declare Function UsbPt104OpenUnitViaIp Lib "usbpt104.dll" (ByRef handle As Short, ByVal serial As String, ByVal ipAddress As String) As UInteger
    Declare Function UsbPt104CloseUnit Lib "usbpt104.dll" (ByVal handle As Short) As UInteger
    Declare Function UsbPt104Enumerate Lib "usbpt104.dll" (ByVal details As String, ByRef length As UInteger, ByVal type As CommunicationType) As UInteger
    Declare Function UsbPt104GetUnitInfo Lib "usbpt104.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UInteger) As UInteger

    Declare Function UsbPt104SetChannel Lib "usbpt104.dll" (ByVal handle As Short, ByVal channel As UsbPt104Channels, ByVal measurementType As UsbPt104DataType, ByVal noOfWires As Short) As UInteger
    Declare Function UsbPt104SetMains Lib "usbpt104.dll" (ByVal handle As Short, ByVal sixty_hertz As UShort) As UInteger
    Declare Function UsbPt104GetValue Lib "usbpt104.dll" (ByVal handle As Short, ByVal channel As UsbPt104Channels, ByRef value As Integer, ByVal filtered As Short) As UInteger

    Declare Function UsbPt104IpDetails Lib "usbpt104.dll" (ByVal handle As Short, ByRef enabled As Short, ByVal ipAddress As String, ByRef length As UShort, ByRef listeningPort As UShort,
                                                           ByVal type As IpDetailsType) As UInteger

    Declare Sub Sleep Lib "kernel32.dll" (ByVal timeMs As Integer)

#End Region

End Module
