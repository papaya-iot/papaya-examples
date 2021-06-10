"""
Copyright (C) 2020 Piek Solutions LLC

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
"""

import visa
import papaya_uartinst as uartinst
import papaya_uarthttpinst as uarthttp
import time

ip = '192.168.2.211'

# initiate new uart device
pwr = uartinst.Agilent_E3631(ip)

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
# if the data to be read in is not small, then the Byte Timeout(us) should be set to a small value to 
# improve the response time. On the other hand, when reading in a large amount of data, 
# the users will need to increase the Byte Timeout (us) to a few seconds in order to completely read 
# in the data. Note that the Byte Timeout value also depends on the baud rate.
# 
# set default config: baud rate 9600, 8 numbits, no parity, 1 stopbits,
#             msg timo 5s, byte timo 100000 us
# =============================================================================

pwr.set_config(9600, 7, 2, 1, 5000, 100000)
time.sleep(2)   # allow time for config to process

config = pwr.get_config()   # read back config as a dictionary
print(config)
print('baudrate is: ', config["baud"])

# instrument must be set to remote mode
# pwr.write("syst:rem")
pwr.writeBytes('737973743a72656d')   # using bytes
time.sleep(1)
# print(pwr.query("syst:err?"))

# query *IDN? in bytes
print(pwr.queryBytes('2a49444e3f'))

for i in range(10):
    print('step %d' % i)
    print('\t', pwr.queryCurrent())
    print('\t', pwr.queryVoltage())

pwr.close()


# HTTP Test Code
pwr_http = uarthttp.Agilent_E3631(ip)
pwr_http.set_config(9600, 7, 2, 1, 5000, 100000)
print('uart config: ', pwr_http.get_config())
time.sleep(1)  # allow time for config to process

# set to remote mode, cmd syst:rem
pwr_http.writeBytes('737973743a72656d')   # using bytes
time.sleep(1)

# query *IDN? in bytes
print(pwr_http.queryBytes('2a49444e3f'))

for i in range(10):
    print('step %d' % i)
    print('\t', pwr_http.queryCurrent())
    print('\t', pwr_http.queryVoltage())

# no need to close resource, since using http endpoint
