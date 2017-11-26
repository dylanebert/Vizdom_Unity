import tensorflow as tf
from tensorflow.examples.tutorials.mnist import input_data
import argparse
import sys
import numpy as np

class Model:
	def __init__(self, properties):
		self.properties = properties
	
		self.mnist = input_data.read_data_sets("MNIST_data/", one_hot=True)

		self.x = tf.placeholder(tf.float32, [None, 784])
		
		self.W = tf.Variable(tf.zeros([784, 10]))
		self.b = tf.Variable(tf.zeros([10]))
		self.y = tf.nn.softmax(tf.matmul(self.x, self.W) + self.b)

		self.y_ = tf.placeholder(tf.float32, [None, 10])
		self.cross_entropy = tf.reduce_mean(-tf.reduce_sum(self.y_ * tf.log(self.y), reduction_indices=[1]))
		self.train_step = tf.train.GradientDescentOptimizer(.5).minimize(self.cross_entropy)
		
		self.correct_prediction = tf.equal(tf.argmax(self.y, 1), tf.argmax(self.y_, 1))
		self.accuracy = tf.reduce_mean(tf.cast(self.correct_prediction, tf.float32))
		
		self.sess = tf.InteractiveSession()
		tf.global_variables_initializer().run()
			
	def train_batch_and_report_loss(self):
		batch_xs, batch_ys = self.mnist.train.next_batch(self.properties.batch_size)
		_, loss = self.sess.run([self.train_step, self.cross_entropy], feed_dict = {self.x: batch_xs, self.y_: batch_ys})
		return loss
			
	def get_accuracy(self):
		accuracy_val = self.sess.run(self.accuracy, feed_dict = {self.x: self.mnist.test.images, self.y_: self.mnist.test.labels})
		return float(accuracy_val)
	
class Properties:
	def __init__(self, batch_size):
		self.batch_size = batch_size
	
if __name__ == '__main__':
	batch_size = 100
	properties = Properties(batch_size)
	model = Model(properties)
	loss = model.train_batch_and_report_loss()
	print(loss)