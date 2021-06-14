"""
Copyright (C) 2020 Piek Solutions LLC

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

"""
import visa
import time


class UartInstrument:

    def __init__(self, ip):
        # gpib address 29 is hardcoded for UART
        resource_name = "TCPIP0::%s::inst29::INSTR" % ip
        rm = visa.ResourceManager()
        self.instr = rm.open_resource(resource_name)
        self.instr.timeout = 10000

    def close(self):
        self.instr.close()

    def read(self):
        """
        read from uart device
        :return: response string from device
        """
        # byte[0]: 0x01 for query cmd
        # byte[1]: length of query cmd
        # byte[2:]: bytes of command string
        
        len_cmd = 0
        bytes_to_write = bytes([0x01]) + len_cmd.to_bytes(2, 'little') 
        #print(bytes_to_write, len(bytes_to_write))
        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()
            return data
        except ValueError:
            print("uart failed read")

    def query(self, command):
        """
        query uart device with command string, adding newline to the end
        :param command: (string)
        :return: response string from device
        """
        # byte[0]: 0x01 for query cmd
        # byte[1]: length of query cmd
        # byte[2:]: bytes of command string

        len_cmd = len(command) + 1
        data_bytes = command.encode('utf-8') + bytes([0x0a])    # cmd bytes and newline
        bytes_to_write = bytes([0x01]) + len_cmd.to_bytes(2, 'little') + data_bytes
        # print(bytes_to_write, len(bytes_to_write))

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()
            return data
        except ValueError:
            print("uart failed query")

    def queryBytes(self, command):
        """
        query uart device with hex coded command string, adding newline to the end
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: response string from device
        """
        # byte[0]: 0x01 for query cmd
        # byte[1]: length of query cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) + 1
        data_bytes = cmd + bytes([0x0a])    # cmd bytes and newline
        bytes_to_write = bytes([0x01]) + len_cmd.to_bytes(2, 'little') + data_bytes
        # print(bytes_to_write, len(bytes_to_write))

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()
            return data
        except ValueError:
            print("uart failed queryBytes")
            
    def queryBytesRaw(self, command):
        """
        query uart device with hex coded command string
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: response string from device
        """
        # byte[0]: 0x01 for query cmd
        # byte[1]: length of query cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) 
        data_bytes = cmd     # cmd bytes wo newline
        bytes_to_write = bytes([0x01]) + len_cmd.to_bytes(2, 'little') + data_bytes
        #print(bytes_to_write, len(bytes_to_write))

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()
            return data
        except ValueError:
            print("uart failed queryBytesRaw")

    def write(self, command):
        """
        write command string to uart instrument adding newline to the end
        :param command: (string)
        :return: None
        """
        # byte[0]: 0x02 for write cmd
        # byte[1]: length of write cmd
        # byte[2:]: bytes of command string

        len_cmd = len(command) + 1
        data_bytes = command.encode('utf-8') + bytes([0x0a])
        bytes_to_write = bytes([2]) + len_cmd.to_bytes(2, 'little') + data_bytes

        try:
            self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart failed write")

    def writeBytes(self, command):
        """
        write hex coded command string to uart instrument, append a newline by default
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: None
        """
        # byte[0]: 0x02 for write cmd
        # byte[1]: length of write cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) + 1
        data_bytes = cmd + bytes([0x0a])
        bytes_to_write = bytes([2]) + len_cmd.to_bytes(2, 'little') + data_bytes

        try:
            self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart failed writeBytes")
            
    def writeBytesRaw(self, command):
        """
        write hex coded command string to uart instrument
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: None
        """
        # byte[0]: 0x02 for write cmd
        # byte[1]: length of write cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) 
        data_bytes = cmd     # cmd bytes wo newline
        bytes_to_write = bytes([2]) + len_cmd.to_bytes(2, 'little') + data_bytes
        print(bytes_to_write)

        try:
            self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart failed writeBytesRaw")
            
    def NSBwriteBytesRaw(self, command):
        """
        Non-standard baudrate write hex coded command string to uart instrument
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: None
        """
        # byte[0]: 0x08 for write cmd
        # byte[1]: length of write cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) 
        data_bytes = cmd     # cmd bytes wo newline
        bytes_to_write = bytes([0x08]) + len_cmd.to_bytes(2, 'little') + data_bytes
        #print(bytes_to_write)

        try:
            self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart failed Non-Standard Buadrate writeBytesRaw")
 
    def NSBqueryBytesRaw(self, command):
        """
        Non-standard baudrate query uart device with hex coded command string
        :param command: (string) hex encoded command string, ex: '02c9'
        :return: response string from device
        """
        # byte[0]: 0x07 for query cmd
        # byte[1]: length of query cmd
        # byte[2:]: bytes of command string

        cmd = bytes.fromhex(command)
        len_cmd = len(cmd) 
        data_bytes = cmd     # cmd bytes wo newline
        bytes_to_write = bytes([0x07]) + len_cmd.to_bytes(2, 'little') + data_bytes
        # print(bytes_to_write, len(bytes_to_write))

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()
            return data
        except ValueError:
            print("uart failed Non Standard Baudrate queryBytesRaw")

# =============================================================================
# Both Byte Timeout(us) and Msg Timeout (ms) are related to reading from the RS232 interface. 
# The users will need to change the default values when they are reading a large amount of data 
# or an instrument that is slow to respond (ie slow measurement etc.). The default Msg Timeout(ms) 
# is set to 5000ms or 5s: any read operation on the RS232 will wait for 5s for data to be 
# available on the RxD line before it timeout and finishes. 
# 
# Byte Timeout(us) is related to the read duration to read data on the RxD line. When it is set 
# to 100000us or 0.1s, the RxD line is read for 0.1s, even if all the data that has been transmitted
# has been read before the 0.1s interval is complete. For example, 
# if the data to be read in is small, then the Byte Timeout(us) should be set to a small value to 
# improve the response time. On the other hand, when reading in a large amount of data, 
# the users will need to increase the Byte Timeout (us) to a few seconds in order to completely read 
# in the data. Note that the Byte Timeout value also depends on the baud rate.
# 
# =============================================================================

    def set_config(self, data_rate, num_bits, parity, stop_bits, msg_timeout, byte_timeout):
        """
        set uart configuration
        :param data_rate: (int) baud rate
        :param num_bits: (int) number of bits in a message (7 or 8)
        :param parity: (int) 0=None, 1=Odd, 2=Even
        :param stop_bits: (int) stopbit value
        :param msg_timeout: (int) message timeout in ms
        :param byte_timeout: (int) byte read timeout in us
        :return:
        """
        # byte[0]: 0x03 for config
        # byte[1]: data length
        # byte[2:]: configUart(byteConfig - bitnums, parity, stop,
        #          (int) baudrate, (int) msgtimeout, (int) bytetimeout)

        cmd_byte = bytes([0x03])
        len_data = 13
        len_data_bytes = len_data.to_bytes(2, 'little')
        config_byte = bytes([((num_bits-5) if num_bits >= 5 else 0x00) << 0x04 | parity << 0x01 | stop_bits-1])
        # config byte structure:
        # ------------------------------------------------------#
        # bit7, bit6, bit5, bit4 | bit3, bit2, bit1 | bit0      #
        # RS_CHAR_8              | RS_PARITY_NONE   | RS_STOP_1 #
        # ------------------------------------------------------#
        data_rate_bytes = data_rate.to_bytes(4, 'little')
        msg_timo_bytes = msg_timeout.to_bytes(4, 'little')
        byt_timo_bytes = byte_timeout.to_bytes(4, 'little')

        bytes_to_write = cmd_byte + len_data_bytes + config_byte + data_rate_bytes + \
                         msg_timo_bytes + byt_timo_bytes

        try:
            # print(list(bytes_to_write))
            return self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart device failed write")

    def get_config(self):
        """
        read config from uart device
        :return: dictionary of config values
        """
        len_data = 0
        len_data_bytes = len_data.to_bytes(2, 'little')
        bytes_to_write = bytes([0x04]) + len_data_bytes

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()

            # we already know these values
            # print(int.from_bytes(data[0:1], 'little'))  # read uart cmd byte
            # print(int.from_bytes(data[1:3], 'big'))     # length of data

            config_byte = data[3]
            numbits = ((config_byte & 0xf0) >> 4) + 5
            parity = (config_byte & 0x06) >> 1
            stopbits = (config_byte & 0x01) + 1

            baud = int.from_bytes(data[4:8], 'little')
            m_timo = int.from_bytes(data[8:12], 'little')
            b_timo = int.from_bytes(data[12:16], 'little')

            config = {
                "baud": baud,
                "numbits": numbits,
                "parity": parity,
                "stopbits": stopbits,
                "m_timo": m_timo,
                "b_timo": b_timo
            }

            return config
        except ValueError:
            print("uart failed read")

    def set_NSBconfig(self, ibrd=2,fbrd=32, num_bits=8, parity=0, stop_bits=1, msg_timeout=5000, byte_timeout=100, rxfifo =1):
        """
        set uart configuration for fractional baudrate (Non Standard)
        :param ibrd is integer divider
        :param fbrd is fractional divider
        :param num_bits: (int) number of bits in a message (5,6,7 or 8)
        :param parity: (int) 0=None, 1=Odd, 2=Even
        :param stop_bits: (int) stopbit value
        :param msg_timeout: (int) message timeout in ms
        :param byte_timeout: (int) byte read timeout in us
        :param rxfifo: (int) rxfifo is used
        :return:
        """
        # byte[0]: 0x03 for config
        # byte[1]: data length
        # byte[2:]: configUart(byteConfig - bitnums, parity, stop,
        #          (int) baudrate, (int) msgtimeout, (int) bytetimeout)
        
        cmd_byte = bytes([0x05])
        len_data = 13
        len_data_bytes = len_data.to_bytes(2, 'little')
        tmp = (num_bits - 5) if num_bits >= 5 else 0x00
        tmp = tmp + 8 if rxfifo == 1 else tmp
        config_byte = bytes([tmp << 0x04 | parity << 0x01 | stop_bits-1])
        # data_rate_bytes = data_rate.to_bytes(4, 'little')
        ibrd_bytes = ibrd.to_bytes(2, 'little')
        fbrd_bytes = fbrd.to_bytes(2, 'little')
        msg_timo_bytes = msg_timeout.to_bytes(4, 'little')
        byt_timo_bytes = byte_timeout.to_bytes(4, 'little')

        bytes_to_write = cmd_byte + len_data_bytes + config_byte + ibrd_bytes + fbrd_bytes + \
                         msg_timo_bytes + byt_timo_bytes

        try:
            #print(list(bytes_to_write))
            return self.instr.write_raw(bytes_to_write)
        except ValueError:
            print("uart device failed write")
            
    def get_NSBconfig(self):
        """
        read config from uart device
        :return: dictionary of config values
        """
        len_data = 0
        len_data_bytes = len_data.to_bytes(2, 'little')
        bytes_to_write = bytes([0x06]) + len_data_bytes

        try:
            self.instr.write_raw(bytes_to_write)
            data = self.instr.read_raw()

            # print(int.from_bytes(data[0:1], 'little'))  # read uart cmd byte
            # print(int.from_bytes(data[1:3], 'big'))     # length of data

            # config byte structure:
            # ------------------------------------------------------#
            # bit7, bit6, bit5, bit4 | bit3, bit2, bit1 | bit0      #
            # RS_CHAR_8              | RS_PARITY_NONE   | RS_STOP_1 #
            # ------------------------------------------------------#
            config_byte = data[3]
            fifo = (config_byte & 0x80) >> 7
            numbits = ((config_byte & 0x70) >> 4) + 5
            parity = (config_byte & 0x06) >> 1
            stopbits = (config_byte & 0x01) + 1

            baud = int.from_bytes(data[4:8], 'little')
            m_timo = int.from_bytes(data[8:12], 'little')
            b_timo = int.from_bytes(data[12:16], 'little')

            config = {
                "baud": baud,
                "numbits": numbits,
                "parity": parity,
                "stopbits": stopbits,
                "m_timo": m_timo,
                "b_timo": b_timo,
                "fifo": fifo
            }

            return config
        except ValueError:
            print("uart failed read")

    def calc_NSBbaudrate(self, ibrd=2,fbrd=32):
        # method to calculate the fractional baudrate
        # using the uart clock of 62.5MHz
        # this does not set the baudrate to the Papaya GPIB Controller
        # the ibrd has the minimum value of 1
        if (ibrd > 0):
            uartClock = 62.5e6
            fracDiv = ibrd + float(fbrd)/64.0
            return (uartClock/(16.0*fracDiv))
        else:
            print("min ibrd value is 1")
        

class Agilent_E3631(UartInstrument):
    def _get_outPutOnOff(self):
        try:
            resp = self.query(':outp?')
            self._get_outPutOnOff = int(resp)
        except ValueError:
            print('Agilent E3631 query fails')
        return self._get_outPutOnOff

    def _set_outPutOnOff(self, x):
        try:
            cmd = 'outp ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 write fails')
        self._get_outPutOnOff = x

    outputOnOff = property(_get_outPutOnOff, _set_outPutOnOff, "outputOnOff property")

    def queryCurrent(self):
        try:
            resp = self.query(':meas:curr:dc?')
            return float(resp)
        except ValueError:
            print('Agilent E3631 query fails')

    def queryVoltage(self):
        try:
            resp = self.query(':meas:volt:dc?')
            return float(resp)
        except ValueError:
            print('Agilent E3631 query fails')

    def selectPowerSupply(self,x):
        try:
            # select instrument
            # 1 is P6V, 2 is P25V and 3 is N25V
            cmd = 'INST:NSEL ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 select PS fails')
            
    def setP6VSupply(self,x):
        try:
            # P6V is 1
            self.write('INST:NSEL 1')
            cmd = 'volt ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 set PS fails')
    
    def queryP6VSetVoltage(self):
        try:
            # P6V is 1
            self.write('INST:NSEL 1')
            time.sleep(0.3)  # consecutive is too fast for uart
            val = self.query('volt?')
        except ValueError:
            print('Agilent E3631 query PS fails')
        return float(val)
    
    def setP25VSupply(self, x):
        try:
            # P25V is 2
            self.write('INST:NSEL 2')
            cmd = 'volt ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 set PS fails')
    
    def queryP25VSetVoltage(self):
        try:
            # P6V is 1
            self.write('INST:NSEL 2')
            time.sleep(0.3) # consecutive is too fast for uart
            val = self.query('volt?')
        except ValueError:
            print('Agilent E3631 query PS fails')
        return float(val)
    
    def setN25VSupply(self,x):
        try:
            # P6V is 1
            self.write('INST:NSEL 3')
            cmd = 'volt ' + str(x)
            self.write(cmd)
        except ValueError:
            print('Agilent E3631 set PS fails')
    
    def queryN25VSetVoltage(self):
        try:
            # P6V is 1
            self.write('INST:NSEL 3')
            time.sleep(0.3) # consecutive is too fast for uart
            val = self.query('volt?')
        except ValueError:
            print('Agilent E3631 query PS fails')
        return float(val)


class Vex5Brain(UartInstrument):
    # This example demonstrates the Non-Standard or fractional UART
    # baud rate capability of the Papaya GPIB Controller. The Vex Rebotics V5
    # is chonsen because the UART baud rate is 1.5625Mbits/s 
    # The Papaya GPIB Controller is used to snoop and decode the Vex Robotics 
    # motor messaged. The details of V5 Brain microcontroller 
    # and the V5 motor can be found 
    # https://www.vexrobotics.com/276-4810.html and
    # https://www.vexrobotics.com/276-4840.html respectively.
    # A RJ11 splitter and and RS485 transceiver are needed.
    # https://www.amazon.com/Telephone-Training-Adapter-Connections-Headset/dp/B075P29P2Q/ref=sr_1_3?dchild=1&keywords=rj11+splitter+4P4C&qid=1623637005&sr=8-3
    # https://www.vexforum.com/t/lidar-4-wire-uart-with-vex-v5/79430/8
    # https://www.mouser.com/ProductDetail/Texas-Instruments/SN65HVD11P?qs=QViXGNcIEAsnINFbXL01HQ%3D%3D
    _resp = bytes([0x02,0x76,0x01,0x12,0xef,0x00,0x00,0x00,0x00,0x00,0x32,0x00,0x00,0x00,0x7f,0x27])

    def configBrain(self):
        try:
            self.set_NSBconfig(2,32,stop_bits=1,msg_timeout=1000, byte_timeout=250,rxfifo=1,num_bits=8)
        except ValueError:
            print('Vex5Brain fails')

    def contRead(self, iternum=800):
        count = 0
        try:
            while count <= iternum:
                val = self.NSBqueryBytesRaw('')
                if len(val) == 32:
                    if (val[16] != 0x02):
                        for i_ in range(16,32):
                            print('cnt %d, i_: %d: %02x'%(count,i_,val[i_]))   #'{0:02x}'.format(reg)
                    #else:
                    #    print('cnt %d'%count)
                if len(val) == 16:
                    if (val[0] != 0x02 and val[0] != 0x76):
                        for i_ in range(0,16):
                            print('cnt %d, i_: %d: %02x'%(count,i_,val[i_]))
                count = count + 1
        except ValueError:
            print('Vex5Brain fails')

    def motorDecode(self, iternum=100):
        count = 0
        try:
            while count <= iternum:
                val = self.NSBqueryBytesRaw('')
                if len(val) == 32:
                    if val[0] == 0x76:
                        temp = int(val[1])
                        current = int.from_bytes(val[2:4], byteorder='little',signed =False)
                        ticks = int.from_bytes(val[4:8],byteorder='little',signed =True)
                        speed = int.from_bytes(val[8:10], byteorder='little',signed =True)
                        voltage = int.from_bytes(val[10:12], byteorder='little',signed =True)
                        print('temp: %d C, I: %d mA, ticks: %d, speed: %d t/s, mV: %d V'%(temp,current,ticks,speed,voltage))
                         
                if(len(val) == 16):
                    if (val[0] == 0x76):
                        temp = int(val[1])
                        current = int.from_bytes(val[2:4], byteorder='little',signed =False)
                        ticks = int.from_bytes(val[4:8],byteorder='little',signed =True)
                        speed = int.from_bytes(val[8:10], byteorder='little',signed =True)
                        voltage = int.from_bytes(val[10:12], byteorder='little',signed =True)
                        print('temp: %d C, I: %d mA, ticks: %d, speed: %d t/s, V: %d mV'%(temp,current,ticks,speed,voltage))
                
                count = count + 1
                    
        except ValueError:
            print('Vex5Brain fails')


if __name__ == '__main__':
    print('papaya UART demo')
    papaya_ip = "192.168.2.127"

    #pwr = Agilent_E3631(papaya_ip)
    # set default config: baud rate 9600, 8 numbits, no parity, 1 stopbits,
    #             msg timo 5s, byte timo 200000 us
    #pwr.set_config(9600, 7, 2, 1, 5000, 200000)
    #time.sleep(2)  # allow time for config to process
    #pwr.write("syst:rem")  # needed for inst control using
    
    vex = Vex5Brain(papaya_ip)
    vex.configBrain()
