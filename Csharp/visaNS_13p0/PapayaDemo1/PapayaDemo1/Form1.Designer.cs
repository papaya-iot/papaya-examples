/*
Copyright (C) 2020 Piek Solutions LLC
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


namespace PapayaDemo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ipAddrTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gpibAddrTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.responseTextBox = new System.Windows.Forms.TextBox();
            this.readButton = new System.Windows.Forms.Button();
            this.writeButton = new System.Windows.Forms.Button();
            this.queryButton = new System.Windows.Forms.Button();
            this.LockDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.timeOutTextBox = new System.Windows.Forms.TextBox();
            this.timeOutLabel = new System.Windows.Forms.Label();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.i2cAddrTextBox = new System.Windows.Forms.TextBox();
            this.i2cAddrLabel = new System.Windows.Forms.Label();
            this.i2cRegisterTextBox = new System.Windows.Forms.TextBox();
            this.i2cLabel = new System.Windows.Forms.Label();
            this.i2cNumOfBytesTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.gpibTab = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.i2cTab = new System.Windows.Forms.TabPage();
            this.RunI2CScript = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.i2cComboBox = new System.Windows.Forms.ComboBox();
            this.i2cRespTextBox = new System.Windows.Forms.TextBox();
            this.i2cQueryButton = new System.Windows.Forms.Button();
            this.i2cScanButton = new System.Windows.Forms.Button();
            this.i2cWriteButton = new System.Windows.Forms.Button();
            this.uartTab = new System.Windows.Forms.TabPage();
            this.getUartConfig = new System.Windows.Forms.Button();
            this.uartReadButton = new System.Windows.Forms.Button();
            this.uartRespTextBox = new System.Windows.Forms.TextBox();
            this.uartQueryButton = new System.Windows.Forms.Button();
            this.newLineCheckBox1 = new System.Windows.Forms.CheckBox();
            this.uartWriteButton = new System.Windows.Forms.Button();
            this.uartComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.byteTimeoutComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.msgTimeoutComboBox = new System.Windows.Forms.ComboBox();
            this.setUartConfigButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.uartDataRateComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uartNumBitComboBox = new System.Windows.Forms.ComboBox();
            this.uartStpBitComboBox = new System.Windows.Forms.ComboBox();
            this.uartParityComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.binDecode = new System.Windows.Forms.CheckBox();
            this.swapBinOrder = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.gpibTab.SuspendLayout();
            this.i2cTab.SuspendLayout();
            this.uartTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipAddrTextBox
            // 
            this.ipAddrTextBox.Location = new System.Drawing.Point(95, 16);
            this.ipAddrTextBox.Name = "ipAddrTextBox";
            this.ipAddrTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ipAddrTextBox.Size = new System.Drawing.Size(93, 20);
            this.ipAddrTextBox.TabIndex = 0;
            this.ipAddrTextBox.Text = "192.168.2.54";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "GPIB Address";
            // 
            // gpibAddrTextBox
            // 
            this.gpibAddrTextBox.Location = new System.Drawing.Point(95, 43);
            this.gpibAddrTextBox.Name = "gpibAddrTextBox";
            this.gpibAddrTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gpibAddrTextBox.Size = new System.Drawing.Size(45, 20);
            this.gpibAddrTextBox.TabIndex = 3;
            this.gpibAddrTextBox.Text = "28";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "GPIB Command";
            // 
            // responseTextBox
            // 
            this.responseTextBox.AcceptsReturn = true;
            this.responseTextBox.Location = new System.Drawing.Point(3, 76);
            this.responseTextBox.Multiline = true;
            this.responseTextBox.Name = "responseTextBox";
            this.responseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseTextBox.Size = new System.Drawing.Size(439, 186);
            this.responseTextBox.TabIndex = 6;
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(94, 40);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(83, 30);
            this.readButton.TabIndex = 7;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // writeButton
            // 
            this.writeButton.Location = new System.Drawing.Point(214, 40);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(83, 30);
            this.writeButton.TabIndex = 8;
            this.writeButton.Text = "Write";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // queryButton
            // 
            this.queryButton.Location = new System.Drawing.Point(330, 40);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(83, 30);
            this.queryButton.TabIndex = 9;
            this.queryButton.Text = "Query";
            this.queryButton.UseVisualStyleBackColor = true;
            this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
            // 
            // LockDeviceCheckBox
            // 
            this.LockDeviceCheckBox.AutoSize = true;
            this.LockDeviceCheckBox.Location = new System.Drawing.Point(231, 12);
            this.LockDeviceCheckBox.Name = "LockDeviceCheckBox";
            this.LockDeviceCheckBox.Size = new System.Drawing.Size(87, 17);
            this.LockDeviceCheckBox.TabIndex = 11;
            this.LockDeviceCheckBox.Text = "Lock Device";
            this.LockDeviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // timeOutTextBox
            // 
            this.timeOutTextBox.Location = new System.Drawing.Point(354, 29);
            this.timeOutTextBox.Name = "timeOutTextBox";
            this.timeOutTextBox.Size = new System.Drawing.Size(69, 20);
            this.timeOutTextBox.TabIndex = 12;
            this.timeOutTextBox.Text = "2000";
            // 
            // timeOutLabel
            // 
            this.timeOutLabel.AutoSize = true;
            this.timeOutLabel.Location = new System.Drawing.Point(228, 32);
            this.timeOutLabel.Name = "timeOutLabel";
            this.timeOutLabel.Size = new System.Drawing.Size(102, 13);
            this.timeOutLabel.TabIndex = 13;
            this.timeOutLabel.Text = "Lock TimeOut [ ms ]";
            // 
            // commandComboBox
            // 
            this.commandComboBox.FormattingEnabled = true;
            this.commandComboBox.Location = new System.Drawing.Point(94, 13);
            this.commandComboBox.Name = "commandComboBox";
            this.commandComboBox.Size = new System.Drawing.Size(327, 21);
            this.commandComboBox.TabIndex = 14;
            // 
            // i2cAddrTextBox
            // 
            this.i2cAddrTextBox.Location = new System.Drawing.Point(65, 20);
            this.i2cAddrTextBox.Name = "i2cAddrTextBox";
            this.i2cAddrTextBox.Size = new System.Drawing.Size(52, 20);
            this.i2cAddrTextBox.TabIndex = 15;
            this.i2cAddrTextBox.Text = "/77";
            // 
            // i2cAddrLabel
            // 
            this.i2cAddrLabel.AutoSize = true;
            this.i2cAddrLabel.Location = new System.Drawing.Point(11, 23);
            this.i2cAddrLabel.Name = "i2cAddrLabel";
            this.i2cAddrLabel.Size = new System.Drawing.Size(48, 13);
            this.i2cAddrLabel.TabIndex = 16;
            this.i2cAddrLabel.Text = "I2C Addr";
            // 
            // i2cRegisterTextBox
            // 
            this.i2cRegisterTextBox.Location = new System.Drawing.Point(219, 20);
            this.i2cRegisterTextBox.Name = "i2cRegisterTextBox";
            this.i2cRegisterTextBox.Size = new System.Drawing.Size(56, 20);
            this.i2cRegisterTextBox.TabIndex = 17;
            this.i2cRegisterTextBox.Text = "/F2";
            // 
            // i2cLabel
            // 
            this.i2cLabel.AutoSize = true;
            this.i2cLabel.Location = new System.Drawing.Point(123, 23);
            this.i2cLabel.Name = "i2cLabel";
            this.i2cLabel.Size = new System.Drawing.Size(90, 13);
            this.i2cLabel.TabIndex = 18;
            this.i2cLabel.Text = "I2C Register Addr";
            // 
            // i2cNumOfBytesTextBox
            // 
            this.i2cNumOfBytesTextBox.Location = new System.Drawing.Point(347, 20);
            this.i2cNumOfBytesTextBox.Name = "i2cNumOfBytesTextBox";
            this.i2cNumOfBytesTextBox.Size = new System.Drawing.Size(56, 20);
            this.i2cNumOfBytesTextBox.TabIndex = 19;
            this.i2cNumOfBytesTextBox.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(299, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "# bytes";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.gpibTab);
            this.tabControl1.Controls.Add(this.i2cTab);
            this.tabControl1.Controls.Add(this.uartTab);
            this.tabControl1.Location = new System.Drawing.Point(-3, 69);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 330);
            this.tabControl1.TabIndex = 21;
            // 
            // gpibTab
            // 
            this.gpibTab.BackColor = System.Drawing.SystemColors.Control;
            this.gpibTab.Controls.Add(this.label4);
            this.gpibTab.Controls.Add(this.responseTextBox);
            this.gpibTab.Controls.Add(this.commandComboBox);
            this.gpibTab.Controls.Add(this.label3);
            this.gpibTab.Controls.Add(this.readButton);
            this.gpibTab.Controls.Add(this.writeButton);
            this.gpibTab.Controls.Add(this.queryButton);
            this.gpibTab.Location = new System.Drawing.Point(4, 22);
            this.gpibTab.Name = "gpibTab";
            this.gpibTab.Padding = new System.Windows.Forms.Padding(3);
            this.gpibTab.Size = new System.Drawing.Size(448, 304);
            this.gpibTab.TabIndex = 0;
            this.gpibTab.Text = "GPIB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Response";
            // 
            // i2cTab
            // 
            this.i2cTab.BackColor = System.Drawing.SystemColors.Control;
            this.i2cTab.Controls.Add(this.RunI2CScript);
            this.i2cTab.Controls.Add(this.label6);
            this.i2cTab.Controls.Add(this.i2cComboBox);
            this.i2cTab.Controls.Add(this.i2cRespTextBox);
            this.i2cTab.Controls.Add(this.i2cQueryButton);
            this.i2cTab.Controls.Add(this.i2cScanButton);
            this.i2cTab.Controls.Add(this.i2cWriteButton);
            this.i2cTab.Controls.Add(this.i2cAddrLabel);
            this.i2cTab.Controls.Add(this.label5);
            this.i2cTab.Controls.Add(this.i2cAddrTextBox);
            this.i2cTab.Controls.Add(this.i2cNumOfBytesTextBox);
            this.i2cTab.Controls.Add(this.i2cRegisterTextBox);
            this.i2cTab.Controls.Add(this.i2cLabel);
            this.i2cTab.Location = new System.Drawing.Point(4, 22);
            this.i2cTab.Name = "i2cTab";
            this.i2cTab.Padding = new System.Windows.Forms.Padding(3);
            this.i2cTab.Size = new System.Drawing.Size(448, 304);
            this.i2cTab.TabIndex = 1;
            this.i2cTab.Text = "I2C";
            // 
            // RunI2CScript
            // 
            this.RunI2CScript.Location = new System.Drawing.Point(14, 275);
            this.RunI2CScript.Name = "RunI2CScript";
            this.RunI2CScript.Size = new System.Drawing.Size(85, 23);
            this.RunI2CScript.TabIndex = 28;
            this.RunI2CScript.Text = "RunScriptFile";
            this.RunI2CScript.UseVisualStyleBackColor = true;
            this.RunI2CScript.Click += new System.EventHandler(this.RunI2CScript_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(123, 278);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(241, 16);
            this.label6.TabIndex = 27;
            this.label6.Text = "Addr, Reg, data1,data2, .... in hex format";
            // 
            // i2cComboBox
            // 
            this.i2cComboBox.FormattingEnabled = true;
            this.i2cComboBox.Location = new System.Drawing.Point(6, 85);
            this.i2cComboBox.Name = "i2cComboBox";
            this.i2cComboBox.Size = new System.Drawing.Size(399, 21);
            this.i2cComboBox.TabIndex = 25;
            this.i2cComboBox.Text = "/01";
            // 
            // i2cRespTextBox
            // 
            this.i2cRespTextBox.AcceptsReturn = true;
            this.i2cRespTextBox.Location = new System.Drawing.Point(0, 127);
            this.i2cRespTextBox.Multiline = true;
            this.i2cRespTextBox.Name = "i2cRespTextBox";
            this.i2cRespTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.i2cRespTextBox.Size = new System.Drawing.Size(448, 135);
            this.i2cRespTextBox.TabIndex = 24;
            // 
            // i2cQueryButton
            // 
            this.i2cQueryButton.Location = new System.Drawing.Point(333, 46);
            this.i2cQueryButton.Name = "i2cQueryButton";
            this.i2cQueryButton.Size = new System.Drawing.Size(70, 33);
            this.i2cQueryButton.TabIndex = 23;
            this.i2cQueryButton.Text = "I2C Query";
            this.i2cQueryButton.UseVisualStyleBackColor = true;
            this.i2cQueryButton.Click += new System.EventHandler(this.i2cQueryButton_Click);
            // 
            // i2cScanButton
            // 
            this.i2cScanButton.Location = new System.Drawing.Point(205, 46);
            this.i2cScanButton.Name = "i2cScanButton";
            this.i2cScanButton.Size = new System.Drawing.Size(70, 33);
            this.i2cScanButton.TabIndex = 22;
            this.i2cScanButton.Text = "I2C Scan";
            this.i2cScanButton.UseVisualStyleBackColor = true;
            this.i2cScanButton.Click += new System.EventHandler(this.i2cScanButton_Click);
            // 
            // i2cWriteButton
            // 
            this.i2cWriteButton.Location = new System.Drawing.Point(65, 46);
            this.i2cWriteButton.Name = "i2cWriteButton";
            this.i2cWriteButton.Size = new System.Drawing.Size(70, 33);
            this.i2cWriteButton.TabIndex = 21;
            this.i2cWriteButton.Text = "I2C Write";
            this.i2cWriteButton.UseVisualStyleBackColor = true;
            this.i2cWriteButton.Click += new System.EventHandler(this.i2cWriteButton_Click);
            // 
            // uartTab
            // 
            this.uartTab.BackColor = System.Drawing.SystemColors.Control;
            this.uartTab.Controls.Add(this.getUartConfig);
            this.uartTab.Controls.Add(this.uartReadButton);
            this.uartTab.Controls.Add(this.uartRespTextBox);
            this.uartTab.Controls.Add(this.uartQueryButton);
            this.uartTab.Controls.Add(this.newLineCheckBox1);
            this.uartTab.Controls.Add(this.uartWriteButton);
            this.uartTab.Controls.Add(this.uartComboBox);
            this.uartTab.Controls.Add(this.label13);
            this.uartTab.Controls.Add(this.label14);
            this.uartTab.Controls.Add(this.byteTimeoutComboBox);
            this.uartTab.Controls.Add(this.label12);
            this.uartTab.Controls.Add(this.label11);
            this.uartTab.Controls.Add(this.msgTimeoutComboBox);
            this.uartTab.Controls.Add(this.setUartConfigButton);
            this.uartTab.Controls.Add(this.label10);
            this.uartTab.Controls.Add(this.uartDataRateComboBox);
            this.uartTab.Controls.Add(this.label9);
            this.uartTab.Controls.Add(this.uartNumBitComboBox);
            this.uartTab.Controls.Add(this.uartStpBitComboBox);
            this.uartTab.Controls.Add(this.uartParityComboBox);
            this.uartTab.Controls.Add(this.label8);
            this.uartTab.Controls.Add(this.label7);
            this.uartTab.Location = new System.Drawing.Point(4, 22);
            this.uartTab.Name = "uartTab";
            this.uartTab.Size = new System.Drawing.Size(448, 304);
            this.uartTab.TabIndex = 2;
            this.uartTab.Text = "UART";
            // 
            // getUartConfig
            // 
            this.getUartConfig.Location = new System.Drawing.Point(3, 72);
            this.getUartConfig.Name = "getUartConfig";
            this.getUartConfig.Size = new System.Drawing.Size(97, 22);
            this.getUartConfig.TabIndex = 32;
            this.getUartConfig.Text = "GetUartConfig";
            this.getUartConfig.UseVisualStyleBackColor = true;
            this.getUartConfig.Click += new System.EventHandler(this.getUartConfig_Click);
            // 
            // uartReadButton
            // 
            this.uartReadButton.Location = new System.Drawing.Point(90, 99);
            this.uartReadButton.Name = "uartReadButton";
            this.uartReadButton.Size = new System.Drawing.Size(75, 23);
            this.uartReadButton.TabIndex = 31;
            this.uartReadButton.Text = "Read";
            this.uartReadButton.UseVisualStyleBackColor = true;
            this.uartReadButton.Click += new System.EventHandler(this.uartReadButton_Click);
            // 
            // uartRespTextBox
            // 
            this.uartRespTextBox.AcceptsReturn = true;
            this.uartRespTextBox.Location = new System.Drawing.Point(3, 166);
            this.uartRespTextBox.Multiline = true;
            this.uartRespTextBox.Name = "uartRespTextBox";
            this.uartRespTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uartRespTextBox.Size = new System.Drawing.Size(448, 135);
            this.uartRespTextBox.TabIndex = 30;
            // 
            // uartQueryButton
            // 
            this.uartQueryButton.Location = new System.Drawing.Point(173, 99);
            this.uartQueryButton.Name = "uartQueryButton";
            this.uartQueryButton.Size = new System.Drawing.Size(75, 23);
            this.uartQueryButton.TabIndex = 29;
            this.uartQueryButton.Text = "Query";
            this.uartQueryButton.UseVisualStyleBackColor = true;
            this.uartQueryButton.Click += new System.EventHandler(this.uartQueryButton_Click);
            // 
            // newLineCheckBox1
            // 
            this.newLineCheckBox1.AutoSize = true;
            this.newLineCheckBox1.Checked = true;
            this.newLineCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.newLineCheckBox1.Location = new System.Drawing.Point(357, 135);
            this.newLineCheckBox1.Name = "newLineCheckBox1";
            this.newLineCheckBox1.Size = new System.Drawing.Size(87, 17);
            this.newLineCheckBox1.TabIndex = 28;
            this.newLineCheckBox1.Text = "AddNewLine";
            this.newLineCheckBox1.UseVisualStyleBackColor = true;
            // 
            // uartWriteButton
            // 
            this.uartWriteButton.Location = new System.Drawing.Point(9, 99);
            this.uartWriteButton.Name = "uartWriteButton";
            this.uartWriteButton.Size = new System.Drawing.Size(75, 23);
            this.uartWriteButton.TabIndex = 27;
            this.uartWriteButton.Text = "Write";
            this.uartWriteButton.UseVisualStyleBackColor = true;
            this.uartWriteButton.Click += new System.EventHandler(this.uartWriteButton_Click);
            // 
            // uartComboBox
            // 
            this.uartComboBox.FormattingEnabled = true;
            this.uartComboBox.Location = new System.Drawing.Point(14, 135);
            this.uartComboBox.Name = "uartComboBox";
            this.uartComboBox.Size = new System.Drawing.Size(328, 21);
            this.uartComboBox.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(260, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "us";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(106, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "byte read timeout";
            // 
            // byteTimeoutComboBox
            // 
            this.byteTimeoutComboBox.FormattingEnabled = true;
            this.byteTimeoutComboBox.Location = new System.Drawing.Point(194, 56);
            this.byteTimeoutComboBox.Name = "byteTimeoutComboBox";
            this.byteTimeoutComboBox.Size = new System.Drawing.Size(63, 21);
            this.byteTimeoutComboBox.TabIndex = 16;
            this.byteTimeoutComboBox.Text = "100000";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(427, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "ms";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(281, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "message timeout";
            // 
            // msgTimeoutComboBox
            // 
            this.msgTimeoutComboBox.FormattingEnabled = true;
            this.msgTimeoutComboBox.Location = new System.Drawing.Point(371, 57);
            this.msgTimeoutComboBox.Name = "msgTimeoutComboBox";
            this.msgTimeoutComboBox.Size = new System.Drawing.Size(54, 21);
            this.msgTimeoutComboBox.TabIndex = 13;
            this.msgTimeoutComboBox.Text = "5000";
            // 
            // setUartConfigButton
            // 
            this.setUartConfigButton.Location = new System.Drawing.Point(3, 44);
            this.setUartConfigButton.Name = "setUartConfigButton";
            this.setUartConfigButton.Size = new System.Drawing.Size(97, 22);
            this.setUartConfigButton.TabIndex = 12;
            this.setUartConfigButton.Text = "SetUartConfig";
            this.setUartConfigButton.UseVisualStyleBackColor = true;
            this.setUartConfigButton.Click += new System.EventHandler(this.setUartConfigButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "baudrate";
            // 
            // uartDataRateComboBox
            // 
            this.uartDataRateComboBox.FormattingEnabled = true;
            this.uartDataRateComboBox.Items.AddRange(new object[] {
            "    2400",
            "    4800",
            "    9600",
            "   19200",
            "   38400",
            "   57600",
            " 115200",
            " 230400",
            " 460800",
            " 500000",
            " 576000",
            " 921600",
            "1000000",
            "1152000"});
            this.uartDataRateComboBox.Location = new System.Drawing.Point(64, 19);
            this.uartDataRateComboBox.Name = "uartDataRateComboBox";
            this.uartDataRateComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uartDataRateComboBox.Size = new System.Drawing.Size(59, 21);
            this.uartDataRateComboBox.TabIndex = 10;
            this.uartDataRateComboBox.Text = "9600";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(130, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "# bits";
            // 
            // uartNumBitComboBox
            // 
            this.uartNumBitComboBox.FormattingEnabled = true;
            this.uartNumBitComboBox.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.uartNumBitComboBox.Location = new System.Drawing.Point(169, 19);
            this.uartNumBitComboBox.Name = "uartNumBitComboBox";
            this.uartNumBitComboBox.Size = new System.Drawing.Size(50, 21);
            this.uartNumBitComboBox.TabIndex = 8;
            this.uartNumBitComboBox.Text = "8";
            // 
            // uartStpBitComboBox
            // 
            this.uartStpBitComboBox.FormattingEnabled = true;
            this.uartStpBitComboBox.Items.AddRange(new object[] {
            "1",
            "2"});
            this.uartStpBitComboBox.Location = new System.Drawing.Point(381, 19);
            this.uartStpBitComboBox.Name = "uartStpBitComboBox";
            this.uartStpBitComboBox.Size = new System.Drawing.Size(58, 21);
            this.uartStpBitComboBox.TabIndex = 7;
            this.uartStpBitComboBox.Text = "1";
            // 
            // uartParityComboBox
            // 
            this.uartParityComboBox.FormattingEnabled = true;
            this.uartParityComboBox.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.uartParityComboBox.Location = new System.Drawing.Point(263, 19);
            this.uartParityComboBox.Name = "uartParityComboBox";
            this.uartParityComboBox.Size = new System.Drawing.Size(60, 21);
            this.uartParityComboBox.TabIndex = 6;
            this.uartParityComboBox.Text = "None";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(225, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "parity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(329, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "stop bits";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(351, 53);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(72, 32);
            this.ClearButton.TabIndex = 22;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // binDecode
            // 
            this.binDecode.AutoSize = true;
            this.binDecode.Location = new System.Drawing.Point(231, 47);
            this.binDecode.Name = "binDecode";
            this.binDecode.Size = new System.Drawing.Size(112, 17);
            this.binDecode.TabIndex = 23;
            this.binDecode.Text = "488.2 Bin Decode";
            this.binDecode.UseVisualStyleBackColor = true;
            // 
            // swapBinOrder
            // 
            this.swapBinOrder.AutoSize = true;
            this.swapBinOrder.Location = new System.Drawing.Point(231, 66);
            this.swapBinOrder.Name = "swapBinOrder";
            this.swapBinOrder.Size = new System.Drawing.Size(100, 17);
            this.swapBinOrder.TabIndex = 24;
            this.swapBinOrder.Text = "Swap Bin Order";
            this.swapBinOrder.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 397);
            this.Controls.Add(this.swapBinOrder);
            this.Controls.Add(this.binDecode);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.timeOutLabel);
            this.Controls.Add(this.timeOutTextBox);
            this.Controls.Add(this.LockDeviceCheckBox);
            this.Controls.Add(this.gpibAddrTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipAddrTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "PapayaDemo";
            this.tabControl1.ResumeLayout(false);
            this.gpibTab.ResumeLayout(false);
            this.gpibTab.PerformLayout();
            this.i2cTab.ResumeLayout(false);
            this.i2cTab.PerformLayout();
            this.uartTab.ResumeLayout(false);
            this.uartTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipAddrTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gpibAddrTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox responseTextBox;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Button queryButton;
        private System.Windows.Forms.CheckBox LockDeviceCheckBox;
        private System.Windows.Forms.TextBox timeOutTextBox;
        private System.Windows.Forms.Label timeOutLabel;
        private System.Windows.Forms.ComboBox commandComboBox;
        private System.Windows.Forms.TextBox i2cAddrTextBox;
        private System.Windows.Forms.Label i2cAddrLabel;
        private System.Windows.Forms.TextBox i2cRegisterTextBox;
        private System.Windows.Forms.Label i2cLabel;
        private System.Windows.Forms.TextBox i2cNumOfBytesTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage gpibTab;
        private System.Windows.Forms.TabPage i2cTab;
        private System.Windows.Forms.TabPage uartTab;
        private System.Windows.Forms.Button i2cQueryButton;
        private System.Windows.Forms.Button i2cScanButton;
        private System.Windows.Forms.Button i2cWriteButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox i2cRespTextBox;
        private System.Windows.Forms.ComboBox i2cComboBox;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button RunI2CScript;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox uartParityComboBox;
        private System.Windows.Forms.ComboBox uartStpBitComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox uartNumBitComboBox;
        private System.Windows.Forms.ComboBox uartDataRateComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox byteTimeoutComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox msgTimeoutComboBox;
        private System.Windows.Forms.Button setUartConfigButton;
        private System.Windows.Forms.ComboBox uartComboBox;
        private System.Windows.Forms.Button uartWriteButton;
        private System.Windows.Forms.CheckBox newLineCheckBox1;
        private System.Windows.Forms.Button uartQueryButton;
        private System.Windows.Forms.TextBox uartRespTextBox;
        private System.Windows.Forms.Button uartReadButton;
        private System.Windows.Forms.Button getUartConfig;
        private System.Windows.Forms.CheckBox binDecode;
        private System.Windows.Forms.CheckBox swapBinOrder;
    }
}

