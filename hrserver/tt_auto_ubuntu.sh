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


git clone https://github.com/highwayns/highwayns.git

mysql -uroot -p12345 < highwayns/hrserver/auto_setup/mariadb/conf/ttopen.sql

fi

if [ $# -eq 0 ]; then
  cd highwayns/hrserver/src
else
  cd $1/src
fi

python change_conf.py

sudo apt-get install subversion 

svn checkout http://svn.apache.org/repos/asf/incubator/log4cxx/trunk apache-log4cxx 
./autogen.sh
./configure
make
make check
sudo make install



tar -zxvf protobuf-2.6.1.tar.gz # 解?

sudo apt-get install build-essential # 不装会??

cd protobuf-2.6.1/ # ?入目?

./configure # 配置安装文件

make # ??

make check # ????安装的?境

sudo make install # 安装

git clone https://github.com/libevent/libevent.git
$ mkdir build && cd build
 $ cmake ..     # Default to Unix Makefiles.
 $ make
 $ make verify  # (optional)

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

apt-get install libmysqlclient-dev
apt-get install mariadb-server-10.1

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

lsof -i -Pn