using TodoApi.Models;
using TodoApi.Models.Requests;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TodoService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("ReactClient");

app.MapGet("/api/todos", (TodoService service) => Results.Ok(service.GetAll()));

app.MapPost("/api/todos", (TodoCreateRequest request, TodoService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new { error = "Title is required." });
    }

    var created = service.Add(request.Title.Trim());
    return Results.Created($"/api/todos/{created.Id}", created);
});

app.MapPut("/api/todos/{id}", (int id, TodoUpdateRequest request, TodoService service) =>
{
    var updated = service.Update(id, request);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapPost("/api/todos/{id}/toggle", (int id, TodoService service) =>
{
    var toggled = service.ToggleCompletion(id);
    return toggled is not null ? Results.Ok(toggled) : Results.NotFound();
});

app.MapDelete("/api/todos/{id}", (int id, TodoService service) =>
{
    return service.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.Run("http://localhost:5154");
