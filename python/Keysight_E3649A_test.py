import papaya_visainst as visa

ip = '192.168.2.127'
instr = visa.Keysight_E3649A(ip, '27')

# turn output off and on
print(instr.outputOnOff)
instr.outputOnOff = 0
print(instr.outputOnOff)

# set ranges and protection
print('--- Setting up output 1: ---')
instr.selectOutput(1)
print('voltage range: ' + instr.queryOutputRange())
print('enable protection: ' + instr.queryVoltageProtection())
instr.setVoltageProtection(20.0)
instr.enableVoltageProtection()
instr.setOutputRange('P60V')
print('voltage range: ' + instr.queryOutputRange())
print('enable protection: ' + instr.queryVoltageProtection())

print('--- Setting up output 2: ---')
#demonstrates specifying optional output parameter
print('voltage range: ' + instr.queryOutputRange(2))
print('enable protection: ' + instr.queryVoltageProtection(2))
instr.setVoltageProtection(40.0, 2)
instr.enableVoltageProtection(2)
instr.setOutputRange('P35V', 2)
print('voltage range: ' + instr.queryOutputRange(2))
print('enable protection: ' + instr.queryVoltageProtection(2))

# set voltages
instr.setVoltage(2.5, 1)
instr.setVoltage(5.9, 2)

print(instr.outputOnOff)
instr.outputOnOff = 1
print(instr.outputOnOff)

# query voltages set
print('O1 voltage query: %.2f' % instr.queryVoltage(1))
print('O2 voltage query: %.2f' % instr.queryVoltage(2))

# set voltages
instr.setVoltage(20.5, 1)
instr.setVoltage(15.9, 2)

# query voltages set
print('O1 voltage query: %.2f' % instr.queryVoltage(1))
print('O2 voltage query: %.2f' % instr.queryVoltage(2))

# query current drawn
print('O1 current query: %.2f' % instr.queryCurrent(1))
print('O2 current query: %.2f' % instr.queryCurrent(2))

instr.close()
