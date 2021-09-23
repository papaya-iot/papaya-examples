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

/* 
 * Sample Agilent_E3631 driver using Papaya ppyGpibNetClass.cs
 * Papaya C# driver using National instrument Visa to communicate to the
 * Papaya GPIB Controller.
 * The code is tested with National Instrument VisaNs version 19.0
 *
 */

namespace PapayaDemo

{
    public class Agilent_E3631
    {
        string deviceName;
        string ipAddress;
        VXI11Class vxi11Device;

        // Constructor
        public Agilent_E3631(string name, string address)
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
            get
            {
                vxi11Device.write(":OUTP?");
                if (Convert.ToBoolean(Convert.ToByte(vxi11Device.read()))) return true;
                else return false;
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                vxi11Device.write(":OUTP " + state);
            }
        }
        // queryCurrent
        public double queryCurrent()
        {
            return Convert.ToDouble(vxi11Device.query(":meas:curr:dc?"));
        }
        // queryVoltage
        
        public double queryVoltage()
        {
            return Convert.ToDouble(vxi11Device.query(":meas:volt:dc?"));
        }
        // selectPowersupply
        public void selectPowerSupply(int type)
        {
            if (type > 3 || type < 1)
            {
                throw new Exception("Invalid Type: Type must equal 1, 2 or 3");
            }
            else
            {
                string cmd = (":INST:NSEL" + Convert.ToString(type));
                vxi11Device.write(cmd);
            }
            
        }

        // Helper method for reading (get) voltages from power source
        private double readVoltage(int source)
        {
            vxi11Device.write("INST:NSEL " + source.ToString());
            System.Threading.Thread.Sleep(3000); // Pause for 3 seconds
            double voltage = Convert.ToDouble(vxi11Device.query("volt?"));
            return voltage;
        }
        public double P6Supply
        {
            set
            {
                if (value < 0 || value > 6.18) // If outside voltage range
                {
                    throw new Exception("Outside acceptable voltage range (0 to 6.18 V)");
                }
                else
                {
                    vxi11Device.write("INST:NSEL 1");
                    string cmd = "volt " + Convert.ToString(value);
                    vxi11Device.write(cmd);
                }
            }
            get
            {
                return readVoltage(1);
            }
        }

        public double P25Supply
        {
            set
            {
                if (value < 0 || value > 25.75) // If outside voltage range
                {
                    throw new Exception("Outside acceptable voltage range (0 to 25.75 V)");
                }
                else
                {
                    vxi11Device.write("INST:NSEL 2");
                    string cmd = "volt " + Convert.ToString(value);
                    vxi11Device.write(cmd);
                }
            }
            get
            {
                return readVoltage(2);
            }
        }

        public double N25Supply
        {
            set
            {
                if (value > 0 || value < -25.75) // If outside voltage range
                {
                    throw new Exception("Outside acceptable voltage range (-25.75 to 0 V)");
                }
                else
                {
                    vxi11Device.write("INST:NSEL 3");
                    String cmd = "volt " + Convert.ToString(value);
                    vxi11Device.write(cmd);
                }
            }
            get
            {
                return readVoltage(3);
            }
        }

    }
}
