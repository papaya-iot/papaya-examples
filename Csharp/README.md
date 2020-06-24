C# Examples
========================
Here, we define a C# driver (in `ppyGpibNetClass.cs`) that uses National Instrument VisaNS to communicate with the Papaya Controller.
A fully-functional demo GUI is provided that implements the driver to control and configure instruments connected through GPIB, I2C and RS232/UART.

A driver for Keithley 2400 Source Meter `Keithley2400vxi11.cs` is also included as an example.

Development Environment
------------------------
Visual Studio Versions: Microsoft Visual Studio Premium 2013 or Microsoft Visual Studio Community 2019

National Instrument VISA: There are two versions of NI-VISA implemented in separate folders.
- `visa_13p0/`: version 13.0
- `visa_19p0/`: version 19.0

If you do not have NI-VISA or NI-MAX (which requires VISA drivers) set up already, proceed with version 19.0.

Setup VISA for C++ GUI
------------------------
__Version 13.0__

_You must have this older version of NI-VISA previously installed._
1. Locate `VisaNS.dll` and `NationalInstruments.Common.dll` from NI-VISA's installation.
1. In Visual Studios, open the solution and revise references to point to the previously identified paths.
2. To run the application, build the code. You can also run the PapayaDemo1 executable from the Debug folder once the references are configured correctly.

__Version 19.0__

1. Download NI-VISA v19.0 [here](http://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#305862).
2. Locate `NationalInstruments.Visa.dll` and `Ivi.Visa.dll` from NI-VISA's installation. 
> The path will be similar to `C:\Program Files (x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\`
3. In Visual Studios, open the solution and find the references named _NationalInstruments.Visa_ and _Ivi.Visa.dll_, and make sure it points to the previously identified path.
4. To run the application, build the code. You can also run the PapayaDemo1 executable from the Debug folder once the references are configured correctly.
