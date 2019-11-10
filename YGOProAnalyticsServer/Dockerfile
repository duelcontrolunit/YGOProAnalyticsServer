FROM microsoft/dotnet:2.2-sdk

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN dotnet publish ./YGOProAnalyticsServer.csproj -c $BUILDCONFIG /p:Version=$VERSION
EXPOSE 44326
EXPOSE 53490
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
