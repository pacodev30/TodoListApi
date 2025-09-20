using System.Reflection;

namespace TodoListApi;
public class TodoService
{
    private readonly List<Todo> _list = [];

    // GetAll
    public List<Todo> GetAll() => _list;

    // GetById
    public Todo? GetById(int Id) => _list.Find(_list => _list.Id == Id);

    // Delete
    public bool Delete(int Id)
    {
        if (GetById(Id) is null) return false;
        var todo = GetById(Id);
        if (todo is not null) _list.Remove(todo);
        return true;
    }

    // Add
    public Todo Add(string title)
    {
        var id = _list.Count == 0 ? 1 : _list.Max(x => x.Id) + 1;
        var todo = new Todo(
            id, 
            title, 
            DateTime.Now);
        _list.Add(todo);
        return todo;
    }

    // Update
    public void Update(int id, Todo item)
    {
        Delete(id);
        var newToto = new Todo(id, item.Title, item.StartDate, item.EndDate);
        _list.Add(newToto);
    }
}
