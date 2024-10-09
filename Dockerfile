FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/Remake_Kafka_Example_01.csproj", "Remake_Kafka_Example_01/"]
RUN dotnet restore "Remake_Kafka_Example_01/Remake_Kafka_Example_01.csproj"
COPY . .
RUN dotnet build "Remake_Kafka_Example_01.csproj" -c $BUILD_CONFIGURATION -o /app/build/

FROM build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Remake_Kafka_Example_01.csproj" -c $BUILD_CONFIGURATION -o /app/publish/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Remake_Kafka_Example_01.dll" ]