/*
Copyright (C) 2020 Piek Solutions LLC
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NationalInstruments.Visa;
using System.IO;
using System.Text.RegularExpressions;

/*
 * C# driver using National instrument Visa to communicate to the
 * Papaya GPIB Controller.
 * The codes below use:
 * National Instrument NI-VISA version 19.0
 * 
 * 
 * The code is released under GNU GPLv3 
 * 
 */

namespace PapayaDemo
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();

            
        }

        private void queryButton_Click(object sender, EventArgs e)
        {
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst" + gpibAddrTextBox.Text;
            string cmd = commandComboBox.Text;


            Console.WriteLine(cmd);
            VXI11Class remote_inst = null;// new VXI11Class(gpibAddr, ipstr);
            //string resp = string.Empty;
            byte[] resp  = null;
            try
            {
                remote_inst = new VXI11Class(gpibAddr, ipstr);
                /* if (LockDeviceCheckBox.Checked)
                {
                    // device lock is requested, min 1000 ms is chosen for demo
                    // 
                    if (Convert.ToInt32(timeOutTextBox.Text) >= 1000)
                    {
                        //resp = remote_inst.query(cmd, Convert.ToInt32(timeOutTextBox.Text));
                       resp = remote_inst.queryByte(cmd, Convert.ToInt32(timeOutTextBox.Text));
                    }
                }
                else
                {
                    //resp = remote_inst.query(cmd);
                    resp = remote_inst.queryByte(cmd);
                }*/
                resp = remote_inst.queryByte(cmd);
            }
            catch (Exception ex)
            {
                //resp = ex.ErrorCode.ToString();
                //resp = ex.Message;
                throw ex;
			}

            string readstring = System.Text.Encoding.ASCII.GetString(resp);
            //Console.WriteLine(readstring);
            byte[] bytesAry = resp;// System.Text.Encoding.ASCII.GetBytes(resp);//BitConverter.GetBytes(resp.ToCharArray());
         

            double[] real64 = null;
            if (binDecode.Checked)
            {
                remote_inst.binaryToDouble64(bytesAry, ref real64, swapBinOrder.Checked);
                if (real64 != null)
                {
                    readstring = "";
                    for (int kk = 0; kk < real64.Length; kk++)
                    {
                        if (kk > 0)
                        {
                            readstring += "," + Convert.ToString(real64[kk]);
                        }
                        else
                        {
                            readstring = Convert.ToString(real64[kk]);
                        }
                    }
                }
            }
            responseTextBox.Text += readstring + "\r\n\r\n";
            remote_inst.close();
            commandComboBox.Items.Add(cmd);
            
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst" + gpibAddrTextBox.Text;
            string cmd = commandComboBox.Text; ;
            int timeout_ms = 0;
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            string resp = string.Empty;
            
            try
            {
                /*if (LockDeviceCheckBox.Checked )
                {
                    // device lock is requested, min 1000 ms is chosen for demo
                    if (Convert.ToInt32(timeOutTextBox.Text) >= 1000)
                    {
                        remote_inst.write(cmd, Convert.ToInt32(timeOutTextBox.Text));
                    }
                }
                else
                {
                        remote_inst.write(cmd);
                
                }*/
                remote_inst.write(cmd);
            }
            catch (Exception ex)
            {
                resp = ex.Message;
                responseTextBox.Text += resp + "\r\n";
            }
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst" + gpibAddrTextBox.Text;
            string cmd = commandComboBox.Text; 
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            //string resp = string.Empty;
            string readstring = string.Empty;
            byte[] resp = null;
            try
            {
                /*if (LockDeviceCheckBox.Checked)
                {
                    // device lock is requested, min 1000 ms is chosen for demo
                    // 
                    if (Convert.ToInt32(timeOutTextBox.Text) >= 1000)
                    {
                        resp = remote_inst.read(Convert.ToInt32(timeOutTextBox.Text));
                    }
                }
                else
                {
                    resp = remote_inst.read();
                }*/
                resp = remote_inst.readBytes();
            }
            catch (Exception ex)
            {
                readstring = ex.Message;
            }
            Console.WriteLine(resp);
            readstring = System.Text.Encoding.ASCII.GetString(resp);
            responseTextBox.Text += readstring + "\r\n\r\n";
            remote_inst.close();

        }

        #region i2C

       

        private void i2cScanButton_Click(object sender, EventArgs e)
        {
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst30";
            
            string resp;
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            // scan test
            byte[] i2cInst = new byte[128];
            remote_inst.scanI2c(ref i2cInst);
            i2cRespTextBox.AppendText("i2c scan:\r\n");
            for (int k = 3; k < 128; k++)
            {
                // if not zero, then i2c instrument is present
                if (i2cInst[k] != 0)
                {
                    //Console.WriteLine("Hex: {0:X}", k);
                    resp = String.Format("0x{0:X}",k);
                    i2cRespTextBox.AppendText(resp + "\r\n");
                }
            }
            i2cRespTextBox.AppendText("\r\n");
            remote_inst.close();
        }

        private void i2cWriteButton_Click(object sender, EventArgs e)
        {
            byte i2cAddr = 0;
            byte regAddr = 0;
            byte numOfBytes = 0;
            string inputStr = i2cAddrTextBox.Text;
            string[] strAry = null;

            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst30";

            string resp;
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);

            if (inputStr == string.Empty)
            {
                return;
            }

            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                i2cAddr = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                i2cAddr = Convert.ToByte(inputStr);
            }

            Console.WriteLine("i2c addr: " + i2cAddr.ToString());
            // 
            inputStr = i2cRegisterTextBox.Text;
            if (inputStr == string.Empty)
            {
                return;
            }
            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                regAddr = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                regAddr = Convert.ToByte(inputStr);
            }

            Console.WriteLine("i2c register: " + regAddr.ToString());

            inputStr = i2cNumOfBytesTextBox.Text;

            if (inputStr == string.Empty)
            {
                return;
            }
            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                numOfBytes = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                numOfBytes = Convert.ToByte(inputStr);
            }

            Console.WriteLine("number of bytes I2C write: " + numOfBytes.ToString());


            
            inputStr = i2cComboBox.Text;
            if (inputStr == string.Empty)
            {
                return;
            }
            byte[] data = new byte[numOfBytes] ;

            if (inputStr.Contains(@"/"))
            {

                strAry = inputStr.Split(new char[] { '/' }); // first element is ignored due to / char

                data = new byte[strAry.Length - 1];
                for (int j = 1; j < strAry.Length; j++)
                {
                    data[j - 1] = Convert.ToByte(strAry[j], 16);// or byte.Parse(strAry[j], System.Globalization.NumberStyles.HexNumber);

                }
            }

            i2cRespTextBox.AppendText("i2c write-> add: " + i2cAddr.ToString()+", reg: " +  regAddr.ToString() + "\r\n");

            // check if there is any differences between number of bytes versus i2c data to write (i2cComboBox)
            if (numOfBytes != strAry.Length)
            {
                numOfBytes =  (byte)(strAry.Length -1);
            }
            if (numOfBytes > 255)
            {
                i2cRespTextBox.AppendText(numOfBytes.ToString()+ " bytes is greater than max 255 allowed" + "\r\n");
                remote_inst.close();
                return;
            }
            try
            {
                remote_inst.writeI2c(i2cAddr, regAddr, Convert.ToByte(numOfBytes),  data);
            }
            catch (Exception ex)
            {
                resp = ex.Message;
                i2cRespTextBox.AppendText(resp + "\r\n");
            }
            i2cRespTextBox.AppendText( inputStr+ "\r\n");
            remote_inst.close();
        }

        private void i2cQueryButton_Click(object sender, EventArgs e)
        {
            byte i2cAddr = 0;
            byte regAddr = 0;
            byte numOfBytes = 0;
            string inputStr = i2cAddrTextBox.Text;
            string[] strAry = null;

            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst30";

            string resp;
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);

            if (inputStr == string.Empty)
            {
                return;
            }

            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                i2cAddr = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                i2cAddr = Convert.ToByte(inputStr);
            }

            Console.WriteLine("i2c addr: " + i2cAddr.ToString());
            // 
            inputStr = i2cRegisterTextBox.Text;
            if (inputStr == string.Empty)
            {
                return;
            }
            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                regAddr = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                regAddr = Convert.ToByte(inputStr);
            }

            Console.WriteLine("i2c register: " + regAddr.ToString());

            inputStr = i2cNumOfBytesTextBox.Text;

            if (inputStr == string.Empty)
            {
                return;
            }
            if (inputStr.Contains(@"/"))
            {
                strAry = inputStr.Split(new char[] { '/' });
                numOfBytes = Convert.ToByte(strAry[strAry.Length - 1], 16);
            }
            else
            {
                numOfBytes = Convert.ToByte(inputStr);
            }

            Console.WriteLine("number of bytes I2C read: " + numOfBytes.ToString());


            byte[] data = new byte[numOfBytes];

            i2cRespTextBox.AppendText("i2c read->addr: " + i2cAddr.ToString()+", reg: " +  regAddr.ToString()+"\r\n");

            if (numOfBytes > 255)
            {
                i2cRespTextBox.AppendText(numOfBytes.ToString() + " bytes is greater than max 255 allowed" + "\r\n");
                remote_inst.close();
                return;
            }
            try
            {
                remote_inst.readI2c(i2cAddr, regAddr, Convert.ToByte(numOfBytes), ref data);
            }
            catch (Exception ex)
            {
                resp = ex.Message;
                i2cRespTextBox.AppendText(resp + "\r\n");
                remote_inst.close();
                return;
            }

            for (int j = 0; j < numOfBytes; j++)
            {
                resp = j.ToString() + ": " + String.Format("0x{0:X}", data[j]);
                i2cRespTextBox.AppendText(resp + "\r\n");
            }

            i2cRespTextBox.AppendText("\r\n");

            remote_inst.close();
            i2cComboBox.Items.Add(inputStr);

        }

        //private void RunScriptFile_Click(object sender, EventArgs e)
        //{

        //}

        private void RunI2CScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog openXmlFileDialog = new OpenFileDialog();
            openXmlFileDialog.Title = "Select I2C Script file";
            openXmlFileDialog.Multiselect = false;
            openXmlFileDialog.InitialDirectory = Application.StartupPath;//@"C:\lexar1\csharp\CS2010\"; // @"E:\LEXAR1\tests\";
            //openXmlFileDialog.Filter = "xml files (*.xml)|*.xml | hex files (*.hex)|*.hex";
            openXmlFileDialog.Filter = "csv files |*.csv";
            openXmlFileDialog.ShowDialog();
            string[] fileNames = openXmlFileDialog.FileNames;
            byte i2cAddr = 0;
            byte i2cReg = 0;
            byte[] i2cData = null;
            int j=0;

            if (fileNames.Length>0)
            {
                string ipstr = ipAddrTextBox.Text;
                string gpibAddr = "inst30";

                string resp;
                VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
                StreamReader reader = new StreamReader(fileNames[0]);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] ary = line.Split(new char[] {','});
                    if (ary.Length >=3) // min is i2cAddr, i2cReg, data
                    {
                        i2cAddr = Convert.ToByte(ary[0], 16);
                        i2cReg = Convert.ToByte(ary[1], 16);
                        j = ary.Length - 2;
                        i2cData = new byte[j];
                        for (int k = 0;k<j;k++)
                        {
                            i2cData[k] = Convert.ToByte(ary[2 + k],16);
                        }
                        // ready to write
                        try
                        {
                            Console.WriteLine(Convert.ToByte(i2cData.Length));
                            remote_inst.writeI2c(i2cAddr, i2cReg, Convert.ToByte(i2cData.Length), i2cData);
                            i2cRespTextBox.AppendText(line + "\r\n");
                        }
                        catch (Exception ex)
                        {
                            resp = ex.Message;
                            i2cRespTextBox.AppendText(resp + "\r\n");
                        }
                        

                    }
                    

                }
            }


        }
        #endregion

        

        private void ClearButton_Click(object sender, EventArgs e)
        {
            i2cRespTextBox.Text = "";
            i2cRespTextBox.Update();
            uartRespTextBox.Text = "";
            uartRespTextBox.Update();
        }

        #region UART

        private void setUartConfigButton_Click(object sender, EventArgs e)
        {
            int byteTimeout_us = Convert.ToInt32(byteTimeoutComboBox.Text);
            int msgTimeout_ms = Convert.ToInt32(msgTimeoutComboBox.Text);
            int dataRate = Convert.ToInt32(uartDataRateComboBox.Text);
            byte uartStopbit = Convert.ToByte(uartStpBitComboBox.Text);
            byte uartNumbit = Convert.ToByte(uartNumBitComboBox.Text);
            byte uartParity = 0;

            switch (uartParityComboBox.Text)
            {
                case "None":
                    uartParity = 0;
                    break;
                case "Odd":
                    uartParity = 1;
                    break;
                case "Even":
                    uartParity = 2;
                    break;
                default:
                    break;
            }
            
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst29";


            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            remote_inst.uartSetConfig(dataRate, uartNumbit, uartParity, uartStopbit, msgTimeout_ms, byteTimeout_us);
            remote_inst.close();
        }




        private void uartWriteButton_Click(object sender, EventArgs e)
        {
            string inputStr = uartComboBox.Text;
            string[] strAry = null;
            int len=0;

            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst29";

            string resp;
            byte[] byteAry1= null;

            if (inputStr == string.Empty)
            {
                return;
            }

            if (inputStr.Contains(@"/"))
            {
                // this is in hex
                strAry = inputStr.Split(new char[] { '/' });
                
            }
            else
            {

                if (!newLineCheckBox1.Checked)
                {
                    Regex regex = new Regex(@"\\n");
                    Match m = regex.Match(inputStr);
                    if (m.Success)
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr.ToCharArray(), 0, inputStr.Length - 1);
                        byteAry1[inputStr.Length - 2] = 0x0a; // putting newline in ascii
                    }
                    else
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                    }
                }
                else
                {
                    // check box is check, add new line to the input string
                    inputStr = inputStr + Environment.NewLine;
                    byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                }

                len = byteAry1.Length;
                //byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
            }
            
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            remote_inst.uartWriteBytes(len, byteAry1);
            remote_inst.close();
            uartRespTextBox.AppendText("uart write:");
            uartRespTextBox.AppendText(inputStr);
            
        }

        private void uartQueryButton_Click(object sender, EventArgs e)
        {
            string inputStr = uartComboBox.Text;
            string[] strAry = null;
            int len = 0;

            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst29";

            byte[] resp = null;
            byte[] byteAry1 = null;
            string respStr = string.Empty;

            if (inputStr == string.Empty)
            {
                return;
            }

            if (inputStr.Contains(@"/"))
            {
                // this is in hex
                strAry = inputStr.Split(new char[] { '/' });

            }
            else
            {

                if (!newLineCheckBox1.Checked)
                {
                    Regex regex = new Regex(@"\\n");
                    Match m = regex.Match(inputStr);
                    if (m.Success)
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr.ToCharArray(), 0, inputStr.Length - 1);
                        byteAry1[inputStr.Length - 2] = 0x0a; // putting newline in ascii
                    }
                    else
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                    }
                }
                else
                {
                    // check box is check, add new line to the input string
                    inputStr = inputStr + Environment.NewLine;
                    byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                }

                len = byteAry1.Length;
                //byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
            }

            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            remote_inst.TimeOut = 15000;//Convert.ToInt32(byteTimeoutComboBox.Text) / 1000 + 500;
            Console.WriteLine(remote_inst.TimeOut);
            remote_inst.BufferSize = 40000;
            remote_inst.uartQueryBytes(len, byteAry1, ref resp);

            
            
            string readstring = System.Text.Encoding.ASCII.GetString(resp);
            Console.WriteLine(resp.Length);
            //Console.WriteLine(readstring);
            //uartTextBox.Text = readstring;
            double[] real64 = null;
            if (binDecode.Checked)
            {
                remote_inst.binaryToDouble64(resp, ref real64, swapBinOrder.Checked);
                if (real64 != null)
                {
                    readstring = "";
                    for (int kk = 0; kk < real64.Length; kk++)
                    {
                        if (kk > 0)
                        {
                            readstring += "," + Convert.ToString(real64[kk]);
                        }
                        else
                        {
                            readstring = Convert.ToString(real64[kk]);
                        }
                    }
                }
            }
            uartRespTextBox.AppendText(inputStr);
            uartRespTextBox.AppendText(readstring );
            remote_inst.close();
            uartComboBox.Items.Add(inputStr);
        }

        private void uartReadButton_Click(object sender, EventArgs e)
        {
            // Read is actually a special case of write, with inputstr length of zero
            string inputStr = string.Empty;
            string[] strAry = null;
            int len = 0;

            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst29";

            byte[] resp = null;
            byte[] byteAry1 = null;
            string respStr = string.Empty;

           
            /*if (inputStr.Contains(@"/"))
            {
                // this is in hex
                strAry = inputStr.Split(new char[] { '/' });

            }
            else
            {

                if (!newLineCheckBox1.Checked)
                {
                    Regex regex = new Regex(@"\\n");
                    Match m = regex.Match(inputStr);
                    if (m.Success)
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr.ToCharArray(), 0, inputStr.Length - 1);
                        byteAry1[inputStr.Length - 2] = 0x0a; // putting newline in ascii
                    }
                    else
                    {
                        byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                    }
                }
                else
                {
                    // check box is check, add new line to the input string
                    inputStr = inputStr + Environment.NewLine;
                    byteAry1 = Encoding.ASCII.GetBytes(inputStr);
                }

                len = byteAry1.Length; // this has to be zero
                //byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
            }*/


            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            remote_inst.TimeOut = 15000;//Convert.ToInt32(byteTimeoutComboBox.Text) / 1000 + 500;
            Console.WriteLine(remote_inst.TimeOut);
            remote_inst.BufferSize = 40000;
            
            //remote_inst.uartReadBytes(ref resp);
            remote_inst.uartQueryBytes(0, null, ref resp);
            string readstring = System.Text.Encoding.ASCII.GetString(resp);
            Console.WriteLine(resp.Length);
            //Console.WriteLine(readstring);
            //uartTextBox.Text = readstring;
            double[] real64 = null;
            if (binDecode.Checked)
            {
                remote_inst.binaryToDouble64(resp, ref real64, swapBinOrder.Checked);
                if (real64 != null)
                {
                    readstring = "";
                    for (int kk = 0; kk < real64.Length; kk++)
                    {
                        if (kk > 0)
                        {
                            readstring += "," + Convert.ToString(real64[kk]);
                        }
                        else
                        {
                            readstring = Convert.ToString(real64[kk]);
                        }
                    }
                }
            }


            uartRespTextBox.AppendText(@"uart read:");
            uartRespTextBox.AppendText(readstring + "\r\n");
            remote_inst.close();

        }

        

        private void getUartConfig_Click(object sender, EventArgs e)
        {
            
            string ipstr = ipAddrTextBox.Text;
            string gpibAddr = "inst29";
            string resp;

            int byteTimeout_us= Convert.ToInt32(byteTimeoutComboBox.Text);
            int msgTimeout_ms = Convert.ToInt32(msgTimeoutComboBox.Text);
            int dataRate = Convert.ToInt32(uartDataRateComboBox.Text);
            byte uartStopbit = Convert.ToByte(uartStpBitComboBox.Text);
            byte uartNumbit = Convert.ToByte(uartNumBitComboBox.Text);
            byte uartParity = 0;
            VXI11Class remote_inst = new VXI11Class(gpibAddr, ipstr);
            remote_inst.uartGetConfig(ref dataRate, ref uartNumbit, ref uartParity, ref uartStopbit, ref msgTimeout_ms, ref byteTimeout_us);
            remote_inst.close();

            uartNumBitComboBox.Text = Convert.ToString(uartNumbit);
            uartDataRateComboBox.Text = Convert.ToString(dataRate);
            uartStpBitComboBox.Text = Convert.ToString(uartStopbit);
            msgTimeoutComboBox.Text = Convert.ToString(msgTimeout_ms);
            byteTimeoutComboBox.Text = Convert.ToString(byteTimeout_us);
            switch (uartParity)
            {
                case 0x00:
                    uartParityComboBox.Text = "None";
                    break;
                case 0x01:
                    uartParityComboBox.Text = "Odd";
                    break;
                case 0x02:
                    uartParityComboBox.Text = "Even";
                    break;
                
            }

        }



        #endregion UART

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
