# 🎵 Music Library / Media Manager

[![.NET](https://img.shields.io/badge/.NET-8-blue)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-latest-red)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-blue)](https://www.microsoft.com/en-us/sql-server)
[![Docker](https://img.shields.io/badge/Docker-ready-blue)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/Project%20Status-In%20Development-orange)]()

---

## 📖 Project Overview
The **Music Library / Media Manager** is a modern web application designed to help users **organize, manage, and explore their music collection** efficiently. 

The project currently includes a fully structured **.NET 8 backend** following a **clean and scalable architecture** (Domain, Application, Infrastructure, Api), and a Dockerized SQL Server environment for persistent storage.  
The Angular frontend is **planned but not yet started**.

The goal is to evolve into a complete media platform for uploading, cataloging, storing, and managing audio files.

---

## ✨ Features
- **🎶 Music Library Management**: Upload, store, and organize music files with detailed metadata (title, artist, album, genre, release date, etc.).
- **🔍 Search and Filtering**: Quickly find songs, albums, or artists using advanced search and filters.
- **📂 Playlists & Favorites**: Create, edit, and manage playlists. Mark favorite tracks for easy access.
- **▶️ Audio Playback**: Stream music directly in the app via an integrated audio player.
- **🔐 Authentication**: Secure access with user registration and login.
- **🖥️ RESTful API Backend**: Built with .NET 8 Web API for reliable data handling.
- **🌐 Modern Frontend**: Angular-based responsive interface.
- **💾 Database Integration**: Microsoft SQL Server for secure storage.
- **🐳 Containerized**: Docker-ready for easy deployment.

---

## 🎯 Project Goals
1. Centralize music storage and management.  
2. Provide an efficient and enjoyable way to interact with a music collection.  
3. Support future expansion, such as integration with streaming services or AI recommendations.  
4. Ensure security, performance, and reliability through modern software practices.

---

## 🛠️ Technology Stack
| Layer | Technology |
|-------|------------|
| Backend | .NET 8 Web API |
| Architecture | Clean Architecture (Domain, Application, Infrastructure, Api) |
| Frontend | Angular (latest) |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| Storage | Local filesystem → MinIO (planned) |
| Version Control | Git & GitHub |
| Deployment | Docker |

---

## 🖼️ Screenshots

> *(Screenshots will be added once the frontend is developed)*

---

## 🏗️ Architecture Diagram

```mermaid
graph TD;
    A[Frontend: Angular (Planned)] -->|HTTP Requests| B[Backend: .NET 8 Web API]
    B -->|Business Logic| C[Application Layer]
    C -->|Domain Models| D[Domain Layer]
    C -->|Repositories| E[Infrastructure Layer]
    E -->|EF Core| F[SQL Server Database]
    E -->|File Saving| G[Local Storage]
    G -->|Future| H[MinIO]
    I[Docker] --> F
    I --> B
```

## 📡 Current Endpoints (Implemented / In Progress)

# ✔️ Implemented
GET /api/media/ping

Used to verify that the API is running correctly.

## 🏗️ In Progress

POST /api/media/upload

Saves the uploaded file physically.

Saves metadata in the database.

Uses MediaItem entity & repository.

## ⏳ Planned

GET /api/media – list all uploaded items

DELETE /api/media/{id} – remove media

GET /api/media/{id} – fetch details

## ⚡ Installation & Setup

# 1. Clone the repository
# 2. Start SQL Server via Docker
# 3. Apply EF Core migrations
# 4. Run the API

API available at:

HTTP → http://localhost:5000

HTTPS → https://localhost:7000

## 🚀 Future Improvements
- Complete media upload workflow

- Implement media listing

- Add MinIO storage support

- Add Angular frontend

- Introduce authentication (JWT)

- Add a background worker (MediaProcessor Service)

- Improve logging and validation

## 🤝 Contributing
Contributions are welcome! Please follow the standard Git workflow:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request







