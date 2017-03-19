
#include <memory>
#include <string>
#include "ClientWorker.h"
#include "HttpClient.h"
#include "json/json.h"
#include "util.h"

using namespace std;
extern redisAsyncContext* g_redis_ctx;

CClientWorker::CClientWorker(string cmd) {
    vector<string> cmds;
    str_split(cmd, cmds);
    ExecCmd(cmds);
}

void CClientWorker::ExecCmd(vector<string>& cmds) {
	if (cmds[0] == "redis") {
	    RedisCmd(cmds);
	} else if (cmds.size() == 3) {
	    if (cmds[0] == "login") {
	        Login(cmds[1], cmds[2]);
	    } else if (cmds[0] == "reg") {
	        Register(cmds[1], cmds[2]);
	    }
	} else {
	    cout << "command error" << endl;
	}
    
}

void CClientWorker::RedisCmd(std::vector<string>& cmds) {
    int i = 0;
    string s = "";
    for (auto& cmd : cmds) {
        if (i != 0) {
            s += cmd;
            s += " ";
        }
        i++;
    }
    // log("redis -%s-", s.c_str());
    redisAsyncCommand(g_redis_ctx, [](redisAsyncContext *c, void *r, void *privdata) {
        redisReply *reply = (redisReply*)r;
        if (reply == NULL) return;
        if (reply->str) {
            printf("%s\n", reply->str);
        } else {
            printf("nil\n");
        }
    }, NULL, s.c_str());
}

void CClientWorker::Login(string username, string passwd) {
    // cout << "login " << username << " " << passwd << endl;
    string ip;
    uint16_t port;
    GetMsgServerAddr("http://127.0.0.1:8080/msg_server", ip, port);
    if (!_clientconn) {
        _clientconn = shared_ptr<CClientConn>(new CClientConn(ip, port));
    } else {
        _clientconn->Close();
        _clientconn = shared_ptr<CClientConn>(new CClientConn(ip, port));
    }
    _clientconn->Login(username, passwd);

}

void CClientWorker::Register(string username, string passwd) {
    // cout << "register " << username << " " << passwd << endl;
    string ip;
    uint16_t port;
    GetMsgServerAddr("http://127.0.0.1:8080/msg_server", ip, port);
    if (!_clientconn) {
        _clientconn = shared_ptr<CClientConn>(new CClientConn(ip, port));
    } else {
        _clientconn->Close();
        _clientconn = shared_ptr<CClientConn>(new CClientConn(ip, port));
    }
    _clientconn->Register(username, passwd);
    
}

void CClientWorker::GetMsgServerAddr(string login_url, string& ip, uint16_t& port) {
    CHttpClient httpClient;
    string strResp;
    CURLcode nRet = httpClient.Get(login_url, strResp);
    if(nRet != CURLE_OK)
        throw netex("Get msgserver addr falied. access url:%s error\n", login_url.c_str());
    Json::Reader reader;
    Json::Value value;
    if (!reader.parse(strResp, value))
        throw netex("Get msgserver addr falied. parse response error:%s\n", strResp.c_str());

    uint32_t retCode = value["code"].asUInt();
    if(retCode != 0) {
        string strMsg = value["msg"].asString();
        loge("Get msgserver addr falied. errorMsg:%s\n", strMsg.c_str());
        throw netex("Get msgserver addr falied. errorMsg:%s\n", strMsg.c_str());
    }
    ip = value["priorIP"].asString();
    if (value["port"].isString())
        port = stoul(value["port"].asString());
    else if (value["port"].isUInt())
        port = value["port"].asUInt();
    else
        throw netex("Get msgserver info json type wrong");
}	
