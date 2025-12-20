# 🎵 Music Library / Media Manager

[![.NET](https://img.shields.io/badge/.NET-8-blue)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-latest-red)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-blue)](https://www.microsoft.com/en-us/sql-server)
[![Docker](https://img.shields.io/badge/Docker-ready-blue)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/Project%20Status-In%20Development-orange)]()

---

## 📖 Project Overview

The **Music Library / Media Manager** is a full-stack application designed to manage, store, and organize music and media files together with their metadata.

The project currently consists of:
- a **.NET 8 backend** built using **Clean Architecture**
- a **Dockerized SQL Server** database
- an **Angular frontend** integrated into the solution
- a **background worker service** for media-related processing
- **object storage support via MinIO**

The system exposes RESTful APIs to upload, list, and manage media files while persisting metadata in a relational database and storing files in object storage.

---

## ✨ Features

### ✅ Implemented
- Clean Architecture:
  - Domain
  - Application
  - Infrastructure
  - API
- .NET 8 Web API
- SQL Server running in Docker
- Entity Framework Core with migrations
- Media upload workflow
- Physical file storage
- Metadata persistence using repositories
- Media listing endpoint
- MinIO integration for object storage
- Background worker service (**MediaProcessor**)
- Angular frontend integration
- Docker Compose orchestration

### 🏗️ In Progress
- Media streaming endpoint
- Improved validation and error handling
- Centralized logging strategy

### ⏳ Planned
- Authentication and authorization (JWT)
- User-based media ownership
- Playlists management
- Favorites and tagging
- Media search and filtering
- API versioning
- Health checks & metrics
- CI/CD pipeline with GitHub Actions

---

## 🎯 Project Goals

1. Provide a scalable and maintainable media management backend.
2. Ensure clear separation of responsibilities using Clean Architecture.
3. Support asynchronous and background processing for media-related tasks.
4. Enable future expansion with authentication, playlists, and advanced media features.
5. Maintain a professional development workflow with Docker and GitFlow.

---

## 🛠️ Technology Stack

| Layer | Technology |
|------|------------|
| Backend | .NET 8 Web API |
| Architecture | Clean Architecture |
| Frontend | Angular |
| Database | Microsoft SQL Server (Docker) |
| ORM | Entity Framework Core |
| Object Storage | MinIO |
| Background Worker | .NET Worker Service |
| DevOps | Docker, Docker Compose |
| Version Control | Git & GitHub |

---

## 🖼️ Screenshots

Screenshots will be added as the frontend evolves.

---

## 📡 API Endpoints

### ✔️ Implemented

#### <kbd>POST /api/media/upload</kbd>
- Uploads a media file
- Stores the file in MinIO
- Persists metadata in the database

#### <kbd>GET /api/media</kbd>
Returns the list of uploaded media items.

---

## ⚡ Installation & Setup

#### 1. Clone the repository
```
git clone https://github.com/Mbazie-Kone/MusicLibrary.git

```
#### 2. Start application via Docker
```
docker compose up -d

```
#### 3. Apply EF Core migrations
```
dotnet ef database update -p MusicLibrary.Infrastructure -s MusicLibrary.Api

```
#### 4. Run the API
```
cd MusicLibrary.Api
dotnet run

```

---

## Git workflow

- `main` is protected and represents production-ready code
- `develop` is the integration branch
- Feature branches must be created from `develop`
- Merges into `develop` should use Rebase or Squash
- Merges into `main` should use Rebase and Merge
- `node_modules` must never be committed
- Conflicts must be resolved locally, not via GitHub UI

---

## 🤝 Contributing

This project follows GitFlow.

Pull requests are welcome and should target the `develop` branch.

---

## 📄 License

This project is licensed under the MIT License.
