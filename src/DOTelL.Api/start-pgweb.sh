#!/bin/bash

# Wait until dotell started and listens on port 4317.
while [ -z "`netstat -tln | grep 4317`" ]; do
  echo 'Waiting for DOTelL to start ...'
  sleep 1
done
echo 'DOTelL started.'

# Start pgweb.
echo 'Starting pgweb...'
pgweb --host localhost --user postgres --pass password --bind 0.0.0.0 --listen 5042 --db dotell --skip-open