FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY Inquisition.sln ./
COPY Inquisition/Inquisition.csproj Inquisition/
COPY Inquisition.Reporting/Inquisition.Reporting.csproj Inquisition.Reporting/
COPY Inquisition.Exceptions/Inquisition.Exceptions.csproj Inquisition.Exceptions/
COPY Inquisition.Logging/Inquisition.Logging.csproj Inquisition.Logging/
COPY Inquisition.Database/Inquisition.Database.csproj Inquisition.Database/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Inquisition
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Inquisition.dll"]
