FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env

WORKDIR /home/app

COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore

COPY . .

RUN dotnet publish ./carbon.api/carbon.api.csproj -o /publish/

WORKDIR /publish

ENTRYPOINT ["dotnet", "carbon.api.dll"]
