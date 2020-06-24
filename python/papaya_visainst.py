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
import sys,traceback
import re as regex
import numpy as np


class VisaInstrument:

    def __init__(self, ip, gpib_address):      
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
            
    def _get_ESE(self,x):
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
            
    def _get_SRE(self,x):
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
        
    def getTrace(self,tra='TRACE1'):
        flag = False
        count = 0
        try:
            self.instr.write('trac:data? %s' %tra)
            resp = self.instr.read()
            flag = '\n' in resp
            while (not(flag)):
                tmp = self.instr.read()
                #print('length %i and count %i'% (len(tmp),count))
                resp += (tmp)
                flag = '\n' in tmp                
                count += 1
        except visa.VisaIOError:
            print('error')
            print(tmp)
            resp = tmp
            traceback.print_exc()
            sys.exit(3)
            
        ary = resp.split(',')
        dd =np.array([float(c) for c in ary])
        return dd   


class Anritsu_M4647A(VisaInstrument): 
        
    def sweepOnce(self):
        self.instr.write('TRS;WFS;HLD')
        time.sleep(11)
    
    def readSXX(self,fmt='OS11C'):
        #fmt: OS11C, OS11R,OS12C, OS12R, OS21C, OS21R, OS22C, OS22R
        
        #self.instr.write(':calc1:form:s1p:port port1')
        #set to real imag
        #self.instr.write(':calc1:form %s'%fmt)
        #self.instr.write('OFD')
        #:sens1:freq:data?
        try:
            self.instr.write(fmt) # C here refers to calibrated
            resp = self.instr.read()
            s = regex.findall(r'^#\d+',resp)[0] # get the first elment in string instead of list
            pos = int(s[1]) + 3
            _num =int(s[2:len(s)])  # total number of bytes to read
            resp = resp[pos:len(resp)] # remove the header
            cnt = len(resp)
            while (cnt < _num):
                tmp = self.instr.read()
                cnt += len(tmp)
                resp += tmp
                #print (cnt)
        except visa.VisaIOError:
            traceback.print_exc()
            sys.exit(3)
            
        # make them into real numbers
        y = resp.split('\n')
        y = y[0:len(y)-1] # last element is \n
        real =np.zeros(len(y),dtype=float)
        imag = np.zeros(len(y),dtype=float)
        for i_ in range(0,len(y)):
            valstr = y[i_].split(',') # split into real and imag
            real[i_] = float(valstr[0])
            imag[i_] = float(valstr[1])
            
        c = real + 1.j*imag
        
        return c
    
    def freq(self):
        try:
            self.instr.write(':sens1:freq:data?')
            resp = self.instr.read()
            s = regex.findall(r'^#\d+',resp)[0] # get the first elment in string instead of list
            pos = int(s[1]) + 3
            _num =int(s[2:len(s)])  # total number of bytes to read
            resp = resp[pos:len(resp)] # remove the header
            cnt = len(resp)
            while (cnt < _num):
                tmp = self.instr.read()
                cnt += len(tmp)
                resp += tmp
        except visa.VisaIOError:
            traceback.print_exc()
            sys.exit(3)
            
        y = resp.split('\n')
        y = y[0:len(y)-1] # last element is \n
        val = np.array([float(c) for c in y])
        return val
        
class Keithley_2400(VisaInstrument):
           
    def sourcetype(self,type):
        if type == 'voltage':            
            self.instr.write(':SOUR:FUNC VOLT')
            self.instr.write(':SENS:FUNC "CURR"') # Sensing current
        elif type == 'current':
            self.instr.write(':SOUR:FUNC CURR')
            self.instr.write(':SENS:FUNC "VOLT"') # Sensing current                 
        
    def setvoltage(self,vb,curlimit=0.05):
        self.instr.write(':SENS:CURR:PROT %f'%curlimit) # Set compliance current to 40 mA
        self.instr.write(':SOUR:VOLT:LEV %f'%vb)        
    
    def querycurrent(self):
        try:
            self.instr.write(':FORM:ELEM CURR')
            cur = self.instr.query('READ?')
            c = float(cur)
        except ValueError:
            print('warning: current reading error...')
            print(cur)
            c = -1000        
        return float(c)
    
    def setcurrent(self,cur,vlimit=2):
        self.instr.write(':SENS:VOLT:PROT %f'%vlimit)
        self.instr.write(':SOUR:CURR:LEV %s'%cur)
        
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
            print('Agilent E3631 query fails')
        return self._outputOnOff
  
    def _set_outPutOnOff(self, x):
        try:
            cmd = 'outp ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Agilent E3631 write fails')
        self._outputOnOff = x
      
    outputOnOff = property(_get_outPutOnOff, _set_outPutOnOff, "outputOnOff property")
        
    def queryCurrent(self):
        try:
            resp=self.instr.query(':meas:curr:dc?')
        except ValueError:
            print('Agilent E3631 query failure')
        return float(resp)
    
    def queryVoltage(self):
        try:
            resp=self.instr.query(':meas:volt:dc?')
        except ValueError:
            print('Agilent E3631 query failure')
        return float(resp)


class Agilent_33401(VisaInstrument):

    def acVoltage(self):
        try:
            self.instr.write(':meas:volt:ac?')
            resp = self.instr.read()
        except ValueError:
            print('Agilent 33401 fails query')
        return float(resp)   
    
    def acCurrent(self):
        try:
            self.instr.write(':meas:curr:ac?')
            resp = self.instr.read()
        except ValueError:
            print('Agilent 33401 fails query')
        return float(resp)
    
    def dcVoltage(self):
        try:
            self.instr.write(':meas:volt:dc?')
            resp = self.instr.read()
        except ValueError:
            print('Agilent 33401 fails query')
        return float(resp)   
    
    def dcCurrent(self):
        try:
            self.instr.write(':meas:curr:dc?')
            resp = self.instr.read()
        except ValueError:
            print('Agilent 33401 fails query')
        return float(resp)
    
    
class Keithley_2510(VisaInstrument):

    def querytemp(self):
        try:
            self.instr.write(':MEAS:TEMP?')
            temp = self.instr.read()
            t = float(temp)
        except ValueError:
            print('warning: temp read error...')
            print(temp)
            t = -1000
        return float(t)    
    
    def settemp(self,setT='25'):
        self.instr.write(':SOUR:TEMP %f'%setT)
        
    def _get_output(self):
        try:
            
            resp = self.instr.query(':OUTPUT?')
            self._output = float(resp)
        except ValueError:
            print('Keithley 2510 query fails')
        return self._output
        
    def _set_output(self, x):
        try:
            cmd = ':OUTPUT  ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print('Keithley 2510 write fails')
        self._output = x
        
    output = property(_get_output, _set_output, "output property")
    

class Newport_3150(VisaInstrument):
        
    def querytemp(self):
        temp = self.instr.query(':TEC:T?')
        try:
            t = float(temp)
        except ValueError:
            print('warning: temp read error...')
            print(temp)
            t = -1000
        return float(t)    
    
    def settemp(self,setT='25'):
        self.instr.write(':TEC:T %f'%setT)
    

class powermeter(VisaInstrument):
    
    def queryIDN(self):
        try:
            resp = self.instr.query('*IDN?')
        except ValueError:
            print('Agilent 86163 fails query')
        return resp
    
    def querypower(self):
        opt = self.instr.query('READ:POW?')        
        return float(opt)
    
class dca(VisaInstrument):

    def initialize(self): # initiallize for PAM4 measurement
        pass
    
    def getER(self,source='1',ch='2A'):
        cmd = ':MEASure:EYE:OER:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        er = self.instr.query(':MEASure:EYE:OER?')
        return float(er)
    
    def getOMA(self,source='1',ch='2A'):
        cmd = ':MEASure:EYE:OOMA:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        oma = self.instr.query(':MEASure:EYE:OOMA?')
        return float(oma)
    
    def getRLM(self,source='1',ch='2A'):
        cmd = ':MEASure:EYE:PAM:LINearity:SOURce'+source+' CHAN'+ch
        self.instr.write(cmd)
        RLM = self.instr.query(':MEASure:EYE:PAM:LINearity?')
        return float(RLM)
    
#    def getTDEDQ(self):
#        return self.instr.query()
    
    def autoscale(self):
        self.instr.write(':SYSTem:AUToscale')
        self.instr.ask('*OPC?')
    
    def clear(self):
        self.instr.write(':ACQuire:CDISplay')
        self.instr.ask('*OPC?')
    
    def run(self):
        self.instr.write(':ACQuire:RUN')    
#    def savejpg(self,ch='2A',fname='',path='')
#        cmd = ':DISPlay:WINDow:TIME1:ZSIGnal CHAN'+ch
#        self.instr.write(cmd)
#        self.

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
        except ValueError:
            print('Agilent 86142 write fails')
        self._startWavelength = x
      
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
        except ValueError:
            print('Agilent 86142 write fails')
        self._stopWavelength = x
      
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
        except ValueError:
            print('Agilent 86142 write fails')
        self._traceLength = x
      
    traceLength = property(_get_traceLength, _set_traceLength, "traceLength property")
    
 
    def getTrace(self):
        tmp = ''
        elmcount = []
        try:
            self.instr.write('form ascii')
            self.instr.write('trac? tra')
            resp = self.instr.read()
            flag = '\n' in resp
            count = 0
            while (not(flag)):
                #time.sleep(0.1)
                tmp = self.instr.read()
                #elmcount.append(len(tmp.split(',')))
                #print('length %i and count %i'% (len(tmp),count))
                resp += (tmp)
                flag = '\n' in tmp
                count += 1
                #print (elmcount)
            #print (count)
        except visa.VisaIOError:
            print('error')
            print(tmp)
            resp = tmp
            traceback.print_exc()
            sys.exit(3)
        return resp
    
    def getTrace1(self,pts):
        tmp = ''
        elmcount = []
        count = 0
        itr=0
        try:
            self.instr.write('form ascii')
            self.instr.write('trac? tra')
            resp = self.instr.read()
            #print('resp', len(resp), resp)
            flag = '\n' in resp
            count += len(resp.split(','))
            while (count < pts): #(not(flag)):
                #time.sleep(0.1)
                tmp = self.instr.read()
                count += len(tmp.split(','))
                elmcount.append(count)
                #print('length %i and count %i'% (len(tmp),count))
                resp += (tmp)
                flag = '\n' in tmp
                print(flag)
                itr +=1
            #print (count)
        except visa.VisaIOError:
            print('error')
            print(tmp)
            resp = tmp
            traceback.print_exc()
            sys.exit(3)
        return resp
    
    def getTraceBin(self):
        try:
            self.instr.write('form real32')
            self.instr.write('trac? tra')
            resp = self.instr.read()
        except ValueError:
            print('Agilent 86142 write fails')
        return resp
        
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
        except ValueError:
            print('JDSU HA9 write fails')
        self._attenuation = x
      
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
        except ValueError:
            print('JDSU HA9 write fails')
        self._beamIsBlocked = int(x)
      
    beamIsBlocked = property(_get_beamIsBlocked, _set_beamIsBlocked, "beamIsBlock property")

class N9020A_SpectrumAnalyzer(VisaInstrument):
    _inputCoupling = 'DC'  # default
    _bandwidthResolution_MHz = 0.5
    _bandwidthVideo_MHz = 10
    _sweepPoints = 1001
    _startFreqMHz = 10e-3
    _stopFreqMHz = 1350
    
    
    def _set_inputCoupling(self,x='DC'):
        try:
            cmd = 'INPut:COUPling ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set input coupling')
        self._inputCoupling = str(x)
        
    def _get_inputCoupling(self):
        try:
            resp = self.instr.query('INP:COUP?')
            self._inputCoupling =resp
        except ValueError:
            print ('fails to get input coupling')
        return self._inputCoupling
    inputCoupling = property(_get_inputCoupling, _set_inputCoupling,'input coupling property')
    
    def _set_bandwidthResolution_MHz(self,x=0.5):
        try:
            cmd = 'BANDWIDTH:RESOLUTION ' + str(x) + ' MHZ'
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set bandwidth resolution')
        self._bandwidthResolution_MHz = float(x)
        
    def _get_bandwidthResolution_MHz(self):
        try:
            resp = self.instr.query('BANDWIDTH:RESOLUTION?')
            self._bandwidthResolution_MHz = float(resp)/1e6 # in MHz
        except ValueError:
            print ('fails to get bandwidth resolution')
        return self._bandwidthResolution_MHz
    
    resolutionBW_MHz = property(_get_bandwidthResolution_MHz, _set_bandwidthResolution_MHz,'bandwidth resolution property')
    
    def _set_bandwidthVideo_MHz(self,x=0.5):
        try:
            cmd = 'BANDWIDTH:VIDEO ' + str(x) + ' MHZ'
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set video bandwidth')
        self._bandwidthResolution_MHz = float(x)
        
    def _get_bandwidthVideo_MHz(self):
        try:
            resp = self.instr.query('BANDWIDTH:VIDEO?')
            self._bandwidthResolution_MHz = float(resp)/1e6 # in MHz
        except ValueError:
            print ('fails to get video bandwidth')
        return self._bandwidthResolution_MHz
    
    videoBW_MHz = property(_get_bandwidthVideo_MHz, _set_bandwidthVideo_MHz,'video bandwidth property')
    
    def _set_sweepPoints(self,x=1001):
        try:
            cmd = 'SWEEP:POINTS ' + str(x)
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set sweep points')
        self._sweepPoints = int(x)
        
    def _get_sweepPoints(self):
        try:
            resp = self.instr.query('SWEEP:POINTS?')
            self._sweepPoints = int(resp) # in MHz
        except ValueError:
            print ('fails to get sweep points')
        return self._sweepPoints
    
    sweepPoints = property(_get_sweepPoints, _set_sweepPoints,'sweep points')
    
    
    def _set_startFreqMHz(self,x=10e-3):
        try:
            cmd = 'FREQUENCY:START ' + str(x) + ' MHZ'
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set start frequency')
        self._startFreqMHz = float(x)
        
    def _get_startFreqMHz(self):
        try:
            resp = self.instr.query('FREQUENCY:START?')
            self._startFreqMHz = float(resp)/1e6 # in MHz
        except ValueError:
            print ('fails to get stop frequency')
        return self._startFreqMHz
    
    startFreqMHz = property(_get_startFreqMHz, _set_startFreqMHz,'start frequency property')
    
    def _set_stopFreqMHz(self,x=13.5e3):
        try:
            cmd = 'FREQUENCY:STOP ' + str(x) + ' MHZ'
            self.instr.write(cmd)
        except ValueError:
            print ('fails to set start frequency')
        self._stopFreqMHz = float(x)
        
    def _get_stopFreqMHz(self):
        try:
            resp = self.instr.query('FREQUENCY:STOP?')
            self._stopFreqMHz = float(resp)/1e6 # in MHz
        except ValueError:
            print ('fails to get stop frequency')
        return self._stopFreqMHz
    
    stopFreqMHz = property(_get_stopFreqMHz, _set_stopFreqMHz,'start frequency property')
    
    def getTrace(self):
        _points = self._get_sweepPoints()
        _stopf =self._get_stopFreqMHz()
        _startf =self._get_startFreqMHz()
        _freq = np.linspace(_startf,_stopf,_points)
        tmp = ''
        try:
            self.instr.write('FORMAT:TRACE:DATA ASCII')
            self.instr.write('TRAC? TRACE1')
            resp = self.instr.read()
            flag = '\n' in resp
            count = 0
            while (not(flag)):
                tmp = self.instr.read()
                resp += (tmp)
                flag = '\n' in tmp
                count += 1
        except visa.VisaIOError:
            print('error')
            print(tmp)
            resp = tmp
            traceback.print_exc()
            sys.exit(3)
        resp = resp.split(',')
        y = [float(d) for d in resp]
        y= np.array(y)
        return (_freq, y)
    

class Agilent_86122A(VisaInstrument):
    
    def getFreq(self):
        try:
            self.instr.write(':MEAS:SCAL:POW:FREQ?')
            resp = float(self.instr.read())
        except visa.VisaIOError:
            print('error')
            sys.exit(3)
        return resp