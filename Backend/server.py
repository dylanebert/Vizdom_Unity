import os
import sys
import json
from nn import *
import socket

filenames = []
attributes = []
for filename in os.listdir('data'):
	if not os.path.isdir('data/{0}'.format(filename)):
		filenames += [filename]
		attributes += [filename.split('.')[0].replace('_',' ')]
attributes = json.dumps(attributes)

def nn_init(batch_size):
	properties = Properties(batch_size)
	
	global model
	model = Model(properties)
	
def nn_train():
	global model
	
	loss = model.train_batch_and_report_loss()
	return str(loss)
	
def nn_accuracy():
	global model
	
	return str(model.get_accuracy())

HOST = ''
PORT = 8000
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)
print('Listening...')

conn, addr = s.accept()
print('Connection from {0}'.format(addr))

while 1:
	data = conn.recv(1024)
	if not data: break
	data = data.decode('utf-8')
	flag = data[:2]
	if flag == 'ga': #get attributes
		conn.sendto(attributes.encode(), (HOST, PORT))
	elif flag == 'ni': #neural network init
		data = json.loads(data[2:])
		batch_size = data['batch_size']
		nn_init(batch_size)
	elif flag == 'nt': #neural network train
		loss = nn_train()
		conn.sendto(str(loss).encode(), (HOST, PORT))
	elif flag == 'na': #neural network accuracy
		accuracy = nn_accuracy()
		conn.sendto(str(accuracy).encode(), (HOST, PORT))
	else:
		conn.sendall(data)
conn.close()