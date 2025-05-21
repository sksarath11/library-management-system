import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/Library.css';

const TodoList = () => {
  const [books, setBooks] = useState([]);
  const [borrowedBooks, setBorrowedBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [inquiries, setInquiries] = useState({});
  const [inquiryVisible, setInquiryVisible] = useState({});
  const [copyIndex, setCopyIndex] = useState({});
  const navigate = useNavigate();

  const loggedInUser = JSON.parse(localStorage.getItem('loggedInUser'));
  const userId = loggedInUser ? loggedInUser.userId : 1;

  useEffect(() => {
    const fetchData = async () => {
      try {
        const resBooks = await fetch('http://localhost:5213/todos');
        const resBorrowed = await fetch(`http://localhost:5213/api/UserBooks/user/${userId}`);

        if (!resBooks.ok) throw new Error('Failed to fetch books');
        const booksData = await resBooks.json();
        let borrowedData = resBorrowed.ok ? await resBorrowed.json() : [];

        const borrowedWithTitles = borrowedData.map(borrow => {
          const bookMatch = booksData.find(book => book.bookId === borrow.bookId);
          return { ...borrow, title: bookMatch ? bookMatch.title : 'Unknown Title' };
        });

        const purchasedBookIds = borrowedData.map(ub => ub.bookId);
        const availableBooks = booksData.filter(book => !purchasedBookIds.includes(book.bookId));

        setBooks(availableBooks);
        setBorrowedBooks(borrowedWithTitles);
        setError(null);
        setLoading(false);
      } catch {
        setError('Error loading books');
        setLoading(false);
      }
    };
    fetchData();
  }, [userId]);

  const groupBooksByTitle = books.reduce((acc, book) => {
    if (!acc[book.title]) acc[book.title] = [];
    acc[book.title].push(book);
    return acc;
  }, {});

  const handleLoan = async (bookId) => {
    try {
      const today = new Date().toISOString().split('T')[0];
      const dueDate = new Date();
      dueDate.setDate(new Date().getDate() + 14);
      const due = dueDate.toISOString().split('T')[0];

      const response = await fetch('http://localhost:5213/api/Loan', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          BookId: bookId,
          UserId: userId,
          LoanDate: today,
          DueDate: due,
          ReturnDate: null,
          Status: "Active"
        }),
      });

      if (!response.ok) {
        const err = await response.json();
        throw new Error(err.message || 'Loan creation failed.');
      }

      alert(`Book loaned successfully! Return by: ${due}`);
    } catch (err) {
      alert(`Error: ${err.message}`);
    }
  };

  const handleAction = async (bookId, action) => {
    if (action === 'buy') {
      try {
        const purchaseDate = new Date().toISOString();
        const response = await fetch('http://localhost:5213/api/UserBooks', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ userId, bookId, purchaseDate }),
        });
        if (!response.ok) {
          const err = await response.json();
          throw new Error(err.message || 'Purchase failed.');
        }
        alert('Book purchase successful! Please refresh the page to see updates.');
      } catch (err) {
        alert(`Error: ${err.message}`);
      }
    }
  };

  const handleReturn = async (bookId) => {
    try {
      const res = await fetch(`http://localhost:5213/api/UserBooks/book/${bookId}`, { method: 'DELETE' });
      if (!res.ok) {
        const err = await res.json();
        throw new Error(err.message || 'Failed to return.');
      }
      alert('Book returned successfully!');
      setBorrowedBooks(prev => prev.filter(book => book.bookId !== bookId));
    } catch (err) {
      alert(`Error: ${err.message}`);
    }
  };

  const handleInquiryChange = (bookId, e) => {
    setInquiries(prev => ({ ...prev, [bookId]: e.target.value }));
  };

  const handleInquirySubmit = async (bookId, e) => {
    e.preventDefault();
    const message = inquiries[bookId];
    if (!message.trim()) {
      alert('Please enter your inquiry.');
      return;
    }
    const payload = {
      UserId: userId.toString(),
      Message: message,
      Date: new Date().toISOString().split('T')[0],
      Time: new Date().toTimeString().split(' ')[0],
      BookId: bookId,
    };
    try {
      const res = await fetch('http://localhost:5213/api/IQuery', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      if (!res.ok) {
        const err = await res.json();
        throw new Error(err.message || 'Failed to submit inquiry.');
      }
      alert('Inquiry submitted! We will get back to you.');
      setInquiryVisible(prev => ({ ...prev, [bookId]: false }));
      setInquiries(prev => ({ ...prev, [bookId]: '' }));
    } catch (err) {
      alert(`Error: ${err.message}`);
    }
  };

  const toggleInquiryVisibility = (bookId) => {
    setInquiryVisible(prev => ({ ...prev, [bookId]: !prev[bookId] }));
  };

  const handleNextCopy = (title) => {
    const copies = groupBooksByTitle[title];
    setCopyIndex(prev => ({
      ...prev,
      [title]: (prev[title] || 0) + 1 >= copies.length ? 0 : (prev[title] || 0) + 1,
    }));
  };

  const handleLogout = () => {
    localStorage.removeItem('loggedInUser');
    navigate('/login');
  };

  if (loading) return <div className="library-loading">Loading your library...</div>;

  return (
    <div className="library-wrapper">
      <div className="top-actions">
        <button onClick={handleLogout} className="btn-logout">Logout</button>
      </div>

      {loggedInUser && (
        <div className="user-details">
          <h3>Logged in as: {loggedInUser.name}</h3>
          <p>ID: {loggedInUser.nationalIdNumber}</p>
        </div>
      )}

      <h2>Your Borrowed Books</h2>
      {borrowedBooks.length === 0 ? (
        <div className="library-empty">You haven't borrowed any books yet.</div>
      ) : (
        <div className="library-grid">
          {borrowedBooks.map(book => (
            <div key={book.bookId} className="library-card borrowed">
              <h2>{book.title}</h2>
              <p><strong>Book ID:</strong> {book.bookId}</p>
              <p><strong>Purchase Date:</strong> {new Date(book.purchaseDate).toLocaleDateString()}</p>
              <button onClick={() => handleReturn(book.bookId)} className="btn-return">Return Book</button>
            </div>
          ))}
        </div>
      )}

      <h1 className="library-title">📚 Online Library</h1>
      {Object.keys(groupBooksByTitle).length === 0 ? (
        <div className="library-empty">No books available to display.</div>
      ) : (
        Object.entries(groupBooksByTitle).map(([title, copies]) => {
          const currentIndex = copyIndex[title] || 0;
          const book = copies[currentIndex];

          return (
            <div key={title} className="library-card">
              <h2>{book.title}</h2>
              <p><strong>Author:</strong> {book.author}</p>
              <p><strong>Book Number:</strong> {book.bookNumber}</p>
              <p><strong>Classification:</strong> {book.classification}</p>
              <p><strong>Publisher:</strong> {book.publisher}</p>
              <p><strong>Reference:</strong> {book.isReference ? 'Yes' : 'No'}</p>
              <p><strong>Copy Code:</strong> {book.copyCode}</p>
              <p><strong>Status:</strong> {book.isAvailable ? 'Available' : 'Not Available'}</p>

              <div className="library-actions">
                <button onClick={() => handleLoan(book.bookId)} className="btn-loan">Loan Book</button>
                <button onClick={() => handleAction(book.bookId, 'buy')} className="btn-buy">Buy</button>
                <button onClick={() => handleAction(book.bookId, 'reserve')} className="btn-reserve">Reserve</button>
                <button onClick={() => handleAction(book.bookId, 'return')} className="btn-return">Return</button>
              </div>

              {book.isAvailable && (
                <div className="inquiry-section">
                  <button onClick={() => toggleInquiryVisibility(book.bookId)} className="btn-inquiry">
                    {inquiryVisible[book.bookId] ? 'Close Inquiry' : 'Ask a Question'}
                  </button>
                  {inquiryVisible[book.bookId] && (
                    <div className="inquiry-form">
                      <textarea
                        value={inquiries[book.bookId] || ''}
                        onChange={(e) => handleInquiryChange(book.bookId, e)}
                        placeholder="Type your question here..."
                      />
                      <button onClick={(e) => handleInquirySubmit(book.bookId, e)} className="btn-submit">
                        Submit Inquiry
                      </button>
                    </div>
                  )}
                </div>
              )}

              <button className="btn-next-copy" onClick={() => handleNextCopy(title)}>
                Next Copy
              </button>
            </div>
          );
        })
      )}
    </div>
  );
};

export default TodoList;
