using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using libarary.Data;
using libarary.Models;
using libarary.Services;
/**
var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register TaskService using the database
builder.Services.AddScoped<ITaskService, DbTaskService>();

builder.Services.AddDirectoryBrowser();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply CORS middleware before other middlewares
app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Started.");
    await next();
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Finished.");
});

// CRUD endpoints
app.MapGet("/todos", (ITaskService service) => service.GetTodos());

app.MapGet("/todos/{BookId}", (int BookId, ITaskService service) =>
{
    var targetTodo = service.GetTodoById(BookId);
    return targetTodo is null
        ? Results.NotFound($"Book with BookId {BookId} not found.")
        : Results.Ok(targetTodo);
});

app.MapPost("/todos", (Todo task, ITaskService service) =>
{
    try
    {
        service.AddTodo(task);
        return Results.Created($"/todos/{task.BookId}", task);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});


app.MapDelete("/todos/{BookId}", (int BookId, ITaskService service) =>
{
    try
    {
        service.DeleteById(BookId);
        return Results.NoContent();
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapPut("/todos/{BookId}", (int BookId, Todo updatedTodo, ITaskService service) =>
{
    try
    {
        var updated = service.UpdateById(BookId, updatedTodo);
        return updated is null
            ? Results.NotFound($"Book with BookId {BookId} not found.")
            : Results.Ok(updated);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

public record Todo(int BookId, string Author, string Title, bool IsAvailable);

interface ITaskService
{
    Todo? GetTodoById(int BookId);
    List<Todo> GetTodos();
    void DeleteById(int BookId);
    Todo AddTodo(Todo task);
    Todo? UpdateById(int BookId, Todo updatedTodo);
}
**/

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the UserService
builder.Services.AddScoped<UserService>();

// Register controllers for user registration and other API calls
builder.Services.AddControllers();

// Register TaskService using the database
builder.Services.AddScoped<ITaskService, DbTaskService>();

builder.Services.AddDirectoryBrowser();




// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply CORS middleware before other middlewares
app.UseCors("AllowAll");

// Use controllers
app.MapControllers();  // Make sure controllers are mapped

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Started.");
    await next();
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Finished.");
});

// CRUD endpoints for todos
app.MapGet("/todos", (ITaskService service) => service.GetTodos());
app.MapGet("/todos/{BookId}", (int BookId, ITaskService service) =>
{
    var targetTodo = service.GetTodoById(BookId);
    return targetTodo is null
        ? Results.NotFound($"Book with BookId {BookId} not found.")
        : Results.Ok(targetTodo);
});
app.MapPost("/todos", (Todo task, ITaskService service) =>
{
    try
    {
        service.AddTodo(task);
        return Results.Created($"/todos/{task.BookId}", task);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});
app.MapDelete("/todos/{BookId}", (int BookId, ITaskService service) =>
{
    try
    {
        service.DeleteById(BookId);
        return Results.NoContent();
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(ex.Message);
    }
});
app.MapPut("/todos/{BookId}", (int BookId, Todo updatedTodo, ITaskService service) =>
{
    try
    {
        var updated = service.UpdateById(BookId, updatedTodo);
        return updated is null
            ? Results.NotFound($"Book with BookId {BookId} not found.")
            : Results.Ok(updated);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
