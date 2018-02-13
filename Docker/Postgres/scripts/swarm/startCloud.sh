#!/bin/sh
environmentFile=../environment_test.sh
. $environmentFile

Stack=POSTGRES
Net=${Stack}_default
# Wait for previous stack POSTGRES complete 
while docker network inspect $Net >/dev/null 2>&1; do sleep 1; done
# Start new stack
docker stack deploy -c cloud.yml $Stack

