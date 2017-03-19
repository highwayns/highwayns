
#include <ctime>
#include <cstdlib>
#include <string>

#include "../DBPool.h"
#include "Register.h"
#include "IM.Server.pb.h"
#include "ImLog.h"
#include "../ProxyConn.h"

#include "EncDec.h"

namespace DB_PROXY {

struct UserRegInfo {
    string name;
    string passwd;
    uint32_t gender;
    string nick_name;
    string avatar_url;
    uint32_t department_id;
    string email;
    string real_name;
    string tel;
    string domain;
    uint32_t status;
};
    
bool checkForExist(CDBConn* conn, string name)
{
    string sqlcmd = "select * from IMUser where name='" + name + "'";
    CResultSet* pResultSet = conn->ExecuteQuery(sqlcmd.c_str());
    bool ret = pResultSet->Next() ? true : false;
    delete pResultSet;
    return ret;
}

bool addUser(CDBConn* conn, UserRegInfo& userinfo)
{
    string salt;
    std::srand(std::time(0));
    salt = std::to_string(std::rand()%10000);
    string inPasswd = userinfo.passwd + salt;
    char md5passwd[33];
    CMd5::MD5_Calculate(inPasswd.c_str(), inPasswd.length(), md5passwd);
    string outPasswd(md5passwd);
    string sqlcmd = "insert into IMUser set name='" + 
        userinfo.name + "',password='" + outPasswd + "',salt='" + salt +"',sex='" + 
        to_string(userinfo.gender) + "',domain='" + userinfo.domain + "',nick='" + userinfo.nick_name +
        "',phone='" + userinfo.tel + "',email='" + userinfo.email + "',avatar='" + 
        userinfo.avatar_url + "',departId='" + to_string(userinfo.department_id) + "', status='" +
        to_string(userinfo.status) + "'";
        
    return conn->ExecuteUpdate(sqlcmd.c_str()) ? true : false;
}

uint32_t getUserId(CDBConn* conn, string name)
{
    uint32_t ret;
    string sqlcmd = "select id from IMUser where name = '" + name + "'";
    CResultSet* pResultSet = conn->ExecuteQuery(sqlcmd.c_str());
    if (pResultSet && pResultSet->Next()) {
        ret =  pResultSet->GetInt("id");
    } else {
        ret = 0;
    }
    delete pResultSet;
    return ret;
}

void doRegister(CImPdu* pPdu, uint32_t conn_uuid)
{
    CImPdu* pPduRes = new CImPdu;
    IM::Server::IMDbRegReq msg;
    IM::Server::IMDbRegRes msgRes;
    string name, passwd;
    IM::BaseDefine::UserInfo *res_userinfo;
    IM::BaseDefine::UserInfo user_info;
    uint32_t userId;

    CDBManager* pDBManger = CDBManager::getInstance();
    CDBConn* pDBConn = pDBManger->GetDBConn("teamtalk_slave");
    if (!pDBConn) {
        loge("Get db teamtalk_slave connection failed");
        msgRes.set_result_code(2);
        msgRes.set_result_string("Get db teamtalk_slave connection failed");
        pDBManger->RelDBConn(pDBConn);
    }

do{
    if(!msg.ParseFromArray(pPdu->GetBodyData(), pPdu->GetBodyLength())) {
        loge("msg.ParseFromArray failed");
        msgRes.set_result_code(2);
        msgRes.set_result_string("msg.ParseFromArray failed");
        break;
    }

    name = msg.user_name();
    passwd = msg.password();
    msgRes.set_user_name(name);
    msgRes.set_attach_data(msg.attach_data());
    
    if (checkForExist(pDBConn, name)) {
        loge("username already exist");
        msgRes.set_result_code(2);
        msgRes.set_result_string("username already exist");
        break;
    }

    user_info = msg.user_info();

    UserRegInfo userinfo;
    userinfo.name = name;
    userinfo.passwd = passwd;
    userinfo.gender = user_info.user_gender();
    userinfo.domain = user_info.user_domain();
    userinfo.nick_name = user_info.user_nick_name();
    userinfo.tel = user_info.user_tel();
    userinfo.email = user_info.email();
    userinfo.avatar_url = user_info.avatar_url();
    userinfo.department_id = user_info.department_id();
    userinfo.status = user_info.status();
    userinfo.real_name = user_info.user_real_name();
    
    
    if (!addUser(pDBConn, userinfo)) {
        loge("add user %s failed", name.c_str());
        msgRes.set_result_code(2);
        msgRes.set_result_string("execute sql failed");
    }
    
    userId = getUserId(pDBConn, name);

}while(0);
    
    res_userinfo = msgRes.mutable_user_info();
    res_userinfo->set_user_id(userId);
    res_userinfo->set_user_gender(user_info.user_gender());
    res_userinfo->set_department_id(user_info.department_id());
    res_userinfo->set_user_nick_name(user_info.user_nick_name());
    res_userinfo->set_user_domain(user_info.user_domain());
    res_userinfo->set_avatar_url(user_info.avatar_url());
    res_userinfo->set_email(user_info.email());
    res_userinfo->set_user_tel(user_info.user_tel());
    res_userinfo->set_user_real_name(user_info.user_real_name());
    res_userinfo->set_status(user_info.status());
    
    pPduRes->SetPBMsg(&msgRes);
    pPduRes->SetSeqNum(pPdu->GetSeqNum());
    
    pPduRes->SetServiceId(IM::BaseDefine::SID_OTHER);
    pPduRes->SetCommandId(IM::BaseDefine::CID_OTHER_DB_REGISTER_RES);
    CProxyConn::AddResponsePdu(conn_uuid, pPduRes);
    pDBManger->RelDBConn(pDBConn);
}
    
}