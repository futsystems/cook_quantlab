#!/bin/bash

git pull

supervisorctl stop srv.datafarm


dotnet build Srv.DataFarm/src/Application -c release --no-cache --output /opt/srv.datafarm/release

if [ -f "/opt/srv.datafarm/appsettings.Production.json" ]; then
	cp -f /opt/srv.datafarm/appsettings.Production.json /opt/srv.datafarm/release
fi

supervisorctl start srv.datafarm
