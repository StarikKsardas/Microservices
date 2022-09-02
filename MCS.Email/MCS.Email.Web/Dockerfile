#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MCS.Email.Web/MCS.Email.Web.csproj", "MCS.Email.Web/"]
COPY ["MCS.Email.Domain.Services/MCS.Email.Domain.Services.csproj", "MCS.Email.Domain.Services/"]
COPY ["MCS.Email.Domain.Contracts/MCS.Email.Domain.Contracts.csproj", "MCS.Email.Domain.Contracts/"]
COPY ["MCS.Email.Infrastructure/MCS.Email.Infrastructure.csproj", "MCS.Email.Infrastructure/"]
COPY ["MCS.Email.Web.Contracts/MCS.Email.Web.Contracts.csproj", "MCS.Email.Web.Contracts/"]
RUN dotnet restore "MCS.Email.Web/MCS.Email.Web.csproj"
COPY . .
WORKDIR "/src/MCS.Email.Web"
RUN dotnet build "MCS.Email.Web.csproj" -c Release  -o /app/build

FROM build AS publish
RUN dotnet publish "MCS.Email.Web.csproj" -c Release  -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCS.Email.Web.dll"]