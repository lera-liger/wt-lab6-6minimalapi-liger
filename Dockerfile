# Multi-stage Docker build for ASP.NET Core 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "wt-lab6-6minimalapi-liger.csproj"
RUN dotnet publish "wt-lab6-6minimalapi-liger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "wt-lab6-6minimalapi-liger.dll"]
