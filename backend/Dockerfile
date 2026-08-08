FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MediaTracker.slnx .
COPY MediaTracker.Domain/MediaTracker.Domain.csproj MediaTracker.Domain/
COPY MediaTracker.Application/MediaTracker.Application.csproj MediaTracker.Application/
COPY MediaTracker.Infrastructure/MediaTracker.Infrastructure.csproj MediaTracker.Infrastructure/
COPY MediaTracker.WebAPI/MediaTracker.WebAPI.csproj MediaTracker.WebAPI/

RUN dotnet restore

COPY . .

RUN dotnet publish MediaTracker.WebAPI/MediaTracker.WebAPI.csproj \
    -c Release \
    -o /app/publish

# Generate migration bundle - standalone executable, no SDK needed at runtime
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef migrations bundle \
    --project MediaTracker.Infrastructure \
    --startup-project MediaTracker.WebAPI \
    --output /app/migrate \
    --self-contained \
    --force

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
COPY --from=build /app/migrate ./migrate

COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

EXPOSE 8080
ENTRYPOINT ["sh", "./entrypoint.sh"]