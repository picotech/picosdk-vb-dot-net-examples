'===================================================================================================
'
'	Filename:			PS4000ASigGen.vb
'
'	Description: 
'	    This file demonstrates how to use VB .NET on the PicoScope 4000 series oscilloscopes using 
'       the ps4000a driver in order to control the Arbitrary Waveform/Built-in Signal Generator.
'       The application shows how to output a signal from the signal generator and how to load in 
'       a file containing a defined waveform and output it.
'
'   Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO

Public Class PS4000ASigGen

    Dim _handle As Short
    Dim status As Short
    Dim awgBufferSize As UInteger

    'starts up device and grabs relavent information
    Private Sub start_Click(sender As Object, e As EventArgs) Handles start.Click
        Dim str As String = "     "
        Dim requiredSize As Integer

        status = ps4000aOpenUnit(_handle, vbNullString)

        If _handle = 0 Then
            MessageBox.Show("Cannot open device" & vbNewLine & "error code" & status.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End If

        If status <> 0 Then
            status = ps4000aChangePowerSource(_handle, status)
        End If

        status = ps4000aGetUnitInfo(_handle, str, 255, requiredSize, 3)

        Inputpanel.Visible = True

        '4824 has a AWG buffer size of 16Ks whixch 16 * 1024 = 16384
        awgBufferSize = 16384
        signal_type.SelectedIndex = 0
        sweep_type.SelectedIndex = 0
    End Sub

    'If dc or white noise sweep is not enable so hides button
    Private Sub signal_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles signal_type.SelectedIndexChanged
        If signal_type.SelectedIndex >= 8 Then
            Sweep.Visible = False
            Sweep.Checked = False
        Else
            Sweep.Visible = True
        End If
    End Sub

    'if check box is true then the control to set the sweep are visible
    Private Sub Sweep_CheckedChanged(sender As Object, e As EventArgs) Handles Sweep.CheckedChanged
        Sweep_control.Visible = Sweep.Checked
    End Sub

    'if system has AWG then the choices availble will depend on the ticked box
    Private Sub SIGtoAWG_CheckedChanged(sender As Object, e As EventArgs) Handles SIGtoAWG.CheckedChanged
        increment_text.Text = ""

        awg_label.Visible = SIGtoAWG.Checked
        awg_label2.Visible = SIGtoAWG.Checked
        file_name.Visible = SIGtoAWG.Checked


        'flips true and false whenever system is clicked
        siggen_label.Visible = Not siggen_label.Visible
        signal_type.Visible = Not signal_type.Visible

        If SIGtoAWG.Checked Then
            increment_text.Text = "Time increment (X*12.5ns)"
        Else
            increment_text.Text = "Increment Time Interval (s)"
        End If

    End Sub

    'depending if user wants awg or in built sig gen the diffrent points will turn up
    Private Sub Update_button_Click(sender As Object, e As EventArgs) Handles Update_button.Click

        Dim sweeptype As SweepType
        Dim operation As ExtraOperations = ExtraOperations.PS4000A_ES_OFF
        Dim shots As UInteger = 0
        Dim sweeps As UInteger = 0
        Dim triggertype As SigGenTrigType = SigGenTrigType.PS4000A_SIGGEN_RISING
        Dim triggersource As SigGenTrigSource = SigGenTrigSource.PS4000A_SIGGEN_NONE
        Dim extinthreshold As Short = 0
        Dim startfreq As Double
        Dim stopfreq As Double
        Dim increment As Double
        Dim dwelltime As Double
        Dim offsetvoltage As Integer
        Dim pktopk As UInteger

        'if variables are correct then can use function
        Try
            pktopk = UInt32.Parse(pk_pk.Text) * 1000
            offsetvoltage = Int32.Parse(offset.Text) * 1000
            startfreq = Double.Parse(start_freq.Text)
            If startfreq > 20000000 Or startfreq < 0.03 Then
                Throw New System.Exception
            End If
        Catch ex As Exception

            MessageBox.Show("Invalid for Start Frequency, PktoPk and/or Off Set Value", "Error Signal Generator Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

        If Sweep.Checked Then
            Try
                dwelltime = Single.Parse(time_incre.Text)
                increment = Double.Parse(freq_incre.Text)
                stopfreq = Double.Parse(stop_freq.Text)
                If stopfreq > 20000000 Or stopfreq < startfreq Then

                    Throw New System.Exception

                End If
            Catch ex As Exception

                MessageBox.Show("Incorrect sweep parameter", "Error Signal Generator Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End Try
        Else
            stopfreq = startfreq
            increment = 0
            dwelltime = 0
            sweeptype = sweeptype.PS4000A_UP
        End If


        If SIGtoAWG.Checked Then
            'if the tick box is tru then the arbitrary waveform is dimension will be used 
            Dim file As StreamReader
            Dim waveformsize As Integer = 0
            Dim arbitarywaveform(49151) As Short
            Dim startdelta As UInteger
            Dim stopdelta As UInteger
            Dim deltaphase_increment As UInteger
            Dim index As IndexMode = IndexMode.PS4000A_SINGLE


            'making sure file exists
            Try
                file = New StreamReader(file_name.Text)
            Catch ex As Exception
                MessageBox.Show("Filename does not exsit try again", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            'read all the line to the information from the file upto the maximum awg bufffer size
            Do Until file.EndOfStream Or waveformsize = awgBufferSize

                arbitarywaveform(waveformsize) = Convert.ToInt16(file.ReadLine)
                waveformsize += 1

            Loop


            file.Close()
            Array.Resize(arbitarywaveform, waveformsize) 'resizing array so no extra points

            'need to use detal phase rather than frequncy value as frequency will depend on the number of points
            startdelta = ((startfreq * waveformsize) / awgBufferSize) * (AWG_PHASE_ACCUMULATOR * 1.0 / AWG_DAC_FREQUENCY)
            stopdelta = ((stopfreq * waveformsize) / awgBufferSize) * (AWG_PHASE_ACCUMULATOR * 1.0 / AWG_DAC_FREQUENCY)
            deltaphase_increment = ((increment * waveformsize) / awgBufferSize) * (AWG_PHASE_ACCUMULATOR * 1.0 / AWG_DAC_FREQUENCY)

            status = ps4000aSetSigGenArbitrary(_handle,
                                      offsetvoltage,
                                      pktopk,
                                      startdelta,
                                      stopdelta,
                                      deltaphase_increment,
                                      dwelltime,
                                      arbitarywaveform(0),
                                      waveformsize,
                                      sweeptype,
                                      operation,
                                      index,
                                      shots,
                                      sweeps,
                                      triggertype,
                                      triggersource,
                                      extinthreshold)


        Else


            Dim wavetype As WaveType

            If (wavetype = signal_type.SelectedIndex) <= 8 Then

                'if dc the pktopk must be zero
                If signal_type.SelectedIndex = 8 Then

                    pktopk = 0
                End If


            Else

                operation = (signal_type.SelectedIndex - 8)

            End If

            status = ps4000aSetSigGenBuiltIn(_handle,
                                    offsetvoltage,
                                    pktopk,
                                    wavetype,
                                    startfreq,
                                    stopfreq,
                                    increment,
                                    dwelltime,
                                    sweeptype,
                                    operation,
                                    shots,
                                    sweeps,
                                    triggertype,
                                    triggersource,
                                    extinthreshold)
        End If

    End Sub

    'Whenever the form closes the device will be closed also
    Private Sub AWG_SIGGEN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ps4000aCloseUnit(_handle)
    End Sub

End Class
