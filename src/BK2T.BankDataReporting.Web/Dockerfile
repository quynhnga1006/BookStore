FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/BK2T.BankDataReporting.Web/BK2T.BankDataReporting.Web.csproj", "src/BK2T.BankDataReporting.Web/"]
COPY ["src/BK2T.BankDataReporting.HttpApi/BK2T.BankDataReporting.HttpApi.csproj", "src/BK2T.BankDataReporting.HttpApi/"]
COPY ["src/BK2T.BankDataReporting.Application.Contracts/BK2T.BankDataReporting.Application.Contracts.csproj", "src/BK2T.BankDataReporting.Application.Contracts/"]
COPY ["src/BK2T.BankDataReporting.Domain.Shared/BK2T.BankDataReporting.Domain.Shared.csproj", "src/BK2T.BankDataReporting.Domain.Shared/"]
COPY ["src/BK2T.BankDataReporting.Application/BK2T.BankDataReporting.Application.csproj", "src/BK2T.BankDataReporting.Application/"]
COPY ["src/BK2T.BankDataReporting.Domain/BK2T.BankDataReporting.Domain.csproj", "src/BK2T.BankDataReporting.Domain/"]
COPY ["src/BK2T.BankDataReporting.MongoDB/BK2T.BankDataReporting.MongoDB.csproj", "src/BK2T.BankDataReporting.MongoDB/"]
RUN dotnet restore "src/BK2T.BankDataReporting.Web/BK2T.BankDataReporting.Web.csproj"
COPY . .
WORKDIR "/src/src/BK2T.BankDataReporting.Web"
RUN dotnet build "BK2T.BankDataReporting.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BK2T.BankDataReporting.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BK2T.BankDataReporting.Web.dll"]
