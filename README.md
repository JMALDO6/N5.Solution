# 🧠 Arquitectura Modular con CQRS, Kafka, Elasticsearch y Docker

Este repositorio documenta la implementación de una arquitectura modular en .NET, utilizando patrones CQRS, mensajería con Kafka, persistencia dual con Elasticsearch y SQL Server, y contenedores Docker para despliegue. A continuación se presentan pantallazos clave del sistema en funcionamiento.

---

## 📦 Estructura del Proyecto

- `WebAPI`: Exposición de endpoints REST con Swagger
- `Application`: Lógica de negocio y CQRS con MediatR
- `Infrastructure`: Persistencia con EF Core y Elasticsearch
- `Messaging`: Productores y consumidores Kafka
- `Docker`: Archivos de configuración para contenedores

---

## 🔍 Swagger - Exploración de Endpoints

<img width="1458" height="703" alt="image" src="https://github.com/user-attachments/assets/fbc33189-0447-44f5-9b90-2586590b6287" />


> Visualización de endpoints RESTful, con operaciones GET, POST y PATCH para permisos.

---

## 🗃️ SQL Server - Persistencia Relacional

<img width="480" height="402" alt="image" src="https://github.com/user-attachments/assets/47d44b79-1f04-4924-8dfb-ae695df32158" />

> Consulta directa a la base de datos relacional, mostrando inserciones y actualizaciones desde comandos CQRS.

---

## 📡 Kafka - Mensajería Asíncrona

<img width="1881" height="700" alt="image" src="https://github.com/user-attachments/assets/157120bd-d9b0-4735-87ca-074c43e92575" />

> Visualización de tópico creado, eventos publicados desde comandos y suscripciones desde handlers.

---

## 📈 Elasticsearch - Indexación y Búsqueda

<img width="550" height="742" alt="image" src="https://github.com/user-attachments/assets/cf68f4d1-f676-4d17-b138-e0c5bc21ada4" />

> Indexación de entidades para búsquedas rápidas. Se muestra la estructura del documento y resultados de queries.

---

## 🐳 Docker - Contenedores en Ejecución

<img width="1083" height="254" alt="image" src="https://github.com/user-attachments/assets/ddffcb54-9d30-4c37-997e-bc1dd8227e54" />

> Contenedores activos para WebAPI, Kafka, Kafka UI, Zookeeper y Elasticsearch. Configuración vía `docker-compose.yml`.

---

## 🧪 Pruebas de Integración

- Validación de eventos de dominio
- Persistencia dual (SQL + Elasticsearch)
- Flujo completo desde comando → evento → consumidor

  <img width="413" height="148" alt="image" src="https://github.com/user-attachments/assets/00b5fab5-8db4-4521-a797-ed009b29e78c" />

---

## 🧠 Autor

**Julian Andrés Maldonado**  
Desarrolador .NET

---
