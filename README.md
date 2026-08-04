# MediaTracker

A personal media tracking application for managing movies and TV shows with seasons and episodes.

## Tech Stack

**Backend**
- .NET 10 / ASP.NET Core
- Clean Architecture (Domain, Application, Infrastructure, WebApi)
- Entity Framework Core with PostgreSQL
- TPH inheritance for media types

**Frontend**
- React 19 + TypeScript
- Vite
- Tailwind CSS v4
- React Query

## Project Structure

```
MediaTracker/
├── backend/
│   ├── MediaTracker.Domain/
│   ├── MediaTracker.Application/
│   ├── MediaTracker.Infrastructure/
│   └── MediaTracker.WebApi/
└── frontend/
    └── src/
        ├── api/
        ├── components/
        ├── hooks/
        ├── pages/
        └── types/
```


## Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js (via nvm-windows)
- PostgreSQL or Docker
- pnpm

### Database

The easiest way to start the database is via Docker Compose:

```bash
docker compose up -d
```

> In the future, Docker Compose will also handle the backend and frontend.

Alternatively, set up a PostgreSQL instance manually and fill in the connection string below.

### Backend

1. Fill in `backend/MediaTracker.WebApi/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=mediatracker;Username=postgres;Password=yourpassword"
  },
  "AllowedOrigins": {
    "Frontend": "http://localhost:5173"
  }
}
```

2. Run migrations:
```bash
cd backend
dotnet ef database update --project MediaTracker.Infrastructure --startup-project MediaTracker.WebApi
```

3. Start the API:
```bash
dotnet run --project MediaTracker.WebApi
```

### Frontend

1. Fill in `frontend/.env`:
```bash
VITE_API_BASE_URL=https://localhost:7039
```
2. Install dependencies:
```bash
cd frontend
pnpm install
```

3. Start the dev server:
```bash
pnpm dev
```

Frontend runs on `http://localhost:5173`, backend on `https://localhost:7039`.

## API Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/media` | Get all movies and TV shows |
| DELETE | `/api/media/{id}` | Delete movie or TV show |
| GET | `/api/movies/{id}` | Get movie detail |
| POST | `/api/movies` | Add movie |
| PUT | `/api/movies/{id}` | Update movie |
| GET | `/api/tvshows/{id}` | Get TV show detail |
| POST | `/api/tvshows` | Add TV show |
| PUT | `/api/tvshows/{id}` | Update TV show |
| POST | `/api/tvshows/{tvShowId}/seasons` | Add season |
| PUT | `/api/tvshows/{tvShowId}/seasons/{seasonId}` | Update season |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}` | Delete season |
| POST | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes` | Add episode |
| PUT | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}` | Update episode |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}` | Delete episode |
| PATCH | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}/watched` | Mark watched |