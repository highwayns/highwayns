#!/usr/bin/python

import re, sys

if len(sys.argv) > 1:
    r = sys.argv[1]
else:
    r = 'www.example.com'


while 1:
    s = raw_input('Please enter your IP or domain address: ')
    if not ( re.search(r'^\d+\.\d+\.\d+\.\d+$', s) or re.search(r'^(\w+\.)+[a-z]+$', s) ):
        print 'Input format error'
        print 'You should input ip(xx.xx.xx.xx) or domain(example.com)'
        continue
    else:
        break

def content_change(path):
    f = open(path)
    content = f.read()
    content = content.replace(r, s)
    f.close()

    f = open(path, 'w')
    f.write(content)
    f.close()

if __name__ == '__main__':
    content_change('login_server/loginserver.conf')
    content_change('msg_server/msgserver.conf')