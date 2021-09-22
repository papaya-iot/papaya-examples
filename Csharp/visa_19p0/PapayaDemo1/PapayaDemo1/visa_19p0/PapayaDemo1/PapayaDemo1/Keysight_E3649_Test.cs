using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PapayaDemo
{
    public class Keysight_E3649_Test
    {
        public static void mainTestKeysightE3649()
        {
            Keysight_E3649A keysight = new Keysight_E3649A("gpib0,5", "192.168.2.181");
            testOnOff(keysight);
            testCurrent(keysight);
            testVoltage(keysight);
            testOutputRange(keysight);
            testVoltageProtection(keysight);
            testOutputLowHigh(keysight);
        }

        public static void testOnOff(Keysight_E3649A keysight)
        {
            // Testing whether output is on or off
            for (int i = 0; i < 30; i++)
            {
                keysight.outputOnOff = false;
                bool onOffTest1 = keysight.outputOnOff;
                keysight.outputOnOff = true; // Turn output on
                bool onOffTest2 = keysight.outputOnOff;
                Debug.WriteLine("onOff 1 = " + Convert.ToString(onOffTest1) +
                                "onOff 2 = " + Convert.ToString(onOffTest2));
            }
        }

        public static void testCurrent(Keysight_E3649A keysight)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    keysight.setCurrent(Convert.ToDouble(j), i);
                    Debug.WriteLine(Convert.ToString(keysight.queryCurrent()));
                }
            }
        }

        public static void testVoltage(Keysight_E3649A keysight)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    keysight.setVoltage(Convert.ToDouble(j), i);
                    Debug.WriteLine(Convert.ToString(keysight.queryVoltage()));
                }
            }
        }

        public static void testOutputRange(Keysight_E3649A keysight)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    keysight.setOutputRange(j, i);
                    Debug.WriteLine(Convert.ToString(keysight.queryOutputRange(j)));
                }
            }
        }

        public static void testVoltageProtection(Keysight_E3649A keysight)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    keysight.enableVoltageProtection((j % 2), i);
                    keysight.setVoltageProtection(Convert.ToDouble(j), i);
                    Debug.WriteLine(Convert.ToString(keysight.queryVoltageProtection(j)));
                }
            }
        }

        public static void testOutputLowHigh(Keysight_E3649A keysight)
        {
            for (int i = 1; i < 4; i++)
            {
                // Need to add functions or find way to check these get set correctly
                keysight.setOutputLow(i);
                keysight.setOutputHigh(i);
            }
        }
    }
}
