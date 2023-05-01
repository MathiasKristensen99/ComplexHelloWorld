FROM mysql:5.7.15

ENV MYSQL_DATABASE=HelloWorld \
    MYSQL_ROOT_PASSWORD=MyS£creT7

ADD Database/schema.sql /docker-entrypoint-initdb.d

EXPOSE 3306