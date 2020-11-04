FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Censo.NET.Api/Censo.NET.Api.csproj", "Censo.NET.Api/"]
COPY ["Censo.NET.Application/Censo.NET.Application.csproj", "Censo.NET.Application/"]
COPY ["Censo.NET.Domain/Censo.NET.Domain.csproj", "Censo.NET.Domain/"]
COPY ["Censo.NET.Infrastructure/Censo.NET.Infrastructure.csproj", "Censo.NET.Infrastructure/"]
RUN dotnet restore "Censo.NET.Api/Censo.NET.Api.csproj"
COPY . .
WORKDIR "/src/Censo.NET.Api"
RUN dotnet build "Censo.NET.Api.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "Censo.NET.Api.csproj" -c Release -o /app/publish

FROM base AS final
EXPOSE 2000
WORKDIR /app
COPY wait-for-it.sh ./wait/
RUN ["chmod", "+x", "./wait/wait-for-it.sh"]
COPY --from=publish /app/publish .