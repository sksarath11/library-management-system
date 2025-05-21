import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { FaUsers, FaBook, FaUserTag, FaQuestionCircle, FaCreditCard } from 'react-icons/fa';
import '../styles/DashboardPage.css';

const DashboardPage = () => {
  const [users, setUsers] = useState([]);
  const [books, setBooks] = useState([]);
  const [userBooks, setUserBooks] = useState([]);
  const [inquiries, setInquiries] = useState([]);
  const [loans, setLoans] = useState([]);
  const [message, setMessage] = useState('');
  const [activeSection, setActiveSection] = useState(null);
  const [newBook, setNewBook] = useState({
    BookId: '',
    BookNumber: '',
    Classification: '',
    Title: '',
    Author: '',
    Publisher: '',
    IsReference: false,
    CopyCode: '',
    IsAvailable: true,
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const isLoggedIn = true;
    if (!isLoggedIn) {
      navigate('/librarian-login');
    } else {
      fetchAllData();
    }
  }, [navigate]);

  const fetchAllData = async () => {
    setIsLoading(true);
    try {
      const [userRes, bookRes, userBookRes] = await Promise.all([
        axios.get('http://localhost:5213/api/user/all'),
        axios.get('http://localhost:5213/todos'),
        axios.get('http://localhost:5213/api/UserBooks/all')
      ]);
      setUsers(userRes.data);
      setBooks(bookRes.data);
      setUserBooks(userBookRes.data);
    } catch (error) {
      console.error('Error fetching data:', error);
      setMessage('❌ Error fetching data.');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchInquiries = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get('http://localhost:5213/api/IQuery');
      setInquiries(response.data);
      setActiveSection('inquiries');
    } catch (error) {
      console.error('Error fetching inquiries:', error);
      setMessage('❌ Failed to load inquiries.');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchLoans = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get('http://localhost:5213/api/Loan');
      setLoans(response.data);
      setActiveSection('loans');
    } catch (error) {
      console.error('Error fetching loans:', error);
      setMessage('❌ Failed to load loans.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewBook((prevBook) => ({
      ...prevBook,
      [name]: value,
    }));
  };

  const handleAddBook = async (e) => {
    e.preventDefault();
    if (!newBook.BookNumber || !newBook.Title || !newBook.Author || !newBook.Publisher || !newBook.Classification || !newBook.CopyCode || !newBook.BookId) {
      setMessage('❌ Please fill all fields.');
      return;
    }
    setIsLoading(true);
    try {
      const response = await axios.post('http://localhost:5213/api/task/add', newBook);
      if (response.status === 200) {
        setBooks([...books, response.data]);
        setMessage('✅ Book added successfully!');
        setNewBook({
          BookId: '',
          BookNumber: '',
          Classification: '',
          Title: '',
          Author: '',
          Publisher: '',
          IsReference: false,
          CopyCode: '',
          IsAvailable: true,
        });
      }
    } catch (error) {
      setMessage('❌ Failed to add book.');
      console.error('Error adding book:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleEditUser = (userId) => alert(`Editing user with ID: ${userId}`);
  const handleDeleteUser = (userId) => {
    setUsers(users.filter(user => user.userId !== userId));
    alert(`Deleted user with ID: ${userId}`);
  };
  const handleDeleteBook = (bookId) => setBooks(books.filter(book => book.bookId !== bookId));
  const handleEditBook = (bookId) => alert(`Editing book with ID: ${bookId}`);
  const handleDeleteUserBook = (userId, bookId) => {
    setUserBooks(userBooks.filter(userBook => userBook.userId !== userId || userBook.bookId !== bookId));
    alert(`Deleted user-book relation for user ${userId} and book ${bookId}`);
  };

  return (
    <div className="dashboard-container">
      <h2>Admin Dashboard</h2>
      {message && <p className="error-message">{message}</p>}

      <div className="icon-container">
        <div className="icon-card" onClick={() => setActiveSection('users')}>
          <FaUsers size={40} />
          <h4>Users</h4>
        </div>
        <div className="icon-card" onClick={() => setActiveSection('books')}>
          <FaBook size={40} />
          <h4>Books</h4>
        </div>
        <div className="icon-card" onClick={() => setActiveSection('userBooks')}>
          <FaUserTag size={40} />
          <h4>Borrowed Books</h4>
        </div>
        <div className="icon-card" onClick={() => setActiveSection('newBooks')}>
          <FaBook size={40} />
          <h4>New Books</h4>
        </div>
        <div className="icon-card" onClick={fetchInquiries}>
          <FaQuestionCircle size={40} />
          <h4>Inquiries</h4>
        </div>
        <div className="icon-card" onClick={fetchLoans}>
          <FaCreditCard size={40} />
          <h4>Loans</h4>
        </div>
      </div>

      <div className="data-section">
        {isLoading && <p>Loading...</p>}

        {activeSection === 'users' && users.length > 0 && (
          <div className="data-card">
            <h3>User Data</h3>
            <div className="user-cards">
              {users.map((user) => (
                <div key={user.userId} className="user-card">
                  <div className="user-card-header">
                    <h4>{user.name}</h4>
                    <span className={`user-role ${user.sex.toLowerCase()}`}>{user.sex}</span>
                  </div>
                  <div className="user-info">
                    <p><strong>Email:</strong> {user.address}</p>
                    <p><strong>National ID:</strong> {user.nationalIdNumber}</p>
                  </div>
                  <div className="user-actions">
                    <button onClick={() => handleEditUser(user.userId)} className="btn edit-btn">Edit</button>
                    <button onClick={() => handleDeleteUser(user.userId)} className="btn delete-btn">Delete</button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {activeSection === 'books' && books.length > 0 && (
          <div className="data-card">
            <h3>Books Data</h3>
            <div className="book-cards">
              {books.map((book) => (
                <div key={book.bookId} className="book-card">
                  <div className="book-card-header">
                    <h4>{book.title}</h4>
                    <span className={`book-status ${book.isAvailable ? 'available' : 'not-available'}`}>
                      {book.isAvailable ? 'Available' : 'Not Available'}
                    </span>
                  </div>
                  <div className="book-info">
                    <p><strong>Author:</strong> {book.author}</p>
                    <p><strong>Publisher:</strong> {book.publisher}</p>
                    <p><strong>Classification:</strong> {book.classification}</p>
                    <p><strong>Book Number:</strong> {book.bookNumber}</p>
                  </div>
                  <div className="book-actions">
                    <button onClick={() => handleEditBook(book.bookId)} className="btn edit-btn">Edit</button>
                    <button onClick={() => handleDeleteBook(book.bookId)} className="btn delete-btn">Delete</button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {activeSection === 'userBooks' && userBooks.length > 0 && (
          <div className="data-card">
            <h3>User-Book Data</h3>
            <div className="user-book-cards">
              {userBooks.map((userBook) => (
                <div key={`${userBook.userId}-${userBook.bookId}`} className="user-book-card">
                  <div className="user-book-header">
                    <h4>User ID: {userBook.userId} | Book ID: {userBook.bookId}</h4>
                  </div>
                  <div className="user-book-info">
                    <p><strong>Purchase Date:</strong> {new Date(userBook.purchaseDate).toLocaleString()}</p>
                  </div>
                  <div className="user-book-actions">
                    <button onClick={() => handleDeleteUserBook(userBook.userId, userBook.bookId)} className="btn delete-btn">
                      Delete User-Book
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {activeSection === 'newBooks' && (
          <div className="data-card">
            <h3>Add New Book</h3>
            <form onSubmit={handleAddBook}>
              {['BookId', 'BookNumber', 'Title', 'Author', 'Publisher', 'Classification', 'CopyCode'].map((field) => (
                <input 
                  key={field}
                  type="text" 
                  name={field} 
                  placeholder={field.replace(/([A-Z])/g, ' $1').trim()} 
                  value={newBook[field]} 
                  onChange={handleInputChange} 
                  required 
                />
              ))}
              <div>
                <label>
                  Available:
                  <input 
                    type="checkbox" 
                    name="IsAvailable" 
                    checked={newBook.IsAvailable} 
                    onChange={(e) => setNewBook({ ...newBook, IsAvailable: e.target.checked })} 
                  />
                </label>
              </div>
              <button type="submit" className="btn submit-btn" disabled={isLoading}>
                {isLoading ? 'Adding...' : 'Add Book'}
              </button>
            </form>
          </div>
        )}

        {activeSection === 'inquiries' && inquiries.length > 0 && (
          <div className="data-card">
            <h3>User Inquiries</h3>
            <div className="inquiry-cards">
              {inquiries.map((inq, index) => (
                <div key={index} className="inquiry-card">
                  <h4>{inq.name}</h4>
                  <p><strong>Inquiry:</strong> {inq.message}</p>
                  <p><strong>UserID:</strong> {inq.userId}</p>
                  <p><strong>BookID:</strong> {inq.bookId}</p>
                </div>
              ))}
            </div>
          </div>
        )}

        {activeSection === 'loans' && loans.length > 0 && (
          <div className="data-card">
            <h3>Loan Data</h3>
            <div className="loan-cards">
              {loans.map((loan) => (
                <div key={loan.loanID} className="loan-card">
                  <h4>Loan ID: {loan.loanID}</h4>
                  <p><strong>Book ID:</strong> {loan.bookId || 'N/A'}</p>
                  <p><strong>User ID:</strong> {loan.userId}</p>
                  <p><strong>Loan Date:</strong> {new Date(loan.loanDate).toLocaleString()}</p>
                  <p><strong>Due Date:</strong> {new Date(loan.dueDate).toLocaleString()}</p>
                  <p><strong>Status:</strong> {loan.status}</p>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default DashboardPage;
