cmake_minimum_required(VERSION 2.6)
PROJECT(msgserver)
SET(_ ${PROJECT_SOURCE_DIR})
AUX_SOURCE_DIRECTORY(${_} SRC_LIST)
AUX_SOURCE_DIRECTORY(${_}/../base SRC_BASE)
AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_PB)
AUX_SOURCE_DIRECTORY(${_}/../base/jsoncpp SRC_JSON)

#SET(LIBRARY_OUTPUT_PATH ${_})
SET(EXECUTABLE_OUTPUT_PATH  ${_})
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}")
if(APPLE)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/mac)
    SET(SECURITY_LIB_DIR ${_}/../base/security/lib/mac)
elseif(UNIX)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/linux)
    SET(SECURITY_LIB_DIR ${_}/../base/security/lib/linux)
endif()


ADD_DEFINITIONS( -g -O0 -W -Wall -Wno-deprecated -Wno-unused-parameter -Wno-unused-function -Wno-unused-variable -fdiagnostics-color=always -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11)

INCLUDE_DIRECTORIES(${_} ${_}/../base ${_}/../base/pb/protocol ${_}/../base/security ${_}/../base/jsoncpp ${_}/../include ${_}/../include/event2)
LINK_DIRECTORIES(${_} ${_}/../lib ${SECURITY_LIB_DIR})

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED/STATIC ${SRC_LIST})
ADD_EXECUTABLE(msg_server ${SRC_LIST} ${SRC_BASE} ${SRC_PB} ${SRC_JSON})

TARGET_LINK_LIBRARIES(msg_server log4cxx protobuf-lite crypto pthread curl event hiredis)
 
