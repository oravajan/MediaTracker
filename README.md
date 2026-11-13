# MediaTracker

A personal tracker for movies and TV shows with seasons and episodes. With the optional TMDB integration it also finds new episodes and sequels for you.

Demo: https://mediatracker.up.railway.app/

## Features

- **Movies and TV shows** with rating, watched status and release year. A movie keeps its own release year (from TMDB or entered by hand), the year of a TV show is taken from its earliest season.
- **Seasons and episodes** are managed per show. Season 0 is supported and shown as *Specials*.
- **Watch button** on the home page marks a movie as watched, or the next unwatched episode of a TV show.
- **TMDB search** (optional) while adding or editing: pick a result to link the item to TMDB. Linking a TV show loads all released seasons and episode titles automatically.
- **Sync with TMDB** in the TV show detail reloads seasons and episodes from TMDB. It replaces the existing ones, so watched progress is reset (the app asks for confirmation first).
- **Sequel linking:** when a TMDB-linked movie is marked as watched, the next part of its TMDB collection is added to your list and linked as the following movie, as long as it has already been released.
- **Check for updates** button on the home page runs in the background and looks for news:
  - watched movies without a next movie get their sequel added,
  - TV shows linked to TMDB get missing seasons and episodes added,
  - existing seasons and episodes are never modified (an episode you deleted by hand will come back),
  - only released content is added.

## Tech Stack

**Backend**

- .NET 10 / ASP.NET Core
- Clean Architecture (Domain, Application, Infrastructure, WebAPI)
- Entity Framework Core with PostgreSQL
- TPH inheritance for media types
- xUnit, Moq, FluentAssertions

**Frontend**

- React 19 + TypeScript
- Vite
- Tailwind CSS v4
- React Query

**Infrastructure**

- Docker Compose (PostgreSQL, backend, frontend served by nginx)
- GitHub Actions CI
- Deployed on Railway

## Project Structure

```
MediaTracker/
├── .github/workflows/                CI: backend tests, frontend build
├── backend/
│   ├── MediaTracker.Domain/          entities and business rules
│   ├── MediaTracker.Application/     services, DTOs, mappers, interfaces
│   ├── MediaTracker.Infrastructure/  EF Core, repositories, TMDB client
│   ├── MediaTracker.WebAPI/          controllers, background update runner
│   └── MediaTracker.Tests/           unit and integration tests
├── frontend/
│   └── src/
│       ├── api/
│       ├── components/
│       ├── hooks/
│       ├── pages/
│       └── types/
├── docker-compose.yml
└── .env.example
```

## Getting Started

The project is fully dockerized to provide a seamless developer experience.

### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop) (with Docker Compose)
- Git

### Installation & Running

1. **Clone the repository:**

```bash
git clone https://github.com/oravajan/MediaTracker.git
cd MediaTracker
```

2. **Environment Setup:**
   Copy the provided environment template and fill in your database credentials:

```bash
cp .env.example .env
```

| Variable            | Description                                            |
|---------------------|--------------------------------------------------------|
| `DB_USER`           | PostgreSQL user                                        |
| `DB_PASSWORD`       | PostgreSQL password                                    |
| `DB_NAME`           | PostgreSQL database name                               |
| `TMDB_ACCESS_TOKEN` | TMDB API Read Access Token (optional, see below)       |

3. **Start the application:**

```bash
docker compose up -d --build
```

### TMDB Integration (optional)

MediaTracker can use [The Movie Database (TMDB)](https://www.themoviedb.org) for search, seasons and episodes, sequels and release years.

1. Register for a free account at [themoviedb.org](https://www.themoviedb.org/signup)
2. Go to Settings → API and copy your **API Read Access Token**
3. Add it to your `.env` file:

```
TMDB_ACCESS_TOKEN=your_token_here
```

Without a TMDB token the application still works, but search, sync, sequel linking and the update button are unavailable. Titles are requested from TMDB in Czech (`cs-CZ`).

### What happens in the background?

Once started, Docker automatically orchestrates the entire environment:

- **PostgreSQL:** Initializes the database.
- **Backend API:** Builds the .NET application, applies Entity Framework migrations on startup and listens on port `8080`.
- **Frontend:** Builds the React application and serves the static files with nginx on port `80`. The browser calls the API directly on port `8080`, and the backend allows the frontend origin through CORS.

### Accessing the App

- **Frontend:** [http://localhost](http://localhost)
- **Backend API:** [http://localhost:8080](http://localhost:8080)

To stop the application, simply run

```bash
docker compose down
```

## Running Tests

```bash
cd backend
dotnet test
```

This needs the .NET 10 SDK. It runs unit tests for the domain and services (with mocked repositories) and integration tests that start the whole API in memory with an EF Core in-memory database, so no PostgreSQL is required. The same tests run in GitHub Actions on every push and pull request, together with a frontend build.

## API Overview

Errors are returned as `{ "error": "message" }`: `400` for validation failures and `404` when something is not found. Swagger UI is available only when `ASPNETCORE_ENVIRONMENT=Development`.

### Media

| Method | Endpoint                    | Description                                                                                                          |
|--------|-----------------------------|----------------------------------------------------------------------------------------------------------------------|
| GET    | `/api/media`                | Get all movies and TV shows                                                                                          |
| PATCH  | `/api/media/{id}/watch`     | Mark a movie as watched (and link its sequel if TMDB-linked), or mark the next unwatched episode of a TV show        |
| DELETE | `/api/media/{id}`           | Delete movie or TV show                                                                                              |
| POST   | `/api/media/update`         | Start the background update (`202`, `400` if TMDB is not configured, `409` if an update is already running)         |
| GET    | `/api/media/update/status`  | `true` while the update is running                                                                                   |

### Movies

| Method | Endpoint             | Description        |
|--------|----------------------|--------------------|
| GET    | `/api/movies/{id}`   | Get movie detail   |
| POST   | `/api/movies`        | Add movie          |
| PUT    | `/api/movies/{id}`   | Update movie       |

### TV shows, seasons and episodes

| Method | Endpoint                                                                  | Description                                                           |
|--------|---------------------------------------------------------------------------|-----------------------------------------------------------------------|
| GET    | `/api/tvshows/{id}`                                                       | Get TV show detail with seasons and episodes                          |
| POST   | `/api/tvshows`                                                            | Add TV show (loads seasons and episodes if linked to TMDB)            |
| PUT    | `/api/tvshows/{id}`                                                       | Update TV show                                                        |
| POST   | `/api/tvshows/{id}/sync`                                                  | Replace seasons and episodes with TMDB data (resets watched progress) |
| POST   | `/api/tvshows/{tvShowId}/seasons`                                         | Add season                                                            |
| PUT    | `/api/tvshows/{tvShowId}/seasons/{seasonId}`                              | Update season                                                         |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}`                              | Delete season                                                         |
| POST   | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes`                     | Add episode                                                           |
| PUT    | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}`         | Update episode                                                        |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}`         | Delete episode                                                        |
| PATCH  | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}/watched` | Mark episode as watched                                               |

### TMDB

| Method | Endpoint                                 | Description                                              |
|--------|------------------------------------------|----------------------------------------------------------|
| GET    | `/api/tmdb/status`                       | Whether a TMDB token is configured                       |
| GET    | `/api/tmdb/movies/search?query=`         | Search movies                                            |
| GET    | `/api/tmdb/tvshows/search?query=`        | Search TV shows                                          |
| GET    | `/api/tmdb/tvshows/{tmdbId}/seasons`     | Released seasons of a TV show with episode titles        |
