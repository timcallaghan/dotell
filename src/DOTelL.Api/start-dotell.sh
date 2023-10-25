#!/bin/bash

# Wait until PostgreSQL started and listens on port 5432.
while [ -z "`netstat -tln | grep 5432`" ]; do
  echo 'Waiting for PostgreSQL to start ...'
  sleep 1
done
echo 'PostgreSQL started.'

# Start dotell.
echo 'Starting dotell...'
dotnet /app/DOTelL.Api.dll