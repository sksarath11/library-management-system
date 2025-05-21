using libarary.Models;
using System.Collections.Generic;

namespace libarary.Services
{
    public interface ITaskService
    {
        Todo? GetTodoById(int BookId);
        List<Todo> GetTodos();
        void DeleteById(int BookId);
        Todo AddTodo(Todo task);
        Todo? UpdateById(int BookId, Todo updatedTodo);

        Task AddBookWithCopies(Book input);
        void BuyBook(int bookId);
        void ReserveBook(int bookId);
        void CancelReservation(int bookId);
    }
}
