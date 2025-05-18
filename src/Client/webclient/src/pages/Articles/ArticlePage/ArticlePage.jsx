import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../../../api/axios';
import './ArticlePage.css';

const ArticlePage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [article, setArticle] = useState(null);
  const [category, setCategory] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [comment, setComment] = useState('');
  const [comments, setComments] = useState([]);
  const [editingCommentId, setEditingCommentId] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const [notification, setNotification] = useState({ show: false, message: '' });
  const notificationTimeout = useRef(null);

  // Проверка авторизации и прав
  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    setIsAuthenticated(!!token);
    
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const roles = payload.roles ? payload.roles.split(',') : [];
        setIsAdmin(roles.includes('Admin'));
      } catch (err) {
        console.error('Ошибка декодирования токена:', err);
      }
    }
  }, []);

  // Загрузка статьи и категории
  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const articleResponse = await api.get(`/articles/${id}`);
        setArticle(articleResponse.data);
        
        const categoryResponse = await api.get(`/categories/${articleResponse.data.categoryId}`);
        setCategory(categoryResponse.data);
      } catch (err) {
        console.error('Ошибка загрузки данных:', err);
        setError('Не удалось загрузить статью');
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, [id]);

  const handleAddComment = () => {
    if (!comment.trim()) return;
    
    if (editingCommentId) {
      // Редактирование существующего комментария
      setComments(comments.map(c => 
        c.id === editingCommentId ? { ...c, text: comment } : c
      ));
      setEditingCommentId(null);
    } else {
      // Добавление нового комментария
      const newComment = {
        id: Date.now(),
        text: comment,
        date: new Date().toLocaleString()
      };
      setComments([...comments, newComment]);
    }
    
    setComment('');
  };

  const handleEditComment = (comment) => {
    setComment(comment.text);
    setEditingCommentId(comment.id);
  };

  const handleDeleteComment = (commentId) => {
    setComments(comments.filter(c => c.id !== commentId));
    if (editingCommentId === commentId) {
      setEditingCommentId(null);
      setComment('');
    }
  };

  const handleApproveArticle = async () => {
    try {
      await api.post(`/articles/${id}/approve`);
      setArticle(prev => ({ ...prev, isShouldBeApproved: false }));
      showNotification('Статья успешно одобрена');
    } catch (err) {
      console.error('Ошибка одобрения статьи:', err);
      alert('Не удалось одобрить статью');
    }
  };

  const showNotification = (message) => {
    // Очищаем предыдущий таймер, если он есть
    if (notificationTimeout.current) {
      clearTimeout(notificationTimeout.current);
    }
    
    setNotification({ show: true, message });
    
    // Автоматическое скрытие через 3 секунды
    notificationTimeout.current = setTimeout(() => {
      setNotification({ show: false, message: '' });
      }, 3000);
    };

  // Очищаем таймер при размонтировании компонента
  useEffect(() => {
    return () => {
      if (notificationTimeout.current) {
        clearTimeout(notificationTimeout.current);
      }
    };
  }, []);

  const renderBlock = (block) => {
    switch (block.type) {
      case 30005: // Заголовок
        return <h2 key={block.id} className="article-block heading">{block.data}</h2>;
      case 30006: // Текст
        return <p key={block.id} className="article-block text">{block.data}</p>;
      case 30007: // Код
        return (
          <pre key={block.id} className="article-block code">
            <code>{block.data}</code>
          </pre>
        );
      case 30008: // Изображение
        return (
          <div key={block.id} className="article-block image-container">
            <img src={block.data} alt="Из статьи" className="article-image" />
          </div>
        );
      default:
        return null;
    }
  };

  if (loading) {
    return <div className="loading">Загрузка статьи...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (!article) {
    return <div className="error">Статья не найдена</div>;
  }

  return (
    <div className="article-page">
      <div className="article-header">
        <div className="article-meta">
          <span className="article-category">{category?.name || 'Без категории'}</span>
          {article.isDocumentation && (
            <span className="article-badge">Документация</span>
          )}
          {article.isShouldBeApproved && (
            <span className="approval-badge">Требует одобрения</span>
          )}
        </div>
        <h1 className="article-title">{article.title}</h1>
        <p className="article-description">{article.shortDescription}</p>

        {isAdmin && article.isShouldBeApproved && (
          <div className="approve-section">
            <button 
              onClick={handleApproveArticle}
              className="approve-button"
            >
              Одобрить статью
            </button>
          </div>
        )}
      </div>

      <div className="article-content">
        {article.blocks.map(renderBlock)}
      </div>

      <div className="comments-section">
        <h2 className="comments-title">Комментарии</h2>
        
        {isAuthenticated ? (
          <div className="comment-form">
            <textarea
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              placeholder="Оставьте ваш комментарий..."
              rows="3"
              className="comment-input"
            />
            <button 
              onClick={handleAddComment}
              className="comment-submit"
              disabled={!comment.trim()}
            >
              {editingCommentId ? 'Обновить' : 'Отправить'}
            </button>
            {editingCommentId && (
              <button 
                onClick={() => {
                  setEditingCommentId(null);
                  setComment('');
                }}
                className="comment-cancel"
              >
                Отмена
              </button>
            )}
          </div>
        ) : (
          <p className="auth-notice">
            Для добавления комментариев необходимо <a href="/login">войти</a>
          </p>
        )}

        <div className="comments-list">
          {comments.length === 0 ? (
            <p className="no-comments">Комментариев пока нет</p>
          ) : (
            comments.map(c => (
              <div key={c.id} className="comment-item">
                <div className="comment-text">{c.text}</div>
                <div className="comment-footer">
                  <span className="comment-date">{c.date}</span>
                  {isAuthenticated && (
                    <div className="comment-actions">
                      <button 
                        onClick={() => handleEditComment(c)}
                        className="comment-edit"
                      >
                        <img src="/edit.svg" alt="Редактировать" className="edit-icon" />
                      </button>
                      <button 
                        onClick={() => handleDeleteComment(c.id)}
                        className="comment-delete"
                      >
                        Удалить
                      </button>
                    </div>
                  )}
                </div>
              </div>
            ))
          )}
        </div>
      </div>
      <div className={`notification ${notification.show ? 'show' : ''}`}>
              {notification.message}
       </div>
    </div>
  );
};

export default ArticlePage;