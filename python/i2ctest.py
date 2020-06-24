"""
Copyright (C) 2020 Piek Solutions LLC

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

"""

import visa
import papaya_i2cinst as i2cinst
import papaya_i2chttpinst as i2chttp
import time

### USING VXI11
# initiate new connection, default address 30
conn = i2cinst.I2cConnection("192.168.2.105")
devices = conn.scan()
print('devices present: ', devices)

# create device at i2c address 0x77
bme280 = i2cinst.BME280(conn, 0x77, 1)

# read 4 bytes from _ctrl_hum register
# print(bme280.read(0xf2, 4))
# print(bme280.read(0xfa, 3))

start_time = time.perf_counter()
for i in range(100):
    t, p, h = bme280.read_data()
    print('%d  h=%.2f p=%.1f t=%.2f' % (i, h, p, t))
print('vxi11 elapsed time: ', time.perf_counter() - start_time)
conn.close()


#### USING HTTP
print(i2chttp.scanI2c('192.168.2.105'))
bme280_http = i2chttp.BME280('192.168.2.105', 0x77, 1)
# bme280_http = i2chttp.I2cHttpDevice('192.168.2.105', '77', 1)  # this way works to inherit only r/w methods

print(bme280_http.read('0xf2', 4))

start_time = time.perf_counter()
for i in range(100):
    t, p, h = bme280_http.read_data()
    print('%d  h=%.2f p=%.1f t=%.2f' % (i, h, p, t))
print('http elapsed time: ', time.perf_counter() - start_time)
