/*================================================================
 *   Copyright (C) 2014 All rights reserved.
 *
 *   文件名称：test_client.cpp
 *   创 建 者：Zhang Yuanhao
 *   邮    箱：bluefoxah@gmail.com
 *   创建日期：2014年12月30日
 *   描    述：
 *
 ================================================================*/

#include <vector>
#include <iostream>


#include "TokenValidator.h"
#include "Thread.h"
#include "IM.BaseDefine.pb.h"
#include "IM.Buddy.pb.h"

#include "ClientConn.h"
#include "util.h"
#include "ClientWorker.h"
using namespace std;

redisAsyncContext* g_redis_ctx;
static list<string> s_shell_cmds;
static CLock s_cmds_lock;

void client_loop_callback(void* cbdata, uint8_t msg, uint32_t handle, void* pParam)
{
    CAutoLock autoLock(&s_cmds_lock);
    for (auto& e : s_shell_cmds) {
        CClientWorker worker(e);
    }
    s_shell_cmds.clear();
}

void client_shell_cmds_add(string cmd)
{
    CAutoLock autoLock(&s_cmds_lock);
    s_shell_cmds.push_back(cmd);
}


class CClientShell : public CThread
{
private:
    std::shared_ptr<CClientConn> m_conn;
public:
	void OnThreadRun() {
	    string line;
		while (true)
		{
		    cout << "client> " << flush;
            getline(cin, line);
            if (line.size() == 0) {
                continue;
            }
            client_shell_cmds_add(line);
		}
	}


	redisAsyncContext* RedisSetup() {
	    redisAsyncContext *c = redisAsyncConnect("127.0.0.1", 6379);
        if (c->err) {
            /* Let *c leak for now... */
            loge("Error: %s\n", c->errstr);
            return NULL;
        }
    
        netlib_redis_attach(c);
        redisAsyncSetConnectCallback(c, [](const redisAsyncContext *c, int status) {
            if (status != REDIS_OK) {
                printf("Error: %s\n", c->errstr);
                return;
            }
            printf("Redis Connected...\n");
        });
        
        redisAsyncSetDisconnectCallback(c, [](const redisAsyncContext *c, int status) {
            if (status != REDIS_OK) {
                printf("Error: %s\n", c->errstr);
                return;
            }
            printf("Redis Disconnected...\n");
        });
        
        return c;
	}
    
    void Run() {
        signal(SIGPIPE, SIG_IGN);
        StartThread();
        netlib_init();
        cout << "Start event loop..." << endl;
        
        g_redis_ctx = RedisSetup();

        netlib_add_loop(client_loop_callback, NULL);
        netlib_register_timer(client_conn_timer_callback, NULL, 1000);
        netlib_eventloop();
    }
};

int main(int argc, char* argv[])
{
    backup_core_file();
//    play("message.wav");
    // setlocale(LC_CTYPE, "UTF-8");
    printf("pid is %d\n", getpid());
	signal(SIGPIPE, SIG_IGN);
	
	CClientShell cs;
	cs.Run();

	return 0;
}
