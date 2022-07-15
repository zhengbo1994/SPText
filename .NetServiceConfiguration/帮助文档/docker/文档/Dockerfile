FROM php:8.1.6-fpm
##指定php容器工作目录为www,不存在时将自动创建www
#WORKDIR /www
##拷贝程序到php容器，这里注意因为在compsoe context指定了当前环境是laravel-myadmin子目录，所以将拷贝
##backEnd frontEnd localHost三个文件到php的www目录下
#COPY . /www
#RUN   docker-php-source extract \
#	&& pecl install xdebug-3.1.4 \
#    && docker-php-ext-enable xdebug \
#	&& docker-php-source delete \


