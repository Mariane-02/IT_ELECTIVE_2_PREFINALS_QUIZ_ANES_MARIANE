using WebApplication1.Models;

namespace WebApplication1.Services;

public class ProjectStore
{
    private readonly List<Project> _projects;
    private readonly List<Comment> _comments = new();
    private readonly object _lock = new();
    private int _nextCommentId = 1;

    public ProjectStore()
    {
        _projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Title = "Student Management System – Procedural Core",
                Category = "Console Applications",
                ShortDescription = "A console application built using procedural programming principles to " +
                                   "manage student records, calculate course grade averages, and monitor " +
                                   "academic performance.",
                FullDescription = "This project applies procedural programming to a familiar problem: keeping " +
                                  "track of student records. Records are stored in arrays and passed between " +
                                  "focused methods so that each part of the program does one job — reading " +
                                  "input, computing averages, and displaying results.",
                Features = new[]
                {
                    "Add and list student records",
                    "Compute course grade averages",
                    "Monitor academic performance per student",
                    "Menu-driven console interface"
                },
                Tags = new[] { "C#" },
                GitHubUrl = "https://github.com/Mariane-02/BSIT31E1_PRELIM_H1-ANES_MARIANE.git",
                Thumbnail = "/images/student-management.png"
            },
            new Project
            {
                Id = 2,
                Title = "File Ingestion Engine",
                Category = "Design Patterns",
                ShortDescription = "A flexible backend utility that uses the Strategy and Factory design " +
                                   "patterns to simplify parsing multiple file formats, transforming character " +
                                   "streams, and validating data schemas.",
                FullDescription = "An exercise in keeping code open for extension but closed for modification. " +
                                  "Each file format has its own parsing strategy, and a factory decides which " +
                                  "strategy to hand back, so supporting a new format means adding a class " +
                                  "instead of editing existing ones.",
                Features = new[]
                {
                    "Strategy pattern for per-format parsing",
                    "Factory pattern for selecting the right parser",
                    "Character stream transformation",
                    "Data schema validation"
                },
                Tags = new[] { "C#", "Design Patterns" },
                GitHubUrl = "https://github.com/Mariane-02/-BSIT31E1_PRELIM_H2_Anes_Mariane-Valerie.git",
                Thumbnail = "/images/file-ingestion.png"
            },
            new Project
            {
                Id = 3,
                Title = "FizzBuzz Logic Evaluation",
                Category = "Console Applications",
                ShortDescription = "A console application that showcases control flow design, nested " +
                                   "conditional logic, and modulo-based calculations to generate sequential " +
                                   "numeric outputs.",
                FullDescription = "A small program with a large lesson: the order of conditions matters. " +
                                  "The implementation focuses on readable control flow and on checking the " +
                                  "combined case before the individual ones so the output stays correct.",
                Features = new[]
                {
                    "Modulo-based divisibility checks",
                    "Nested conditional logic",
                    "Loop-driven sequential output",
                    "Clean, readable control flow"
                },
                Tags = new[] { "C#" },
                GitHubUrl = "https://github.com/Mariane-02/BSIT31E1_PRELIM_A1_ANES_MARIANE.git",
                Thumbnail = "/images/fizzbuzz.png"
            },
            new Project
            {
                Id = 4,
                Title = "Console Calculator Engine",
                Category = "Console Applications",
                ShortDescription = "An interactive console application featuring a continuous input loop, " +
                                   "type-conversion checks, error handling for mathematical edge cases, and " +
                                   "conditional arithmetic logic.",
                FullDescription = "A calculator that keeps running until the user chooses to exit. Most of " +
                                  "the work went into the parts users never see when things go right: " +
                                  "rejecting non-numeric input, catching division by zero, and recovering " +
                                  "without crashing the loop.",
                Features = new[]
                {
                    "Continuous input loop until exit",
                    "Safe type conversion with validation",
                    "Division-by-zero and edge case handling",
                    "Four basic arithmetic operations"
                },
                Tags = new[] { "C#" },
                GitHubUrl = "https://github.com/Mariane-02/BSIT31E1_PRELIM_A2_ANES_MARIANE.git",
                Thumbnail = "/images/calculator.png"
            },
            new Project
            {
                Id = 5,
                Title = "HTTP Client Starter",
                Category = "Web Development",
                ShortDescription = "A lightweight ASP.NET Core Web API setup that configures dependency " +
                                   "injection, core service frameworks, HTTP request routing middleware, and " +
                                   "secure controller endpoints.",
                FullDescription = "A starting point for working with HTTP in ASP.NET Core. The project wires " +
                                  "up the service container, registers a typed HTTP client, and exposes " +
                                  "controller endpoints that return JSON, which made the request pipeline " +
                                  "much easier to reason about.",
                Features = new[]
                {
                    "Dependency injection configuration",
                    "Typed HTTP client registration",
                    "Routing middleware setup",
                    "JSON controller endpoints"
                },
                Tags = new[] { "C#", "ASP.NET Core", "JSON" },
                GitHubUrl = "https://github.com/Mariane-02/IT_ELECTIVE_2_PRELIM_EXAM_ANES_MARIANEVALERIE.git",
                Thumbnail = "/images/http-client.png"
            },
            new Project
            {
                Id = 6,
                Title = "Transport Polymorphism Challenge",
                Category = "Object-Oriented Programming",
                ShortDescription = "An object-oriented test harness that validates class inheritance " +
                                   "hierarchies, multiple interface implementations, factory-based object " +
                                   "creation, and polymorphic behavior across different domain entities.",
                FullDescription = "Different transport types share a base class but behave differently at " +
                                  "runtime. The project uses interfaces to describe what each type can do and " +
                                  "a factory to create them, so the calling code works against abstractions " +
                                  "instead of concrete classes.",
                Features = new[]
                {
                    "Base class and inheritance hierarchy",
                    "Multiple interface implementations",
                    "Factory-based object creation",
                    "Runtime polymorphic behaviour"
                },
                Tags = new[] { "C#", "OOP" },
                GitHubUrl = "https://github.com/Mariane-02/Mariane-02-BSIT_31E1_PRELIM_Q1_Anes_Mariane-Valerie.git",
                Thumbnail = "/images/transport.png"
            }
        };
    }

    public IReadOnlyList<Project> GetAll() => _projects;

    public Project? GetById(int id) => _projects.FirstOrDefault(p => p.Id == id);

    public List<Comment> GetComments(int projectId)
    {
        lock (_lock)
        {
            return _comments
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.PostedAt)
                .ToList();
        }
    }

    public int CountComments(int projectId)
    {
        lock (_lock)
        {
            return _comments.Count(c => c.ProjectId == projectId);
        }
    }

    public void AddComment(Comment comment)
    {
        lock (_lock)
        {
            comment.Id = _nextCommentId++;
            comment.PostedAt = DateTime.Now;
            comment.Author = comment.Author.Trim();
            comment.Message = comment.Message.Trim();
            _comments.Add(comment);
        }
    }

    public void DeleteComment(int commentId)
    {
        lock (_lock)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                _comments.Remove(comment);
            }
        }
    }
}
