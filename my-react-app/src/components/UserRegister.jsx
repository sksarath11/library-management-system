import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/UserRegister.css';

const UserRegister = () => {
  const [formData, setFormData] = useState({
    name: '',
    sex: '',
    nationalIdNumber: '',
    address: ''
  });

  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const handleChange = e => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();

    try {
      const response = await fetch('http://localhost:5213/api/user/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (response.ok) {
        setMessage('Registration successful! 🎉');
        setFormData({ name: '', sex: '', nationalIdNumber: '', address: '' });
        setTimeout(() => navigate('/login'), 2000);  // Redirect to login after 2 sec
      } else {
        const errorData = await response.json();
        setMessage(`Error: ${errorData.message || 'Registration failed.'}`);
      }
    } catch (error) {
      setMessage(`Error: ${error.message}`);
    }
  };

  return (
    <div className="register-container">
      <h2>User Registration</h2>
      <form onSubmit={handleSubmit}>
        <label>Name:
          <input type="text" name="name" value={formData.name} onChange={handleChange} required />
        </label>

        <label>Sex:
          <select name="sex" value={formData.sex} onChange={handleChange} required>
            <option value="">Select</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
        </label>

        <label>National ID Number:
          <input type="text" name="nationalIdNumber" value={formData.nationalIdNumber} onChange={handleChange} required />
        </label>

        <label>Address:
          <textarea name="address" value={formData.address} onChange={handleChange} required />
        </label>

        <div className="form-buttons">
          <button type="submit">Register</button>
          <button type="button" onClick={() => navigate('/')}>Cancel</button>
        </div>
      </form>

      {message && <p className="message">{message}</p>}

      <div className="nav-links">
        <Link to="/">🏠 Back to Home</Link> | 
        <Link to="/login">🔑 Go to Login</Link>
      </div>
    </div>
  );
};

export default UserRegister;
