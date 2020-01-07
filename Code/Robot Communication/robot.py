## Developed by J-99

import socketio
import serial
import time

sio = socketio.Client()#logger=True)

serial_loop = True

@sio.event
def connect():
    print(" socket connected!")
    print(' socket-robotics id: ', sio.sid)

@sio.event
def disconnect():
    print(" socket disconnected!")

@sio.on('command')
def on_message(data):
    print('robot received a command!')
    ser.write(data['com'].encode())
    if data['com'] == 'kill':
    	ser.close()

def on_error(error):
    print (error)

#url = input(" Enter server URL : ")

try:
	sio.connect('SERVER_NAME')
	time.sleep(2)
except:
	print("Invalid Server")
	serial_loop = False



try:
	ser = serial.Serial('/dev/ttyUSB0', 9600, timeout = 1)
	ser.flushInput()
except:
	print(" Serial port not live")
	serial_loop = False


while serial_loop:
	try:
	    ser_bytes = ser.readline()
	    decoded_bytes = ser_bytes[0:len(ser_bytes)-2].decode("utf-8")
	    servos = decoded_bytes.split(',')
	    #print(decoded_bytes)
	    try:
	    	sio.emit('servo', { 'x': float(servos[0]), 'y': float(servos[1]), 'z': float(servos[2]) })
	    	print(decoded_bytes)
	    except:
	    	pass
	except:
	    break

sio.disconnect()
