<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PLCM3Form
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
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.UnitInfoTextBox = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.UnitsLabel1 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.ProgressBar3 = New System.Windows.Forms.ProgressBar()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.UnitsLabel2 = New System.Windows.Forms.Label()
        Me.UnitsLabel3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.UsbButton = New System.Windows.Forms.RadioButton()
        Me.EthernetButton = New System.Windows.Forms.RadioButton()
        Me.IPAddrTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(96, 63)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 45)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Open Unit"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.ForeColor = System.Drawing.Color.White
        Me.ProgressBar1.Location = New System.Drawing.Point(214, 188)
        Me.ProgressBar1.MarqueeAnimationSpeed = 1
        Me.ProgressBar1.Maximum = 1000000
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(224, 20)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 1
        '
        'UnitInfoTextBox
        '
        Me.UnitInfoTextBox.Location = New System.Drawing.Point(326, 63)
        Me.UnitInfoTextBox.Multiline = True
        Me.UnitInfoTextBox.Name = "UnitInfoTextBox"
        Me.UnitInfoTextBox.Size = New System.Drawing.Size(196, 103)
        Me.UnitInfoTextBox.TabIndex = 2
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 500
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(423, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 15)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Unit Information"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(452, 188)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(70, 20)
        Me.TextBox1.TabIndex = 4
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Off", "1 mV/A", "10 mV/A", "100 mV/A", "Voltage"})
        Me.ComboBox1.Location = New System.Drawing.Point(96, 187)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox1.TabIndex = 5
        '
        'UnitsLabel1
        '
        Me.UnitsLabel1.AutoSize = True
        Me.UnitsLabel1.Location = New System.Drawing.Point(528, 195)
        Me.UnitsLabel1.Name = "UnitsLabel1"
        Me.UnitsLabel1.Size = New System.Drawing.Size(31, 13)
        Me.UnitsLabel1.TabIndex = 6
        Me.UnitsLabel1.Text = "Units"
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"Off", "1 mV/A", "10 mV/A", "100 mV/A", "Voltage"})
        Me.ComboBox2.Location = New System.Drawing.Point(96, 259)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox2.TabIndex = 7
        '
        'ProgressBar2
        '
        Me.ProgressBar2.ForeColor = System.Drawing.Color.White
        Me.ProgressBar2.Location = New System.Drawing.Point(214, 259)
        Me.ProgressBar2.MarqueeAnimationSpeed = 1
        Me.ProgressBar2.Maximum = 1000000
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(224, 20)
        Me.ProgressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar2.TabIndex = 8
        '
        'ProgressBar3
        '
        Me.ProgressBar3.ForeColor = System.Drawing.Color.White
        Me.ProgressBar3.Location = New System.Drawing.Point(214, 339)
        Me.ProgressBar3.MarqueeAnimationSpeed = 1
        Me.ProgressBar3.Maximum = 1000000
        Me.ProgressBar3.Name = "ProgressBar3"
        Me.ProgressBar3.Size = New System.Drawing.Size(224, 20)
        Me.ProgressBar3.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar3.TabIndex = 9
        '
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Items.AddRange(New Object() {"Off", "1 mV/A", "10 mV/A", "100 mV/A", "Voltage"})
        Me.ComboBox3.Location = New System.Drawing.Point(96, 339)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox3.TabIndex = 10
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(452, 260)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(70, 20)
        Me.TextBox2.TabIndex = 11
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(452, 340)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(70, 20)
        Me.TextBox3.TabIndex = 12
        '
        'UnitsLabel2
        '
        Me.UnitsLabel2.AutoSize = True
        Me.UnitsLabel2.Location = New System.Drawing.Point(528, 267)
        Me.UnitsLabel2.Name = "UnitsLabel2"
        Me.UnitsLabel2.Size = New System.Drawing.Size(31, 13)
        Me.UnitsLabel2.TabIndex = 13
        Me.UnitsLabel2.Text = "Units"
        '
        'UnitsLabel3
        '
        Me.UnitsLabel3.AutoSize = True
        Me.UnitsLabel3.Location = New System.Drawing.Point(528, 347)
        Me.UnitsLabel3.Name = "UnitsLabel3"
        Me.UnitsLabel3.Size = New System.Drawing.Size(31, 13)
        Me.UnitsLabel3.TabIndex = 14
        Me.UnitsLabel3.Text = "Units"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 193)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Channel 1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 267)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Channel 2"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 347)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Channel 3"
        '
        'UsbButton
        '
        Me.UsbButton.AutoSize = True
        Me.UsbButton.Location = New System.Drawing.Point(214, 63)
        Me.UsbButton.Name = "UsbButton"
        Me.UsbButton.Size = New System.Drawing.Size(47, 17)
        Me.UsbButton.TabIndex = 18
        Me.UsbButton.TabStop = True
        Me.UsbButton.Text = "USB"
        Me.UsbButton.UseVisualStyleBackColor = True
        '
        'EthernetButton
        '
        Me.EthernetButton.AutoSize = True
        Me.EthernetButton.Location = New System.Drawing.Point(214, 91)
        Me.EthernetButton.Name = "EthernetButton"
        Me.EthernetButton.Size = New System.Drawing.Size(65, 17)
        Me.EthernetButton.TabIndex = 19
        Me.EthernetButton.TabStop = True
        Me.EthernetButton.Text = "Ethernet"
        Me.EthernetButton.UseVisualStyleBackColor = True
        '
        'IPAddrTextBox
        '
        Me.IPAddrTextBox.Location = New System.Drawing.Point(217, 120)
        Me.IPAddrTextBox.Name = "IPAddrTextBox"
        Me.IPAddrTextBox.Size = New System.Drawing.Size(98, 20)
        Me.IPAddrTextBox.TabIndex = 20
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(214, 143)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "IP Address"
        '
        'PLCM3Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(615, 389)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.IPAddrTextBox)
        Me.Controls.Add(Me.EthernetButton)
        Me.Controls.Add(Me.UsbButton)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.UnitsLabel3)
        Me.Controls.Add(Me.UnitsLabel2)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.ProgressBar3)
        Me.Controls.Add(Me.ProgressBar2)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.UnitsLabel1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UnitInfoTextBox)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "PLCM3Form"
        Me.Text = "PicoLog CM3 Current Data Logger VB .NET GUI Example"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents UnitInfoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents UnitsLabel1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents ProgressBar2 As System.Windows.Forms.ProgressBar
    Friend WithEvents ProgressBar3 As System.Windows.Forms.ProgressBar
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents UnitsLabel2 As System.Windows.Forms.Label
    Friend WithEvents UnitsLabel3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents UsbButton As System.Windows.Forms.RadioButton
    Friend WithEvents EthernetButton As System.Windows.Forms.RadioButton
    Friend WithEvents IPAddrTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label

End Class
