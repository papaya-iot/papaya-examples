import papaya_visainst as visa

ip = '192.168.2.127'
instr = visa.Keysight_E3649A(ip, '27')

# turn output off and on
print('turning output off')
instr.outputOnOff = 0
print('output status: ', instr.outputOnOff)
print()

# set ranges and protection
print('--- Setting up output 1: ---')
instr.selectOutput(1)
print('voltage range: ' + instr.queryOutputRange())
print('enable protection: ' + str(instr.queryVoltageProtection()))
instr.setVoltageProtection(20.0)
instr.enableVoltageProtection()
instr.setOutputRange('P60V')
print('voltage range: ' + instr.queryOutputRange())
print('enable protection: ' + str(instr.queryVoltageProtection()))

print('--- Setting up output 2: ---')
# demonstrates specifying optional output parameter
print('voltage range: ' + instr.queryOutputRange(2))
print('enable protection: ' + str(instr.queryVoltageProtection(2)))
instr.setVoltageProtection(40.0, 2)
instr.enableVoltageProtection(2)
instr.setOutputRange('P35V', 2)
print('voltage range: ' + instr.queryOutputRange(2))
print('enable protection: ' + str(instr.queryVoltageProtection(2)))

# turn output on before setting voltages
print('output status: ', instr.outputOnOff)
print('turning output on')
instr.outputOnOff = 1
print('output status: ', instr.outputOnOff)
print()

# set voltages
instr.setVoltage(2.5, 1)
instr.setVoltage(5.9, 2)

# query voltages set
print('O1 voltage query: %.2f' % instr.queryVoltage(1))
print('O2 voltage query: %.2f' % instr.queryVoltage(2))
print()

# set voltages
instr.setVoltage(5.5, 1)
instr.setVoltage(15.9, 2)

# query voltages set
print('O1 voltage query: %.2f' % instr.queryVoltage(1))
print('O2 voltage query: %.2f' % instr.queryVoltage(2))
print()

# query current drawn
print('O1 current query: %.2f' % instr.queryCurrent(1))
print('O2 current query: %.2f' % instr.queryCurrent(2))

instr.close()
