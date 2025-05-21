import React from 'react';
import './UserList.css';

const UserList = ({ users }) => {
  return (
    <div className="user-list-container">
      <h2>User List</h2>
      <div className="user-cards">
        {users.map(user => (
          <div key={user.userId} className="user-card">
            <div className="user-card-header">
              <h3>{user.name}</h3>
              <span className="user-role">{user.sex}</span>
            </div>
            <div className="user-info">
              <p><strong>Email:</strong> {user.address}</p>
              <p><strong>National ID:</strong> {user.nationalIdNumber}</p>
              <p><strong>Address:</strong> {user.address}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default UserList;
