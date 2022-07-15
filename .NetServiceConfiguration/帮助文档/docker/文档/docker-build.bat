::创建和启动容器，-d以后台方式运行
::docker compose up -d

::强制重新构建镜像，所以修改镜像dockerfile配置后需要使用--force-recreate
::docker-compose up -d --force-recreate
docker-compose up --build -d

::停止一个已经运行的容器，但不删除它，可通过docker-compose start重新启动它
::docker-compose stop

::停止并删除容器
::docker-compose down
