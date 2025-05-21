import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/HomePage.css';

const HomePage = () => {
  return (
    <div className="home-container">
      <header className="home-header">
        <h1>📚 Welcome to SARASAVI Library Management System</h1>
        <p><center>Access books, manage loans, and stay curious!</center></p>
      </header>

      <div className="home-buttons">
        <Link to="/login" className="btn primary">User Login</Link>
        <Link to="/register" className="btn secondary">User Register</Link>
        <Link to="/librarian-login" className="btn librarian">Librarian Login</Link>
      </div>

      <nav className="home-nav">
        <Link to="/inquiry" className="nav-link">Inquiry</Link>
        <Link to="/return" className="nav-link">Return</Link>
        <Link to="/loan" className="nav-link">Loan</Link>
      </nav>

      <footer className="home-footer">
        <p>© {new Date().getFullYear()} Your Library. All rights reserved.</p>
      </footer>
    </div>
  );
};

export default HomePage;
