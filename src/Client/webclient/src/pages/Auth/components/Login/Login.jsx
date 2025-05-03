import React, { useState } from 'react';

const Login = () => {
  const [formData, setFormData] = useState({ username: '', password: '' });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    console.log(formData);
    fetch('http://localhost:9001/api/Identity/login', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(formData),
    })
      .then(response => { 
        if (!response.ok) {
          console.log('response not Ok ');
          alert('Введены некорректные данные');
          throw new Error(`Ошибка сервера: ${response.statusText}`);
        }
        return response.json();
        })
      .then(data => {
          localStorage.setItem('jwt', data.jwtToken);
          localStorage.setItem('username', formData.username);
          console.log('data in localstorage added');
          window.location.href = '/';
      })
      .catch(error => {console.error('Ошибка авторизации:', error); });
  };

  return (
    <div className="container mt-4">
      <h2>Авторизация</h2>
      <form onSubmit={handleSubmit} className="needs-validation text-start">
        <div className="mb-3">
          <label htmlFor="username" className="form-label">Логин</label>
          <input
            type="text"
            id="username"
            name="username"
            className="form-control"
            value={formData.username}
            onChange={handleChange}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="password" className="form-label">Пароль</label>
          <input
            type="password"
            id="password"
            name="password"
            className="form-control"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>
        <div className="d-flex justify-content-center">
            <button type="submit" className="btn btn-primary">Войти</button>
        </div>
      </form>
    </div>
  );
};

export default Login;