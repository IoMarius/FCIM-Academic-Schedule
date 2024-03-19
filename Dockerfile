FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-20240312-windowsservercore-ltsc2019  AS builder


WORKDIR /app
COPY . .

RUN nuget restore C:\app\FCIM-Academic-Schedules.sln

RUN msbuild eProiect/eProiect.csproj /p:OutputPath=c:/out1


FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8
WORKDIR /inetpub/wwwroot
COPY --from=builder /out1/_PublishedWebsites/eProiect .
SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';", "icacls", "c:/inetpub/wwwroot", "/grant", "Everyone:(OI)(CI)F"]
HEALTHCHECK CMD powershell -command "try { $response = iwr http://localhost; if ($response.StatusCode -eq 200) { exit 0 } else { exit 1 } } catch { exit 1 }"

