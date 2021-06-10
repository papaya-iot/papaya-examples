"""
Copyright (C) 2020 Piek Solutions LLC

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

"""

import requests
import urllib.parse


class UartHttpInstrument:

    def __init__(self, ip):
        # gpib address 29 is hardcoded for UART
        self.url = 'http://' + ip + '/uart/'

    def read(self):
        """
        read uart device
        :return: response string from device
        """
        try:
            req_url = self.url + 'read/'
            resp = requests.get(url=req_url)
            return resp.content.decode('utf-8')
        except ValueError:
            print("uart failed read")

    def query(self, command):
        """
        query uart device with command string, adding newline to the end
        :param command: (str)
        :return: response string from device
        """
        try:
            command += '\\n'
            cmd = urllib.parse.quote(command)  # escape special chars
            req_url = self.url + 'query/' + cmd
            resp = requests.get(url=req_url)
            return resp.content.decode('utf-8')
        except ValueError:
            print("uart failed query")

    def queryBytes(self, command):
        """
        query uart device with command string, adding newline to the end
        :param command: (str) hex-encoded, with 2 hex digits per byte
        :return: (bytes) response bytes from device
        """
        try:
            command += '0a'
            req_url = self.url + 'bquery/' + command
            resp = requests.get(url=req_url)
            return resp.content
        except ValueError:
            print("uart failed queryBytes")

    def write(self, command):
        """
        write command string to uart instrument
        :param command: (str)
        :return: success
        """
        try:
            cmd = urllib.parse.quote(command)  # escape special chars
            req_url = self.url + 'write/' + cmd
            requests.get(url=req_url)
        except ValueError:
            print("uart failed write")

    def writeBytes(self, command):
        """
        write command string to uart instrument
        :param command: (str) hex-encoded, with 2 hex digits per byte
        :return: None
        """
        try:
            command += '0a'
            req_url = self.url + 'bwrite/' + command
            requests.get(url=req_url)
        except ValueError:
            print("uart failed write")

    def set_config(self, data_rate, num_bits, parity, stop_bits, msg_timeout, byte_timeout):
        """
        set uart configuration
        :param data_rate: (int) baud rate
        :param num_bits: (int) number of bits in a message (7 or 8)
        :param parity: (int) 0=None, 1=Odd, 2=Even
        :param stop_bits: (int) stopbit value
        :param msg_timeout: (int) message timeout in ms
        :param byte_timeout: (int) byte read timeout in us
        :return: None
        """

        params = 'baud=%d&numbits=%d&parity=%d&stopbits=%d&m_timo=%d&b_timo=%d' \
                 % (data_rate, num_bits, parity, stop_bits, msg_timeout, byte_timeout)

        try:
            req_url = self.url + 'config/?' + params
            requests.get(req_url)
        except ValueError:
            print('uart device failed set config')

    def get_config(self):
        try:
            req_url = self.url + 'getconfig/'
            resp = requests.get(req_url).json(strict=False)
            return resp
        except ValueError:
            print('uart device failed get config')


class Agilent_E3631(UartHttpInstrument):
    def _get_outPutOnOff(self):
        try:
            resp = self.query(':outp?')
            self._startWavelength = int(resp)
        except ValueError:
            print('Agilent E3631 query fails')
        return self._outpuOnOff

    def _set_outPutOnOff(self, x):
        try:
            cmd = 'outp ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 write fails')
        self._outpuOnOff = x

    outputOnOff = property(_get_outPutOnOff, _set_outPutOnOff, "outputOnOff property")

    def queryCurrent(self):
        try:
            resp = self.query(':meas:curr:dc?')
        except ValueError:
            print('Agilent E3631 query failure')
        return float(resp)

    def queryVoltage(self):
        try:
            resp = self.query(':meas:volt:dc?')
        except ValueError:
            print('Agilent E3631 query failure')
        return float(resp)

