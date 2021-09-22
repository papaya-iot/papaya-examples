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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapayaDemo
{
    public class Keysight_E3649A
    {
        string deviceName;
        string ipAddress;
        VXI11Class vxi11Device;

        public Keysight_E3649A(string name, string address)
         {
             this.deviceName = name;
             this.ipAddress = address;
             // Set lockDevice = false, timeout = 2000 ms
             vxi11Device = new VXI11Class(name, address, false, 2000);
         }

        public string DeviceName
        {
            set
            {
                this.deviceName = value;
            }
            get
            {
                return this.deviceName;
            }
        }

        public string IPAddress
        {
            set
            {
                this.ipAddress = value;
            }
            get
            {
                return this.ipAddress;
            }
        }
        // get_outPutOnOff
        public bool outputOnOff
        {
            set 
            {
                try
                {
                    string state;
                    if (value) state = "1";
                    else state = "0";
                    vxi11Device.write(":OUTP " + state);
                }
                catch (System.Exception)
                {
                    throw new Exception("Agilent E3649A write outp on/off fails");
                }
                
            }
            get 
            {
                vxi11Device.write(":OUTP?");
                try 
                {
                    if (Convert.ToBoolean(Convert.ToByte(vxi11Device.read()))) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Exception("Agilent E3649A query outp on/off fails");
                }
            }
        }
        public double queryCurrent()
        {
            return Convert.ToDouble(vxi11Device.query(":meas:curr:dc?"));
        }
        public double queryVoltage()
        {
            return Convert.ToDouble(vxi11Device.query(":meas:volt:dc?"));
        }
        public void setCurrent(double curr, int output_num=0) 
        {
            try
            {
                if (output_num != 0) 
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                vxi11Device.write("CURR " + Convert.ToString(curr));
            }
            catch (System.Exception)
            {
                throw new Exception("Agilent E3649A set current fails");
            }
        }

        public void setVoltage(double volt, int output_num=0) 
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                } 
                vxi11Device.write("VOLT " + Convert.ToString(volt));
            }
            catch (System.Exception)
            {
                throw new Exception("Agilent E3649A set voltage fails");
            }
        }

        public void selectOutput(int output_num)
        {
            try {
                vxi11Device.write("INST:NSEL" + Convert.ToString(output_num));
            } catch (System.Exception)
            {
                throw new Exception("Agilent E3649A select output fails");
            }
        }

        public double queryOutputRange(int output_num=0)
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                double resp = Convert.ToDouble(vxi11Device.query(":VOLT:RANG?"));
                return resp;
            }
            catch (System.Exception)
            {
                throw new Exception("Agilent_E3649A queryOutputRange failed");
            }
        }

        public void setOutputRange(int volt_range, int output_num=0)
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                vxi11Device.write(":VOLT:RANG " + Convert.ToString(volt_range));
            }
            catch (System.Exception)
            {
                
                throw new Exception("Agilent_E3649A set output range failed");
            }
        }

        public void setOutputLow(int output_num = 0)
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                vxi11Device.write(":VOLT:RANG LOW");
            }
            catch (System.Exception)
            {
                
                throw new Exception("Agilent_E3649A set output low failed");
            }
        }

        public void setOutputHigh(int output_num = 0)
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                vxi11Device.write(":VOLT:RANG HIGH");
            }
            catch (System.Exception)
            {
                
                throw new Exception("Agilent_E3649A set output high failed");
            }
        }

        public void enableVoltageProtection(int enable=1, int output_num = 0) 
        {
            try 
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                vxi11Device.write("VOLT:PROT:STAT " + Convert.ToString(enable));
            }
            catch (System.Exception)
            {
                throw new Exception("Agilent_E3649A enable voltage protection failed");
            }
        }

        public void setVoltageProtection(double volt, int output_num = 0)
        {
            try
            {
                if (output_num != 0) 
                {
                    vxi11Device.write("INST:NSEL " + Convert.ToString(output_num));
                }
                vxi11Device.write("VOLT:PROT " + Convert.ToString(volt));
            }
            catch (System.Exception) {
                throw new Exception("Agilent_E3649A set voltage protection failed");
            }
        }

        public Tuple<String,String> queryVoltageProtection(int output_num = 0)
        {
            try
            {
                {
                    String ena = vxi11Device.query("VOLT:PROT:STAT?");
                    String level = vxi11Device.query("VOLT:PROT?");
                    return Tuple.Create(ena, level);
                }
            }
            catch (System.Exception)
            {
                throw new Exception("Agilent_E3649A query voltage protection failed");
            }
        }
    }
}