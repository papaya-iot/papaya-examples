"""
Copyright (C) 2020 Piek Solutions LLC

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

"""

import pyvisa as visa
import time
import sys, traceback
import re as regex
import numpy as np


class VisaInstrument:

    def __init__(self, ip, gpib_address):
        """
        initialize visa instrument resource
        :param ip: (str) ip address of Papaya
        :param gpib_address: (str) GPIB address of instrument
        """
        resource_name = "TCPIP0::%s::inst%s::INSTR" % (ip, gpib_address)
        print(resource_name)
        rm = visa.ResourceManager()
        self.instr = rm.open_resource(resource_name)
        self.instr.timeout = 10000

    def close(self):
        self.instr.close()

    def cls(self):
        try:
            self.instr.write('*CLS')
        except ValueError:
            print('*CLS fails to clear')
            
    def _set_ESE(self, x):
        try:
            cmd = '*ESE ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print ('*ESE write fails')
            
    def _get_ESE(self, x):
        try:
            resp = self.instr.query('*ESE?')
            self._output = float(resp)
        except ValueError:
            print('*ESE query fails')
        return self._output
            
    ESE = property(_get_ESE, _set_ESE, "ESE property")
    
    def _set_SRE(self, x):
        try:
            cmd = '*SRE ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print ('*SRE write fails')
            
    def _get_SRE(self, x):
        try:
            resp = self.instr.query('*SRE?')
            self._output = float(resp)
        except ValueError:
            print('*SRE query fails')
        return self._output
            
    SRE = property(_get_SRE, _set_SRE, "SRE property")
    
    def queryIDN(self):
        try:
            data = self.instr.query('*IDN?')
            return data
        except ValueError:
            print('*IDN query fails')
             

class Keysight_N9030B(VisaInstrument):

    def getTrace(self, tra='TRACE1'):
        count = 0
        try:
            self.instr.write('trac:data? %s' %tra)
            resp = self.instr.read()
            flag = '\n' in resp
            while not flag:
                tmp = self.instr.read()
                resp += tmp
                flag = '\n' in tmp                
                count += 1
        except visa.VisaIOError:
            print('error getting trace')
            print(tmp)
            traceback.print_exc()
            sys.exit(3)
            
        ary = resp.split(',')
        dd = np.array([float(c) for c in ary])
        return dd

    def getTraceXY(self, tra='san1'):
        count = 0
        try:
            self.instr.write('fetch:%s?' %tra)
            resp = self.instr.read()
            flag = '\n' in resp
            while not flag:
                tmp = self.instr.read()
                resp += tmp
                flag = '\n' in tmp                
                count += 1
        except visa.VisaIOError:
            print('error getting xy trace')
            print(tmp)
            traceback.print_exc()
            sys.exit(3)
        ary = resp.split(',')
        dd = np.array([float(c) for c in ary])
        return dd  


class Anritsu_M4647A(VisaInstrument): 
        
    def sweepOnce(self):
        self.instr.write('TRS;WFS;HLD')
        time.sleep(11)
    
    def readSXX(self, fmt='OS11C'):
        try:
            self.instr.write(fmt)  # C here refers to calibrated
            resp = self.instr.read()
            s = regex.findall(r'^#\d+', resp)[0]  # get the first elm in string instead of list
            pos = int(s[1]) + 3
            _num = int(s[2:len(s)])  # total number of bytes to read
            resp = resp[pos:len(resp)]  # remove the header
            cnt = len(resp)
            while cnt < _num:
                tmp = self.instr.read()
                cnt += len(tmp)
                resp += tmp
        except visa.VisaIOError:
            traceback.print_exc()
            sys.exit(3)

        # make them into real numbers
        y = resp.split('\n')
        y = y[0:len(y)-1]  # last element is \n
        real = np.zeros(len(y), dtype=float)
        imag = np.zeros(len(y), dtype=float)
        for i_ in range(0, len(y)):
            valstr = y[i_].split(',')  # split into real and imag
            real[i_] = float(valstr[0])
            imag[i_] = float(valstr[1])
            
        c = real + 1.j*imag
        
        return c
    
    def freq(self):
        try:
            self.instr.write(':sens1:freq:data?')
            resp = self.instr.read()
            s = regex.findall(r'^#\d+', resp)[0]  # get the first elm in string instead of list
            pos = int(s[1]) + 3
            _num = int(s[2:len(s)])  # total number of bytes to read
            resp = resp[pos:len(resp)]  # remove the header
            cnt = len(resp)
            while cnt < _num:
                tmp = self.instr.read()
                cnt += len(tmp)
                resp += tmp
        except visa.VisaIOError:
            traceback.print_exc()
            sys.exit(3)
            
        y = resp.split('\n')
        y = y[0:len(y)-1]  # last element is \n
        val = np.array([float(c) for c in y])
        return val


class Keithley_2400(VisaInstrument):
           
    def sourcetype(self, type):
        if type == 'voltage':            
            self.instr.write(':SOUR:FUNC VOLT')
            self.instr.write(':SENS:FUNC "CURR"')
        elif type == 'current':
            self.instr.write(':SOUR:FUNC CURR')
            self.instr.write(':SENS:FUNC "VOLT"')
        
    def setvoltage(self, vb, curlimit=0.05):
        self.instr.write(':SENS:CURR:PROT %f' % curlimit)
        self.instr.write(':SOUR:VOLT:LEV %f' % vb)
    
    def querycurrent(self):
        try:
            self.instr.write(':FORM:ELEM CURR')
            cur = self.instr.query('READ?')
            c = float(cur)
        except ValueError:
            print('Keithley 2400 warning: current reading error...')
            print(cur)
            c = -1000        
        return float(c)
    
    def setcurrent(self, cur, vlimit=2):
        self.instr.write(':SENS:VOLT:PROT %f' % vlimit)
        self.instr.write(':SOUR:CURR:LEV %s' % cur)
        
    def _get_output(self):
        try:
            resp = self.instr.query(':OUTPUT?')
            self._output = float(resp)
        except ValueError:
            print('Keithley 2400 query fails')
        return self._output
        
    def _set_output(self, x):
        try:
            cmd = ':OUTPUT  ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Keithley 2400 write fails')
        self._output = x
        
    output = property(_get_output, _set_output, "output property")


class Agilent_E3631(VisaInstrument):
   
    def _get_outPutOnOff(self):
        try:
            resp = self.instr.query(':outp?')
            self._outputOnOff = resp
        except ValueError:
            print('Agilent E3631 query outp fails')
        return self._outputOnOff
  
    def _set_outPutOnOff(self, x):
        try:
            cmd = 'outp ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 write outp fails')
        self._outputOnOff = x
      
    outputOnOff = property(_get_outPutOnOff, _set_outPutOnOff, "outputOnOff property")
        
    def queryCurrent(self):
        try:
            resp=self.instr.query(':meas:curr:dc?')
        except ValueError:
            print('Agilent E3631 query current fails')
        return float(resp)
    
    def queryVoltage(self):
        try:
            resp=self.instr.query(':meas:volt:dc?')
        except ValueError:
            print('Agilent E3631 query voltage fails')
        return float(resp)

    def selectPowerSupply(self, x):
        """
        select power supply instrument,
        :param x: (int) 1 is P6V, 2 is P25V and 3 is N25V
        :return: none
        """
        try:
            cmd = 'INST:NSEL ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 select power supply fails')

    def setP6VSupply(self, x):
        try:
            # P6V is 1
            self.instr.write('INST:NSEL 1')
            cmd = 'volt ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 set P6V fails')

    def queryP6VSetVoltage(self):
        try:
            # P6V is 1
            self.instr.write('INST:NSEL 1')
            val = self.instr.query('volt?')
        except ValueError:
            print('Agilent E3631 query P6V fails')
        return float(val)

    def setP25VSupply(self,x):
        try:
            # P25V is 2
            self.instr.write('INST:NSEL 2')
            cmd = 'volt ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 set P25V fails')

    def queryP25VSetVoltage(self):
        try:
            # P25V is 2
            self.instr.write('INST:NSEL 2')
            val = self.instr.query('volt?')
        except ValueError:
            print('Agilent E3631 query P25V fails')
        return float(val)

    def setN25VSupply(self, x):
        # N25V is 3
        try:
            self.instr.write('INST:NSEL 3')
            cmd = 'volt ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 set N25V fails')

    def queryN25VSetVoltage(self):
        # N25V is 3
        try:
            self.instr.write('INST:NSEL 3')
            val = self.instr.query('volt?')
        except ValueError:
            print('Agilent E3631 query N25V fails')
        return float(val)


class Keysight_E3649A(VisaInstrument):

    def _get_outputOnOff(self):
        """
        query output state
        :return: 0(OFF) or 1(ON)
        """
        try:
            resp = self.instr.query('OUTP?')
            self._outputOnOff = resp.rstrip()
        except ValueError:
            print('Agilent E3649A query outp on/off fails')
        return self._outputOnOff

    def _set_outputOnOff(self, x):
        """
        turn output on or off
        :param x: either ON or OFF
        :return: None
        """
        try:
            self.instr.write('OUTP ' + str(x))
        except ValueError:
            print('Agilent E3649A write outp on/off fails')
        self._outputOnOff = x

    outputOnOff = property(_get_outputOnOff, _set_outputOnOff, "outputOnOff property")

    def queryCurrent(self, output_num=None):
        """
        query current of selected output
        :param output_num: (int) the output to query (None|1|2);
            default value None uses the output previously set.
        :return: (float) current
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            resp = self.instr.query('MEAS:CURR:DC?')
            return float(resp)
        except visa.VisaIOError or ValueError:
            print('Agilent E3649A query current fails')

    def setCurrent(self, curr, output_num=None):
        """
        query current of selected output
        :param curr: (float) the desired current level
        :param output_num: (int) the output to query (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write('CURR ' + str(curr))
        except visa.VisaIOError or ValueError:
            print('Agilent E3649A query current fails')

    def queryVoltage(self, output_num=None):
        """
        query voltage of selected output
        :param output_num: (int) the output to read (None|1|2);
            default value None uses the output previously set.
        :return: (float) voltage
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            resp = self.instr.query('MEAS:VOLT:DC?')
            return float(resp)
        except visa.VisaIOError or ValueError:
            print('Agilent E3649A query voltage fails')

    def setVoltage(self, volt, output_num=None):
        """
        set voltage of selected output
        :param volt: (float) the desired voltage level
        :param output_num: (int) the output to set (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write('VOLT ' + str(volt))
        except visa.VisaIOError or ValueError:
            print('Agilent E3649A set voltage fails')

    def selectOutput(self, output_num):
        """
        select which output to modify
        :param output_num: (int) the output to modify (1|2)
        :return: None
        """
        try:
            self.instr.write('INST:NSEL ' + str(output_num))
        except visa.VisaIOError:
            print('Agilent E3649A select output fails')

    def queryOutputRange(self, output_num=None):
        """
        query range setting of selected output
        :param output_num: (int) the output to read (None|1|2);
            default value None uses the output previously set.
        :return: (str) P35V or P60V
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            resp = self.instr.query(':VOLT:RANG?')
            return resp.rstrip()
        except visa.VisaIOError:
            print('Agilent E3649A query output range fails')

    def setOutputRange(self, volt_range, output_num=None):
        """
        set voltage range of selected output
        :param volt_range: the voltage range to set output to (P35V|LOW|P60V|HIGH)
        :param output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write(':VOLT:RANG ' + str(volt_range))
        except visa.VisaIOError:
            print('Agilent E3649A set output voltage fails')

    def setOutputLow(self, output_num=None):
        """
        set voltage range of selected output to 35V
        :param output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write(':VOLT:RANG LOW')
        except visa.VisaIOError:
            print('Agilent E3649A set output voltage LOW fails')

    def setOutputHigh(self, output_num=None):
        """
        set voltage range of output to 60V
        :param output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write(':VOLT:RANG HIGH')
        except visa.VisaIOError:
            print('Agilent E3649A set output voltage HIGH fails')

    def enableVoltageProtection(self, enable=1, output_num=None):
        """
        enable or disable the overvoltage protection function.
        :param enable: (0|1|OFF|ON)
        :param output_num: output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write(':VOLT:PROT:STAT ' + str(enable))
        except visa.VisaIOError:
            print('Agilent E3649A enable voltage protection fails')

    def setVoltageProtection(self, volt, output_num=None):
        """
        set the voltage level at which the overvoltage protection
        (OVP) circuit will trip.
        :param volt:  voltage level, 'MIN', or 'MAX'
        :param output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: None
        """
        try:
            if output_num:
                self.instr.write('INST:NSEL ' + str(output_num))
            self.instr.write(':VOLT:PROT ' + str(volt))
        except visa.VisaIOError:
            print('Agilent E3649A set output voltage protection fails')

    def queryVoltageProtection(self, output_num=None):
        """
        query the protection state and voltage level at which the
        overvoltage protection (OVP) circuit will trip.
        :param output_num: (int) the output to modify (None|1|2);
            default value None uses the output previously set.
        :return: tuple (int, str) consisting of enable 0 (OFF) or 1 (ON)
            and the voltage trip level.
        """
        try:
            ena = self.instr.query('VOLT:PROT:STAT?')
            level = self.instr.query('VOLT:PROT?')
            return ena.rstrip(), level.rstrip()
        except visa.VisaIOError:
            print('Agilent E3649A query output voltage protection fails')


class Agilent_33401(VisaInstrument):

    def acVoltage(self):
        try:
            self.instr.write(':meas:volt:ac?')
            resp = self.instr.read()
            return float(resp)
        except ValueError:
            print('Agilent 33401 query ac volt fails')

    def acCurrent(self):
        try:
            self.instr.write(':meas:curr:ac?')
            resp = self.instr.read()
            return float(resp)
        except ValueError:
            print('Agilent 33401 query ac curr fails')

    def dcVoltage(self):
        try:
            self.instr.write(':meas:volt:dc?')
            resp = self.instr.read()
            return float(resp)
        except ValueError:
            print('Agilent 33401 query dc volt fails')

    def dcCurrent(self):
        try:
            self.instr.write(':meas:curr:dc?')
            resp = self.instr.read()
            return float(resp)
        except ValueError:
            print('Agilent 33401 query dc curr fails')

    
class Keithley_2510(VisaInstrument):

    def querytemp(self):
        try:
            self.instr.write(':MEAS:TEMP?')
            temp = self.instr.read()
            t = float(temp)
        except ValueError:
            print('Keithley 2510 warning: temp read error...')
            print(temp)
            t = -1000
        return float(t)    
    
    def settemp(self, setT='25'):
        self.instr.write(':SOUR:TEMP %f' % setT)
        
    def _get_output(self):
        try:
            resp = self.instr.query(':OUTPUT?')
            self._output = float(resp)
        except ValueError:
            print('Keithley 2510 query outp fails')
        return self._output
        
    def _set_output(self, x):
        try:
            cmd = ':OUTPUT  ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Keithley 2510 write outp fails')
        self._output = x
        
    output = property(_get_output, _set_output, "output property")
    

class Newport_3150(VisaInstrument):
        
    def querytemp(self):
        temp = self.instr.query(':TEC:T?')
        try:
            t = float(temp)
        except ValueError:
            print('Newport 3150 warning: temp read error...')
            print(temp)
            t = -1000
        return float(t)    
    
    def settemp(self, setT='25'):
        self.instr.write(':TEC:T %f' % setT)


class Agilent_8163(VisaInstrument):
    
    def queryIDN(self):
        try:
            resp = self.instr.query('*IDN?')
        except ValueError:
            print('Agilent 8163 fails query')
        return resp
    
    def querypower(self):
        try:
            opt = self.instr.query('READ:POW?')
        except ValueError:
            print('Agilent 8163 fails query')
        return float(opt)


class Keysight_Dca(VisaInstrument):

    def initialize(self):  # initiallize for PAM4 measurement
        pass
    
    def get_er(self, source='1', ch='2A'):
        cmd = ':MEASure:EYE:OER:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        try:
            er = self.instr.query(':MEASure:EYE:OER?')
            return float(er)
        except ValueError:
            print('Keysight dca error')
    
    def getOMA(self, source='1', ch='2A'):
        cmd = ':MEASure:EYE:OOMA:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        try:
            oma = self.instr.query(':MEASure:EYE:OOMA?')
            return float(oma)
        except ValueError:
            print('Keysight dca error')
    
    def getRLM(self, source='1', ch='2A'):
        cmd = ':MEASure:EYE:PAM:LINearity:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        try:
            rlm = self.instr.query(':MEASure:EYE:PAM:LINearity?')
            return float(rlm)
        except ValueError:
            print('Keysight dca error')
    
    def autoscale(self):
        self.instr.write(':SYSTem:AUToscale')
        try:
            self.instr.query('*OPC?')
        except ValueError:
            print('Keysight dca error')
    
    def clear(self):
        self.instr.write(':ACQuire:CDISplay')
        try:
            self.instr.query('*OPC?')
        except ValueError:
            print('Keysight dca error')
    
    def run(self):
        self.instr.write(':ACQuire:RUN')


class Agilent_86142(VisaInstrument):

    def _get_startWavelength(self):
        try:
            resp = self.instr.query(':sens:wav:star?')
            self._startWavelength = float(resp)
        except ValueError:
            print('Agilent 86142 query fails')
        return self._startWavelength
  
    def _set_startWavelength(self, x):
        try:
            cmd = ':sens:wav:star ' + str(x)
            self.instr.write(cmd)
            self._startWavelength = x
        except visa.VisaIOError:
            print('Agilent 86142 write fails')

    startWavelength = property(_get_startWavelength, _set_startWavelength, "startWavelength property")
    
    def _get_stopWavelength(self):
        try:
            resp = self.instr.query(':sens:wav:stop?')
            self._startWavelength = float(resp)
        except ValueError:
            print('Agilent 86142 query fails')
        return self._startWavelength
  
    def _set_stopWavelength(self, x):
        try:
            cmd = ':sens:wav:stop ' + str(x)
            self.instr.write(cmd)
            self._stopWavelength = x
        except visa.VisaIOError:
            print('Agilent 86142 write fails')

    stopWavelength = property(_get_stopWavelength, _set_stopWavelength, "stopWavelength property")
  
    def _get_traceLength(self):
        try:
            resp = self.instr.query(':SENS:SWE:POIN?')
            self._traceLength = float(resp)
        except ValueError:
            print('Agilent 86142 query fails')
        return self._traceLength
  
    def _set_traceLength(self, x):
        try:
            cmd = ':SENS:SWE:POIN  ' + str(x)
            self.instr.write(cmd)
            self._traceLength = x
        except ValueError:
            print('Agilent 86142 write fails')

    traceLength = property(_get_traceLength, _set_traceLength, "traceLength property")
 
    def getTrace(self):
        tmp = ''
        try:
            self.instr.write('form ascii')
            self.instr.write('trac? tra')
            resp = self.instr.read()
            flag = '\n' in resp
            count = 0
            while not flag:
                tmp = self.instr.read()
                resp += tmp
                flag = '\n' in tmp
                count += 1
        except visa.VisaIOError:
            print('error')
            print(tmp)
            traceback.print_exc()
            sys.exit(3)
        return resp
    
    def getTrace1(self, pts):
        tmp = ''
        elmcount = []
        count = 0
        itr=0
        try:
            self.instr.write('form ascii')
            self.instr.write('trac? tra')
            resp = self.instr.read()
            count += len(resp.split(','))
            while count < pts:
                tmp = self.instr.read()
                count += len(tmp.split(','))
                elmcount.append(count)
                resp += tmp
                itr += 1
        except visa.VisaIOError:
            print('error')
            print(tmp)
            traceback.print_exc()
            sys.exit(3)
        return resp
    
    def getTraceBin(self):
        try:
            self.instr.write('form real32')
            self.instr.write('trac? tra')
            resp = self.instr.read()
            return resp
        except ValueError:
            print('Agilent 86142 write fails')


class JDSU_HA9(VisaInstrument):
    _attenuation = 0
    _beamIsBlocked = 0
    
    def _get_attenuation(self):
        try:
            resp = self.instr.query('att?')
            self._attenuation = float(resp)
        except ValueError:
            print('JDSU HA9 query fails')
        return self._attenuation
  
    def _set_attenuation(self, x):
        try:
            cmd = 'att ' + str(x)
            self.instr.write(cmd)
            self._attenuation = x
        except ValueError:
            print('JDSU HA9 write fails')

    attenuation = property(_get_attenuation, _set_attenuation, "attenuation property")
    
    def _get_beamIsBlocked(self):
        try:
            resp = self.instr.query('D?')
            self._beamIsBlocked = int(resp)
        except ValueError:
            print('JDSU HA9 query fails')
        return self._beamIsBlocked
  
    def _set_beamIsBlocked(self, x):
        try:
            cmd = 'D ' + str(int(x))
            self.instr.write(cmd)
            self._beamIsBlocked = int(x)
        except ValueError:
            print('JDSU HA9 write fails')

    beamIsBlocked = property(_get_beamIsBlocked, _set_beamIsBlocked, "beamIsBlock property")


class N9020A_SpectrumAnalyzer(VisaInstrument):
    _inputCoupling = 'DC'  # default
    _bandwidthResolution_MHz = 0.5
    _bandwidthVideo_MHz = 10
    _sweepPoints = 1001
    _startFreqMHz = 10e-3
    _stopFreqMHz = 1350
    _traceAve = 1
    _contSweep = 0
    
    def _set_contSweep(self, x=1):
        try:
            cmd = ':INIT:CONT ' + str(x)
            self.instr.write(cmd)
            self._contSweep = str(x)
        except ValueError:
            print('N9020A fails to set cont sweep config')

    def _get_contSweep(self):
        try:
            resp = self.instr.query(':INIT:CONT?')
            self._contSweep=resp
        except ValueError:
            print('N9020A fails to get cont sweep config')
        return self._contSweep
    contSweep = property(_get_contSweep, _set_contSweep, 'input coupling property')
    
    def _set_inputCoupling(self, x='DC'):
        try:
            cmd = 'INPut:COUPling ' + str(x)
            self.instr.write(cmd)
            self._inputCoupling = str(x)
        except ValueError:
            print('N9020A fails to set input coupling')

    def _get_inputCoupling(self):
        try:
            resp = self.instr.query('INP:COUP?')
            self._inputCoupling = resp
        except ValueError:
            print('N9020A fails to get input coupling')
        return self._inputCoupling
    inputCoupling = property(_get_inputCoupling, _set_inputCoupling, 'input coupling property')
    
    def _set_bandwidthResolution_MHz(self,x=0.5):
        try:
            cmd = 'BANDWIDTH:RESOLUTION ' + str(x) + ' MHZ'
            self.instr.write(cmd)
            self._bandwidthResolution_MHz = float(x)
        except ValueError:
            print('N9020A fails to set bandwidth resolution')

    def _get_bandwidthResolution_MHz(self):
        try:
            resp = self.instr.query('BANDWIDTH:RESOLUTION?')
            self._bandwidthResolution_MHz = float(resp)/1e6  # in MHz
        except ValueError:
            print('N9020A fails to get bandwidth resolution')
        return self._bandwidthResolution_MHz
    
    resolutionBW_MHz = property(_get_bandwidthResolution_MHz, _set_bandwidthResolution_MHz, 'bandwidth resolution property')
    
    def _set_bandwidthVideo_MHz(self, x=0.5):
        try:
            cmd = 'BANDWIDTH:VIDEO ' + str(x) + ' MHZ'
            self.instr.write(cmd)
            self._bandwidthResolution_MHz = float(x)
        except ValueError:
            print('N9020A fails to set video bandwidth')

    def _get_bandwidthVideo_MHz(self):
        try:
            resp = self.instr.query('BANDWIDTH:VIDEO?')
            self._bandwidthResolution_MHz = float(resp)/1e6  # in MHz
        except ValueError:
            print('N9020A fails to get video bandwidth')
        return self._bandwidthResolution_MHz
    
    videoBW_MHz = property(_get_bandwidthVideo_MHz, _set_bandwidthVideo_MHz, 'video bandwidth property')
    
    def _set_sweepPoints(self,x=1001):
        try:
            cmd = 'SWEEP:POINTS ' + str(x)
            self.instr.write(cmd)
            self._sweepPoints = int(x)
        except ValueError:
            print('N9020A fails to set sweep points')

    def _get_sweepPoints(self):
        try:
            resp = self.instr.query('SWEEP:POINTS?')
            self._sweepPoints = int(resp)  # in MHz
        except ValueError:
            print('N9020A fails to get sweep points')
        return self._sweepPoints
    
    sweepPoints = property(_get_sweepPoints, _set_sweepPoints, 'sweep points')

    def _set_startFreqMHz(self,x=10e-3):
        try:
            cmd = 'FREQUENCY:START ' + str(x) + ' MHZ'
            self.instr.write(cmd)
            self._startFreqMHz = float(x)
        except ValueError:
            print('N9020A fails to set start frequency')

    def _get_startFreqMHz(self):
        try:
            resp = self.instr.query('FREQUENCY:START?')
            self._startFreqMHz = float(resp)/1e6  # in MHz
        except ValueError:
            print('N9020A fails to get stop frequency')
        return self._startFreqMHz
    
    startFreqMHz = property(_get_startFreqMHz, _set_startFreqMHz,'start frequency property')
    
    def _set_stopFreqMHz(self, x=13.5e3):
        try:
            cmd = 'FREQUENCY:STOP ' + str(x) + ' MHZ'
            self.instr.write(cmd)
            self._stopFreqMHz = float(x)
        except ValueError:
            print('N9020A fails to set start frequency')

    def _get_stopFreqMHz(self):
        try:
            resp = self.instr.query('FREQUENCY:STOP?')
            self._stopFreqMHz = float(resp)/1e6  # in MHz
        except ValueError:
            print('N9020A fails to get stop frequency')
        return self._stopFreqMHz
    
    stopFreqMHz = property(_get_stopFreqMHz, _set_stopFreqMHz, 'start frequency property')
    
    def _set_traceAve(self, x=1):
        try:
            if x >= 1:
                cmd = 'ACP:AVER:COUN ' + str(x)
                self.instr.write(cmd)
            if x == 0:
                self.instr.write('ACPower:AVERage OFF')
            self._traceAve = int(x)
        except ValueError:
            print('N9020A fails to set trace average')

    def _get_traceAve(self):
        try:
            resp = self.instr.query('ACPower:AVERage:COUNt?')
            self._traceAve = int(resp)
        except ValueError:
            print('N9020A fails to get stop frequency')
        return self._traceAve

    traceAve = property(_get_traceAve, _set_traceAve, 'trace average')
    
    def getTrace(self):
        _points = self._get_sweepPoints()
        _stopf = self._get_stopFreqMHz()
        _startf = self._get_startFreqMHz()
        _freq = np.linspace(_startf, _stopf, _points)
        tmp = ''
        try:
            self.instr.write('FORMAT:TRACE:DATA ASCII')
            self.instr.write('TRAC? TRACE1')
            resp = self.instr.read()
            flag = '\n' in resp
            count = 0
            while not flag:
                tmp = self.instr.read()
                resp += (tmp)
                flag = '\n' in tmp
                count += 1
        except visa.VisaIOError:
            print('N9020A get trace error')
            print(tmp)
            resp = tmp
            traceback.print_exc()
            sys.exit(3)
        resp = resp.split(',')
        y = [float(d) for d in resp]
        y = np.array(y)
        return _freq, y
    
    def setMarkerPos(self,pos=0):
        
        _points = self._get_sweepPoints()
        cmd = 'calc:mark1:X:pos:cent ' + str(pos)
        try:
            if pos < _points:
                self.instr.write(cmd)
        except visa.VisaIOError:
            print('N9020A write error: ' + cmd)
        
    def getMarkerNoise(self, pos=0):
        # cmd = 'CALC:MARK:FUNCNOIS'
        try:
            # self.instr.write(cmd)
            self.setMarkerPos(pos)
            val = self.instr.query('CALC:MARK:Y?')
            return float(val)
        except visa.VisaIOError:
            print('N9020A getMarkerNoise error')

    def getMarkerNoiceTrace(self):
        _points = self._get_sweepPoints()
        _stopf = self._get_stopFreqMHz()
        _startf = self._get_startFreqMHz()
        _freq = np.linspace(_startf, _stopf, _points)
        try:
            self.instr.write('CALC:MARK:FUNCNOIS')
            _points = self._get_sweepPoints()
        except visa.VisaIOError:
            print('N9020A getMarkerNoiceTrace error')
                
        # preallocate array
        data = np.zeros(_points, dtype=float)
        try:
            for i in range(0, _points,1):
                self.instr.write('calc:mark1:X:pos:cent %d' % i)
                val = self.instr.query('CALC:MARK:Y?')
                data[i] = float(val)
        except ValueError:
            print('N9020A getMarkerNoiceTrace error')
        return _freq, data
    
    def setTraceType(self, x='WRITe'):
        try:
            cmd = 'trace1:type %s' % x
            self.instr.write(cmd)
        except visa.VisaIOError:
            print('N9020A trace type write error %s' % x)
            
    def getTraceType(self):
        try:
            cmd = 'trace1:type?'
            resp = self.instr.query(cmd)
        except visa.VisaIOError:
            print('N9020A trace type query error')
        return resp


class Agilent_86122A(VisaInstrument):
    
    def getFreq(self):
        try:
            self.instr.write(':MEAS:SCAL:POW:FREQ?')
            resp = float(self.instr.read())
            return resp
        except visa.VisaIOError:
            print('Agilent 86122A error')

    def getMultipleFreq(self):
        try:
            self.instr.write(':MEAS:ARR:POW:FREQ?')
            resp = self.instr.read()
            return resp
        except visa.VisaIOError:
            print('Agilent 86122A error')


class Agilent_N5183B(VisaInstrument):
    
    def _get_outPutOnOff(self):
        try:
            resp = self.instr.query(':outp?')
            self._outputOnOff = resp
        except ValueError:
            print('Agilent N5183B query fails')
        return self._outputOnOff
  
    def _set_outPutOnOff(self, x):
        try:
            cmd = 'outp ' + str(x)
            self.instr.write(cmd)
            self._outputOnOff = x
        except ValueError:
            print('Agilent N5183B write fails')

    outputOnOff = property(_get_outPutOnOff, _set_outPutOnOff, "outputOnOff property")
    
    def setFreq(self, freq_Hz=1000000):
        try:
            cmd = ':freq ' + str(freq_Hz)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent N5183B write fails')
        
    def getFreq(self):
        try:
            resp = self.instr.query(':outp?')
            return float(resp)
        except ValueError:
            print('Agilent N5183B write fails')

    def setPowerLevel(self, pow_dBm=-20.0):
        try:
            cmd = ':pow:lev %d' % pow_dBm
            self.instr.write(cmd)
        except ValueError:
            print('Agilent N5183B write fails')
            
    def getPowerLevel(self):
        try:
            cmd = ':pow:lev?'
            resp = self.instr.query(cmd)
            return float(resp)
        except ValueError:
            print('Agilent N5183B query fails')

    
class SRS(VisaInstrument): 
    _pidPolarity = 0
    _pidLoop = 0
    
    def PIDConnect(self):
        try:
            self.instr.write('CONN 7, \"ZZZ\"')
            time.sleep(1)
        except ValueError:
            print('SRS Connect fails') 
        
    def PIDDiscoonect(self):
        try:
            self.instr.write('\"ZZZ\"')
        except ValueError:
            print('SRS Disconnect fails')
        
    def _PIDPolaritySet(self, pol=0):
        try:
            self.instr.write('APOL %d' % int(pol))
            self.instr._pidPolarity = int(pol)
        except ValueError:
            print('SRS APOL set fails')

    def _PIDPolarityGet(self):
        try:
            resp = self.instr.query('APOL?')
            self._pidPolarity = int(resp)
        except ValueError:
            print('SRS APOL set fails')
        return self._pidPolarity
    
    PIDPolarity = property(_PIDPolarityGet, _PIDPolaritySet, 'PID Polarity')
    
    def _setPIDLoop(self, loop=0):
        try:
            self.instr.write('AMAN %d' % int(loop))
        except ValueError:
            print('SRS AMAN set fails')
        self._pidLoop = int(loop)
    
    def _getPIDLoop(self):
        try:
            resp = self.instr.query('AMAN?')
            self._pidLoop = int(resp)
        except ValueError:
            print('SRS AMAN get fails')
        return self._pidLoop
    
    PIDLoop = property(_getPIDLoop, _setPIDLoop, 'PID Loop on/off')
    
    def setMout(self, val=0):
        cmd = 'MOUT %f' % val
        print('setting Mout %s' % cmd)
        try:
            self.instr.write(cmd)
        except ValueError:
            print('SRS MOUT set fails')
            
    def getMout(self):
        try:
            resp = self.instr.query('MOUT?')
            return float(resp)
        except ValueError:
            print('SRS MOUT get fails')


class Agilent8163A(VisaInstrument):

    def setVoa(self, x):
        try:
            cmd = ':INPUT1:CHAN1:att ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent 8163A write fails')
            
    def getVoa(self):
        try:
            cmd = ':INPUT1:CHAN1:att?'
            val = self.instr.query(cmd)
            return float(val)
        except ValueError:
            print('Agilent 8163A query fails')
            
    def getOpm(self, ch):
        try:
            self.instr.write('*CLS')
            power = self.instr.query(':FETC2:CHAN{}:POW? '.format(ch))
            return float(power)
        except ValueError:
            print('Agilent 8163A query error')

    def initOpm(self):
        try:
            self.instr.write('*CLS')
            for i in range(1, 2):
                self.write(':SENS2:CHAN{}:POW:WAV 1550.0nm'.format(i))
                self.write(':SENS2:CHAN{}:POW:ATIM 200ms'.format(i))
        except ValueError:
            print('Agilent 8163A write error')
