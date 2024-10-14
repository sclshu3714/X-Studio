# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 44344
EXPOSE 44345
EXPOSE 44346
EXPOSE 44355

# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/XStudio.HttpApi.Host/XStudio.HttpApi.Host.csproj", "src/XStudio.HttpApi.Host/"]
COPY ["src/XStudio.Application/XStudio.Application.csproj", "src/XStudio.Application/"]
COPY ["src/XStadio.Modules/XStudio.Common/XStudio.Common.csproj", "src/XStadio.Modules/XStudio.Common/"]
COPY ["src/XStudio.Domain/XStudio.Domain.csproj", "src/XStudio.Domain/"]
COPY ["src/XStudio.Domain.Shared/XStudio.Domain.Shared.csproj", "src/XStudio.Domain.Shared/"]
COPY ["src/XStudio.Application.Contracts/XStudio.Application.Contracts.csproj", "src/XStudio.Application.Contracts/"]
COPY ["src/XStudio.EntityFrameworkCore/XStudio.EntityFrameworkCore.csproj", "src/XStudio.EntityFrameworkCore/"]
COPY ["src/XStudio.HttpApi/XStudio.HttpApi.csproj", "src/XStudio.HttpApi/"]
RUN dotnet restore "./src/XStudio.HttpApi.Host/XStudio.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/XStudio.HttpApi.Host"
RUN dotnet build "./XStudio.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./XStudio.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 切换到 root 用户以执行 apt-get 命令
USER root
RUN apt-get update && apt-get install -q -y --no-install-recommends \
    procps \
    net-tools \
    && rm -rf /var/lib/apt/lists/*

USER app
ENTRYPOINT ["dotnet", "XStudio.HttpApi.Host.dll"]