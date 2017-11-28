import os
import sys
import json
from nn import *
import socket

filenames = []
attributes = []
attributes_to_filenames = {}
for filename in os.listdir('data'):
	if not os.path.isdir('data/{0}'.format(filename)):
		filenames += [filename]
		attribute = filename.split('.')[0].replace('_',' ')
		attributes += [attribute]
		attributes_to_filenames[attribute] = filename
attributes = json.dumps(attributes)

def nn_init(batch_size, train_input_filename, train_answer_filename, test_input_filename, test_answer_filename):
	properties = Properties(batch_size, attributes_to_filenames[train_input_filename], attributes_to_filenames[train_answer_filename], attributes_to_filenames[test_input_filename], attributes_to_filenames[test_answer_filename])
	
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
		train_input_filename = data['train_input_filename']
		train_answer_filename = data['train_answer_filename']
		test_input_filename = data['test_input_filename']
		test_answer_filename = data['test_answer_filename']
		nn_init(batch_size, train_input_filename, train_answer_filename, test_input_filename, test_answer_filename)
	elif flag == 'nt': #neural network train
		loss = nn_train()
		conn.sendto(str(loss).encode(), (HOST, PORT))
	elif flag == 'na': #neural network accuracy
		accuracy = nn_accuracy()
		conn.sendto(str(accuracy).encode(), (HOST, PORT))
	else:
		conn.sendall(data)
conn.close()