import { useState, useEffect } from 'react';
import { Link, useNavigate  } from 'react-router-dom';
import api from '../../api/axios';
import './ArticlesPage.css';

const ArticlesPage = () => {
  const navigate = useNavigate();
  const [articles, setArticles] = useState([]);
  const [isAuthorized, setisAuthorized] = useState(false);
  const [categories, setCategories] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [contentTypes, setContentTypes] = useState({
    documentation: true,
    author: true
  });// null - оба типа, true - только доки, false - только авторские
  const [sortBy, setSortBy] = useState('date'); // 'date' или 'popular'
  const [pagination, setPagination] = useState({
    pageNo: 1,
    pageSize: 10,
    totalPages: 1
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Загрузка категорий
  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    if (token){
      setisAuthorized(true);
    }

    const loadCategories = async () => {
      try {
        const response = await api.get('/Categories');
        setCategories(response.data);
      } catch (err) {
        console.error('Ошибка загрузки категорий:', err);
      }
    };
    
    loadCategories();
  }, []);

  // Загрузка статей
  useEffect(() => {
    const loadArticles = async () => {
      try {
        setLoading(true);
        setError(null);
        
        const endpoint = sortBy === 'date' 
          ? '/articles/paginated/date' 
          : '/articles/paginated/popular';
        
        // Формируем параметры запроса
        const params = new URLSearchParams();
        params.append('PageNo', pagination.pageNo);
        params.append('PageSize', pagination.pageSize);

        // Добавляем категории, если они выбраны
        selectedCategories.forEach((catId, i) => {
          params.append('Categories[' + i + ']', catId);
        });
        
        // Определяем IsDocumentation на основе выбранных типов контента
        if (contentTypes.documentation && !contentTypes.author) {
          params.append('IsDocumentation', true);
        } else if (!contentTypes.documentation && contentTypes.author) {
          params.append('IsDocumentation', false);
        }

        const response = await api.get(endpoint, { params });
        setArticles(response.data.items);
        setPagination(prev => ({
          ...prev,
          totalPages: response.data.totalPages,
          currentPage: response.data.currentPage
        }));
        
      } catch (err) {
        console.error('Ошибка загрузки статей:', err);
        setError('Не удалось загрузить статьи');
        setArticles([]);
      } finally {
        setLoading(false);
      }
    };
    
    loadArticles();
  }, [sortBy, selectedCategories, contentTypes, pagination.pageNo, pagination.pageSize]);

  const handleCategoryToggle = (categoryId) => {
    setSelectedCategories(prev => 
      prev.includes(categoryId) 
        ? prev.filter(id => id !== categoryId) 
        : [...prev, categoryId]
    );
    setPagination(prev => ({ ...prev, pageNo: 1 })); // Сброс на первую страницу при изменении фильтров
  };

  const handleContentTypeToggle = (type) => {
    // Если пытаемся отключить последний активный тип - игнорируем
    if (contentTypes[type] && 
        Object.values(contentTypes).filter(v => v).length <= 1) {
      return;
    }

    // Если включаем тип, который был выключен
    if (!contentTypes[type]) {
      setContentTypes(prev => ({
        ...prev,
        [type]: true
      }));
    } 
    // Если отключаем тип (и останется хотя бы один активный)
    else {
      setContentTypes(prev => ({
        ...prev,
        [type]: false
      }));
    }

    setPagination(prev => ({ ...prev, pageNo: 1 }));
  };

  const handleSortToggle = (type) => {
    setSortBy(type);
    setPagination(prev => ({ ...prev, pageNo: 1 }));
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= pagination.totalPages) {
      setPagination(prev => ({ ...prev, pageNo: newPage }));
    }
  };

  const navigateToArticle = (id) => {
    navigate(`/articles/${id}`);
  };

  const renderContentTypeCheckbox = (type, label) => {
    const isBlocked = contentTypes[type] && 
                       Object.values(contentTypes).filter(v => v).length <= 1;

    return (
      <label className={`checkbox-label ${isBlocked ? 'disabled' : ''}`}>
        <input
          type="checkbox"
          checked={contentTypes[type]}
          onChange={() => handleContentTypeToggle(type)}
          className="checkbox-input"
          disabled={isBlocked}
        />
        <span className="checkbox-custom"></span>
        {label}
      </label>
    );
  };

  if (loading) {
    return <div className="loading">Загрузка...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="articles-container">
      <div className="articles-header">
        <div className="header-top">
          <h1>Статьи</h1>
          {isAuthorized && (
            <Link to="/articles/create" className="create-article-button">
              Написать статью
            </Link>
          )}
        </div>
      </div>

        <div className="articles-content">
        <div className="filters-sidebar">
          <div className="filter-section">
            <h3 className="filter-title">Сортировка</h3>
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={sortBy === 'date'}
                onChange={() => handleSortToggle('date')}
                className="checkbox-input"
              />
              <span className="checkbox-custom"></span>
              Новые
            </label>
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={sortBy === 'popular'}
                onChange={() => handleSortToggle('popular')}
                className="checkbox-input"
              />
              <span className="checkbox-custom"></span>
              Популярные
            </label>
          </div>

          <div className="filter-section">
            <h3 className="filter-title">Источник</h3>
              {renderContentTypeCheckbox('documentation', 'Документация')}
              {renderContentTypeCheckbox('author', 'Авторские статьи')}
          </div>

          <div className="filter-section">
            <h3 className="filter-title">Категории</h3>
            {categories.map(category => (
              <label key={category.id} className="checkbox-label">
                <input
                  type="checkbox"
                  checked={selectedCategories.includes(category.id)}
                  onChange={() => handleCategoryToggle(category.id)}
                  className="checkbox-input"
                />
                <span className="checkbox-custom"></span>
                {category.name}
              </label>
            ))}
          </div>
        </div>

        <div className="articles-main">
          {articles.length === 0 ? (
            <div className="no-articles">Статьи не найдены</div>
          ) : (
            <>
              <div className="articles-grid">
                {articles.map(article => {
                  const category = categories.find(c => c.id === article.categoryId);
                  return (
                    <div 
                      key={article.id} 
                      className="article-card"
                      onClick={() => navigateToArticle(article.id)}
                    >
                      <div className="article-header">
                        <span className="article-category">
                          {category?.name || 'Без категории'}
                        </span>
                        <h3 className="article-title">{article.title}</h3>
                      </div>
                      <p className="article-description">
                        {article.shortDescription}
                      </p>
                      {article.isDocumentation && (
                        <span className="article-badge">Документация</span>
                      )}
                    </div>
                  );
                })}
              </div>
              
              <div className="pagination">
                <button 
                  onClick={() => handlePageChange(pagination.pageNo - 1)}
                  disabled={pagination.pageNo === 1}
                >
                  Назад
                </button>
                
                <span className="page-info">
                  Страница {pagination.pageNo} из {pagination.totalPages}
                </span>
                
                <button 
                  onClick={() => handlePageChange(pagination.pageNo + 1)}
                  disabled={pagination.pageNo === pagination.totalPages}
                >
                  Вперед
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default ArticlesPage;