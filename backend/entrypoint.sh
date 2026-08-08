#!/bin/sh
set -e

echo "Running migrations..."
until ./migrate --connection "${ConnectionStrings__DefaultConnection}"; do
    echo "Migration failed, retrying in 5s..."
    sleep 5
done

echo "Starting backend..."
exec dotnet MediaTracker.WebAPI.dll