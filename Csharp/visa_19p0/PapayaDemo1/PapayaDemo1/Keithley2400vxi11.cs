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
 * Sample Keithley 2400 driver using Papaya ppyGpibNetClass.cs
 * Papaya C# driver using National instrument Visa to communicate to the
 * Papaya GPIB Controller.
 * The code is tested with National Instrument VisaNs version 19.0
 *
 */

namespace PapayaDemo

{
    public class Keithley2400vxi11
    {
        public enum SourceModeType
        {current=0, voltage=1};

        public enum SenseModeType
        { AllOn, AllOff, Voltage, Current, Resistance };

        public enum SelectTerminals
        { Front, Rear };


        string deviceName;
        string ipAddress;
        VXI11Class vxi11Device;


        public Keithley2400vxi11(string name, string address, int timeout)
        {
            this.deviceName = name;
            this.ipAddress = address;
            vxi11Device = new VXI11Class(name, address,false,2000);
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


        public double VoltageSetpoint
        {
            get
            {

                vxi11Device.write(":SOUR:VOLT:LEV?");
                double voltageSetpoint = Convert.ToDouble(vxi11Device.read());
                return voltageSetpoint;
            }
            set
            {
                vxi11Device.write(":SOUR:VOLT:LEV " + value);
            }
        }

        public SourceModeType SourceMode
        {
            get
            {
                SourceModeType sourceMode;
                vxi11Device.write(":SOUR:FUNC?");
                string response = vxi11Device.read().Trim();
                switch (response)
                {
                    case "VOLT":
                        sourceMode = SourceModeType.voltage;
                        break;
                    case "CURR":
                        sourceMode = SourceModeType.current;
                        break;
                    default:
                        throw new Exception("Unknown Mode");
                }
                return sourceMode;
            }
            set
            {
                string mode;
                if (value == SourceModeType.voltage) mode = "VOLT";
                else mode = "CURR";
                vxi11Device.write(":SOUR:FUNC " + mode);
            }
        }

        public double CurrentCompliance
        {
            get
            {
                double currentCompliance = -99.0;
                vxi11Device.write(":SENS:CURR:PROT?");
                currentCompliance = Convert.ToDouble(vxi11Device.read());
                return currentCompliance;
            }
            set
            {

                vxi11Device.write(":SENS:CURR:PROT " + value);
            }
        }


        public double VoltageCompliance
        {
            get
            {
                double voltageCompliance = -99.0;
                vxi11Device.write(":SENS:VOLT:PROT?");
                voltageCompliance = Convert.ToDouble(vxi11Device.read());
                return voltageCompliance;
            }
            set
            {

                vxi11Device.write(":SENS:VOLT:PROT " + value);
            }
        }

        public bool OutputIsOn
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
        public bool FourWireIsOn
        {
            get
            {
                vxi11Device.write(":SYST:RSEN?");
                if (Convert.ToBoolean(Convert.ToByte(vxi11Device.read()))) return true;
                else return false;
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                vxi11Device.write(":SYST:RSEN " + state);
            }
        }
        public SenseModeType SenseMode
        {
            set
            {
                string mode = null;
                if (value == SenseModeType.AllOn || value == SenseModeType.AllOff)
                {
                    if (value == SenseModeType.AllOn)
                    {
                        vxi11Device.write(":SENS:FUNC:ON:ALL");
                    }
                    if (value == SenseModeType.AllOff)
                    {
                        vxi11Device.write(":SENS:FUNC:OFF:ALL");
                    }
                }
                else
                {
                    if (value == SenseModeType.Voltage)
                    {
                        mode = "'VOLT'";
                    }
                    if (value == SenseModeType.Current)
                    {
                        mode = "'CURR'";
                    }
                    if (value == SenseModeType.Resistance)
                    {
                        mode = "'RES'";
                    }
                    vxi11Device.write(":SENS:FUNC:ON " + mode);
                }
            }
        }


        public SelectTerminals Terminal
        {
            get
            {
                SelectTerminals terminal;
                vxi11Device.write(":ROUT:TERM?");
                string response = vxi11Device.read().Trim();
                switch (response)
                {
                    case "FRON":
                        terminal = SelectTerminals.Front;
                        break;
                    case "REAR":
                        terminal = SelectTerminals.Rear;
                        break;
                    default:
                        throw new Exception("Unknown State");
                }
                return terminal;
            }
            set
            {
                if (value == SelectTerminals.Front) vxi11Device.write(":ROUT:TERM FRON");
                else vxi11Device.write(":ROUT:TERM REAR");
            }
        }

        #region methods

        public double GetCurrent()
        {
            double retVal = 0;
            vxi11Device.write(":SOUR:CURR?");
            retVal = Convert.ToDouble(vxi11Device.read());
            return retVal;
        }
        public double GetVoltage()
        {
            double retVal = 0;
            vxi11Device.write(":SOUR:VOLT?");
            retVal = Convert.ToDouble(vxi11Device.read());
            return retVal;
        }

        public double ReadCurrent()
        {
            vxi11Device.write(":FORM:ELEM CURR");
            vxi11Device.write("READ?");
            string retval = vxi11Device.read();
            return Convert.ToDouble(retval);
        }

        public double ReadVoltage()
        {
            vxi11Device.write(":FORM:ELEM VOLT");
            vxi11Device.write("READ?");
            string retval = vxi11Device.read();
            return Convert.ToDouble(retval);
        }
        #endregion methods

    }
}
