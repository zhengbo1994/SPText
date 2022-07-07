# 1.副本集



~~~shell
docker pull mongo

mkdir -p /app/docker/mongo1/db   #创建挂载的db目录

mkdir -p /app/docker/mongo2/db   #创建挂载的db目录

mkdir -p /app/docker/mongo3/db   #创建挂载的db目录
#第一台：
docker run --name mongo-server1 -p 30001:27017 --restart=always -v /app/docker/mongo1/db:/data/db -v /etc/localtime:/etc/localtime -d mongo  --replSet "rs0"  --bind_ip_all
#第二台：
docker run --name mongo-server2 -p 30002:27018 --restart=always -v /app/docker/mongo2/db:/data/db -v /etc/localtime:/etc/localtime -d mongo  --replSet "rs0"  --bind_ip_all
#第三台：
docker run --name mongo-server3 -p 30003:27019 --restart=always -v /app/docker/mongo3/db:/data/db -v /etc/localtime:/etc/localtime -d mongo  --replSet "rs0"  --bind_ip_all

 

#进入容器 进入主的容器
docker exec -it mongo-server1 /bin/bash 
#连接客户端
mongo

rs.initiate()

rs.add("localhost:30002") 

rs.addArb("localhost:30003")
#如果想要在从节点查询（默认是客户端直连是不可查询的）
如果想要在端口30002 从服务上执行查询，则连接上之后需要执行 db.getMongo().setSlaveOk(); 
~~~

## 

# 2.分片

### 2.1下载 mongo最新版本

docker pull mongo 

### 2.2创建配置服务复制集

192.198.1.201

~~~shell
docker run -d --name configsvr0 -p 10021:27019 -v /home/mongodb/data/cs/configsvr0:/data/configdb mongo --configsvr --replSet "rs_configsvr" --bind_ip_all
~~~

192.198.1.202

~~~shell
docker run -d --name configsvr1 -p 10022:27019 -v /home/mongodb/data/cs/configsvr1:/data/configdb mongo --configsvr --replSet "rs_configsvr" --bind_ip_all
~~~

192.198.1.203

~~~shell
docker run -d --name configsvr2 -p 10023:27019 -v /home/mongodb/data/cs/configsvr2:/data/configdb mongo --configsvr --replSet "rs_configsvr" --bind_ip_all
~~~

### 2.3初始化配置服务复制集：

192.168.3.201上执行

docker exec -it configsvr0 bash
mongo --host 192.168.1.201 --port 10021

```
 rs.initiate(
   {
      _id: "rs_configsvr",
      configsvr: true,
      members: [
         { _id: 0, host : "192.168.1.201:10021" },
         { _id: 1, host : "192.168.1.202:10022" },
         { _id: 2, host : "192.168.1.203:10023" }
      ]
   }
);
```

### 2.4.创建分片复制集

192.168.1.201：

~~~shell
docker run --name shardsvr00 -p 10031:27018 -d -v /home/mongodb/data/sh/shardsvr00:/data/db mongo --shardsvr --replSet "rs_shardsvr0" --bind_ip_all

docker run --name shardsvr10 -p 10041:27018 -d -v /home/mongodb/data/sh/shardsvr10:/data/db mongo --shardsvr --replSet "rs_shardsvr1" --bind_ip_all


~~~

192.168.1.202：

~~~shell
docker run --name shardsvr01 -p 10032:27018 -d -v /home/mongodb/data/sh/shardsvr00:/data/db mongo --shardsvr --replSet "rs_shardsvr0" --bind_ip_all
docker run --name shardsvr11 -p 10042:27018 -d -v /home/mongodb/data/sh/shardsvr11:/data/db mongo --shardsvr --replSet "rs_shardsvr1" --bind_ip_all
~~~

192.168.3.203：

~~~shell
docker run --name shardsvr02 -p 10033:27018 -d -v /home/mongodb/data/sh/shardsvr00:/data/db mongo --shardsvr --replSet "rs_shardsvr0" --bind_ip_all
docker run --name shardsvr12 -p 10043:27018 -d -v /home/mongodb/data/sh/shardsvr12:/data/db mongo --shardsvr --replSet "rs_shardsvr1" --bind_ip_all
~~~

### 2.5初始化副本集

192.168.1.201执行

docker exec -it shardsvr00 bash
mongo --host 192.168.1.201 --port 10031

~~~shell
rs.initiate(
   {
      _id: "rs_shardsvr0",
      members: [
         { _id: 0, host : "192.168.1.201:10031" },
         { _id: 1, host : "192.168.1.202:10032" },
         { _id: 2, host : "192.168.1.203:10033" }
      ]
   }
);
~~~

docker exec -it shardsvr10 bash
mongo --host 192.168.1.201 --port 10041

~~~shell
rs.initiate(
   {
      _id: "rs_shardsvr0",
      members: [
         { _id: 0, host : "192.168.1.201:10041" },
         { _id: 1, host : "192.168.1.202:10042" },
         { _id: 2, host : "192.168.1.203:10043" }
      ]
   }
);
~~~

### 2.6.创建mongos，连接mongos到分片集群

192.168.1.201：

docker run --name mongos0 -d -p 10011:27017 --entrypoint "mongos" mongo --configdb rs_configsvr/192.168.1.201:10021,192.168.1.202:10022,192.168.1.203:10023 --bind_ip_all

192.168.3.202：

docker run --name mongos1 -d -p 10012:27017 --entrypoint "mongos" mongo --configdb rs_configsvr/192.168.1.201:10021,192.168.1.202:10022,192.168.1.203:10023 --bind_ip_all

### 2.7添加分片到集群

192.168.3.201：

docker exec -it mongos0 bash
mongo --host 192.168.1.201 --port 10011

sh.addShard("rs_shardsvr0/192.168.1.201:10031,192.168.1.202:10032,192.168.1.203:10033")
sh.addShard("rs_shardsvr1/192.168.1.201:10041,192.168.1.202:10042,192.168.1.203:10043")

## 数据库 启用 分片

sh.enableSharding("test")

## 分片集合

对 test.order 的 _id 字段进行哈希分片：

sh.shardCollection("test.order", {"_id": "hashed" })

## 插入数据

use test
for (i = 1;i <= 100;i=i+1){
db.order.insert({'price': 1})
}

 

## 查看数据分布 

db.order.find().count()

## 3.管理工具

~~~shell
##这个比较完美可以用---还有其他好多好多

docker run -it --rm \
    --name mongo-express \
    -p 8081:8081 \
    -e ME_CONFIG_OPTIONS_EDITORTHEME="ambiance" \
    -e ME_CONFIG_MONGODB_SERVER="192.168.1.201" \
	-e ME_CONFIG_MONGODB_PORT="10011"          \
    -e ME_CONFIG_BASICAUTH_USERNAME="admin" \
    -e ME_CONFIG_BASICAUTH_PASSWORD="admin" \
    mongo-express
~~~



~~~shell
# 有异常.,结合做容器的哥们没做好 .刚开始是好的,过一会就关了,因为node.js

docker pull fourfire/admin-mongo //拉取镜像

docker run -d -p 8001:1234 --name admin-mongo --restart always fourfire/admin-mongo 

docker run -d -p 8082:1234 --name admin-mongo 0x59/admin-mongo:latest

admin-mongo默认开启了login-auth
# default login config
"username":"admin",
"password":"admin1234"

-- 如果不需要取消用户名和密码则不需要执行下面指令

docker exec -it admin-mongo bash

vim /home/admin-mongo/config/app.json


{
  "app": {
    "host": "0.0.0.0", //listen mongo ip
    "port": 1234, //listen port
    "username": "admin", //如果不想开启登录限制，删除 username和password配置即可
    "password": "admin1234",
    "locale": "zh-cn", //local language,or "en"
    "context": "dbApp", 
    "monitoring": true
  }
}

docker restart admin-mongo
~~~



