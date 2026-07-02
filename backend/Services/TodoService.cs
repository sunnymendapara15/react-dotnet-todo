using TodoApi.Models;
using TodoApi.Models.Requests;

namespace TodoApi.Services;

public class TodoService
{
    private readonly List<TodoItem> _items = new();
    private int _nextId = 1;

    public IEnumerable<TodoItem> GetAll() => _items;

    public TodoItem? Get(int id) => _items.FirstOrDefault(item => item.Id == id);

    public TodoItem Add(string title)
    {
        var todo = new TodoItem
        {
            Id = _nextId++,
            Title = title,
            IsCompleted = false
        };

        _items.Add(todo);
        return todo;
    }

    public TodoItem? Update(int id, TodoUpdateRequest request)
    {
        var existing = Get(id);
        if (existing is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            existing.Title = request.Title.Trim();
        }

        if (request.IsCompleted.HasValue)
        {
            existing.IsCompleted = request.IsCompleted.Value;
        }

        return existing;
    }

    public TodoItem? ToggleCompletion(int id)
    {
        var existing = Get(id);
        if (existing is null)
        {
            return null;
        }

        existing.IsCompleted = !existing.IsCompleted;
        return existing;
    }

    public bool Delete(int id) => _items.RemoveAll(item => item.Id == id) > 0;
}
