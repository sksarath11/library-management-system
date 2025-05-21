import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './components/HomePage';
import UserRegister from './components/UserRegister';
import UserLogin from './components/UserLogin';
import TodoList from './components/TodoList';
import LibrarianLogin from './components/LibrarianLogin';
import Dashboard from './components/DashboardPage'; // Import Dashboard component

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/register" element={<UserRegister />} />
        <Route path="/login" element={<UserLogin />} />
        <Route path="/librarian-login" element={<LibrarianLogin />} />
        <Route path="/dashboard" element={<Dashboard />} /> {/* Add route for Dashboard */}
        <Route path="/todolist" element={<TodoList />} />
      </Routes>
    </Router>
  );
}

export default App;
