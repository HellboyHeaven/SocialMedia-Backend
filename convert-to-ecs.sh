#!/bin/bash

# Скрипт для конвертации docker-compose.yml в ECS-совместимый формат

echo "Конвертирую docker-compose.yml для ECS..."

# Создаем временный файл
cp docker-compose.yml docker-compose.ecs.yml

# 1. Меняем версию на поддерживаемую ECS
sed -i 's/^# Определяем сети$/version: "3"/' docker-compose.ecs.yml

# 2. Удаляем секции, которые ECS не поддерживает
sed -i '/^networks:$/,/^$/d' docker-compose.ecs.yml
sed -i '/^secrets:$/,/^$/d' docker-compose.ecs.yml
sed -i '/^volumes:$/,/^$/d' docker-compose.ecs.yml

# 3. Удаляем YAML anchors (x-* секции)
sed -i '/^x-.*:$/,/^$/d' docker-compose.ecs.yml

# 4. Упрощаем environment секции - разворачиваем anchors вручную
cat > docker-compose.ecs.yml << 'EOF'
version: "3"

services:
  gateway-service:
    image: gateway:latest
    ports:
      - "5000:8080"
      - "5001:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development

  auth-service:
    image: auth:latest
    ports:
      - "5100:8080"
      - "5101:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=auth-db;Port=5432;Database=db;Username=postgres;Password=postgres
      - Kafka__BootstrapServers=kafka:9092
      - Kafka__GroupId=auth-service-group
      - Kafka__AllowAutoCreateTopics=true
      - Kafka__ClientId=message-bus-client
    depends_on:
      - kafka
      - auth-db

  post-service:
    image: post:latest
    ports:
      - "5200:8080"
      - "5201:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=post-db;Port=5432;Database=db;Username=postgres;Password=postgres
      - Kafka__BootstrapServers=kafka:9092
      - Kafka__GroupId=post-service-group
      - AWS__AccessKey=REMOVED_SECRET
      - AWS__SecretKey=REMOVED_SECRET
      - AWS__Region=eu-north-1
      - AWS__BucketName=social-media.media
    depends_on:
      - kafka
      - post-db

  profile-service:
    image: profile:latest
    ports:
      - "5300:8080"
      - "5301:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=profile-db;Port=5432;Database=db;Username=postgres;Password=postgres
      - Kafka__BootstrapServers=kafka:9092
      - Kafka__GroupId=profile-service-group
    depends_on:
      - kafka
      - profile-db

  comment-service:
    image: comment:latest
    ports:
      - "5400:8080"
      - "5401:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=comment-db;Port=5432;Database=db;Username=postgres;Password=postgres
      - Kafka__BootstrapServers=kafka:9092
      - Kafka__GroupId=comment-service-group
      - AWS__AccessKey=REMOVED_SECRET
      - AWS__SecretKey=REMOVED_SECRET
      - AWS__Region=eu-north-1
    depends_on:
      - kafka
      - post-db

  like-service:
    image: like:latest
    ports:
      - "5500:8080"
      - "5501:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Database=Host=like-db;Port=5432;Database=db;Username=postgres;Password=postgres
      - Kafka__BootstrapServers=kafka:9092
      - Kafka__GroupId=like-service-group
    depends_on:
      - kafka
      - like-db

  # Базы данных
  auth-db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=db
    ports:
      - "5433:5432"

  post-db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=db
    ports:
      - "5434:5432"

  profile-db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=db
    ports:
      - "5435:5432"

  comment-db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=db
    ports:
      - "5436:5432"

  like-db:
    image: postgres:15
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=db
    ports:
      - "5437:5432"

  # Инфраструктура
  zookeeper:
    image: confluentinc/cp-zookeeper:7.5.0
    ports:
      - "2181:2181"
    environment:
      - ZOOKEEPER_CLIENT_PORT=2181
      - ZOOKEEPER_TICK_TIME=2000

  kafka:
    image: confluentinc/cp-kafka:7.5.0
    ports:
      - "9092:9092"
    environment:
      - KAFKA_BROKER_ID=1
      - KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181
      - KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=PLAINTEXT:PLAINTEXT
      - KAFKA_LISTENERS=PLAINTEXT://0.0.0.0:9092
      - KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://kafka:9092
      - KAFKA_INTER_BROKER_LISTENER_NAME=PLAINTEXT
      - KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1
      - KAFKA_AUTO_CREATE_TOPICS_ENABLE=true
    depends_on:
      - zookeeper

  adminer:
    image: adminer
    ports:
      - "4000:8080"
EOF

echo "Файл docker-compose.ecs.yml создан!"
echo ""
echo "Теперь используйте команду:"
echo "ecs-cli compose --file docker-compose.ecs.yml --project-name myproject service up"
echo ""
echo "После деплоя можете удалить временный файл:"
echo "rm docker-compose.ecs.yml"
