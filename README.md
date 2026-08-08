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
├── frontend/
│   └── src/
│       ├── api/
│       ├── components/
│       ├── hooks/
│       ├── pages/
│       └── types/
└── .env
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

3. **Start the application:**

```bash
docker compose up -d
```

### What happens in the background?

Once started, Docker automatically orchestrates the entire environment:

- **PostgreSQL:** Initializes the database.
- **Backend API:** Builds the .NET application, automatically applies Entity Framework migrations, and starts the server
  on port `8080`.
- **Frontend:** Builds the React application and serves it via a Nginx reverse proxy on port `80` (seamlessly handling
  routing and CORS).

### Accessing the App

- **Frontend:** [http://localhost](http://localhost)
- **Backend API:** [http://localhost:8080](http://localhost:8080)

To stop the application, simply run

```bash
docker compose down
```

## API Overview

| Method | Endpoint                                                                  | Description                               |
|--------|---------------------------------------------------------------------------|-------------------------------------------|
| GET    | `/api/media`                                                              | Get all movies and TV shows               |
| PATCH  | `/api/media/{id}/watch`                                                   | Mark any media (movie/episode) as watched |
| DELETE | `/api/media/{id}`                                                         | Delete movie or TV show                   |
| GET    | `/api/movies/{id}`                                                        | Get movie detail                          |
| POST   | `/api/movies`                                                             | Add movie                                 |
| PUT    | `/api/movies/{id}`                                                        | Update movie                              |
| PATCH  | `/api/movies/{id}/watched`                                                | Mark movie as watched                     |
| GET    | `/api/tvshows/{id}`                                                       | Get TV show detail                        |
| POST   | `/api/tvshows`                                                            | Add TV show                               |
| PUT    | `/api/tvshows/{id}`                                                       | Update TV show                            |
| POST   | `/api/tvshows/{tvShowId}/seasons`                                         | Add season                                |
| PUT    | `/api/tvshows/{tvShowId}/seasons/{seasonId}`                              | Update season                             |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}`                              | Delete season                             |
| POST   | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes`                     | Add episode                               |
| PUT    | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}`         | Update episode                            |
| DELETE | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}`         | Delete episode                            |
| PATCH  | `/api/tvshows/{tvShowId}/seasons/{seasonId}/episodes/{episodeId}/watched` | Mark episode as watched                   |