#!/bin/sh
environmentFile=../environment_test.sh
. $environmentFile
docker run -d \
	--name=postgres \
	-e POSTGRES_shared_buffers=${DB_POSTGRES_shared_buffers} \
	-p 5432:5432 \
	-v POSTGRES_db:/var/lib/pgsql/data/ \
	-v /etc/localtime:/etc/localtime \
	flexberry/alt.p8-postgresql	
