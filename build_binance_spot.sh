#!/bin/bash

git pull

supervisorctl stop tickhandler.binance.spot


dotnet build BinanceHandler/ -c release --no-cache --output /opt/tickhandler/binance/spot/release

if [ -f "/opt/tickhandler/binance/spot/appsettings.Production.json" ]; then
	cp -f /opt/tickhandler/binance/spot/appsettings.Production.json /opt/tickhandler/binance/spot/release
fi

supervisorctl start tickhandler.binance.spot
