#ifndef __IM_REGISTER_H__
#define __IM_REGISTER_H__

#include "ImPduBase.h"

namespace DB_PROXY {

void doRegister(CImPdu* pPdu, uint32_t conn_uuid);

};

#endif