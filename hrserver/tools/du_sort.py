import sys, os

lineList = []

f = open(sys.argv[1])
fw = open(sys.argv[1] + '.sort', 'w')
def tran(a):
    if a[-1] == 'T':
        aa = float(a[:-1]) * 1000 * 1000 * 1000
    elif a[-1] == 'G':
        aa = float(a[:-1]) * 1000 * 1000
    elif a[-1] == 'M':
        aa = float(a[:-1]) * 1000
    else:
        aa = float(a[:-1])
    return aa

def gt(a, b):
    return tran(a) > tran(b)
    
for line in f:
    print line
    a, b = line.split()
    i = 0
    for tline in lineList:
        if gt(a, tline[0]):
            break
        else:
            i += 1
    lineList.insert(i, (a, b,))

for e in lineList:
    print '%s  %s%s'%(e[0], os.getcwd(), e[1][1:])
    print >> fw, '%s  %s%s'%(e[0], os.getcwd(), e[1][1:])






