#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2525

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
CMD ls -la
COPY ["./ELKApiSolution/ELKApi/ELKApi.csproj", "ELKApi/"]
WORKDIR "/src/ELKApi"
RUN dotnet restore "./ELKApi.csproj"
COPY . .
RUN dotnet build "ELKApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ELKApi.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=https://+:2525
ENV ASPNETCORE_HTTPS_PORT=2525
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="elk_api"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/elk_api.pfx
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ELKApi.dll"]