# ===== Этап 1: Сборка =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем всё остальное и публикуем
COPY . ./
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===== Этап 2: Запуск =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "wt-lab7-7postgres-efcore-liger.dll"]