/*
 * ServInfo.h
 *
 *  Created on: 2013-7-19
 *      Author: ziteng@mogujie.com
 */

#ifndef SERVINFO_H_
#define SERVINFO_H_

#include <memory>
#include "util.h"
#include "imconn.h"
#include "ConfigFileReader.h"

#define MAX_RECONNECT_CNT	64
#define MIN_RECONNECT_CNT	4


typedef struct {
	string		server_ip;
	uint16_t	server_port;
	uint32_t	idle_cnt;
	uint32_t	reconnect_cnt;
	CImConn*	serv_conn;
} serv_info_t;

template <class T>
void serv_init(serv_info_t* server_list, uint32_t server_count)
{
	for (uint32_t i = 0; i < server_count; i++) {
		T* pConn = new T();
		pConn->Connect(server_list[i].server_ip.c_str(), server_list[i].server_port, i);
		server_list[i].serv_conn = pConn;
		server_list[i].idle_cnt = 0;
		server_list[i].reconnect_cnt = MIN_RECONNECT_CNT / 2;
	}
}

template <class T>
void serv_check_reconnect(serv_info_t* server_list, uint32_t server_count)
{
	T* pConn;
	for (uint32_t i = 0; i < server_count; i++) {
		pConn = (T*)server_list[i].serv_conn;
		if (!pConn) {
			server_list[i].idle_cnt++;
			if (server_list[i].idle_cnt >= server_list[i].reconnect_cnt) {
				pConn = new T();
				pConn->Connect(server_list[i].server_ip.c_str(), server_list[i].server_port, i);
				server_list[i].serv_conn = pConn;
			}
		}
	}
}

template <class T>
void serv_reset(serv_info_t* server_list, uint32_t server_count, uint32_t serv_idx)
{
	if (serv_idx >= server_count) {
		return;
	}

	server_list[serv_idx].serv_conn = NULL;
	server_list[serv_idx].idle_cnt = 0;
	server_list[serv_idx].reconnect_cnt *= 2;
	if (server_list[serv_idx].reconnect_cnt > MAX_RECONNECT_CNT) {
		server_list[serv_idx].reconnect_cnt = MIN_RECONNECT_CNT;
	}
}

serv_info_t* read_server_config(CConfigFileReader* config_file, const char* server_ip_format,
		const char* server_port_format, uint32_t& server_count);

template <class T>
struct CServInfo {
	string		server_ip;
	uint16_t	server_port;
	uint32_t	idle_cnt;
	uint32_t	reconnect_cnt;
	std::shared_ptr<T>	serv_conn;

	static void Init(CServInfo<T>* server_list, uint32_t server_count) {
		for (uint32_t i = 0; i < server_count; i++) {
			auto pConn = std::shared_ptr<T>(new T());
			pConn->Connect(server_list[i].server_ip.c_str(), server_list[i].server_port, i);
			server_list[i].serv_conn = pConn;
			server_list[i].idle_cnt = 0;
			server_list[i].reconnect_cnt = MIN_RECONNECT_CNT / 2;
		}
	}
	
	static void CheckReconnect(CServInfo<T>* server_list, uint32_t server_count) {
		for (uint32_t i = 0; i < server_count; i++) {
			auto pConn = server_list[i].serv_conn;
			if (!pConn) {
				server_list[i].idle_cnt++;
				// if (server_list[i].idle_cnt >= server_list[i].reconnect_cnt) {
				if (1) {
					pConn = std::shared_ptr<T>(new T());
					pConn->Connect(server_list[i].server_ip.c_str(), server_list[i].server_port, i);
					server_list[i].serv_conn = pConn;
				}
			}
		}
	}

	static void Reset(CServInfo<T>* server_list, uint32_t server_count, uint32_t serv_idx) {
		if (serv_idx >= server_count) {
			return;
		}
	
		server_list[serv_idx].serv_conn = nullptr;
		server_list[serv_idx].idle_cnt = 0;
		server_list[serv_idx].reconnect_cnt *= 2;
		if (server_list[serv_idx].reconnect_cnt > MAX_RECONNECT_CNT) {
			server_list[serv_idx].reconnect_cnt = MIN_RECONNECT_CNT;
		}
	}
	
	static CServInfo<T>* ReadServerConfig(CConfigFileReader* config_file, const char* server_ip_format,
			const char* server_port_format, uint32_t& server_count) {
				
		char server_ip_key[64];
		char server_port_key[64];
	
		server_count = 0;
	
		// get server_count first;
		while (true) {
			sprintf(server_ip_key, "%s%d", server_ip_format, server_count + 1);
			sprintf(server_port_key, "%s%d", server_port_format, server_count + 1);
			char* server_ip_value = config_file->GetConfigName(server_ip_key);
			char* server_port_value = config_file->GetConfigName(server_port_key);
			if (!server_ip_value || !server_port_value) {
				break;
			}
	
			server_count++;
		}
	
		if (server_count == 0) {
			return NULL;
		}
	
		auto server_list = new CServInfo<T> [server_count];
	
		for (uint32_t i = 0; i < server_count; i++) {
			sprintf(server_ip_key, "%s%d", server_ip_format, i + 1);
			sprintf(server_port_key, "%s%d", server_port_format, i + 1);
			char* server_ip_value = config_file->GetConfigName(server_ip_key);
			char* server_port_value = config_file->GetConfigName(server_port_key);
			server_list[i].server_ip = server_ip_value;
			server_list[i].server_port = atoi(server_port_value);
		}
	
		return server_list;
	}

};

#endif /* SERVINFO_H_ */
