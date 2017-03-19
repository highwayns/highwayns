cmake_minimum_required(VERSION 2.6)
PROJECT(Base)
SET(_ ${PROJECT_SOURCE_DIR})
AUX_SOURCE_DIRECTORY(${_} SRC_LIST1)
AUX_SOURCE_DIRECTORY(${_}/pb/protocol SRC_LIST2)
AUX_SOURCE_DIRECTORY(${_}/jsoncpp SRC_LIST3)

if(APPLE)
    SET(PB_LIB_DIR ${_}/pb/lib/mac)
elseif(UNIX)
    SET(PB_LIB_DIR ${_}/pb/lib/linux)
endif()

SET(LIBRARY_OUTPUT_PATH ${_}/../lib)
#SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${_}")
#SET(EXECUTABLE_OUTPUT_PATH  ${_}/bin)

ADD_DEFINITIONS( -g -O0 -W -Wno-deprecated -Wno-unused-parameter -Wno-unused-function -Wno-unused-variable -fdiagnostics-color=always -Wall -D_REENTRANT -D_FILE_OFFSET_BITS=64 -DAC_HAS_INFO
-DAC_HAS_WARNING -DAC_HAS_ERROR -DAC_HAS_CRITICAL -DTIXML_USE_STL
-DAC_HAS_DEBUG -DLINUX_DAEMON -std=c++11)

INCLUDE_DIRECTORIES(${_} ${_}/jsoncpp ${_}/pb ${_}/pb/protocol ${_}/../include ${_}/../include/event2)
LINK_DIRECTORIES(${_} ${_}/../lib)

ADD_LIBRARY(base STATIC ${SRC_LIST1} ${SRC_LIST2} ${SRC_LIST3})

#ADD_XXXX必须在TARGET_LINK_LIBRARIES前面，否则会报错
#ADD_LIBRARY(${PROJECTNAME} SHARED ${SRC_LIST})
#ADD_EXECUTABLE(${PROJECTNAME} ${SRC_LIST})

TARGET_LINK_LIBRARIES(base pthread crypto log4cxx event hiredis)
