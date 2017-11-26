import os
import sys
import json
#from nn import *
import socket

filenames = []
attributes = []
for filename in os.listdir('data'):
	if not os.path.isdir('data/{0}'.format(filename)):
		filenames += [filename]
		attributes += [filename.split('.')[0].replace('_',' ')]
attributes = json.dumps(attributes)

HOST = ''
PORT = 8000
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)
conn, addr = s.accept()
print('Connection from {0}'.format(addr))
while 1:
	data = conn.recv(1024)
	if not data: break
	if data[:2].decode('utf-8') == 'ga':
		conn.sendto(attributes.encode(), (HOST, PORT))
	else:
		conn.sendall(data)
conn.close()

'''@app.route('/attributes')
def get_attributes():
	return attributes

@app.route('/nn_init', methods = ['POST', 'GET'])
def nn_init():
	try:
		batch_size = int(request.args.get('batch_size', ''))
	except:
		batch_size = 100
	
	properties = Properties(batch_size)
	
	global model
	model = Model(properties)
	
	return '1'
	
@app.route('/nn_train_batch')
def nn_train_batch():
	global model
	
	loss = model.train_batch_and_report_loss()
	return str(loss)

@app.route('/accuracy')
def get_accuracy():
	global model
	
	return str(model.get_accuracy())'''