# 🧱 Solución Modular con SQL Server, Elasticsearch y Kafka

Esta solución implementa una arquitectura limpia basada en **CQRS**, con **persistencia dual** y **mensajería desacoplada** usando **Kafka**. Está diseñada para ser eficiente, escalable y fácil de mantener.

---

## ⚙️ Tecnologías

- **.NET 8** – Plataforma base
- **SQL Server** – Fuente de verdad (lecturas y escrituras)
- **Elasticsearch** – Motor de búsqueda rápido (solo lectura)
- **Apache Kafka** – Bus de eventos para desacoplar procesos
- **EF Core** – Acceso a SQL Server
- **Nest** – Cliente para Elasticsearch
- **MediatR** – Mediador para comandos y queries

---
