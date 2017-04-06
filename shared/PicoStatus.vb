'========================================================================================
'	Filename:			PicoStatus.vb
'
'	Description: 
'	This file defines the PICO_STATUS codes defined in the PicoStatus.h header file.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. All rights reserved.
'
'========================================================================================

Module PicoStatus

    Public Const PICO_OK = &H0                  ' The PicoScope is functioning correctly.
    Public Const PICO_NOT_FOUND = &H3           ' No Pico Technology device could be found.
    Public Const PICO_MAX_UNITS_OPENED = &H1    ' An attempt has been made to open more than <API>_MAX_UNITS.
    Public Const PICO_MEMORY_FAIL = &H2         ' Not enough memory could be allocated on the host machine.
    Public Const PICO_INVALID_HANDLE = &HC      ' There is no device with the handle value passed.
    Public Const PICO_INVALID_PARAMETER = &HD   ' A parameter value is not valid.
    Public Const PICO_INVALID_TIMEBASE = &HE    ' The timebase is not supported or is invalid.


    Public Const PICO_BUSY = &H27               ' The device is busy so data cannot be returned yet.

    Public Const PICO_CANCELLED = &H3A          ' A block collection has been cancelled.

    Public Const PICO_POWER_SUPPLY_CONNECTED = &H119        ' 4-channel scopes only: The DC power supply is connected.
    Public Const PICO_POWER_SUPPLY_NOT_CONNECTED = &H11A    ' 4-channel scopes only: The DC power supply is not connected.
    Public Const PICO_POWER_SUPPLY_REQUEST_INVALID = &H11B  ' Incorrect power mode passed for current power source.
    Public Const PICO_POWER_SUPPLY_UNDERVOLTAGE = &H11C     ' The supply voltage from the USB source is too low.
    Public Const PICO_CAPTURING_DATA = &H11D                ' The oscilloscope is in the process of capturing data.
    Public Const PICO_USB3_0_DEVICE_NON_USB3_0_PORT = &H11E ' A USB 3.0 device is connected to a non-USB 3.0 port.

    Public Const PICO_NOT_SUPPORTED_BY_THIS_DEVICE = &H11F  ' A function has been called that is not supported by the current device.

    Public Const PICO_INVALID_DEVICE_RESOLUTION = &H120                 ' The device resolution is invalid (out of range).
    Public Const PICO_INVALID_NUMBER_CHANNELS_FOR_RESOLUTION = &H121    ' The number of channels that can be enabled is limited in 15 and 16-bit modes. (Flexible Resolution Oscilloscopes only)
    Public Const PICO_CHANNEL_DISABLED_DUE_TO_USB_POWERED = &H122       ' USB power not sufficient for all requested channels.

End Module
