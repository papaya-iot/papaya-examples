# Python Examples
Included in this folder are libraries and test programs for each of the 3 
communication protocols.
Generic instrument classes are defined in files named `papaya_<protocol>inst.py`;
these can be imported and customized for specific instruments. Among these are:
```
python/
- papaya_visainst.py
- papaya_httpinst.py
- papaya_i2cinst.py
- papaya_i2chttpinst.py
- papaya_uartinst.py
- papaya_uarthttpinst.py
```

The remaining files are example files that implement these instrument classes.

Required Python Libraries
-------------------------------------
For the visa interface, PyVISA (1.10.1+) is required. See setup_visa.txt for further instructions.

For the http interface, please make sure to install:
- http
- urllib3 (1.25.3+)
- requests (2.22.0+)

Setup VISA on Windows
-------------------------------------
1. Make sure Python is installed, navigate to directory containing `python.exe`
    - With PyCharm, to find out path go to File>Settings>Project>Project Interpreter
    - Ex: `cd C:\ProgramData\pycharm_venv\venv_3_7\Scripts`
2. Install pip (if necessary)
    - Download get-pip.py from https://bootstrap.pypa.io/get-pip.py
    - In cmd, run `python get-pip.py`
    - Verify installation with `pip -V`
        - You may have to navigate to directory of pip.exe to do this
3. Run `pip install -U pyvisa`
4. Install [NI-VISA Library](https://pyvisa.readthedocs.io/en/latest/faq/getting_nivisa.html#windows)
5. Test a Python program that imports the VISA library

Setup VISA on Linux/MacOS
-------------------------------------
1. Install PyVISA with PIP: `pip install -U pyvisa`
2. Install [NI-VISA Library](https://pyvisa.readthedocs.io/en/latest/faq/getting_nivisa.html)
3. Test a Python program that imports the VISA library


Download NI-VISA
-------------------------------------
download ni-visa at http://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#305862
run package manager to install NI-Max
in visual studios, add reference
