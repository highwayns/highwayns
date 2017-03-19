cmake_minimum_required(VERSION 2.6)
PROJECT(msfs_server)
SET(_ ${PROJECT_SOURCE_DIR})
AUX_SOURCE_DIRECTORY(${_} SRC_LIST)
AUX_SOURCE_DIRECTORY(${_}/../base SRC_BASE)
AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_PB)
AUX_SOURCE_DIRECTORY(${_}/../base/jsoncpp SRC_JSON)

#SET(LIBRARY_OUTPUT_PATH ${_})
SET(EXECUTABLE_OUTPUT_PATH  ${_})

ADD_DEFINITIONS(-Wno-deprecated -Wno-unused-parameter -Wno-unused-function -Wno-unused-variable -fdiagnostics-color=always -g -O0 -W -Wall -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11)

INCLUDE_DIRECTORIES(${_} ${_}/../base ${_}/../base/pb/protocol ${_}/../base/jsoncpp/ ${_}/../include ${_}/../include/event2)
LINK_DIRECTORIES(${_} ${_}/../lib)

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED/STATIC ${SRC_LIST})
ADD_EXECUTABLE(msfs_server ${SRC_LIST} ${SRC_BASE} ${SRC_PB} ${SRC_JSON})

TARGET_LINK_LIBRARIES(msfs_server pthread log4cxx protobuf-lite curl crypto event hiredis)
 
