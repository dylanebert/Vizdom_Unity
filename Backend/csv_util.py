import csv
import sys

if not len(sys.argv) is 2:
	sys.exit('csv_util.py attr')

attr = sys.argv[1]
infile = 'data/{0}.csv'.format(attr)
outfile = 'data/{0}_out.csv'.format(attr)

r = open(infile, 'r')
w = open(outfile, 'w', newline = '')

reader = csv.DictReader(r)

fieldnames = ['_id', attr]
writer = csv.DictWriter(w, fieldnames)

writer.writeheader()
for row in reader:
	try:
		writer.writerow({'_id': row['Country Name'], attr: float(row['2010'])})
	except:
		pass