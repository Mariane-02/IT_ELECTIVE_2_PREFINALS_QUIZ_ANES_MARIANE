# IT ELECTIVE 2 – Midterm A1: MVC Portfolio

**Mariane Valerie Añes** · BSIT · Lyceum of Alabang

An ASP.NET Core MVC portfolio application that presents my projects with GitHub
links, descriptions, and thumbnail images, organised through a table of contents,
with a detail page and a comment section for every project.

## Login Details

The application uses a hardcoded administrator account.

| Field    | Value            |
|----------|------------------|
| Username | `admin`          |
| Password | `Portfolio@2026` |

Log in at `/Account/Login`, or use the **Admin log in** button in the sidebar.
Signing in is only needed to delete comments — browsing projects and posting
comments is open to everyone.

> Note: these credentials are hardcoded in `Services/AuthService.cs` because the
> activity requires it. A production application would store hashed credentials
> in a database or a secrets store instead.

## Features

- Single-page home with hero, about, skills, and contact sections
- **Table of contents** grouping all projects by type, each linking to its detail page
- Project cards with thumbnail, description, technology badges, and GitHub link
- **Detail page per project** with a full overview and key features list
- **Comment section per project** with server-side validation
- Hardcoded login using cookie authentication
- Comment deletion restricted to the signed-in administrator

## Security Measures

- Cookie authentication with `HttpOnly` and `SameSite=Strict` cookies
- Anti-forgery tokens on every POST form
- Constant-time credential comparison
- The same error message for a wrong username and a wrong password
- Lockout after 5 failed login attempts for 5 minutes
- Open redirect protection using `Url.IsLocalUrl`
- Length and content validation on comment input
- Razor output encoding on all user-submitted text (prevents stored XSS)

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- C#
- Cookie Authentication
- Bootstrap 5 + Bootstrap Icons, custom maroon and whitesmoke theme
- In-memory data store (no database required)

## How to Run

Open `ConsoleApp1/IT_ELECTIVE_2_MIDTERM_A1.sln` in Visual Studio and press F5, or:

```
cd ConsoleApp1/WebApplication1
dotnet run
```

## Project Structure

```
ConsoleApp1/WebApplication1/
├── Program.cs              Services, authentication, and pipeline setup
├── Models/                 Project, Comment, LoginViewModel, ProjectDetailsViewModel
├── Services/
│   ├── ProjectStore.cs     Project data and in-memory comments
│   └── AuthService.cs      Hardcoded credential checking and lockout
├── Controllers/            Home, Projects, Account
├── Views/
│   ├── Home/Index.cshtml       Home page with the table of contents
│   ├── Projects/Details.cshtml Detail page and comment section
│   └── Account/Login.cshtml    Login form
└── wwwroot/
    ├── css/site.css
    └── images/             Project thumbnails
```

## Adding Thumbnails

Place screenshots in `ConsoleApp1/WebApplication1/wwwroot/images/` using these
file names:

| Project                                  | File                      |
|------------------------------------------|---------------------------|
| Student Management System                | `student-management.png`  |
| File Ingestion Engine                    | `file-ingestion.png`      |
| FizzBuzz Logic Evaluation                | `fizzbuzz.png`            |
| Console Calculator Engine                | `calculator.png`          |
| HTTP Client Starter                      | `http-client.png`         |
| Transport Polymorphism Challenge         | `transport.png`           |

Recommended size is about 800×450 pixels. If a file is missing, the card falls
back to the existing `briefing.png` icon instead of showing a broken image.

## Note on Comments

Comments are stored in memory and reset when the application restarts, so the
project runs without any database setup.

## Author

Mariane Valerie Añes
GitHub: https://github.com/Mariane-02
