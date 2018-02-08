#!/bin/sh
docker run -d \
	--name=postgres \
	-e POSTGRES_shared_buffers=1440MB \
	dh.ics.perm.ru/kaf/alt.p8-postgresql9.6-ru
