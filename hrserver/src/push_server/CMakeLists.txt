cmake_minimum_required(VERSION 2.6)
PROJECT(pushserver)
SET(_ ${PROJECT_SOURCE_DIR})
AUX_SOURCE_DIRECTORY(${_} SRC_LIST1)
AUX_SOURCE_DIRECTORY(${_}/socket SRC_LIST2)
AUX_SOURCE_DIRECTORY(${_}/../base/pb/protocol SRC_LIST3)


#SET(LIBRARY_OUTPUT_PATH ${_})
SET(EXECUTABLE_OUTPUT_PATH  ${_})
if(APPLE)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/mac)
elseif(UNIX)
    SET(PB_LIB_DIR ${_}/../base/pb/lib/linux)
endif()

ADD_DEFINITIONS( -g -O0 -fkeep-inline-functions -W -Wall -Wno-deprecated -fdiagnostics-color=always -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11)

INCLUDE_DIRECTORIES(${_} ${_}/../base/pb/protocol ${_}/../base/pb ${_}/../base/slog)
LINK_DIRECTORIES(${_} ${PB_LIB_DIR} ${_}/../base/slog/lib)

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED/STATIC ${SRC_LIST})
ADD_EXECUTABLE(push_server ${SRC_LIST1} ${SRC_LIST2} ${SRC_LIST3})

TARGET_LINK_LIBRARIES(push_server pthread protobuf-lite ssl slog)
