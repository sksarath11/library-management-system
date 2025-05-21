import React, { useEffect, useState } from 'react';
import { fetchTodos, deleteTodo, updateTodo } from '../services/api';
import '../components/TodosList.css';

type Todo = {
    bookId: number;
    bookNumber: string;
    classification: string;
    title: string;
    publisher: string;
    isReference: boolean;
    isAvailable: boolean;
    author: string;
    copyCode: string;
    description: string;
};

const TodosList: React.FC = () => {
    const [todos, setTodos] = useState<Todo[]>([]);
    const [editingTodo, setEditingTodo] = useState<Todo | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadTodos = async () => {
            try {
                const data = await fetchTodos();
                setTodos(data);
            } catch (err: any) {
                console.error('Error fetching todos:', err);
                setError('Failed to fetch todos');
            }
        };
        loadTodos();
    }, []);

    const handleDelete = async (bookId: number) => {
        try {
            await deleteTodo(bookId);
            setTodos(prev => prev.filter(todo => todo.bookId !== bookId));
        } catch (err) {
            console.error('Error deleting todo:', err);
            setError('Failed to delete todo');
        }
    };

    const handleEdit = (todo: Todo) => {
        setEditingTodo(todo);
    };

    const handleUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingTodo) return;

        try {
            const updatedTodo = await updateTodo(editingTodo.bookId, editingTodo);
            setTodos(prev =>
                prev.map(todo => todo.bookId === editingTodo.bookId ? updatedTodo : todo)
            );
            setEditingTodo(null);
        } catch (err) {
            console.error('Error updating todo:', err);
            setError('Failed to update todo');
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        if (!editingTodo) return;
        const { name, value, type, checked } = e.target;
        setEditingTodo({
            ...editingTodo,
            [name]: type === 'checkbox' ? checked : value,
        });
    };

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div>
            <h1>Book List</h1>
            <ul>
                {todos.map(todo => (
                    <li key={todo.bookId}>
                        {editingTodo && editingTodo.bookId === todo.bookId ? (
                            <form onSubmit={handleUpdate}>
                                <input type="number" name="bookId" value={editingTodo.bookId} disabled />
                                <input type="text" name="bookNumber" value={editingTodo.bookNumber} onChange={handleChange} required />
                                <input type="text" name="classification" value={editingTodo.classification} onChange={handleChange} required />
                                <input type="text" name="title" value={editingTodo.title} onChange={handleChange} required />
                                <input type="text" name="publisher" value={editingTodo.publisher} onChange={handleChange} required />
                                <input type="text" name="author" value={editingTodo.author} onChange={handleChange} required />
                                <input type="text" name="copyCode" value={editingTodo.copyCode} onChange={handleChange} required />
                                <textarea name="description" value={editingTodo.description} onChange={handleChange} required />
                                <label>
                                    Reference:
                                    <input type="checkbox" name="isReference" checked={editingTodo.isReference} onChange={handleChange} />
                                </label>
                                <label>
                                    Available:
                                    <input type="checkbox" name="isAvailable" checked={editingTodo.isAvailable} onChange={handleChange} />
                                </label>
                                <button type="submit">Save</button>
                                <button type="button" onClick={() => setEditingTodo(null)}>Cancel</button>
                            </form>
                        ) : (
                            <div>
                                <strong>Book Number:</strong> {todo.bookNumber} <br />
                                <strong>Classification:</strong> {todo.classification} <br />
                                <strong>Title:</strong> {todo.title} <br />
                                <strong>Publisher:</strong> {todo.publisher} <br />
                                <strong>Author:</strong> {todo.author} <br />
                                <strong>Copy Code:</strong> {todo.copyCode} <br />
                                <strong>Description:</strong> {todo.description} <br />
                                <strong>Is Reference:</strong> {todo.isReference ? 'Yes' : 'No'} <br />
                                <strong>Is Available:</strong> {todo.isAvailable ? 'Yes' : 'No'} <br />
                                <button onClick={() => handleEdit(todo)}>Edit</button>
                                <button onClick={() => handleDelete(todo.bookId)}>Delete</button>
                            </div>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TodosList;
