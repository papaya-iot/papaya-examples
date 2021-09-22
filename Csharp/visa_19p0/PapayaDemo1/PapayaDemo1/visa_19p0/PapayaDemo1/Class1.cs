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
        // Command == 1, Query (Write then read)
        // Commmad == 2, Write
        // Command == 3,4 Set/Get UART Configurations
        // Command == 5,6 Set/Get Non-Standard or Fractional baud rate
        // Command == 7 Query bytes of baud rate
        // Command == 8 Write baud rate 
        string deviceName;
        string ipAddress;

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
        public bool getOutputOnOff()
        {
            vxi11Device.write(":OUTP?");
            if (Convert.ToBoolean(Convert.ToByte(vxi11Device.read()))) return true;
            else return false;

        }
        // set_outPutOnOff
        public bool setOutputOnOff()
        {
            string state;
            if (value) state = "1";
            else state = "0";
            vxi11Device.write(":OUTP " + state);
            return value;
        }
        // queryCurrent
        public double queryCurrent()
        {
            vxi11Device.write("FORM:ELEM CURR");
            return vxi11Device.query(":meas:curr:dc?");
        }
        // queryVoltage
        public double queryVoltage()
        {
            vxi11Device.write("FORM:ELEM CURR");
            return vxii11Device.query(":meas:volt:dc?");
        }
        // selectPowersupply
        public void selectPowerSupply(int type)
        {
            string cmd = (":INST:NSEL" + (string)(type));
            vxi11Device.write(cmd);
        }
        // Set P6VSupply
        public void setP6Supply(int volt)
        {
            vxi11Device.write("Inst:NSEL 1");
            string cmd = "volt " + (string)(volt);
            vxi11Device.write(cmd);
        }
        // queryP6SetVoltage
        public float queryP6SetVoltage()
        {
            vxi11Device.write("INST:NSEL 1");
            float voltage = vxi11Device.query("volt?");
            return voltage;
        }
        // setP25VSupply
        public void setP25VSupply()
        {
            vxi11Device.write("INST:NSEL 2");
            float voltage = vxi11Device.query("volt?");
            return voltage;
        }
        // queryP25VSetVoltage
        public void queryP25VSetVoltage()
        {
            vxi11Device.write("INST:NSEL 2");
            System.Threading.Thread.Sleep(3000); // Pause for 3 seconds
            return (float)vxi11Device.query("volt?");
        }
        // setN25VSupply
        public void setN25VSupply(int volt)
        {
            vxi11Device.write("INST:NSEL 3");
            cmd = "volt " + (string)(volt);
            vxi11Device.write(cmd);
        }
        // queryN25VSetVoltage
        public void queryN25VSetVoltage()
        {
            vxi11Device.write("INST:NSEL 3");
            System.Threading.Thread.Sleep(3000); // Pause for 3 seconds
            return (float)vxi11Device.query("volt?");
        }

    }



}
