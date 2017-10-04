import tensorflow as tf
from flask import Flask
app = Flask(__name__)

data = open('data/data.json').read()
print(data)

@app.route('/data_raw')
def data_raw():
	return data

