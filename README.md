# Simple Task Manager Project

An example full‑stack application showcasing a .NET 8 Minimal API with a React (Vite + TypeScript) frontend. It demonstrates:
- Minimal API endpoints for task management
- EF Core (SQLite) for persistence
- SignalR for real‑time updates (broadcast new task events)
- Optional OpenAI integration to summarize tasks
- A lightweight React UI consuming the API and SignalR hub


## Tech stack
- Backend: .NET 8, Minimal API, EF Core (SQLite), FluentValidation, Mapster, DispatchR mediator, SignalR, Swagger
- Frontend: React 19, Vite 7, TypeScript, Redux Toolkit, React‑Redux, SignalR client
- Tests: NUnit, FluentAssertions, NSubstitute, WebApplicationFactory, EFCore InMemory


## Project layout
```
SimpleTaskManagerProject.sln
├─ SimpleTaskManagerProject/                 # .NET 8 API + SignalR + EF Core
│  ├─ Api/                                   # Minimal API route registrations
│  ├─ Hubs/                                  # SignalR hubs
│  ├─ Infrastructure/                        # DbContext, Handlers, Requests, Services
│  ├─ Models/                                # EF entities
│  ├─ Migrations/                            # EF Core migrations
│  ├─ Properties/launchSettings.json         # Launch profile & env vars
│  ├─ appsettings.json                       # Connection string (SQLite: app.db)
│  └─ ui/                                    # React + Vite frontend
└─ SimpleTaskManagerProject.Tests/           # Unit & API tests
```


## Backend (API)

### Prerequisites
- .NET 8 SDK
- SQLite (no manual setup needed; EF will create `app.db`)
- Optional: OpenAI API key for the summary endpoint

### Configure OpenAI for the summary endpoint
The summary endpoint (`GET /tasks/summary`) calls OpenAI via `OpenAI.Chat.ChatClient`. To enable it you must provide your OpenAI API key.

Preferred options:
1) Fill it in `SimpleTaskManagerProject/Properties/launchSettings.json`:
```
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "OPENAI_API_KEY": "your_api_key_here"
}
```
2) Or set an environment variable before running the app:
- macOS/Linux: `export OPENAI_API_KEY=your_api_key_here`
- Windows (Powershell): `$env:OPENAI_API_KEY = "your_api_key_here"`

If the key is not set, the summary endpoint will not work as intended.

### Database and migrations (SQLite)
Connection string is configured in `appsettings.json` as `Data Source=app.db`.

Before running the API for the first time, execute all migrations so the `app.db` file is created:
- Install EF Core CLI (if you don’t have it):
  - `dotnet tool install --global dotnet-ef`
  - or update: `dotnet tool update --global dotnet-ef`
- From the solution root:
  - macOS/Linux/Windows:
    - `cd SimpleTaskManagerProject`
    - `dotnet ef database update`

This will create or update `app.db` in the `SimpleTaskManagerProject` directory.

### Run the API
From the solution root:
- `cd SimpleTaskManagerProject`
- `dotnet run`

The API will start (by default) at `http://localhost:5218`.
- Swagger UI: `http://localhost:5218/swagger`
- SignalR hub: `/tasks-hub`

### API endpoints
- `POST /tasks` — Create a task (validates input)
- `GET /tasks` — Get all tasks
- `GET /tasks/summary` — Return a summary (requires `OPENAI_API_KEY`)

When a task is created, the server broadcasts a `taskCreated` message via SignalR to all connected clients.

### Tests
From the solution root:
- `dotnet test`


## Frontend (React + Vite)
Located under `SimpleTaskManagerProject/ui`.

### Prerequisites
- Node.js 18+
- npm 9+

### Install dependencies
From the `ui` folder:
- `npm install`

### NPM scripts (run inside `SimpleTaskManagerProject/ui`)
- `npm run dev` — Start dev server
- `npm run build` — Type‑check + production build
- `npm run preview` — Preview the production build
- `npm run lint` — Run ESLint (flat config)
- `npm run lint:fix` — ESLint with auto‑fix
- `npm run format` — Prettier write
- `npm run format:check` — Prettier check only

### Development flow
1) Start the API:
   - `cd SimpleTaskManagerProject`
   - `dotnet run`
2) Start the UI in another terminal:
   - `cd SimpleTaskManagerProject/ui`
   - `npm run dev`
3) Open the UI in the browser (Vite will print the URL). The app will:
   - Load tasks from the API
   - Receive real‑time updates through SignalR (`taskCreated` events)
   - Allow creating new tasks
   - Request a summary of tasks (requires `OPENAI_API_KEY` configured on the API)


## Notes
- CORS is configured to allow any origin/headers/methods for simplicity.
- The codebase includes example unit tests for API routes and handlers. All external dependencies are mocked where appropriate.
- Current date/time for this document: 2025-10-30 18:32.
