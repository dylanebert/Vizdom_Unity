from flask import Flask
from flask_cors import CORS
import os
import sys
import json
from nn import get_accuracy

app = Flask(__name__)
CORS(app)

filenames = []
attributes = []
for filename in os.listdir('data'):
	if not os.path.isdir('data/{0}'.format(filename)):
		filenames += [filename]
		attributes += [filename.split('.')[0].replace('_',' ')]
attributes = json.dumps(attributes)

@app.route('/attributes')
def get_attributes():
	return attributes

@app.route('/nn')
def run_nn():
	accuracy = str(get_accuracy())
	return accuracy