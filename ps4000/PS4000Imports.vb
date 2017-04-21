'===================================================================================================
'	
'   Filename: PS4000Imports.vb
'
'	Description: 
'	This file defines enumerations, functions and structures from the ps4000.h C header file.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Imports System.Runtime.InteropServices

Public Class PS4000Imports

#Region "Driver Enums"

    ' Enumerations
    ' ============

    Enum MODEL_TYPE
        MODEL_NONE = 0
        MODEL_PS4223 = 4223
        MODEL_PS4224 = 4224
        MODEL_PS4423 = 4423
        MODEL_PS4424 = 4424
        MODEL_PS4226 = 4226
        MODEL_PS4227 = 4227
        MODEL_PS4262 = 4262
    End Enum


    Enum VoltageRange
        PS4000_10MV
        PS4000_20MV
        PS4000_50MV
        PS4000_100MV
        PS4000_200MV
        PS4000_500MV
        PS4000_1V
        PS4000_2V
        PS4000_5V
        PS4000_10V
        PS4000_20V
        PS4000_50V
        PS4000_100V
    End Enum


    Enum Channel
        PS4000_CHANNEL_A
        PS4000_CHANNEL_B
        PS4000_CHANNEL_C
        PS4000_CHANNEL_D
        PS4000_EXTERNAL
    End Enum

    Enum ThresholdMode
        LEVEL
        WINDOW
    End Enum

    Enum TriggerState
        CONDITION_DONT_CARE
        CONDITION_TRUE
        CONDITION_FALSE
        CONDITION_MAX
    End Enum


    Enum LevelTrig
        'Above=0, Below=1, Rising / None=2, Falling=3, Rising_Or_Falling=4, Above_Lower=5, Below_Lower=6, Rising_Lower=7, Falling_Lower=8,
        ABOVE
        BELOW
        RISING
        NONE = RISING
        FALLING
        RISING_OR_FALLING
        ABOVE_LOWER
        BELOW_LOWER
        RISING_LOWER
        FALLING_LOWER
    End Enum

    Enum UnitInfo
        DRIVER_VERSION
        USB_VERSION
        HARDWARE_VERSION
        VARIANT_INFO
        BATCH_AND_SERIAL
        CAL_DATE
        KERNEL_VERSION
        DIGITAL_HARDWARE_VERSION
        ANALOGUE_HARDWARE_VERSION
        FIRMWARE_VERSION_1
        FIRMWARE_VERSION_2
        MAC_ADDRESS
    End Enum

#End Region

#Region "Driver Structures"

    ' Structures
    ' ==========

    Structure TRIGGER_CHANNEL_PROPERTIES
        Dim thresholdUpper As Short
        Dim thresholdUpperHysteresis As UShort
        Dim thresholdLower As Short
        Dim thresholdLowerHysteresis As UShort
        Dim Channel As Channel
        Dim thresholdMode As ThresholdMode
    End Structure

    Structure TRIGGER_CONDITIONS
        Dim channelA As TriggerState
        Dim channelB As TriggerState
        Dim channelC As TriggerState
        Dim channelD As TriggerState
        Dim external As TriggerState
        Dim aux As TriggerState
        Dim pwq As TriggerState
    End Structure

#End Region

#Region "Class Structures"

    Structure ChannelSettings
        Dim DCCoupled As Boolean
        Dim range As VoltageRange
        Dim enabled As Boolean
    End Structure

    Structure UNIT_MODEL
        Dim handle As Short
        Dim model As MODEL_TYPE
        Dim firstRange As VoltageRange
        Dim lastRange As VoltageRange
        Dim SigGen As Boolean
        Dim channelCount As Short
        Dim channelSettings() As ChannelSettings
        Dim maxADCValue As Short
    End Structure

    Structure TRIG_SETTINGS
        Dim ChanTrig() As Boolean
        Dim NumberTrigs As Short
        'Dim TrigType As TrigLogic
        Dim TrigVoltage As Short
        Dim TrigHist As Short
    End Structure

#End Region

#Region "Constants"

    Private Const _DRIVER_FILENAME = "ps4000.dll"

    ' Constants
    ' =========

    Public Const PICO_OK = 0                       ' PICO_OK returned when API call succeeds
    Public Const PS4262_MAX_VALUE = 32767          ' MAX ADC value used when converting between ADC values and mV for the PicoScope 4262
    Public Const PS4000_MAX_VALUE = 32764          ' MAX ADC value used when converting between ADC values and mV for all other PicoScope 4000 Series models

    Public Shared inputRanges() As Integer = New Integer(12) {10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000} ' ranges in mV

#End Region

#Region "Driver Imports"

#Region "Driver Delegates"
    ' Delegate declaration
    ' --------------------

    ' Block mode
    Public Delegate Sub ps4000BlockReady(handle As Short, status As UInteger, pVoid As IntPtr)

#End Region

    ' Function definitions
    ' ====================

    Declare Function ps4000OpenUnit Lib "ps4000.dll" (ByRef handle As Short) As UInteger
    Declare Sub ps4000CloseUnit Lib "ps4000.dll" (ByVal handle As Short)

    Declare Function ps4000GetUnitInfo Lib "ps4000.dll" (ByVal handle As Short, ByVal str As String, ByVal strLength As Short, ByRef size As Short, ByVal info As UnitInfo) As UInteger
    Declare Function ps4000SetChannel Lib "ps4000.dll" (ByVal handle As Short, ByVal channel As Channel, ByVal enabled As Short, ByVal dc As Short, ByVal range As VoltageRange) As UInteger
    Declare Function ps4000GetTimebase Lib "ps4000.dll" (ByVal handle As Short, ByVal timebase As UInteger, ByVal noSamples As Integer, ByRef timeInterval As Integer, ByVal oversample As Short,
                                                         ByRef maxSamples As Integer, ByVal segment As UShort) As UInteger

    Declare Function ps4000RunBlock Lib "ps4000.dll" (ByVal handle As Short, ByVal noPreTriggerSamples As Integer, ByVal noPostTriggerSamples As Integer, ByVal timebase As UInteger,
                                                        ByVal oversample As Short, ByRef timeIndisposed As Integer, ByVal segmentIndex As UShort, ByVal lpps4000BlockReady As ps4000BlockReady, pParam As IntPtr) As UInteger

    ' Declaration below does not work with .NET Framework 3.5
    'Declare Function ps4000SetDataBuffer Lib "ps4000.dll" (ByVal handle As Short, ByVal channel As Channel, ByRef buffer As Short, ByVal length As Integer) As UInteger

    <DllImport(_DRIVER_FILENAME, EntryPoint:="ps4000SetDataBuffer")>
    Public Shared Function ps4000SetDataBuffer(handle As Short, channel As Channel, buffer As Short(), bufferLth As Integer) As UInteger
    End Function


    Declare Function ps4000GetValues Lib "ps4000.dll" (ByVal handle As Short, ByVal startIndex As UInteger, ByRef numSamples As UInteger, ByVal downSampleRatio As UInteger, ByVal downsampleRatioMode As Short,
                                                       ByVal segmentIndex As UShort, ByRef overflow As Short) As UInteger

    Declare Function ps4000Stop Lib "ps4000.dll" (ByVal handle As Short) As UInteger

    ' Trigger Functions
    ' -----------------

    Declare Function ps4000SetSimpleTrigger Lib "ps4000.dll" (ByVal handle As Short, ByVal enable As Short, ByVal source As Channel, ByVal threshold As Short, ByVal direction As LevelTrig, ByVal delay As UInteger,
                                                              ByVal autoTriggerMs As Short) As UInteger

    Declare Function ps4000SetTriggerChannelDirections Lib "ps4000.dll" (ByVal handle As Short, ByVal channelA As LevelTrig, ByVal channelB As LevelTrig, ByVal channelC As LevelTrig, ByVal channelD As LevelTrig,
                                                                         ByVal ext As LevelTrig, ByVal aux As LevelTrig) As UInteger

    Declare Function ps4000SetTriggerChannelProperties Lib "ps4000.dll" (ByVal handle As Short, ByRef channelProperties As TRIGGER_CHANNEL_PROPERTIES, ByVal nChannelProperties As Short,
                                                                         ByVal auxOutputEnable As Short, ByVal autoTrigMs As Integer) As UInteger

    Declare Function ps4000SetTriggerChannelConditions Lib "ps4000.dll" (ByVal handle As Short, ByRef conditions As TRIGGER_CONDITIONS, ByVal nConditions As Short) As UInteger

#End Region

End Class