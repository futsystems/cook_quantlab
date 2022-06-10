#!/bin/bash
  
supervisorctl stop srv.notify

git submodule init
git submodule update
git pull

dotnet build src/Application/ -c release --no-cache --output /opt/srv.notify/release


if [ -f "/opt/srv.notify/appsettings.Production.json" ]; then
	cp -f /opt/srv.notify/appsettings.Production.json /opt/srv.notify/release
fi

supervisorctl start srv.notify
