#!/bin/sh
#!/bin/sh
image=flexberry/alt.p8-postgresql
docker push $image
docker service update \
        --force \
        --image $image \
        --detach=false \
        POSTGRES_postgres

