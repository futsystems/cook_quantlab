#!/bin/bash

git pull

supervisorctl stop tickhandler.binance.usd


dotnet build BinanceHandlerU/ -c release --no-cache --output /opt/tickhandler/binance/usd/release

if [ -f "/opt/tickhandler/binance/usd/appsettings.Production.json" ]; then
	cp -f /opt/tickhandler/binance/usd/appsettings.Production.json /opt/tickhandler/binance/usd/release
fi

supervisorctl start tickhandler.binance.usd
