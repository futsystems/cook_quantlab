FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

RUN mkdir /app
RUN mkdir /app/bin
RUN mkdir /app/config
RUN mkdir /app/logs

COPY publish/ app/bin/
WORKDIR /app/bin
ENTRYPOINT ["dotnet", "UniCryptoLab.Srv.Product.dll"]
