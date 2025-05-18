import { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams  } from 'react-router-dom';
import { Guid } from 'js-guid';
import api from '../../../api/axios';
import './ArticleEditor.css';

const ArticleEditor = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  
  // Состояния редактора
  const [category, setCategory] = useState('');
  const [title, setTitle] = useState('');
  const [shortDescription, setShortDescription] = useState('');
  const [blocks, setBlocks] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [userRoles, setUserRoles] = useState([]);
  const [isAuthorized, setIsAuthorized] = useState(false);
  const [notification, setNotification] = useState({ show: false, message: '' });
  const notificationTimeout = useRef(null);
  
  // Состояния для управления добавлением блоков
  const [showBlockTypeSelector, setShowBlockTypeSelector] = useState(false);
  const fileInputRefs = useRef({});

    useEffect(() => {
    const token = localStorage.getItem('accessToken');
    if (!token) {
      navigate('/');
      return;
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const roles = payload.roles ? payload.roles.split(',') : [];
      setUserRoles(roles);
      setIsAuthorized(true);
    } catch (error) {
      console.error('Ошибка декодирования токена:', error);
      navigate('/');
    }
  }, [navigate]);

  // Загрузка данных при монтировании
  useEffect(() => {
    if (!isAuthorized) return;

    const loadData = async () => {
      try {
        setIsLoading(true);
        setError(null);
        
        // Параллельная загрузка категорий и статьи (если есть id)
        const [categoriesResponse, articleResponse] = await Promise.all([
          api.get('/Categories'),
          id ? api.get(`/Articles/${id}`) : Promise.resolve(null)
        ]);
        
        setCategories(categoriesResponse.data);
        
        // Если загружаем существующую статью
        if (articleResponse) {
          const article = articleResponse.data;
          setTitle(article.title);
          setCategory(article.categoryId);
          setShortDescription(article.shortDescription || '');
          setBlocks(article.blocks || []);
        }
      } catch (err) {
        console.error('Ошибка загрузки данных:', err);
        setError('Не удалось загрузить данные');
      } finally {
        setIsLoading(false);
      }
    };
    
    loadData();
  }, [id, isAuthorized]);

  // Создание новой статьи при изменении заголовка и категории
  useEffect(() => {
    console.log(title);
    console.log(shortDescription);
    console.log(category);
    if (!id && title && category && shortDescription) {
      const createArticle = async () => {
        try {
          const response = await api.post('/Articles', {
            id: Guid.newGuid().toString(),
            title,
            shortDescription,
            categoryId: category
          });
          
          if (response.data) {
            navigate(`/articles/create/${response.data}`, { replace: true });
          }
        } catch (err) {
          console.error('Ошибка создания статьи:', err);
        }
      };
      
      console.log('Create');
      // Дебаунс для избежания частых запросов
      const timer = setTimeout(createArticle, 1000);
      return () => clearTimeout(timer);
    }
  }, [id, title, category, shortDescription, navigate]);

  // Обновление заголовка статьи
  useEffect(() => {
    if (id && title) {
      const updateTitle = async () => {
        try {
          await api.put('/Articles', {
            id,
            newTitle: title,
            newShortDescription: shortDescription
          });
        } catch (err) {
          console.error('Ошибка обновления заголовка:', err);
        }
      };
      
      const timer = setTimeout(updateTitle, 1000);
      return () => clearTimeout(timer);
    }
  }, [id, title, shortDescription]);

  // Обработчики блоков
  const handleAddBlockClick = () => setShowBlockTypeSelector(true);

  const selectBlockType = (type) => {
    setShowBlockTypeSelector(false);
    
    const newBlock = {
      id: Guid.newGuid().toString(),
      type: type,
      data: '',
    };
    
    setBlocks([...blocks, newBlock]);
  };

  const handleBlockChange = (id, value) => {
    setBlocks(blocks.map(block => 
      block.id === id ? { ...block, data: value } : block
    ));
  };

  const handleImageUpload = async (blockId, e) => {
    const file = e.target.files[0];
    if (!file) return;

    try {
      setIsSubmitting(true);
      const formData = new FormData();
      formData.append('file', file);
      
      const oldBlock = blocks.find(b => b.id === blockId);
      const oldFileUrl = oldBlock?.data || '';
      
      const response = await api.post(
        `/Articles/image?${oldBlock?.data == null ? '' :  'oldFileUrl='+encodeURIComponent(oldFileUrl)}'`, 
        formData, 
        { headers: { 'Content-Type': 'multipart/form-data' } }
      );

      setBlocks(blocks.map(block => 
        block.id === blockId ? { ...block, data: response.data } : block
      ));
    } catch (err) {
      console.error('Ошибка загрузки изображения:', err);
      alert('Не удалось загрузить изображение');
    } finally {
      setIsSubmitting(false);
    }
  };

  const removeBlock = (id) => {
    setBlocks(blocks.filter(block => block.id !== id));
  };

  // Сохранение блоков
  const saveBlocks = async () => {
    if (!id) return false;

    try {
      setIsSubmitting(true);
      const command = {
        ArticleId: id,
        Blocks: blocks.map(block => ({
          Id: block.id,
          BlockType: block.type,
          Data: block.data
        }))
      };

      await api.post('/Articles/block', command);
      return true;
    } catch (err) {
      console.error('Ошибка сохранения блоков:', err);
      alert('Не удалось сохранить изменения');
      return false;
    } finally {
      setIsSubmitting(false);
    }
  };

  const moveBlockUp = (index) => {
    if (index === 0) return;
    const newBlocks = [...blocks];
    [newBlocks[index], newBlocks[index - 1]] = [newBlocks[index - 1], newBlocks[index]];
    setBlocks(newBlocks);
  };

  const moveBlockDown = (index) => {
    if (index === blocks.length - 1) return;
    const newBlocks = [...blocks];
    [newBlocks[index], newBlocks[index + 1]] = [newBlocks[index + 1], newBlocks[index]];
    setBlocks(newBlocks);
  };

  const isAuthor = userRoles.includes('Author') || userRoles.includes('Admin');
  console.log(userRoles);
   // Функция для показа уведомления
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

  const handleSaveDraft = async () => {
    if (!id) return;
    
    const saved = await saveBlocks();
    if (saved) {
      showNotification('Черновик успешно сохранен');
    }
  };

  // Обработчики действий
  const handlePublish = async () => {
    if (!id) return;
    
    const saved = await saveBlocks();
    if (!saved) return;

    try {
      setIsSubmitting(true);
      await api.post('/Articles/publish', { 
        articleId: id,
        currentUserName: localStorage.getItem('username') 
      });
      
      navigate(`/articles/${id}`);
    } catch (err) {
      console.error('Ошибка публикации:', err);
      alert('Не удалось опубликовать статью');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!id || !window.confirm('Вы уверены, что хотите удалить статью?')) return;

    try {
      setIsSubmitting(true);
      await api.delete('/Articles', { data: { id } });
      navigate('/articles');
    } catch (err) {
      console.error('Ошибка удаления:', err);
      alert('Не удалось удалить статью');
    } finally {
      setIsSubmitting(false);
    }
  };

  // Вспомогательные функции
  const getBlockTypeName = (type) => {
    switch(type) {
      case 30005: return 'Заголовок';
      case 30006: return 'Текст';
      case 30007: return 'Код';
      case 30008: return 'Изображение';
      default: return 'Неизвестный тип';
    }
  };

  if (isLoading) {
    return <div className="loading">Загрузка...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="article-editor">
      <h1>КОНСТРУКТОР СТАТЬИ</h1>

      <div className="form-group">
        <label>Категория*</label>
        <select 
          value={category} 
          onChange={(e) => !id && setCategory(e.target.value)}
          required
          disabled={!!id}
        >
          <option value="">Выберите категорию</option>
          {categories.map(cat => (
            <option key={cat.id} value={cat.id}>{cat.name}</option>
          ))}
        </select>
      </div>

      <div className="form-group">
        <label>Заголовок статьи*</label>
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Введите заголовок"
          required
        />
      </div>

      <div className="form-group">
        <label>Краткое описание*</label>
        <textarea
          value={shortDescription}
          onChange={(e) => setShortDescription(e.target.value)}
          placeholder="Введите краткое описание статьи"
          rows="3"
          required
        />
      </div>

      <div className="blocks-container">
        {blocks.map((block, index) => (
          <div key={block.id} className="block-item">
            <div className="block-header">
              <span className="block-type">
                {getBlockTypeName(block.type)}
              </span>
              <div className="block-controls">
                <button 
                  onClick={() => moveBlockUp(index)} 
                  disabled={index === 0 || isSubmitting}
                  className="move-button"
                >
                  ↑
                </button>
                <button 
                  onClick={() => moveBlockDown(index)} 
                  disabled={index === blocks.length - 1 || isSubmitting}
                  className="move-button"
                >
                  ↓
                </button>
                <button 
                  onClick={() => removeBlock(block.id)}
                  disabled={isSubmitting}
                  className="remove-block-button"
                >
                  x
                </button>
              </div>
            </div>

            {renderBlockContent(block)}
          </div>
        ))}
      </div>

      {showBlockTypeSelector && (
  <div className={`block-type-selector-overlay ${showBlockTypeSelector ? 'show' : ''}`}>
    <div className="block-type-selector">
      <h3>Выберите тип блока</h3>
      <div className="block-type-options">
        <button onClick={() => selectBlockType(30005)}>Заголовок</button>
        <button onClick={() => selectBlockType(30006)}>Текст</button>
        <button onClick={() => selectBlockType(30007)}>Код</button>
        <button onClick={() => selectBlockType(30008)}>Изображение</button>
      </div>
      <button 
        onClick={() => setShowBlockTypeSelector(false)}
        className="cancel-button"
      >
        Отмена
      </button>
    </div>
  </div>
)}

      <button 
        onClick={handleAddBlockClick}
        className="add-block-button"
        disabled={isSubmitting || !id}
      >
        + Добавить блок
      </button>

      <div className="editor-actions">
        <button 
          onClick={handlePublish} 
          disabled={isSubmitting || !id}
          className="publish-button"
        >
          {isSubmitting 
            ? 'Отправка...' 
            : isAuthor 
              ? 'Опубликовать' 
              : 'Отправить на проверку'}
        </button>
        
        <button 
          onClick={handleSaveDraft} 
          disabled={isSubmitting || !id}
          className="draft-button"
        >
          {isSubmitting ? 'Сохранение...' : 'Сохранить черновик'}
        </button>
        
        <button 
          onClick={handleDelete} 
          disabled={isSubmitting || !id}
          className="delete-button"
        >
          Удалить
        </button>
      </div>
    </div>
  );

  // Функция рендеринга содержимого блока
  function renderBlockContent(block) {
    switch(block.type) {
      case 30005: // Заголовок
        return (
          <input
            type="text"
            value={block.data}
            onChange={(e) => handleBlockChange(block.id, e.target.value)}
            placeholder="Введите заголовок"
            className="block-input"
            disabled={isSubmitting}
          />
        );
        
      case 30006: // Текст
        return (
          <textarea
            value={block.data}
            onChange={(e) => handleBlockChange(block.id, e.target.value)}
            placeholder="Введите текст"
            rows="5"
            className="block-textarea"
            disabled={isSubmitting}
          />
        );
        
      case 30007: // Код
        return (
          <textarea
            value={block.data}
            onChange={(e) => handleBlockChange(block.id, e.target.value)}
            placeholder="Введите код"
            rows="5"
            className="block-textarea code-block"
            disabled={isSubmitting}
          />
        );
        
      case 30008: // Изображение
        return (
          <div className="image-block">
            {block.data ? (
              <>
                <img 
                  src={block.data} 
                  alt="Preview" 
                  className="image-preview"
                />
                <input
                  type="file"
                  accept="image/*"
                  onChange={(e) => handleImageUpload(block.id, e)}
                  className="image-upload-input"
                  id={`image-upload-${block.id}`}
                  disabled={isSubmitting}
                  ref={el => fileInputRefs.current[block.id] = el}
                />
                <label 
                  htmlFor={`image-upload-${block.id}`}
                  className="change-image-button"
                >
                  Изменить изображение
                </label>
              </>
            ) : (
              <div className="image-upload-container">
                <input
                  type="file"
                  accept="image/*"
                  onChange={(e) => handleImageUpload(block.id, e)}
                  className="image-upload-input"
                  id={`image-upload-${block.id}`}
                  disabled={isSubmitting}
                  ref={el => fileInputRefs.current[block.id] = el}
                />
                <label 
                  htmlFor={`image-upload-${block.id}`}
                  className="upload-image-button"
                >
                  Загрузить изображение
                </label>
              </div>
            )}
            <div className={`notification ${notification.show ? 'show' : ''}`}>
              {notification.message}
            </div>
          </div>
        );
        
      default:
        return null;
    }
  }
};

export default ArticleEditor;