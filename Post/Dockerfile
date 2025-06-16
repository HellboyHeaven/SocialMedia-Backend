ARG SERVICE=Post
ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:9.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:9.0

# Base for build
FROM ${DOTNET_SDK} AS publish
ARG SERVICE
WORKDIR /src
EXPOSE 8080
EXPOSE 8081
COPY . .
WORKDIR /src
RUN dotnet restore ./${SERVICE}.sln
RUN dotnet publish "./${SERVICE}.API/${SERVICE}.API.csproj" -c Release -o /app/publish --no-restore


FROM ${DOTNET_RUNTIME} AS base
ARG SERVICE
WORKDIR /app
COPY --from=publish /app/publish .

ENV SERVICE=${SERVICE}
ENTRYPOINT dotnet ${SERVICE}.API.dll
