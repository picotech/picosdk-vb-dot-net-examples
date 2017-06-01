<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PS2000BlockVBGui
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
        Me.openButton = New System.Windows.Forms.Button()
        Me.closeButton = New System.Windows.Forms.Button()
        Me.unitInfoTextBox = New System.Windows.Forms.TextBox()
        Me.getBlockDataButton = New System.Windows.Forms.Button()
        Me.unitInfoLabel = New System.Windows.Forms.Label()
        Me.timebaseLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.timebaseNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.numSamplesNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.samplingIntervalLabel = New System.Windows.Forms.Label()
        Me.samplingIntervalTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.numSamplesCollectedTextBox = New System.Windows.Forms.TextBox()
        Me.dataTextBox = New System.Windows.Forms.TextBox()
        Me.dataLabel = New System.Windows.Forms.Label()
        CType(Me.timebaseNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'openButton
        '
        Me.openButton.Location = New System.Drawing.Point(12, 12)
        Me.openButton.Name = "openButton"
        Me.openButton.Size = New System.Drawing.Size(75, 23)
        Me.openButton.TabIndex = 0
        Me.openButton.Text = "Open"
        Me.openButton.UseVisualStyleBackColor = True
        '
        'closeButton
        '
        Me.closeButton.Enabled = False
        Me.closeButton.Location = New System.Drawing.Point(107, 12)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(75, 23)
        Me.closeButton.TabIndex = 1
        Me.closeButton.Text = "Close"
        Me.closeButton.UseVisualStyleBackColor = True
        '
        'unitInfoTextBox
        '
        Me.unitInfoTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.unitInfoTextBox.Location = New System.Drawing.Point(12, 68)
        Me.unitInfoTextBox.Multiline = True
        Me.unitInfoTextBox.Name = "unitInfoTextBox"
        Me.unitInfoTextBox.ReadOnly = True
        Me.unitInfoTextBox.Size = New System.Drawing.Size(170, 139)
        Me.unitInfoTextBox.TabIndex = 2
        '
        'getBlockDataButton
        '
        Me.getBlockDataButton.Enabled = False
        Me.getBlockDataButton.Location = New System.Drawing.Point(233, 138)
        Me.getBlockDataButton.Name = "getBlockDataButton"
        Me.getBlockDataButton.Size = New System.Drawing.Size(104, 23)
        Me.getBlockDataButton.TabIndex = 3
        Me.getBlockDataButton.Text = "Get Block Data"
        Me.getBlockDataButton.UseVisualStyleBackColor = True
        '
        'unitInfoLabel
        '
        Me.unitInfoLabel.AutoSize = True
        Me.unitInfoLabel.Location = New System.Drawing.Point(12, 52)
        Me.unitInfoLabel.Name = "unitInfoLabel"
        Me.unitInfoLabel.Size = New System.Drawing.Size(50, 13)
        Me.unitInfoLabel.TabIndex = 4
        Me.unitInfoLabel.Text = "Unit Info:"
        '
        'timebaseLabel
        '
        Me.timebaseLabel.AutoSize = True
        Me.timebaseLabel.Location = New System.Drawing.Point(202, 70)
        Me.timebaseLabel.Name = "timebaseLabel"
        Me.timebaseLabel.Size = New System.Drawing.Size(56, 13)
        Me.timebaseLabel.TabIndex = 5
        Me.timebaseLabel.Text = "Timebase:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(202, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Number of samples:"
        '
        'timebaseNumericUpDown
        '
        Me.timebaseNumericUpDown.Location = New System.Drawing.Point(329, 68)
        Me.timebaseNumericUpDown.Maximum = New Decimal(New Integer() {18, 0, 0, 0})
        Me.timebaseNumericUpDown.Name = "timebaseNumericUpDown"
        Me.timebaseNumericUpDown.Size = New System.Drawing.Size(66, 20)
        Me.timebaseNumericUpDown.TabIndex = 7
        Me.timebaseNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'numSamplesNumericUpDown
        '
        Me.numSamplesNumericUpDown.Location = New System.Drawing.Point(329, 98)
        Me.numSamplesNumericUpDown.Maximum = New Decimal(New Integer() {8064, 0, 0, 0})
        Me.numSamplesNumericUpDown.Name = "numSamplesNumericUpDown"
        Me.numSamplesNumericUpDown.Size = New System.Drawing.Size(66, 20)
        Me.numSamplesNumericUpDown.TabIndex = 8
        Me.numSamplesNumericUpDown.Value = New Decimal(New Integer() {2000, 0, 0, 0})
        '
        'samplingIntervalLabel
        '
        Me.samplingIntervalLabel.AutoSize = True
        Me.samplingIntervalLabel.Location = New System.Drawing.Point(202, 177)
        Me.samplingIntervalLabel.Name = "samplingIntervalLabel"
        Me.samplingIntervalLabel.Size = New System.Drawing.Size(90, 13)
        Me.samplingIntervalLabel.TabIndex = 9
        Me.samplingIntervalLabel.Text = "Sampling interval:"
        '
        'samplingIntervalTextBox
        '
        Me.samplingIntervalTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.samplingIntervalTextBox.Location = New System.Drawing.Point(329, 174)
        Me.samplingIntervalTextBox.Name = "samplingIntervalTextBox"
        Me.samplingIntervalTextBox.ReadOnly = True
        Me.samplingIntervalTextBox.Size = New System.Drawing.Size(66, 20)
        Me.samplingIntervalTextBox.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(202, 210)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Num. samples collected:"
        '
        'numSamplesCollectedTextBox
        '
        Me.numSamplesCollectedTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.numSamplesCollectedTextBox.Location = New System.Drawing.Point(329, 207)
        Me.numSamplesCollectedTextBox.Name = "numSamplesCollectedTextBox"
        Me.numSamplesCollectedTextBox.ReadOnly = True
        Me.numSamplesCollectedTextBox.Size = New System.Drawing.Size(66, 20)
        Me.numSamplesCollectedTextBox.TabIndex = 12
        '
        'dataTextBox
        '
        Me.dataTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.dataTextBox.Location = New System.Drawing.Point(425, 70)
        Me.dataTextBox.Multiline = True
        Me.dataTextBox.Name = "dataTextBox"
        Me.dataTextBox.ReadOnly = True
        Me.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dataTextBox.Size = New System.Drawing.Size(193, 137)
        Me.dataTextBox.TabIndex = 13
        '
        'dataLabel
        '
        Me.dataLabel.AutoSize = True
        Me.dataLabel.Location = New System.Drawing.Point(425, 52)
        Me.dataLabel.Name = "dataLabel"
        Me.dataLabel.Size = New System.Drawing.Size(33, 13)
        Me.dataLabel.TabIndex = 14
        Me.dataLabel.Text = "Data:"
        '
        'PS2000BlockVBGui
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 261)
        Me.Controls.Add(Me.dataLabel)
        Me.Controls.Add(Me.dataTextBox)
        Me.Controls.Add(Me.numSamplesCollectedTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.samplingIntervalTextBox)
        Me.Controls.Add(Me.samplingIntervalLabel)
        Me.Controls.Add(Me.numSamplesNumericUpDown)
        Me.Controls.Add(Me.timebaseNumericUpDown)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.timebaseLabel)
        Me.Controls.Add(Me.unitInfoLabel)
        Me.Controls.Add(Me.getBlockDataButton)
        Me.Controls.Add(Me.unitInfoTextBox)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.openButton)
        Me.Name = "PS2000BlockVBGui"
        Me.Text = "PicoScope 2000 Series Block GUI Capture Example"
        CType(Me.timebaseNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents openButton As Button
    Friend WithEvents closeButton As Button
    Friend WithEvents unitInfoTextBox As TextBox
    Friend WithEvents getBlockDataButton As Button
    Friend WithEvents unitInfoLabel As Label
    Friend WithEvents timebaseLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents timebaseNumericUpDown As NumericUpDown
    Friend WithEvents numSamplesNumericUpDown As NumericUpDown
    Friend WithEvents samplingIntervalLabel As Label
    Friend WithEvents samplingIntervalTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents numSamplesCollectedTextBox As TextBox
    Friend WithEvents dataTextBox As TextBox
    Friend WithEvents dataLabel As Label
End Class
