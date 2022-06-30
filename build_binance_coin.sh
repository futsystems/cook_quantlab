#!/bin/bash

git pull

supervisorctl stop tickhandler.binance.coin


dotnet build BinanceHandlerCoin/ -c release --no-cache --output /opt/tickhandler/binance/coin/release

if [ -f "/opt/tickhandler/binance/coin/appsettings.Production.json" ]; then
	cp -f /opt/tickhandler/binance/coin/appsettings.Production.json /opt/tickhandler/binance/coin/release
fi

supervisorctl start tickhandler.binance.coin
