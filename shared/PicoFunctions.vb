'========================================================================================
'	Filename:			PicoFunctions.vb
'
'	Description: 
'	This file defines functions that can be used across Pico Technology VB .NET examples.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Module PicoFunctions

    ' Common Values
    ' ============= 

    Public inputRanges() As Integer = New Integer(12) {10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000} ' ranges in mV


    ' ******************************************************************************************************************************************************************
    ' adcToMv - Converts from raw ADC values to mV values. The mV value returned depends upon the ADC count, and the voltage range set for the channel. 
    '
    ' Parameters - raw          : An integer holding the ADC count to be converted to mV
    '            - range        : A value indicating where in the 'inputRanges' array the range value can be found
    '            - maxADCValue  : maximum ADC count value  
    '
    ' Returns    - value converted into mV
    ' *******************************************************************************************************************************************************************

    Function adcToMv(ByVal raw As Integer, ByVal range As Integer, ByVal maxADCValue As Short)

        Dim mVVal As Single        ' Use this variable to force data to be returned as an integer

        mVVal = (CSng(raw) * inputRanges(range)) / maxADCValue

        Return mVVal

    End Function


    ' ******************************************************************************************************************************************************************
    ' mvToAdc - Converts from mV into ADC value. The ADC count returned depends upon the mV value, and the voltage range set for the channel. 
    '
    ' Parameters - mv           : A Short value holding the mv value to be converted to the ADC count
    '            - range        : A value indicating where in the 'inputRanges' array the range value can be found
    '            - maxADCValue  : maximum ADC count value
    '
    ' Returns    - value converted into an ADC count
    ' *******************************************************************************************************************************************************************
    Function mvToAdc(ByVal mv As Short, ByVal range As Integer, ByVal maxADCValue As Short) As Short

        Dim adcCount As Short

        adcCount = CShort((mv / inputRanges(range)) * maxADCValue)

        Return adcCount

    End Function

End Module
