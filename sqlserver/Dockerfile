FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y

USER root

RUN mkdir -p /var/opt/mssql/data \
    && mkdir -p /var/opt/mssql/log \
    && mkdir -p /var/opt/mssql/secrets

USER mssql

EXPOSE 1433