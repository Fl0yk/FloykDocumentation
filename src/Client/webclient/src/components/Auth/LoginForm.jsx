import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../../api/axios';
import './LoginForm.css';

export default function LoginForm() {
  const [form, setForm] = useState({ username: '', password: '' });
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const response = await api.post('/Identity/login', {
        username: form.username,
        password: form.password
      });

      // Сохраняем токены в localStorage
      console.log(response.data);
      localStorage.setItem('accessToken', response.data.jwtToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      
      // Обновляем состояние аутентификации в приложении
      window.dispatchEvent(new Event('storage'));
      
      navigate("/", { replace: true });
    } catch (err) {
      setError(err.response?.data?.message || 'Неверное имя пользователя или пароль');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2 className="login-title">Вход</h2>
        
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label htmlFor="username" className="form-label">Имя пользователя</label>
            <input
              id="username"
              name="username"
              type="text"
              className="form-input"
              placeholder="Введите имя пользователя"
              value={form.username}
              onChange={handleChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="password" className="form-label">Пароль</label>
            <input
              id="password"
              name="password"
              type="password"
              className="form-input"
              placeholder="Введите пароль"
              value={form.password}
              onChange={handleChange}
              required
            />
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <button 
            type="submit" 
            className="login-button"
            disabled={isLoading}
          >
            {isLoading ? 'Вход...' : 'Войти'}
          </button>
        </form>
      </div>
    </div>
  );
}