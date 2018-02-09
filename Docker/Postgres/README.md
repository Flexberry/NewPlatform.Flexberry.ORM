# Описание
[Образ flexberry/alt.p8-postgresql](https://hub.docker.com/r/flexberry/alt.p8-postgresql/) поддерживает функционал базы данных Postres.
Характерные особенности образа:
- в качестве базового образа используется  [образ дистрибутив ALTLinux P8](https://hub.docker.com/r/fotengauer/altlinux-p8/). входяшего  в [Единый реестр российских программ для электронных вычислительных машин и баз данных](https://reestr.minsvyaz.ru/);
- задание основных параметров конфигурации при запуске контейнера путем определения параметров среды. 

## Запуск образа в виде swarm сервиса

При запуске образа в виде swarm сервиса можно вопользоваться следующим шаблоном YML-файла описания серисов:
```
version: '3.2'

services:
  postgres:
    image: flexberry/alt.p8-postgresql
    ports:
      - 5432:5432
    volumes:
      - db:/var/lib/pgsql/data/
      - /etc/localtime:/etc/localtime
          deploy:
    placement:
      constraints:
        - node.labels.role == ...
        - node.hostname == ...

    environment:
      - POSTGRES_shared_buffers=${DB_POSTGRES_shared_buffers}
      - ...

volumes:
  db:
```


## Запуск образа в виде контейнера
