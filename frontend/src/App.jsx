import { useEffect, useState } from "react";

const API_URL = import.meta.env.VITE_API_URL ?? "http://localhost:5154/api/todos";

function App() {
  const [todos, setTodos] = useState([]);
  const [newTitle, setNewTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchTodos = async () => {
    setLoading(true);
    setError("");

    try {
      const response = await fetch(API_URL);
      if (!response.ok) {
        throw new Error("Unable to load todos");
      }

      const payload = await response.json();
      setTodos(payload);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTodos();
  }, []);

  const addTodo = async () => {
    if (!newTitle.trim()) {
      return;
    }

    const response = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title: newTitle.trim() })
    });

    if (!response.ok) {
      setError("Unable to add todo");
      return;
    }

    setNewTitle("");
    fetchTodos();
  };

  const toggleTodo = async (id) => {
    const response = await fetch(`${API_URL}/${id}/toggle`, {
      method: "POST"
    });

    if (response.ok) {
      fetchTodos();
    } else {
      setError("Unable to update todo status");
    }
  };

  const deleteTodo = async (id) => {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE"
    });

    if (response.ok) {
      setTodos((current) => current.filter((todo) => todo.id !== id));
    } else {
      setError("Unable to delete todo");
    }
  };

  return (
    <main className="app-shell">
      <section className="panel">
        <header>
          <h1>React + .NET Todo</h1>
          <p>Lightweight UI built with Vite + minimal .NET API.</p>
        </header>

        <div className="new-todo">
          <input
            type="text"
            value={newTitle}
            onChange={(event) => setNewTitle(event.target.value)}
            placeholder="What needs to be done?"
            onKeyDown={(event) => event.key === "Enter" && addTodo()}
          />
          <button type="button" onClick={addTodo}>
            Add
          </button>
        </div>

        {error && <p className="error">{error}</p>}

        {loading ? (
          <p className="muted">Loading todos...</p>
        ) : (
          <ul className="todo-list">
            {todos.map((todo) => (
              <li key={todo.id} className={todo.isCompleted ? "completed" : "pending"}>
                <label>
                  <input
                    type="checkbox"
                    checked={todo.isCompleted}
                    onChange={() => toggleTodo(todo.id)}
                  />
                  <span>{todo.title}</span>
                </label>
                <button type="button" onClick={() => deleteTodo(todo.id)}>
                  Remove
                </button>
              </li>
            ))}
          </ul>
        )}

        <p className="muted">All changes sync instantly with the backend API.</p>
      </section>
    </main>
  );
}

export default App;
