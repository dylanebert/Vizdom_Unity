import tensorflow as tf
from tensorflow.examples.tutorials.mnist import input_data
import argparse
import sys
import numpy as np

class Model:
	def __init__(self, properties):
		self.properties = properties
	
		self.mnist = input_data.read_data_sets("MNIST_data/", one_hot=True)

		x_size = 784
		y_size = 10
		
		self.x = tf.placeholder(tf.float32, [None, x_size])
		self.y_ = tf.placeholder(tf.float32, [None, y_size])
		
		W = []
		b = []
		layers = []
		
		#First layer
		W.append(tf.Variable(tf.random_normal([x_size, properties.hidden_layer_sizes[0]], stddev=.1)))
		b.append(tf.Variable(tf.random_normal([properties.hidden_layer_sizes[0]], stddev=.1)))
		
		#Middle layers
		for i in range(1, len(properties.hidden_layer_sizes)):
			W.append(tf.Variable(tf.random_normal([properties.hidden_layer_sizes[i-1], properties.hidden_layer_sizes[i]], stddev=.1)))
			b.append(tf.Variable(tf.random_normal([properties.hidden_layer_sizes[i]], stddev=.1)))
			
		#Last layer
		W.append(tf.Variable(tf.random_normal([properties.hidden_layer_sizes[-1], y_size], stddev=.1)))
		b.append(tf.Variable(tf.random_normal([y_size], stddev=.1)))
		
		#First layer matmul
		layers.append(tf.nn.relu(tf.matmul(self.x, W[0]) + b[0]))
		
		#Rest layers matmul
		for i in range(1, len(properties.hidden_layer_sizes)):
			layers.append(tf.nn.relu(tf.matmul(layers[i-1], W[i]) + b[i]))
		
		#Predicted y
		y = tf.matmul(layers[-1], W[-1]) + b[-1]
		
		self.cross_entropy = tf.reduce_sum(tf.nn.softmax_cross_entropy_with_logits(labels=self.y_, logits=y))
		self.train_step = tf.train.AdamOptimizer(.01).minimize(self.cross_entropy)
		
		self.correct_prediction = tf.equal(tf.argmax(y, 1), tf.argmax(self.y_, 1))
		self.accuracy = tf.reduce_mean(tf.cast(self.correct_prediction, tf.float32))
		
		self.sess = tf.InteractiveSession()
		tf.global_variables_initializer().run()
			
	def train_batch_and_report_loss(self):
		batch_xs, batch_ys = self.mnist.train.next_batch(self.properties.batch_size)
		_, loss = self.sess.run([self.train_step, self.cross_entropy], feed_dict = {self.x: batch_xs, self.y_: batch_ys})
		return loss
			
	def get_accuracy(self):
		total_accuracy = 0
		num_batches = 10000 // self.properties.batch_size
		for _ in range(num_batches):
			batch_xs, batch_ys = self.mnist.test.next_batch(self.properties.batch_size)
			accuracy_val = self.sess.run(self.accuracy, feed_dict = {self.x: batch_xs, self.y_: batch_ys})
			total_accuracy += accuracy_val
		total_accuracy /= float(num_batches)
		return total_accuracy
	
class Properties:
	def __init__(self, batch_size, train_input_filename, train_answer_filename, test_input_filename, test_answer_filename, hidden_layer_sizes):
		self.batch_size = batch_size
		self.train_input_filename = train_input_filename
		self.train_answer_filename = train_answer_filename
		self.test_input_filename = test_input_filename
		self.test_answer_filename = test_answer_filename
		self.hidden_layer_sizes = hidden_layer_sizes
	
if __name__ == '__main__':
	properties = Properties(100, "", "", "", "", [100, 50])
	model = Model(properties)
	for _ in range(1000):
		loss = model.train_batch_and_report_loss()
		print(loss)
	accuracy = model.get_accuracy()
	print(accuracy)