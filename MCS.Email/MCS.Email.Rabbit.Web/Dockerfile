#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MCS.Email.Rabbit.Web/MCS.Email.Rabbit.Web.csproj", "MCS.Email.Rabbit.Web/"]
RUN dotnet restore "MCS.Email.Rabbit.Web/MCS.Email.Rabbit.Web.csproj"
COPY . .
WORKDIR "/src/MCS.Email.Rabbit.Web"
RUN dotnet build "MCS.Email.Rabbit.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCS.Email.Rabbit.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCS.Email.Rabbit.Web.dll"]