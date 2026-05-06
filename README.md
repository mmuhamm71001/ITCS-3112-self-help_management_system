# StudentPlanner – Self-Help Management System

## 1. Project Overview

StudentPlanner is a console application built in C# that helps students manage assignments, personal goals, and daily habits in one place. It is designed for any student who wants a lightweight, file-based planner that does not require a database or internet connection.

### Core Features

- **User accounts** – register and log in; passwords are hashed with SHA-256 before being saved to a file
- **Task management** – add, view, update status, and delete tasks
- **Two task types** – academic Assignments (with course name) and Personal Goals (Health, Career, Finance, Learning, or custom), with optional recurring schedules
- **Action plan generator** – automatically builds a day-by-day or phased plan when a task is added, based on days remaining and the user's stated challenge
- **Daily check-in** – new users answer a quick mood check (Great / Okay / Stressed) with optional notes
- **Profile view** – shows account info and a summary of task statuses

### Assumptions and Constraints

- All data is saved to plain text files (`users.txt` and `tasks.txt`); no external database is used
- Only one user is active per session
- Action plans are generated fresh each session and are not persisted between runs
- Passwords are never stored as plain text

---

## 2. Build & Run Instructions

### Tools and Versions

| Tool | Version Used |
|---|---|
| Language | C# 12 |
| Runtime | .NET 8 (also tested on .NET 10) |
| IDE | Visual Studio 2022 or VS Code with C# Dev Kit |
| Build Tool | `dotnet` CLI (included with .NET SDK) |
| OS | Windows, macOS, or Linux |

### Running from the Command Line

```bash
# 1. Clone the repository
git clone https://github.com/mmuhamm71001/ITCS-3112-self-help_management_system.git
cd ITCS-3112-self-help_management_system

# 2. Build the project
dotnet build StudentPlanner.csproj

# 3. Run the project
dotnet run --project StudentPlanner.csproj
```

### Running in Visual Studio

1. Open `ITCS-3112-self-help_management_system.sln`
2. Make sure `StudentPlanner` is set as the startup project
3. Press `F5` to run with the debugger, or `Ctrl+F5` to run without

### First-Time Setup

- The app will prompt you to log in or create an account — choose **2** to register first
- After registering, a short daily check-in prompt appears before the main menu
- `users.txt` and `tasks.txt` are created automatically on first run; no manual configuration is needed
- No command-line arguments are required

---

## 3. Required OOP Features

| OOP Feature | File(s) | Line Numbers | Reasoning / Purpose |
|---|---|---|---|
| **Inheritance #1** – `Person` (abstract base) | `src/Domain/Models/Person.cs` | 3–18 | Defines shared `name` and `email` fields and the abstract `DisplayProfile()` method that every person type must implement. Keeps identity logic in one place. |
| **Inheritance #2** – `User` extends `Person` | `src/Domain/Models/User.cs` | 8–83 | Inherits `name` and `email` from `Person`, then adds password hashing, goal storage, and `IAuthenticatable`. The `base(name, email)` call on line 14 delegates field initialization upward. |
| **Inheritance #3** – `Task` (abstract base) | `src/Domain/Models/Task.cs` | 5–33 | Defines shared `title`, `dueDate`, and `status` fields and declares `GetSummary()` and `Serialize()` as abstract so each subclass must provide its own implementation. |
| **Inheritance #4** – `Assignment` extends `Task` | `src/Domain/Models/Assignment.cs` | 5–27 | Reuses all `Task` fields and adds `courseName`. Overrides `GetSummary()` (line 15) and `Serialize()` (line 20) with assignment-specific formatting. |
| **Inheritance #5** – `PersonalGoal` extends `Task` | `src/Domain/Models/PersonalGoal.cs` | 5–45 | Reuses `Task` and adds `category`, `progressPct`, and recurrence fields. Overrides `GetSummary()` (line 27) and `Serialize()` (line 35) with goal-specific formatting. |
| **Interface #1** – `IActionPlan` / `ActionPlan` | `src/Domain/Interfaces/IActionPlan.cs` (5–14), `src/Domain/Models/ActionPlan.cs` (6) | 5–14 / 6 | Defines the contract for any action-planning component. `Dashboard` depends only on `IActionPlan`, so the concrete `ActionPlan` can be swapped without touching the UI layer. |
| **Interface #2** – `IAuthenticatable` / `User` | `src/Domain/Interfaces/IAuthenticatable.cs` (3–6), `src/Domain/Models/User.cs` (8, 48–51) | 3–6 / 48–51 | Requires any user type to expose an `Authenticate(password)` method. Isolates authentication logic from identity data, making it easy to add alternative account types. |
| **Interface #3** – `IDailyCheckin` / `DailyCheckin` | `src/Domain/Interfaces/IDailyCheckin.cs` (3–7), `src/Domain/Models/DailyCheckin.cs` (5) | 3–7 / 5 | `Program.cs` interacts only through `IDailyCheckin` (line 42), so the check-in implementation is fully replaceable without changing startup code. |
| **Interface #4** – `ITaskRepository` / `TaskRepository` | `src/Domain/Interfaces/ITaskRepository.cs` (5–9), `src/Repositories/TaskRepository.cs` (8) | 5–9 / 8 | Decouples task persistence from the rest of the app. `Dashboard` receives `ITaskRepository` through its constructor, so the backing store (flat file, database, etc.) can change independently. |
| **Interface #5** – `IUserRepository` / `UserRepository` | `src/Domain/Interfaces/IUserRepository.cs` (5–9), `src/Repositories/UserRepository.cs` (8) | 5–9 / 8 | Same separation-of-concerns principle applied to user persistence; `Program.cs` only uses the interface (line 23), not the concrete class. |
| **Polymorphism #1** – `GetSummary()` override | `src/Domain/Models/Task.cs` (17), `src/Domain/Models/Assignment.cs` (15–18), `src/Domain/Models/PersonalGoal.cs` (27–33) | 17 / 15–18 / 27–33 | `GetSummary()` is declared `abstract` in `Task`. When `Dashboard` loops over a `List<Task>` and calls `GetSummary()`, the runtime dispatches to the correct override — C# virtual dispatch in action. |
| **Polymorphism #2** – `Serialize()` override | `src/Domain/Models/Task.cs` (18), `src/Domain/Models/Assignment.cs` (20–23), `src/Domain/Models/PersonalGoal.cs` (35–38) | 18 / 20–23 / 35–38 | `TaskRepository.SaveTasks()` calls `task.Serialize(email)` on every item in the list without knowing the concrete type. Each subclass automatically writes its own pipe-delimited format. |
| **Polymorphism #3** – `DisplayProfile()` override | `src/Domain/Models/Person.cs` (14), `src/Domain/Models/User.cs` (28–46) | 14 / 28–46 | `DisplayProfile()` is abstract in `Person` and fully implemented in `User`. Any future person type (e.g., Advisor) would provide its own display without modifying the caller. |
| **Access Modifiers** | `src/Domain/Models/Person.cs` (5–6), `src/Domain/Models/User.cs` (10–11, 74), `src/Domain/Models/Task.cs` (7–9) | 5–6 / 10–11, 74 / 7–9 | Fields are `private` in `User` and `Task` so external classes cannot mutate state directly. Shared base-class fields (`name`, `email`, `title`, etc.) are `protected` so subclasses can read them without exposing them publicly. `HashPassword()` is `private static` because it is an internal implementation detail that should never be callable from outside. |
| **Struct** – `TaskStats` | `src/Domain/Models/TaskStats.cs` | 8–30 | A small value type that aggregates task counts by status. Using a struct instead of a class is appropriate here because the data is short-lived, stack-allocated, and copied cheaply — no heap allocation or reference semantics needed. |
| **Enum #1** – `TaskStatus` | `src/Domain/Enums/TaskStatus.cs` | 3 | Defines the three valid task states (`NotStarted`, `InProgress`, `Complete`). Using an enum prevents invalid string comparisons and makes switch statements exhaustive. |
| **Enum #2** – `MoodStatus` | `src/Domain/Enums/MoodStatus.cs` | 3–8 | Represents the three daily check-in moods. Passed directly into `RecordMood()` instead of a raw string, so the compiler catches any typo or out-of-range value at build time. |
| **Data Structure** – `List<Task>` | `src/Domain/Models/ActionPlan.cs` (10), `src/Presentation/Dashboard.cs` (11), `src/Repositories/TaskRepository.cs` (36) | 10 / 11 / 36 | `List<Task>` is used throughout because tasks need dynamic insertion, ordered iteration, and index-based access. The generic type parameter enforces that only `Task` objects (and subclasses) can be stored. |
| **I/O – Console** | `src/Presentation/Program.cs` (17–21), `src/Presentation/Dashboard.cs` | 17–21 | All user interaction uses `Console.ReadLine()` for input and `Console.WriteLine()` for output, demonstrating standard library console I/O throughout the presentation layer. |
| **I/O – File** | `src/Repositories/TaskRepository.cs` (19, 31), `src/Repositories/UserRepository.cs` (43, 49) | 19, 31 / 43, 49 | `File.ReadAllLines()` and `File.WriteAllLines()` from `System.IO` handle reading and writing `users.txt` and `tasks.txt`, replacing any database dependency with plain text persistence. |

---

## 4. Design Patterns

| Pattern Name | Category | File Name | Line Numbers | Rationale |
|---|---|---|---|---|
| Factory Method | Creational | `src/Factories/TaskFactory.cs` | 14–28 | The `Create` method centralizes object construction so `Dashboard` never calls `Assignment` or `PersonalGoal` constructors directly. Adding a new task type only requires a new branch in one place, leaving all callers unchanged. |
| Template Method | Behavioral | `src/Domain/Models/ActionPlan.cs` | 85–120 | `DisplayWrittenPlan()` defines the fixed skeleton — compute days left, print headers, delegate to a phase helper, print footer — while `WriteDayByDayPlan` (197–213), `WriteTwoPhasePlan` (215–227), and `WriteThreePhasePlan` (229–246) fill in the varying steps. The overall algorithm structure never changes even as the output differs per timeline. |
| Strategy | Behavioral | `src/Domain/Interfaces/IActionPlan.cs` (1–15), `src/Presentation/Dashboard.cs` (12–16) | 1–15 / 12–16 | `Dashboard` holds an `IActionPlan` field and receives the concrete implementation through its constructor. This decouples the UI from any specific planning algorithm — a different `IActionPlan` implementation can be injected without touching `Dashboard` code at all. |

---

## 5. Design Decisions

### How the Components Fit Together

The project is split into two namespaces:

- `StudentPlanner.Domain` — all logic and data classes (models, interfaces, enums, struct)
- `StudentPlanner.Presentation` — the `Dashboard` UI class
- `StudentPlanner.Repositories` — file-backed implementations of `ITaskRepository` and `IUserRepository`

`Program.cs` acts as the composition root: it instantiates the concrete repository and plan objects and injects them into `Dashboard` through its constructor. `Dashboard` never depends on concrete types — only on `IActionPlan` and `ITaskRepository`.

### Key Abstractions and Tradeoffs

**`Task` as the central abstraction.** Everything in the system works with `Task` references. The two subclasses (`Assignment`, `PersonalGoal`) handle their own formatting and serialization, so the rest of the app never needs to branch on type. Adding a third task type means creating a new subclass and updating `TaskFactory` — nothing else changes.

**Interface injection over direct instantiation.** `Dashboard` receives `IActionPlan` and `ITaskRepository` rather than creating them itself. `Program.cs` receives `IUserRepository` the same way. This makes each component testable in isolation and keeps the startup logic in one place.

**`Person`/`User` split.** For the current scope this split may seem like over-engineering, but it provides a clear extension point if the system ever needs to distinguish advisors, admins, or other roles without duplicating identity fields.

**Flat-file persistence.** Plain text files were chosen to keep the project self-contained and avoid a database dependency. Each domain class handles its own serialization format, so the repository classes stay thin. The main tradeoff is that concurrent writes are unsafe, and action plans cannot be persisted between sessions.

### What We Would Do Differently

- **Persist action plans.** Currently a plan is regenerated each session. Storing the generated plan text alongside each task would let users review their plans after closing the app.
- **Abstract console I/O.** The presentation layer currently calls `Console` directly, which makes automated testing difficult. Wrapping I/O in an `IConsole` interface would let tests inject a fake terminal.
- **Async file access.** `File.ReadAllLines` blocks the thread. For a larger dataset, switching to async file APIs would keep the UI responsive.
