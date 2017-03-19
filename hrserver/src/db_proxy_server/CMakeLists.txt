cmake_minimum_required(VERSION 2.6)
PROJECT(db_proxy_server)
SET(_ ${PROJECT_SOURCE_DIR})
AUX_SOURCE_DIRECTORY(${_} SRC_LIST)
AUX_SOURCE_DIRECTORY(${_}/business SRC_LIST)
AUX_SOURCE_DIRECTORY(${_}/../base SRC_BASE)
AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_PB)
AUX_SOURCE_DIRECTORY(${_}/../base/jsoncpp SRC_JSON)
#FOREACH(F ${SRC_LIST})
#MESSAGE(STATUS ${F})
#ENDFOREACH(F)

#SET(LIBRARY_OUTPUT_PATH ${_})
SET(EXECUTABLE_OUTPUT_PATH  ${_})
if(APPLE)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/mac)
elseif(UNIX)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/linux)
endif()

SET(MYSQL_INCLUDE_DIR /usr/include/mysql)
SET(MYSQL_LIB /usr/lib64/mysql)

ADD_DEFINITIONS( -g -O0 -W -Wno-deprecated -Wno-unused-parameter -Wno-unused-function -Wno-unused-variable -fdiagnostics-color=always -Wall -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11 -DENCRYPT)

INCLUDE_DIRECTORIES(${_} ${_}/businiess ${_}/../base ${_}/../base/jsoncpp ${_}/../base/pb/protocol ${_}/../include ${MYSQL_INCLUDE_DIR} ${_}/../include/event2)
LINK_DIRECTORIES(${_} ${PB_LIB_DIR} ${MYSQL_LIB} ${_}/../lib)

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED/STATIC ${SRC_LIST})
ADD_EXECUTABLE(db_proxy_server ${SRC_LIST} ${SRC_BASE} ${SRC_PB} ${SRC_JSON})

TARGET_LINK_LIBRARIES(db_proxy_server pthread protobuf-lite mysqlclient_r hiredis curl log4cxx crypto event hiredis)
 
