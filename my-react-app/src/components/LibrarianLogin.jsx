import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; // Import useNavigate for redirection
import '../styles/LibrarianLogin.css';

const LibrarianLogin = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate(); // Initialize useNavigate

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:5213/api/superuser/checkUser', {
        username,
        password
      });

      setMessage(`✅ ${response.data}`);
      // After successful login, navigate to DashboardPage
      navigate('/dashboard'); // Redirect to the DashboardPage
    } catch (error) {
      if (error.response) {
        setMessage(`❌ ${error.response.data}`);
      } else {
        setMessage('❌ Error connecting to server.');
      }
    }
  };

  return (
    <div className="librarian-login-container">
      <h2>Librarian Login</h2>
      <form onSubmit={handleLogin} className="librarian-login-form">
        <div className="form-group">
          <label>Username:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="btn librarian">Login</button>
      </form>

      {message && <p className="login-message">{message}</p>}
    </div>
  );
};

export default LibrarianLogin;
