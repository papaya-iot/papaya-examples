using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PapayaDemo
{
    class Agilent_33401_Test
    {
        public static void Main4()
        {
            Agilent_33401 agilent = new Agilent_33401("gpib0,27", "192.168.2.181");
            double acVoltage; double acCurrent; double dcVoltage; double dcCurrent;
            double twoWireRead; double fourWireRead; double measureDiode; double db;
            for (int i = 0; i < 10; i++)
            {
                acVoltage = agilent.acVoltage();
                Debug.WriteLine("AC Volt is " + acVoltage.ToString());

                acCurrent = agilent.acCurrent();
                Debug.WriteLine("AC Curr is " + acCurrent.ToString());

                dcVoltage = agilent.dcVoltage();
                Debug.WriteLine("DC Volt is " + dcVoltage.ToString());

                dcCurrent = agilent.acVoltage();
                Debug.WriteLine("DC Curr is " + dcCurrent.ToString());

                twoWireRead = agilent.twoWireRes();
                Debug.WriteLine("2 wire resistance is " + twoWireRead.ToString());

                fourWireRead = agilent.fourWireRes();
                Debug.WriteLine("4 wire resistance is " + fourWireRead.ToString());

                measureDiode = agilent.measureDiode();
                Debug.WriteLine("Diode reading is " + measureDiode.ToString());

                db = agilent.dBValue();
                Debug.WriteLine("dB reading is " + db.ToString());
                
            }
            // Functions to Test: setTwoWires, setFourWires, measureDiode
        }
    }
}
