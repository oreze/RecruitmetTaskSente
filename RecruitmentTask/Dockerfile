FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RecruitmentTask/RecruitmentTask.csproj", "RecruitmentTask/"]
RUN dotnet restore "RecruitmentTask/RecruitmentTask.csproj"
COPY . .
WORKDIR "/src/RecruitmentTask"
RUN dotnet build "RecruitmentTask.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecruitmentTask.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecruitmentTask.dll"]
