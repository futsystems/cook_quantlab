#!/bin/bash

git pull

supervisorctl stop srv.tickwriter.trades


dotnet build TickWriter/ -c release --no-cache --output /opt/srv.tickwriter.trades/release

if [ -f "/opt/srv.tickwriter.trades/appsettings.Production.json" ]; then
	cp -f /opt/srv.tickwriter.trades/appsettings.Production.json /opt/srv.tickwriter.trades/release
fi

supervisorctl start srv.tickwriter.trades
