import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../../../api/axios';
import './ProfilePage.css';

const ProfilePage = () => {
  const [userInfo, setUserInfo] = useState(null);
  const [articles, setArticles] = useState([]);
  const [activeTab, setActiveTab] = useState('published');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isEditingUsername, setIsEditingUsername] = useState(false);
  const [newPublicUsername, setNewPublicUsername] = useState('');
  const navigate = useNavigate();
  const [notification, setNotification] = useState({ show: false, message: '' });
  const notificationTimeout = useRef(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        
        // Загружаем информацию о пользователе
        const userResponse = await api.get('/users/current/info');
        setUserInfo(userResponse.data);
        
        // Загружаем статьи пользователя
        const articlesResponse = await api.get('/articles/paginated/current');
        setArticles(articlesResponse.data.items);
        
      } catch (err) {
        console.error('Ошибка загрузки данных:', err);
        setError('Не удалось загрузить данные профиля');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleAvatarUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    try {
      const formData = new FormData();
      formData.append('formFile', file);
      
      const response = await api.put('/users/avatar', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      
      setUserInfo({ ...userInfo, avatar: response.data });
      showNotification('Ваша аватарка успешно обновлена');
    } catch (err) {
      console.error('Ошибка загрузки аватарки:', err);
      alert('Не удалось загрузить аватар');
    }
  };

  const handleUsernameEdit = () => {
    setIsEditingUsername(true);
  };

  const handleUsernameSave = async () => {
    if (!newPublicUsername.trim()) {
      alert('Публичное имя не может быть пустым');
      return;
    }

    try {
      await api.put('/users', { 
        NewPublicUsername: newPublicUsername 
      });
      
      setUserInfo({ ...userInfo, publicUsername: newPublicUsername });
      setIsEditingUsername(false);
      showNotification('Публичное имя успешно изменено');
    } catch (err) {
      console.error('Ошибка изменения имени:', err);
      alert('Не удалось изменить публичное имя');
    }
  };

const handleUsernameCancel = () => {
    setNewPublicUsername(userInfo.publicUsername);
    setIsEditingUsername(false);
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

  const navigateToArticle = (id) => {
    navigate(`/articles/${id}`);
  };

  if (loading) {
    return <div className="loading">Загрузка...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (!userInfo) {
    return <div className="error">Пользователь не найден</div>;
  }

  const publishedArticles = articles.filter(article => article.isPublished);
  const draftArticles = articles.filter(article => !article.isPublished);

  return (
    <div className="profile-container">
      <div className="profile-header">
        <div className="avatar-container">
          {userInfo.avatar ? (
            <img src={userInfo.avatar} alt="Аватар" className="avatar" />
          ) : (
            <div className="avatar-placeholder">
              {userInfo.publicUsername.charAt(0).toUpperCase()}
            </div>
          )}
          <label className="avatar-upload-label">
            <input 
              type="file" 
              accept="image/*" 
              onChange={handleAvatarUpload}
              className="avatar-upload-input"
            />
            <img 
              src="/upload.svg" 
              alt="Профиль"
              width="20px"
            />
          </label>
        </div>
        
        <div className="user-info">
          {isEditingUsername ? (
            <div className="username-edit">
              <input
                type="text"
                value={newPublicUsername}
                onChange={(e) => setNewPublicUsername(e.target.value)}
                className="username-input"
                maxLength={50}
              />
              <div className="username-actions">
                <button 
                  onClick={handleUsernameSave}
                  className="save-button"
                >
                  Сохранить
                </button>
                <button 
                  onClick={handleUsernameCancel}
                  className="cancel-button"
                >
                  Отмена
                </button>
              </div>
            </div>
          ) : (
            <div className="username-display">
              <h1 className="username">{userInfo.publicUsername}</h1>
              <button 
                onClick={handleUsernameEdit}
                className="edit-username-button"
              >
                <img 
                  src="/edit.svg" 
                  alt="Изменить"
                  width="16px"
                />
              </button>
            </div>
          )}
          <p className="email">{userInfo.email}</p>
          <p className="login">{userInfo.username}</p>
        </div>
      </div>

      <div className="articles-section">
        <div className="tabs">
          <button 
            className={`tab-button ${activeTab === 'published' ? 'active' : ''}`}
            onClick={() => setActiveTab('published')}
          >
            Опубликованные
          </button>
          <button 
            className={`tab-button ${activeTab === 'drafts' ? 'active' : ''}`}
            onClick={() => setActiveTab('drafts')}
          >
            Черновики
          </button>
        </div>

        <div className="articles-list">
          {activeTab === 'published' ? (
            publishedArticles.length > 0 ? (
              publishedArticles.map(article => (
                <div 
                  key={article.id} 
                  className="article-card"
                  onClick={() => navigateToArticle(article.id)}
                >
                  <h3 className="article-title">{article.title}</h3>
                  <p className="article-date">
                    Опубликовано: {new Date(article.dateOfPublication).toLocaleDateString()}
                  </p>
                  <button 
                    className="edit-button"
                    onClick={() => navigate(`/articles/create/${article.id}`)}
                  >
                    <img 
                        src="/edit.svg" 
                        alt="Профиль"
                      /> Изменить
                  </button>
                </div>
                
              ))
            ) : (
              <p className="no-articles">Нет опубликованных статей</p>
            )
          ) : (
            draftArticles.length > 0 ? (
              draftArticles.map(article => (
                <div 
                  key={article.id} 
                  className={`article-card ${article.isShouldBeApproved ? 'needs-approval' : ''}`}
                >
                  <h3 className="article-title">{article.title}</h3>
                  {article.isShouldBeApproved && (
                    <div className="approval-badge">Ожидает подтверждения</div>
                  )}
                  <button 
                    className="edit-button"
                    onClick={() => navigate(`/articles/create/${article.id}`)}
                  >
                    <img 
                        src="/edit.svg" 
                        alt="Профиль"
                      /> Изменить
                  </button>
                </div>
              ))
            ) : (
              <p className="no-articles">Нет черновиков</p>
            )
          )}
        </div>
      </div>
      <div className={`notification ${notification.show ? 'show' : ''}`}>
              {notification.message}
            </div>
    </div>
  );
};

export default ProfilePage;