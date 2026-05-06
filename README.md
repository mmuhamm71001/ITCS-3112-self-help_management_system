# StudentPlanner – Self-Help Management System

## 1. Project Overview

This project is a console app built in C# that helps students manage their assignments, personal goals, and daily habits all in one place. We built it as a self-help management system where users can log in, add tasks, track their progress, and get a personalized action plan generated for them based on what they're working on.

The app supports two main task types: academic Assignments and Personal Goals (things like fitness, career, finance, learning, or anything custom). When you add a task, it walks you through a couple of quick questions and then generates a written step-by-step plan based on how many days you have left and what your biggest challenge is.

### Features

* User accounts: register and log in; passwords are hashed with SHA-256 before being saved to a file
* Task management: add, view, update status, and delete tasks
* Personal Goals: can be set as recurring (daily, weekly, or monthly) with an optional target end date
* Action plan generator: builds a day-by-day or phased plan automatically when you add a task
* Daily check-in: new users answer a quick mood check (Great / Okay / Stressed) with optional notes
* Profile view: shows your info and a summary of task statuses

### Assumptions / Constraints

* Everything is saved to plain text files (users.txt and tasks.txt); no database needed
* Only one user is logged in at a time
* The action plan is generated fresh each session and is not saved between runs
* Passwords are never stored as plain text

## 2. Build & Run Instructions

### What You Need

* .NET SDK 6.0 or later (We used .NET 8)
* Visual Studio 2022, VS Code with C# extension, or JetBrains Rider
* Windows, macOS, or Linux

### Running from the Command Line

```bash
cd ITCS-3112-self-help_management_system

dotnet build StudentPlanner.csproj

dotnet run --project StudentPlanner.csproj
```

### Running in Visual Studio

1. Open `ITCS-3112-self-help_management_system.sln`
2. Make sure `StudentPlanner` is set as the startup project
3. Press `F5` to run or `Ctrl+F5` to run without the debugger

### First Time Running

* The app will ask if you want to log in or create an account. Pick 2 to register first
* After registering, you will get a short daily check-in prompt before the main menu loads
* `users.txt` and `tasks.txt` are created automatically the first time you run the app

## 3. Required OOP Features

### Inheritance #1 – Person → User

**Files:** `Person.cs`, `User.cs`

We made `Person` an abstract base class that holds `name` and `email` since those are shared by anyone in the system. `User` extends it and adds password hashing and a goal list. This avoids duplicating fields and lets us treat any person type the same way where needed.

### Inheritance #2 – Task → Assignment

**Files:** `Task.cs`, `Assignment.cs`

`Task` holds the common fields like `title`, `dueDate`, and `status`. `Assignment` inherits all of that and adds `courseName`, then overrides `GetSummary()` and `Serialize()` to format things correctly for assignments.

### Inheritance #3 – Task → PersonalGoal

**Files:** `Task.cs`, `PersonalGoal.cs`

`PersonalGoal` reuses everything from `Task` and adds `category`, `progressPct`, and whether it is recurring. Keeping goal logic in its own class makes the code much cleaner than putting everything into one large `Task` class.

### Interface #1 – IActionPlan / ActionPlan

**Files:** `IActionPlan.cs`, `ActionPlan.cs`

The Dashboard only talks to `IActionPlan`, not the concrete class. That means if we ever wanted to swap out the planning logic, we would not have to change the Dashboard.

### Interface #2 – IAuthenticatable / User

**Files:** `IAuthenticatable.cs`, `User.cs`

This makes sure any user type has to implement `Authenticate()`. It keeps authentication logic organized and makes it easier to add more account types later.

### Interface #3 – IDailyCheckin / DailyCheckin

**Files:** `IDailyCheckin.cs`, `DailyCheckin.cs`

`Program.cs` only uses `IDailyCheckin`, so the check-in logic can easily be changed later without affecting startup code.

### Polymorphism #1 – GetSummary() override

**Files:** `Task.cs`, `Assignment.cs`, `PersonalGoal.cs`

`GetSummary()` is abstract in `Task`, and both subclasses override it with their own format. When the Dashboard loops through a `List<Task>` and calls `GetSummary()`, C# automatically calls the correct version depending on the object type.

### Polymorphism #2 – Serialize() override

**Files:** `Task.cs`, `Assignment.cs`, `PersonalGoal.cs`

`TaskService.SaveTasks()` just calls `task.Serialize(email)` on every item without caring about the type. Each subclass writes its own format automatically.

### Polymorphism #3 – DisplayProfile() override

**Files:** `Person.cs`, `User.cs`

`DisplayProfile()` is abstract in `Person` and implemented in `User`. If we added more person types later, they could each display their own profile differently.

### Access Modifiers

Fields are private and accessed through public getters so outside classes cannot directly modify data. Shared base class fields are protected. Helper methods like `HashPassword()` are `private static`.

### Struct – TaskStats

**File:** `TaskStats.cs`

`TaskStats` is a struct used to count tasks by status. It is small and temporary, so using a struct made sense.

### Enum #1 – TaskStatus

**File:** `TaskStatus.cs`

Used to track whether a task is `NotStarted`, `InProgress`, or `Complete`. This is cleaner and safer than using strings.

### Enum #2 – MoodStatus

**File:** `MoodStatus.cs`

Used for the daily check-in moods: `Great`, `Okay`, and `Stressed`.

### Data Structure – List<Task>

**Files:** `Dashboard.cs`, `TaskService.cs`, `ActionPlan.cs`

We used `List<Task>` because tasks need to be added, iterated through, and accessed by index. It fits this type of dynamic collection well.

### I/O – Console UI + File Storage

**Files:** `Program.cs`, `Dashboard.cs`, `DailyCheckin.cs`, `ActionPlan.cs`, `UserService.cs`, `TaskService.cs`

User interaction uses `Console.ReadLine()` and `Console.WriteLine()`. Data saving and loading uses `File.ReadAllLines` and `File.WriteAllLines` from `System.IO`.

## 4. Design Patterns

### Factory Method

**Category:** Creational
**File:** `TaskFactory.cs`

We used a factory so the Dashboard does not need to know which constructor to call for each task type. The factory creates either an `Assignment` or a `PersonalGoal` based on the user choice. This keeps the Dashboard cleaner and makes adding new task types easier later.

### Template Method

**Category:** Behavioral
**File:** `ActionPlan.cs`

`DisplayWrittenPlan()` controls the overall process for generating an action plan. It calculates days left, decides the style, and then calls helper methods depending on the timeline. The structure stays the same while the details change.

### Strategy via Interface

**Category:** Behavioral
**Files:** `IActionPlan.cs`, `ActionPlan.cs`, `Dashboard.cs`

Dashboard stores an `IActionPlan` reference instead of a concrete `ActionPlan` object. This means different planning systems could be swapped in later without changing Dashboard code.

## 5. Design Decisions

### How It’s Structured

We split the code into two namespaces:

* `StudentPlanner.Domain` for logic and data classes
* `StudentPlanner.Presentation` for the Dashboard UI

`Program.cs` acts as the entry point and connects everything together.

### Why We Designed It This Way

The biggest design choice was making `Task` the main abstraction. Everything in the system works with `Task` references. If we add a new task type later, we only need to create a subclass and update `TaskFactory`.

We also avoided depending on concrete classes where possible. Dashboard uses `IActionPlan` instead of `ActionPlan` directly to keep the UI separate from the planning logic.

The `Person/User` split may seem unnecessary for a small project, but it helps if the system grows later to include advisors or admins.

### Persistence

We used plain text files instead of a database to keep the project simple and self-contained. Each domain class handles its own serialization and deserialization.

### What We Would Do Differently

One limitation is that action plans are not saved between sessions. Another issue is that adding a new task overwrites the previous action plan context. Ideally, each task would store its own plan.

The console I/O is also tightly connected to the presentation layer, which makes unit testing harder. If we had more time, we would definitely abstract that out further.