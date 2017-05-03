<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PS4000ASigGen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.start = New System.Windows.Forms.Button()
        Me.Inputpanel = New System.Windows.Forms.Panel()
        Me.awg_label2 = New System.Windows.Forms.TextBox()
        Me.Update_button = New System.Windows.Forms.Button()
        Me.siggen_label = New System.Windows.Forms.Label()
        Me.awg_label = New System.Windows.Forms.Label()
        Me.file_name = New System.Windows.Forms.TextBox()
        Me.signal_type = New System.Windows.Forms.ComboBox()
        Me.pk_pk = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.offset = New System.Windows.Forms.TextBox()
        Me.start_freq = New System.Windows.Forms.TextBox()
        Me.Sweep_control = New System.Windows.Forms.Panel()
        Me.increment_text = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.time_incre = New System.Windows.Forms.TextBox()
        Me.freq_incre = New System.Windows.Forms.TextBox()
        Me.stop_freq = New System.Windows.Forms.TextBox()
        Me.sweep_type = New System.Windows.Forms.ComboBox()
        Me.Sweep = New System.Windows.Forms.CheckBox()
        Me.SIGtoAWG = New System.Windows.Forms.CheckBox()
        Me.Inputpanel.SuspendLayout()
        Me.Sweep_control.SuspendLayout()
        Me.SuspendLayout()
        '
        'start
        '
        Me.start.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!)
        Me.start.Location = New System.Drawing.Point(198, 182)
        Me.start.Name = "start"
        Me.start.Size = New System.Drawing.Size(280, 139)
        Me.start.TabIndex = 0
        Me.start.Text = "Start"
        Me.start.UseVisualStyleBackColor = True
        '
        'Inputpanel
        '
        Me.Inputpanel.Controls.Add(Me.awg_label2)
        Me.Inputpanel.Controls.Add(Me.Update_button)
        Me.Inputpanel.Controls.Add(Me.siggen_label)
        Me.Inputpanel.Controls.Add(Me.awg_label)
        Me.Inputpanel.Controls.Add(Me.file_name)
        Me.Inputpanel.Controls.Add(Me.signal_type)
        Me.Inputpanel.Controls.Add(Me.pk_pk)
        Me.Inputpanel.Controls.Add(Me.Label1)
        Me.Inputpanel.Controls.Add(Me.Label4)
        Me.Inputpanel.Controls.Add(Me.Label2)
        Me.Inputpanel.Controls.Add(Me.offset)
        Me.Inputpanel.Controls.Add(Me.start_freq)
        Me.Inputpanel.Controls.Add(Me.Sweep_control)
        Me.Inputpanel.Controls.Add(Me.Sweep)
        Me.Inputpanel.Controls.Add(Me.SIGtoAWG)
        Me.Inputpanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Inputpanel.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Inputpanel.Location = New System.Drawing.Point(0, 0)
        Me.Inputpanel.Name = "Inputpanel"
        Me.Inputpanel.Size = New System.Drawing.Size(695, 501)
        Me.Inputpanel.TabIndex = 1
        Me.Inputpanel.Visible = False
        '
        'awg_label2
        '
        Me.awg_label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.awg_label2.Location = New System.Drawing.Point(484, 294)
        Me.awg_label2.Multiline = True
        Me.awg_label2.Name = "awg_label2"
        Me.awg_label2.ReadOnly = True
        Me.awg_label2.Size = New System.Drawing.Size(166, 144)
        Me.awg_label2.TabIndex = 27
        Me.awg_label2.Text = "Please make sure that the file only has one value per line, in the range -32768 t" & _
    "o +32767"
        Me.awg_label2.Visible = False
        '
        'Update_button
        '
        Me.Update_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Update_button.Location = New System.Drawing.Point(542, 452)
        Me.Update_button.Name = "Update_button"
        Me.Update_button.Size = New System.Drawing.Size(108, 37)
        Me.Update_button.TabIndex = 26
        Me.Update_button.Text = "Update"
        Me.Update_button.UseVisualStyleBackColor = True
        '
        'siggen_label
        '
        Me.siggen_label.AutoSize = True
        Me.siggen_label.Location = New System.Drawing.Point(255, 9)
        Me.siggen_label.Name = "siggen_label"
        Me.siggen_label.Size = New System.Drawing.Size(117, 25)
        Me.siggen_label.TabIndex = 25
        Me.siggen_label.Text = "Signal Type"
        '
        'awg_label
        '
        Me.awg_label.AutoSize = True
        Me.awg_label.Location = New System.Drawing.Point(4, 9)
        Me.awg_label.Name = "awg_label"
        Me.awg_label.Size = New System.Drawing.Size(471, 25)
        Me.awg_label.TabIndex = 24
        Me.awg_label.Text = "Please enter filename (don't forget the .txt extension):"
        Me.awg_label.Visible = False
        '
        'file_name
        '
        Me.file_name.Location = New System.Drawing.Point(56, 37)
        Me.file_name.Name = "file_name"
        Me.file_name.Size = New System.Drawing.Size(295, 30)
        Me.file_name.TabIndex = 23
        Me.file_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.file_name.Visible = False
        '
        'signal_type
        '
        Me.signal_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.signal_type.FormattingEnabled = True
        Me.signal_type.Items.AddRange(New Object() {"Sine", "Square", "Triangle", "Ramp Up", "Ramp Down", "Sin(x)/x", "Gaussian", "Half Sine", "DC", "White Noise", "PRBS"})
        Me.signal_type.Location = New System.Drawing.Point(245, 37)
        Me.signal_type.Name = "signal_type"
        Me.signal_type.Size = New System.Drawing.Size(121, 33)
        Me.signal_type.TabIndex = 22
        '
        'pk_pk
        '
        Me.pk_pk.Location = New System.Drawing.Point(266, 210)
        Me.pk_pk.Name = "pk_pk"
        Me.pk_pk.Size = New System.Drawing.Size(100, 30)
        Me.pk_pk.TabIndex = 21
        Me.pk_pk.Text = "2000"
        Me.pk_pk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(467, 182)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 25)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Offset (mV)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(261, 182)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 25)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "PktoPk (mV)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Label2.Location = New System.Drawing.Point(15, 182)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(194, 25)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Start Frequency (Hz)"
        '
        'offset
        '
        Me.offset.Location = New System.Drawing.Point(472, 210)
        Me.offset.Name = "offset"
        Me.offset.Size = New System.Drawing.Size(100, 30)
        Me.offset.TabIndex = 17
        Me.offset.Text = "0"
        Me.offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'start_freq
        '
        Me.start_freq.Location = New System.Drawing.Point(18, 210)
        Me.start_freq.Name = "start_freq"
        Me.start_freq.Size = New System.Drawing.Size(100, 30)
        Me.start_freq.TabIndex = 16
        Me.start_freq.Text = "1000"
        Me.start_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Sweep_control
        '
        Me.Sweep_control.Controls.Add(Me.increment_text)
        Me.Sweep_control.Controls.Add(Me.Label7)
        Me.Sweep_control.Controls.Add(Me.Label6)
        Me.Sweep_control.Controls.Add(Me.Label5)
        Me.Sweep_control.Controls.Add(Me.time_incre)
        Me.Sweep_control.Controls.Add(Me.freq_incre)
        Me.Sweep_control.Controls.Add(Me.stop_freq)
        Me.Sweep_control.Controls.Add(Me.sweep_type)
        Me.Sweep_control.Location = New System.Drawing.Point(12, 347)
        Me.Sweep_control.Name = "Sweep_control"
        Me.Sweep_control.Size = New System.Drawing.Size(372, 142)
        Me.Sweep_control.TabIndex = 12
        Me.Sweep_control.Visible = False
        '
        'increment_text
        '
        Me.increment_text.AutoSize = True
        Me.increment_text.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.increment_text.Location = New System.Drawing.Point(1, 100)
        Me.increment_text.Name = "increment_text"
        Me.increment_text.Size = New System.Drawing.Size(244, 25)
        Me.increment_text.TabIndex = 7
        Me.increment_text.Text = "Increment Time Interval (s)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Label7.Location = New System.Drawing.Point(3, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(194, 25)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Stop Frequency (Hz)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Label6.Location = New System.Drawing.Point(1, 70)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(196, 25)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Frequency Increment"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Label5.Location = New System.Drawing.Point(3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(123, 25)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Sweep Type"
        '
        'time_incre
        '
        Me.time_incre.Location = New System.Drawing.Point(248, 98)
        Me.time_incre.Name = "time_incre"
        Me.time_incre.Size = New System.Drawing.Size(100, 30)
        Me.time_incre.TabIndex = 3
        Me.time_incre.Text = "10"
        Me.time_incre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'freq_incre
        '
        Me.freq_incre.Location = New System.Drawing.Point(248, 68)
        Me.freq_incre.Name = "freq_incre"
        Me.freq_incre.Size = New System.Drawing.Size(100, 30)
        Me.freq_incre.TabIndex = 2
        Me.freq_incre.Text = "10"
        Me.freq_incre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'stop_freq
        '
        Me.stop_freq.Location = New System.Drawing.Point(248, 38)
        Me.stop_freq.Name = "stop_freq"
        Me.stop_freq.Size = New System.Drawing.Size(100, 30)
        Me.stop_freq.TabIndex = 1
        Me.stop_freq.Text = "2000"
        Me.stop_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'sweep_type
        '
        Me.sweep_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sweep_type.FormattingEnabled = True
        Me.sweep_type.Items.AddRange(New Object() {"Up", "Down", "Up Down", "Down Up"})
        Me.sweep_type.Location = New System.Drawing.Point(248, 6)
        Me.sweep_type.Name = "sweep_type"
        Me.sweep_type.Size = New System.Drawing.Size(121, 33)
        Me.sweep_type.TabIndex = 0
        '
        'Sweep
        '
        Me.Sweep.AutoSize = True
        Me.Sweep.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.Sweep.Location = New System.Drawing.Point(12, 313)
        Me.Sweep.Name = "Sweep"
        Me.Sweep.Size = New System.Drawing.Size(147, 29)
        Me.Sweep.TabIndex = 11
        Me.Sweep.Text = "Sweep Mode"
        Me.Sweep.UseVisualStyleBackColor = True
        '
        'SIGtoAWG
        '
        Me.SIGtoAWG.AutoSize = True
        Me.SIGtoAWG.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!)
        Me.SIGtoAWG.Location = New System.Drawing.Point(472, 37)
        Me.SIGtoAWG.Name = "SIGtoAWG"
        Me.SIGtoAWG.Size = New System.Drawing.Size(80, 29)
        Me.SIGtoAWG.TabIndex = 1
        Me.SIGtoAWG.Text = "AWG"
        Me.SIGtoAWG.UseVisualStyleBackColor = True
        '
        'PS4000ASigGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 501)
        Me.Controls.Add(Me.Inputpanel)
        Me.Controls.Add(Me.start)
        Me.Name = "PS4000ASigGen"
        Me.Text = "PicoScope 4000 Series (ps4000a) AWG and Signal Generator Example"
        Me.Inputpanel.ResumeLayout(False)
        Me.Inputpanel.PerformLayout()
        Me.Sweep_control.ResumeLayout(False)
        Me.Sweep_control.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents start As System.Windows.Forms.Button
    Friend WithEvents Inputpanel As System.Windows.Forms.Panel
    Friend WithEvents SIGtoAWG As System.Windows.Forms.CheckBox
    Friend WithEvents signal_type As System.Windows.Forms.ComboBox
    Friend WithEvents pk_pk As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents offset As System.Windows.Forms.TextBox
    Friend WithEvents start_freq As System.Windows.Forms.TextBox
    Friend WithEvents Sweep_control As System.Windows.Forms.Panel
    Friend WithEvents increment_text As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents time_incre As System.Windows.Forms.TextBox
    Friend WithEvents freq_incre As System.Windows.Forms.TextBox
    Friend WithEvents stop_freq As System.Windows.Forms.TextBox
    Friend WithEvents sweep_type As System.Windows.Forms.ComboBox
    Friend WithEvents Sweep As System.Windows.Forms.CheckBox
    Friend WithEvents awg_label As System.Windows.Forms.Label
    Friend WithEvents file_name As System.Windows.Forms.TextBox
    Friend WithEvents siggen_label As System.Windows.Forms.Label
    Friend WithEvents awg_label2 As System.Windows.Forms.TextBox
    Friend WithEvents Update_button As System.Windows.Forms.Button

End Class
