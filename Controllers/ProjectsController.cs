using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class ProjectsController : Controller
{
    private readonly ProjectStore _store;

    public ProjectsController(ProjectStore store)
    {
        _store = store;
    }

    public IActionResult Details(int id)
    {
        var project = _store.GetById(id);
        if (project == null)
        {
            return NotFound();
        }

        var model = new ProjectDetailsViewModel
        {
            Project = project,
            Comments = _store.GetComments(id),
            NewComment = new Comment { ProjectId = id }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddComment(ProjectDetailsViewModel model)
    {
        var project = _store.GetById(model.NewComment.ProjectId);
        if (project == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Project = project;
            model.Comments = _store.GetComments(project.Id);
            return View("Details", model);
        }

        _store.AddComment(model.NewComment);
        TempData["CommentPosted"] = "Your comment has been posted.";

        return RedirectToAction(nameof(Details), new { id = project.Id });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteComment(int commentId, int projectId)
    {
        _store.DeleteComment(commentId);
        return RedirectToAction(nameof(Details), new { id = projectId });
    }
}
