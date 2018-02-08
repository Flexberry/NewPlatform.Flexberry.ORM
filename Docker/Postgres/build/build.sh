#!/bin/sh
export thisURL=`~kaf/docker/getThisURL.sh`

unset image tags
. ./conf.sh

#noCache=--no-cache

docker build  $noCache\
  --build-arg http_proxy=$http_proxy \
  -t $image .;

for tag in $tags
do
  docker rmi $tags
  docker tag $image $tags
done
