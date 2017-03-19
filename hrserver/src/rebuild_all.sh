#!/bin/bash

set -x

if [ $# -eq 1 ] && [ $1 == "clean" ]; then
  cd ./login_server
  ./clean.sh
  cd ../msg_server
  ./clean.sh
  cd ../route_server
  ./clean.sh
  cd ../db_proxy_server
  ./clean.sh
  cd ../msfs_server
  ./clean.sh
  cd ../file_server
  ./clean.sh
  
  exit 0
fi


cd ./login_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../route_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../msg_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../db_proxy_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../file_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../msfs_server
if [ ! -d "CMakeFiles" ]; then
  cmake .
fi
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../login_server
./login_server -d > /dev/null 2>&1

cd ../msg_server
./msg_server -d > /dev/null 2>&1

cd ../route_server
./route_server -d > /dev/null 2>&1

cd ../msfs_server
./msfs_server -d > /dev/null 2>&1

cd ../file_server
./file_server -d > /dev/null 2>&1

cd ../db_proxy_server
./db_proxy_server -d > /dev/null 2>&1