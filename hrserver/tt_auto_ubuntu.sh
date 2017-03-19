#!/bin/sh

set -x

if [ $# -eq 0 ]; then

sudo apt-get update

sudo apt-get install -y software-properties-common
sudo add-apt-repository ppa:git-core/ppa
sudo add-apt-repository ppa:ubuntu-toolchain-r/test

sudo apt-get update

sudo apt-get install -y git

sudo apt-get install -y cmake
sudo apt-get install -y g++-5

rm /usr/bin/g++
ln -s /usr/bin/g++-5 /usr/bin/g++

sudo apt-get install -y libcurl4-openssl-dev
sudo apt-get install -y apache2-dev libapr1-dev libaprutil1-dev

sudo apt-get install -y redis-server
sudo apt-get install -y mysql-server


git clone https://git.oschina.net/benben-de-eggs/teamtalk-server-benben.git

mysql -uroot -p12345 < teamtalk-server-benben/auto_setup/mariadb/conf/ttopen.sql

fi

if [ $# -eq 0 ]; then
  cd teamtalk-server-benben/src
else
  cd $1/src
fi

python change_conf.py

cd ./login_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../route_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../msg_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../db_proxy_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../file_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../msfs_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
make
if [ $? -ne 0 ]
then
  exit 1
fi

cd ../http_msg_server
if [ -d "CMakeFiles" ]; then
./clean.sh
fi
cmake .
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

cd ../http_msg_server
./http_msg_server -d > /dev/null 2>&1