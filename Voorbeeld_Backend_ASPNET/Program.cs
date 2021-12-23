using Microsoft.EntityFrameworkCore;
using Voorbeeld_Backend_ASPNET;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/markten", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/markten/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/markten", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/markten/{todo.Id}", todo);
});

app.UseCorsMiddleware();

app.Run();

class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Locatie { get; set; }
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}