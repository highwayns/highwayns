#ifndef __CLIENT_WORKER_H__
#define __CLIENT_WORKER_H__

#include <vector>
#include <string>
#include <memory>
#include "ClientConn.h"

typedef enum {
    ON_CONFIRM_LOGIN = 1,
    ON_CONFIRM_REGISTER
} on_confirm_state_t;

typedef struct {
    std::string username;
    std::string passwd;
    on_confirm_state_t state;
} on_confirm_data_t;


class CClientWorker
{
    std::shared_ptr<CClientConn> _clientconn;
public:
    CClientWorker(std::string cmd);
    void ExecCmd(std::vector<std::string>& cmds);
    void Login(std::string username, std::string passwd);
    void Register(std::string username, std::string passwd);
    void GetMsgServerAddr(std::string login_url, std::string& ip, uint16_t& port);
    void ConnectMsgServer(on_confirm_data_t& data);
    
    void RedisCmd(std::vector<string>& cmds);
};


#endif