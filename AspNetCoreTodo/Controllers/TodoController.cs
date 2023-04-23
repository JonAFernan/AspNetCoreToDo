using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers;
[Authorize]
public class TodoController : Controller
{
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        IdentityUser currentUser = await _userManager.GetUserAsync(User);
        if(currentUser == null) return Challenge();

        TodoItem[] items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

        TodoViewModel model = new TodoViewModel()
        {
            Items = items
        };

        return View(model);
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(TodoItem newItem)
    {
        if(!ModelState.IsValid) return RedirectToAction("Index");

        IdentityUser currentUser = await _userManager.GetUserAsync(User);
        if(currentUser == null) return Challenge();

        bool succesfull = await _todoItemService.AddItemAsync(newItem ,currentUser);
        
        if (!succesfull) return BadRequest("Could not add a item");

        return RedirectToAction("Index");
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDone(Guid id)
    {
        if(id == Guid.Empty) return RedirectToAction("Index");

        IdentityUser currentUser = await _userManager.GetUserAsync(User);
        if(currentUser == null) return Challenge();

        bool succesfull = await _todoItemService.MarkDoneAsync(id, currentUser);
        
        if(!succesfull) return BadRequest("Could not add a item");

        return RedirectToAction("Index");
    }
}