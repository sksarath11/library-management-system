import React from 'react';
import './UserDashboard.css';

const UserDashboard = ({ users, onDelete }) => {
  return (
    <div className="user-dashboard-container">
      <h2 className="dashboard-heading">User Dashboard</h2>
      <div className="user-cards">
        {users.length > 0 ? (
          users.map((user) => (
            <div key={user.userId} className="user-card">
              <div className="user-card-header">
                <h3>{user.name}</h3>
                <span className={`user-role ${user.sex.toLowerCase()}`}>{user.sex}</span>
              </div>
              <div className="user-info">
                <p><strong>Email:</strong> {user.address}</p>
                <p><strong>National ID:</strong> {user.nationalIdNumber}</p>
                <p><strong>Address:</strong> {user.address}</p>
              </div>
              <button className="btn-delete" onClick={() => onDelete(user.userId)}>Delete</button>
            </div>
          ))
        ) : (
          <p>No users available</p>
        )}
      </div>
    </div>
  );
};

export default UserDashboard;
