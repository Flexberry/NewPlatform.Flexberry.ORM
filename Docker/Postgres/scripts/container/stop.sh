#!/bin/sh
name=postgres
docker stop $name
docker rm $name
