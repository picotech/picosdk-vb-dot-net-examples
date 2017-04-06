'========================================================================================
'	Filename:			PicoInfo.vb
'
'	Description: 
'	This file defines the PICO_INFO values defined in the PicoStatus.h header file.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. All rights reserved.
'
'========================================================================================

Module PicoInfo

    Public Const PICO_DRIVER_VERSION = &H0
    Public Const PICO_USB_VERSION = &H1
    Public Const PICO_HARDWARE_VERSION = &H2
    Public Const PICO_VARIANT_INFO = &H3
    Public Const PICO_BATCH_AND_SERIAL = &H4
    Public Const PICO_CAL_DATE = &H5
    Public Const PICO_KERNEL_VERSION = &H6
    Public Const PICO_DIGITAL_HARDWARE_VERSION = &H7
    Public Const PICO_ANALOGUE_HARDWARE_VERSION = &H8
    Public Const PICO_FIRMWARE_VERSION_1 = &H9
    Public Const PICO_FIRMWARE_VERSION_2 = &HA
    Public Const PICO_MAC_ADDRESS = &HB

End Module
