import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/UserLogin.css';

const UserLogin = () => {
  const [nationalId, setNationalId] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async e => {
    e.preventDefault();
    setIsLoading(true);
    setError(''); // Reset any previous error messages

    if (!nationalId) {
      setError('National ID is required.');
      setIsLoading(false);
      return;
    }

    try {
      const response = await fetch('http://localhost:5213/api/user/all');
      if (!response.ok) throw new Error('Failed to fetch users');

      const users = await response.json();
      const user = users.find(u => u.nationalIdNumber === nationalId);

      if (user) {
        localStorage.setItem('loggedInUser', JSON.stringify(user)); // Store user in localStorage
        navigate('/todolist');
      } else {
        setError('Invalid National ID Number.');
      }
    } catch (err) {
      setError(`Error: ${err.message}`);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <h2>User Login</h2>
      <form onSubmit={handleLogin}>
        <label>National ID Number:
          <input
            type="text"
            value={nationalId}
            onChange={e => setNationalId(e.target.value)}
            required
          />
        </label>
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Logging in...' : 'Login'}
        </button>
      </form>
      {error && <p className="error">{error}</p>}

      <div className="nav-links">
        <p>Don’t have an account? <a href="/register">Register</a></p>
        <p><a href="/">🏠 Back to Home</a></p>
      </div>
    </div>
  );
};

export default UserLogin;
