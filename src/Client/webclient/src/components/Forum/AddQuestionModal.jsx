import { useState } from 'react';
import api from '../../api/axios';
import './AddQuestionModal.css';

const AddQuestionModal = ({ isOpen, onClose, onQuestionAdded }) => {
  const [form, setForm] = useState({ title: '', description: '' });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const response = await api.post('/Questions', {
        title: form.title,
        description: form.description
      });
      
      onQuestionAdded(response.data);
      onClose();
      setForm({ title: '', description: '' });
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка при создании вопроса');
    } finally {
      setIsLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>×</button>
        <h2>Задать новый вопрос</h2>
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Заголовок</label>
            <input
              name="title"
              type="text"
              value={form.title}
              onChange={handleChange}
              required
              minLength={5}
              maxLength={100}
            />
          </div>
          
          <div className="form-group">
            <label>Описание</label>
            <textarea
              name="description"
              value={form.description}
              onChange={handleChange}
              required
              minLength={10}
              rows={5}
            />
          </div>
          
          {error && <div className="error-message">{error}</div>}
          
          <div className="modal-actions">
            <button type="button" onClick={onClose} disabled={isLoading}>
              Отмена
            </button>
            <button type="submit" disabled={isLoading}>
              {isLoading ? 'Отправка...' : 'Опубликовать'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddQuestionModal;