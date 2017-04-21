'===================================================================================================
'
'	Filename:			USBTC08Imports.vb
'
'	Description: 
'	This file defines enumerations and functions and structures from the usbtc08.h C header file.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'===================================================================================================

Module USBTC08Imports

    ' Enumerations
    ' ============

    Enum USBTC08Channels
        USBTC08_CHANNEL_CJC = 0
        USBTC08_CHANNEL_1 = 1
        USBTC08_CHANNEL_2 = 2
        USBTC08_CHANNEL_3 = 3
        USBTC08_CHANNEL_4 = 4
        USBTC08_CHANNEL_5 = 5
        USBTC08_CHANNEL_6 = 6
        USBTC08_CHANNEL_7 = 7
        USBTC08_CHANNEL_8 = 8
    End Enum

    Enum USBTC08MainsFrequency
        USBTC08_REJECT_50HZ = 0
        USBTC08_REJECT_60HZ = 1
    End Enum

    Enum USBTC08TempUnits
        USBTC08_UNITS_CENTIGRADE = 0
        USBTC08_UNITS_FAHRENHEIT = 1
        USBTC08_UNITS_KELVIN = 2
        USBTC08_UNITS_RANKINE = 3
    End Enum

    ' Functions
    ' =========

    Declare Function usb_tc08_set_channel Lib "usbtc08.dll" (ByVal handle As Short, ByVal channel As USBTC08Channels, ByVal tc_type As Char) As Short

    Declare Function usb_tc08_run Lib "usbtc08.dll" (ByVal handle As Short, ByVal interval_ms As Integer) As Integer

    Declare Function usb_tc08_get_temp Lib "usbtc08.dll" (ByVal handle As Short, ByRef temp_buffer As Single, ByRef times_ms_buffer As Integer, ByVal buffer_length As Integer, ByRef overflow As Short, ByVal channel As Short, ByVal units As Short, ByVal fill_missing As Short) As Integer

    Declare Function usb_tc08_get_temp_deskew Lib "usbtc08.dll" (ByVal handle As Short, ByRef temp_buffer As Single, ByRef times_ms_buffer As Integer, ByVal buffer_length As Integer, ByRef overflow As Short, ByVal channel As Short, ByVal units As Short, ByVal fill_missing As Short) As Integer

    Declare Function usb_tc08_get_single Lib "usbtc08.dll" (ByVal handle As Short, ByRef temp As Single, ByRef overflow_flags As Short, ByVal units As USBTC08TempUnits) As Short

    Declare Function usb_tc08_open_unit Lib "usbtc08.dll" () As Short

    Declare Function usb_tc08_close_unit Lib "usbtc08.dll" (ByVal handle As Short) As Short

    Declare Function usb_tc08_stop Lib "usbtc08.dll" (ByVal handle As Short) As Short

    Declare Function usb_tc08_set_mains Lib "usbtc08.dll" (ByVal handle As Short, ByVal sixty_hertz As USBTC08MainsFrequency) As Short

    Declare Function usb_tc08_get_minimum_interval_ms Lib "usbtc08.dll" (ByVal handle As Short) As Integer

    Declare Function usb_tc08_get_formatted_info Lib "usbtc08.dll" (ByVal handle As Short, ByVal unit_info As String, ByVal string_length As Short) As Short

    Declare Function usb_tc08_get_unit_info2 Lib "usbtc08.dll" (ByVal handle As Short, ByVal unit_info As String, ByVal string_length As Short, ByVal line As Short) As Short

    Declare Function usb_tc08_get_last_error Lib "usbtc08.dll" (ByVal handle As Short) As Short

End Module
