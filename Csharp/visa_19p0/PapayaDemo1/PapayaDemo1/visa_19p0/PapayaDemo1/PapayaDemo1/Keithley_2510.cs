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
    public class Keithley_2510
    {
        string deviceName;
        string ipAddress;
        VXI11Class vxi11Device;

        public Keithley_2510(string name, string address)
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

        public string checkForError()
        {
            return vxi11Device.query("syst:err?");
        }

        public double temp // Temperature must be between 0 and 40 degrees
        {
            set
            {
                try
                {
                    vxi11Device.write(":SOUR:TEMP " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 temp read error");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query(":MEAS:TEMP?"));
                }
                catch (System.Exception)
                {
                    return -1000.0;
                    throw new System.Exception("Keithley 2510 warning: temp read error...");
                }
            }
        }

        public double output
        {
            set
            {
                try
                {
                    vxi11Device.write(":OUTPUT " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 write outp fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query(":OUTPUT?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 query outp fails");
                }
            }
        }

        /* Protection 
         *  Control 
         *      (Enable/Disable) 
         *  LOLIM 
         *      1 to 200(100 RTD or thermistor)
         *      5 to 2k(1k RTD or thermistor)
         *      50 to 20k(10k thermistor)
         *      500 to 200k(100k thermistor)
         *  HILIM
         *      50 to 1k (100 thermistor)
         *      500 to 9.999k (1k thermistor)
         *      5k to 80k (10k thermistor)
         *      50k to 200k (100k thermistor)
         *      50 to 250 (100 RTD)
         *      500 to 3k (1k RTD)
        */
        public int protControl
        {
            set
            {
                try
                {
                    // if value == true, then turn on, otherwise turn off
                    string state;
                    if (value == 1) state = "1";
                    else state = "0";
                    vxi11Device.write("SOUR:TEMP:PROT:STATE " + state);
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set protection control fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToInt32(vxi11Device.query("SOUR:TEMP:PROT:STATE?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get protection control fails");
                }
            }
        }

        public double protLow
        {
            set
            {
                try
                {
                    vxi11Device.write("SOUR:TEMP:PROT:LOW " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set protection low limit fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SOUR:TEMP:PROT:LOW?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get protection low limit fails");
                }
            }
        }

        public double protHigh
        {
            set
            {
                try
                {
                    vxi11Device.write("SOUR:TEMP:PROT:HIGH " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set protection high limit fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SOUR:TEMP:PROT:HIGH?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get protection high limit fails");
                }
            }
        }

        // Thermistor ranges 100, 1K 10K, 100K
        public double thermistorRange
        {
            set
            {
                try
                {
                    vxi11Device.write("SENS:TEMP:THER:RANGE " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set thermistor range fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SENS:TEMP:THER:RANGE?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get thermistor range fails");
                }
            }
        }

        //Thermistor Source Current ranges AUTO, 3.3uA, 10uA, 33.3uA, 100uA, 833.3uA
        public double thermistorSourceCurrent
        {
            set
            {
                try
                {
                    vxi11Device.write("SENS:TEMP:CURR " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set thermistor source current fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SENS:TEMP:CURR?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get thermistor source current fails");
                }
            }
        }
        //sens:curr:prot:lev?

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

        // PID Proportional, Integral, and Derivative Constant (0 to 100000)
        public double pidGain
        {
            set
            {
                try
                {
                    vxi11Device.write("SOUR:TEMP:LCON " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set temperature gain fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SOUR:TEMP:LCON?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get temperature gain failss");
                }
            }
        }
        public double pidDerivative
        {
            set
            {
                try
                {
                    vxi11Device.write("SOUR:TEMP:LCON:DER " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set temperature pid derivative fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SOUR:TEMP:LCON:DER?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get temperature pid fails");
                }
            }
        }

        public double pidIntegral
        {
            set
            {
                try
                {
                    vxi11Device.write("SOUR:TEMP:LCON:INT " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 set temperature pid integral fails");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query("SOUR:TEMP:LCON:INT?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Keithley 2510 get temperature pid integral fails");
                }
            }
        }
    }
}