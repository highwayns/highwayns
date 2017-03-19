#!/usr/bin/python


import sys, os, signal, pdb

def kill_all():
    for e in ['login', 'route', 'msg', 'db_proxy', 'msfs', 'file', 'http_msg']:
        try:
            path = '%s_server'%e
            os.chdir(path)
            kill(path)
        except:
            pass
        finally:
            os.chdir('..')

    
def kill(path):
    pid = open('server.pid').read()
    os.kill(int(pid), signal.SIGTERM)
    

if __name__ == '__main__':

    if len(sys.argv) == 1:
        kill_all()
        
    else:
        kill('%s_server'%sys.argv[1])