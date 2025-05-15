import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../../api/axios';
import './RegisterForm.css';

export default function RegisterForm() {
  const [form, setForm] = useState({ 
    username: '', 
    email: '', 
    password: '' 
  });
  const [step, setStep] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      await api.post('Identity/registration', {
        username: form.username,
        email: form.email,
        password: form.password
      });
      
      setStep(2);
    } catch (err) {
      setError(err.response?.data?.message || "Ошибка регистрации");
    } finally {
      setIsLoading(false);
    }
  };

  if (step === 2) {
    return (
      <div className="register-container">
        <div className="register-card">
          <h2 className="register-title">Почти готово!</h2>
          <div className="register-message">
            <p>Мы отправили письмо с подтверждением на <strong>{form.email}</strong>.</p>
            <p>Пожалуйста, проверьте вашу почту и перейдите по ссылке для завершения регистрации.</p>
          </div>
          <button 
            className="register-button"
            onClick={() => navigate('/')}
          >
            Вернуться на главную
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="register-container">
      <div className="register-card">
        <h2 className="register-title">Регистрация</h2>
        
        <form onSubmit={handleSubmit} className="register-form">
          <div className="form-group">
            <label htmlFor="username" className="form-label">Имя пользователя</label>
            <input
              id="username"
              name="username"
              type="text"
              className="form-input"
              placeholder="Придумайте имя пользователя"
              value={form.username}
              onChange={handleChange}
              required
              minLength={3}
              maxLength={30}
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="email" className="form-label">Email</label>
            <input
              id="email"
              name="email"
              type="email"
              className="form-input"
              placeholder="Введите ваш email"
              value={form.email}
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
              placeholder="Придумайте пароль"
              value={form.password}
              onChange={handleChange}
              required
              minLength={6}
            />
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <button 
            type="submit" 
            className="register-button"
            disabled={isLoading}
          >
            {isLoading ? 'Отправка...' : 'Зарегистрироваться'}
          </button>
        </form>
      </div>
    </div>
  );
}