# React + .NET Todo App

Simple todo list with a React (Vite) frontend consuming a minimal .NET 8 backend API.

## Features

- Browse, add, toggle, and remove todo items.
- Minimal API backend with in-memory storage and Swagger documentation.
- React frontend with live fetching + optimistic updates.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- Optional: [npm](https://www.npmjs.com/) (comes with Node.js) or [pnpm](https://pnpm.io/) / [Yarn](https://yarnpkg.com/) to run the frontend.

## Running the backend

```bash
cd backend
dotnet run
```

The API listens on `http://localhost:5154` with CORS configured to allow the React dev server. Swagger UI is available at `http://localhost:5154/swagger` while the backend is running.

## Running the frontend

```bash
cd frontend
npm install
npm run dev -- --host 0.0.0.0 --port 5173
```

The frontend defaults to `http://localhost:5173`. It calls the backend via `VITE_API_URL`, which is defined in `frontend/src/App.jsx` and defaults to `http://localhost:5154/api/todos`. Override it by creating a `.env` file in `frontend/` with `VITE_API_URL=https://your-api-host/api/todos`.

## Project structure

- `backend/` — .NET 8 Web API (minimal API, in-memory store, Swagger).
- `frontend/` — Vite + React SPA consuming the API.
- `README.md` — This file.

Enjoy! Let me know if you want deployment or tests next.
