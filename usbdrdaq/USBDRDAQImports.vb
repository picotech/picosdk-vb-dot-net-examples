'===================================================================================================
'	
' Filename: USBDrDAQImports.vb
'
'	Description: 
'	This file defines enumerations and functions from the UsbDrDAQApi.h C header file.
'
' Copyright (C) 2016-2019 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Module USBDrDAQImports

  ' Enumerations
  ' ============

  Enum UsbDrDaqInputs
    USB_DRDAQ_CHANNEL_EXT1 = 1          'Ext. sensor 1
    USB_DRDAQ_CHANNEL_EXT2              'Ext. sensor 2
    USB_DRDAQ_CHANNEL_EXT3              'Ext. sensor 3
    USB_DRDAQ_CHANNEL_SCOPE             'Scope channel
    USB_DRDAQ_CHANNEL_PH                'PH
    USB_DRDAQ_CHANNEL_RES               'Resistance
    USB_DRDAQ_CHANNEL_LIGHT             'Light
    USB_DRDAQ_CHANNEL_TEMP              'Thermistor
    USB_DRDAQ_CHANNEL_MIC_WAVE          'Microphone waveform
    USB_DRDAQ_CHANNEL_MIC_LEVEL         'Microphone level
    USB_DRDAQ_MAX_CHANNELS = USB_DRDAQ_CHANNEL_MIC_LEVEL
  End Enum

  Enum UsbDrDaqScopeRange
    USB_DRDAQ_1V25
    USB_DRDAQ_2V5
    USB_DRDAQ_5V
    USB_DRDAQ_10V
  End Enum

  Enum UsbDrDaqWave
    USB_DRDAQ_SINE
    USB_DRDAQ_SQUARE
    USB_DRDAQ_TRIANGLE
    USB_DRDAQ_RAMP_UP
    USB_DRDAQ_RAMP_DOWN
    USB_DRDAQ_DC
  End Enum

  Enum UsbDrDaqGPIO
    USB_DRDAQ_GPIO_1 = 1
    USB_DRDAQ_GPIO_2
    USB_DRDAQ_GPIO_3
    USB_DRDAQ_GPIO_4
  End Enum

  Enum UsbDrDaqInfo
    USBDrDAQ_DRIVER_VERSION
    USBDrDAQ_USB_VERSION
    USBDrDAQ_HARDWARE_VERSION
    USBDrDAQ_VARIANT_INFO
    USBDrDAQ_BATCH_AND_SERIAL
    USBDrDAQ_CAL_DATE
    USBDrDAQ_KERNEL_DRIVER_VERSION
    USBDrDAQ_ERROR
    USBDrDAQ_SETTINGS
    USBDrDAQ_FIRMWARE_VERSION
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

  Declare Function UsbDrDaqOpenUnit Lib "usbdrdaq.dll" (ByRef handle As Short) As UInteger
  Declare Function UsbDrDaqCloseUnit Lib "usbdrdaq.dll" (ByVal handle As Short) As UInteger
  Declare Function UsbDrDaqSetInterval Lib "usbdrdaq.dll" (ByVal handle As Short, ByRef us_for_block As UInteger, ByVal ideal_no_of_samples As UInteger, ByRef channels As UsbDrDaqInputs, ByVal no_of_channels As Short) As UInteger
  Declare Function UsbDrDaqGetUnitInfo Lib "usbdrdaq.dll" (ByVal handle As Short, ByVal str As String, ByVal stringLength As Short, ByRef requiredSize As Short, ByVal info As UsbDrDaqInfo) As UInteger
  Declare Function UsbDrDaqSetDO Lib "usbdrdaq.dll" (ByVal handle As Short, ByVal IOChannel As UsbDrDaqGPIO, ByVal value As Short) As UInteger

  ' Data Collection Functions
  ' -------------------------

  Declare Function UsbDrDaqRun Lib "usbdrdaq.dll" (ByVal handle As Short, ByVal no_of_values As UInteger, ByVal method As BlockMethod) As UInteger
  Declare Function UsbDrDaqGetValues Lib "usbdrdaq.dll" (ByVal handle As Short, ByRef values As Short, ByRef noOfValues As UInteger, ByRef overflow As Short, ByRef triggerIndex As UInteger) As UInteger
  Declare Function UsbDrDaqGetValuesF Lib "usbdrdaq.dll" (ByVal handle As Short, ByRef values As Single, ByRef noOfValues As UInteger, ByRef overflow As Short, ByRef triggerIndex As UInteger) As UInteger
  Declare Function UsbDrDaqReady Lib "usbdrdaq.dll" (ByVal handle As Short, ByRef ready As Short) As UInteger
  Declare Function UsbDrDaqStop Lib "usbdrdaq.dll" (ByVal handle As Short) As UInteger

  ' Signal Generator Function
  ' --------------------------

  Declare Function UsbDrDaqSetSigGenBuiltIn Lib "usbdrdaq.dll" (ByVal handle As Integer, ByVal offsetVoltage As Integer, ByVal pkToPk As UInteger, ByVal frequency As Integer, ByVal waveType As UsbDrDaqWave) As UInteger

  ' Windows Kernel Function
  ' -----------------------

  Declare Sub Sleep Lib "kernel32.dll" (ByVal time As UInteger)

End Module