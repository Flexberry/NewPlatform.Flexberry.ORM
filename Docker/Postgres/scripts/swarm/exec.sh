#!/bin/sh
set -- `docker ps | grep flexberry/alt.p8-postgresql:latest`
id=$1
if [ "$id" ]
then
        docker exec -it $id bash
fi

