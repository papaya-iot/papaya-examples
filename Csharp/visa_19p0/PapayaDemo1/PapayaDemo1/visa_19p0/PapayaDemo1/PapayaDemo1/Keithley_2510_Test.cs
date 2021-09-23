using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PapayaDemo
{
    public class Keithley_2510_Test
    {
        static void Main2()
        {
            Keithley_2510 keithley = new Keithley_2510("gpib0,5", "192.168.2.181");
            testTemp(keithley);
            //testOutput(keithley);
        }

        public static void testTemp(Keithley_2510 keithley)
        {
            double tempOut = 0.0;
            for (int i = 0; i < 5; i++)
            {
                keithley.temp = Convert.ToDouble(i);
                tempOut = keithley.temp;
                Debug.WriteLine("Keithley Test, Temp is " + Convert.ToString(tempOut));
            }
        }

        public static void testOutput(Keithley_2510 keithley)
        {
            double queryResult; // Valid outputs are 1 and 0
            for (int i = 0; i < 5; i++)
            {
                keithley.output = Convert.ToDouble(i % 2);
                queryResult = keithley.output;
                Debug.WriteLine("Keithley Test, Output is " + Convert.ToString(queryResult));
            }
        }
    }
}
