cmake_minimum_required(VERSION 2.6)
PROJECT(fileserver)
SET(_ ${PROJECT_SOURCE_DIR})

AUX_SOURCE_DIRECTORY(${_} SRC_LIST)
#AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_LIST2)
AUX_SOURCE_DIRECTORY(${_}/../base SRC_BASE)
AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_PB)
AUX_SOURCE_DIRECTORY(${_}/../base/jsoncpp SRC_JSON)

#SET(LIBRARY_OUTPUT_PATH ${_})
SET(EXECUTABLE_OUTPUT_PATH  ${_})
if(APPLE)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/mac)
elseif(UNIX)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/linux)
endif()


ADD_DEFINITIONS( -g -O0 -W -Wall -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11)

INCLUDE_DIRECTORIES(${_} ${_}/../base ${_}/../base/pb/protocol ${_}/../base/jsoncpp/ ${_}/../include ${_}/../include/event2)
LINK_DIRECTORIES(${_} ${_}/../lib)

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED/STATIC ${SRC_LIST})
ADD_EXECUTABLE(file_server ${SRC_LIST} ${SRC_BASE} ${SRC_PB} ${SRC_JSON})

TARGET_LINK_LIBRARIES(file_server pthread log4cxx crypto curl protobuf-lite uuid event hiredis)
 