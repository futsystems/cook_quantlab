#!/bin/bash

git pull

supervisorctl stop srv.tickwriter.quote


dotnet build TickWriter/src/Application -c release --no-cache --output /opt/srv.tickwriter.quote/release

if [ -f "/opt/srv.tickwriter.quote/appsettings.Production.json" ]; then
	cp -f /opt/srv.tickwriter.quote/appsettings.Production.json /opt/srv.tickwriter.quote/release
fi

supervisorctl start srv.tickwriter.quote
