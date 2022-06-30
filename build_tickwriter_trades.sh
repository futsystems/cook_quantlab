#!/bin/bash

git pull

supervisorctl stop srv.tickwriter.trade


dotnet build TickWriter/ -c release --no-cache --output /opt/srv.tickwriter.trade/release

if [ -f "/opt/srv.tickwriter.trade/appsettings.Production.json" ]; then
	cp -f /opt/srv.tickwriter.trade/appsettings.Production.json /opt/srv.tickwriter.trade/release
fi

supervisorctl start srv.tickwriter.trade
