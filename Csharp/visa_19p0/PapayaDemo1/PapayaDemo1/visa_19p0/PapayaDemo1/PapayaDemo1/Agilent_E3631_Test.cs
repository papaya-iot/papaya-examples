using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PapayaDemo
{
    public class Agilent_E3631_Test
    {
        static void mainTestAgilentE3631()
        {
            Agilent_E3631 agilentTest = new Agilent_E3631("gpib0,5", "192.168.2.181");
            testOnOffAgilentE3631(agilentTest);
            setPowerSuppliesAgilentE3631(agilentTest);
        }
        public static void testOnOffAgilentE3631(Agilent_E3631 agilentTest)
        {
            // Testing whether output is on or off
            for (int i = 0; i < 30; i++)
            {
                agilentTest.outputOnOff = false;
                bool onOffTest1 = agilentTest.outputOnOff;
                agilentTest.outputOnOff = true; // Turn output on
                bool onOffTest2 = agilentTest.outputOnOff;
                Debug.WriteLine("onOff 1 = " + Convert.ToString(onOffTest1) +
                                "onOff 2 = " + Convert.ToString(onOffTest2));
            }
        }

        public static void setPowerSuppliesAgilentE3631(Agilent_E3631 agilentTest)
        {
            // Set the voltage to 3.0, then read it (P6 Supply)
            for (int i = 0; i < 6; i++)
            {
                agilentTest.P6Supply = Convert.ToDouble(i);
                double P6Output = agilentTest.P6Supply;
                Debug.WriteLine("P6 Voltage is " + P6Output);
            }
            // agilentTest.setP6Supply(7.0) // Outside range so should throw exception
            for (int i = 0; i < 25; i++)
            {
                agilentTest.P25Supply = Convert.ToDouble(i);
                double P25Output = agilentTest.P25Supply;
                Debug.WriteLine("P25 Voltage is " + P25Output);
            }

            for (int i = 0; i > -25; i--)
            {
                agilentTest.N25Supply = Convert.ToDouble(i);
                double N25Output = agilentTest.N25Supply;
                Debug.WriteLine("N25 Voltage is " + N25Output);
            }

        }
    }
}
