#import tensorflow as tf
from flask import Flask
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

data = open('data/data.json').read()

@app.route('/data_raw')
def data_raw():
	return data

