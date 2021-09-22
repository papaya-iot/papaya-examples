using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapayaDemo
{
    public class Agilent_86142
    {
        string deviceName;
        string ipAddress;
        VXI11Class vxi11Device;

        public Agilent_86142(string name, string address)
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
        public double startWaveLength_nM
        {
            set
            {
                try
                {
                    vxi11Device.write(":sens:wav:star " + Convert.ToString(value) + "nm");
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 set start wave length failed");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query(":sens:wav:star?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 get start wave length failed");
                }
            }
        }
        public double stopWaveLength_nM
        {
            set
            {
                try
                {
                    vxi11Device.write(":sens:wav:stop " + Convert.ToString(value) + " nm");
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 set stop wave length failed");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query(":sens:wav:stop?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 get stop wave length failed");
                }
            }
        }

        public double traceLength
        {
            set
            {
                try
                {
                    vxi11Device.write(":sens:swe:poin " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 set trace length failed");
                }
            }
            get
            {
                try
                {
                    return Convert.ToDouble(vxi11Device.query(":sens:swe:poin?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 get trace length failed");
                }
            }
        }

        // get/set for init sweep (init:imm)
        public int initSweep
        {
            set
            {
                try
                {
                    vxi11Device.write(":init:cont " + Convert.ToString(value));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 set init sweep failed");
                }
            }
            get
            {
                try
                {
                    return Convert.ToInt32(vxi11Device.query(":init:cont?"));
                }
                catch (System.Exception)
                {
                    throw new System.Exception("Agilent 86142 get init sweep failed");
                }
            }
        }
        public String getTrace()
        {
            try
            {
                //vxi11Device.write("from ascii"); // Line gives undefined header error
                vxi11Device.write("trac? tra");
                String response = vxi11Device.read();
                int count = 0;
                bool flag = response.Contains("\n");
                // Count how many lines in response
                while (!flag)
                {
                    String temp = vxi11Device.read();
                    response = response + temp;
                    flag = temp.Contains("\n");
                    count++;
                }
                return response;
            }
            catch (System.Exception)
            {
                throw new System.Exception("Agilent 86142 get trace failed");
            }
        }

        public String getTrace1(int pts)
        {
            String temp;
            List<int> elmCount = new List<int>();
            int count = 0;
            int itr = 0;
            try
            {
                
                vxi11Device.write("from ascii");
                vxi11Device.write("trac? tra");
                String response = vxi11Device.read();
                count += response.Split(',').Length;
                while (count < pts)
                {
                    temp = vxi11Device.read();
                    count += temp.Split(',').Length;
                    elmCount.Add(count);
                    response = response + temp;
                    itr++;
                }
                return response;
            }
            catch (System.Exception)
            {
                throw new System.Exception("Agilent 86142 get trace 1 failed");
            }
        }

        public double getTraceBin()
        {
            try
            {
                vxi11Device.write("form real32");
                vxi11Device.write("trac? tra");
                return Convert.ToDouble(vxi11Device.read());
            }
            catch (System.Exception)
            {
                throw new System.Exception("Agilent 86142 get trace bin failed");
            }
        }

    }
}