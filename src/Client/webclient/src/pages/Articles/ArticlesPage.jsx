import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../../api/axios';
import './ArticlesPage.css';

const ArticlesPage = () => {
  const [articles, setArticles] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10); // Фиксированный размер страницы
  const [totalPages, setTotalPages] = useState(1);
  const [sortBy, setSortBy] = useState('date');
  const [selectedCategories, setSelectedCategories] = useState([]);

  // Фиксированная высота для карточек статей
  const [cardHeights, setCardHeights] = useState({});

  useEffect(() => {
    const fetchArticles = async () => {
      try {
        setLoading(true);
        let response;
        
        if (selectedCategories.length > 0) {
          const promises = selectedCategories.map(categoryId => 
            api.get('/api/Articles/paginated/category', {
              params: {
                CategoryId: categoryId,
                PageNo: pageNumber,
                PageSize: pageSize
              }
            })
          );
          
          const results = await Promise.all(promises);
          const combinedItems = results.flatMap(res => res.data.items);
          response = {
            data: {
              items: combinedItems,
              currentPage: pageNumber,
              totalPages: Math.max(...results.map(res => res.data.totalPages)),
              pageSize: pageSize
            }
          };
        } else {
          const endpoint = sortBy === 'date' ? 
            '/Articles/paginated/date' : 
            '/Articles/paginated/author';
            
          response = await api.get(endpoint, {
            params: {
              PageNo: pageNumber,
              PageSize: pageSize
            }
          });
        }
        
        setArticles(response.data.items);
        setTotalPages(response.data.totalPages);
        setError(null);
      } catch (err) {
        setError(err.response?.data?.message || 'Ошибка загрузки статей');
        setArticles([]);
      } finally {
        setLoading(false);
      }
    };

    const fetchCategories = async () => {
      try {
        const response = await api.get('/Categories');
        setCategories(response.data);
      } catch (err) {
        console.error('Ошибка загрузки категорий:', err);
      }
    };

    fetchArticles();
    fetchCategories();
  }, [pageNumber, pageSize, sortBy, selectedCategories]);

  // Фиксируем высоту карточек после загрузки
  useEffect(() => {
    if (!loading && articles.length > 0) {
      const heights = {};
      articles.forEach(article => {
        const element = document.getElementById(`article-${article.id}`);
        if (element) {
          heights[article.id] = element.offsetHeight;
        }
      });
      setCardHeights(heights);
    }
  }, [loading, articles]);

  const handlePageChange = (newPage) => {
    if (newPage > 0 && newPage <= totalPages) {
      setPageNumber(newPage);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  };

  const toggleCategory = (categoryId) => {
    setSelectedCategories(prev => 
      prev.includes(categoryId) 
        ? prev.filter(id => id !== categoryId) 
        : [...prev, categoryId]
    );
    setPageNumber(1);
  };

  return (
    <div className="articles-container">
      <div className="articles-header">
        <h1>НОВЫЕ СТАТЬИ</h1>
        <Link to="/articles/new" className="new-article-button">
          Написать статью
        </Link>
      </div>

      <div className="articles-content">
        <div className="articles-sidebar">
          <div className="sort-section">
            <h3>Сортировать:</h3>
            <label className="sort-option">
              <input 
                type="radio" 
                name="sort" 
                checked={sortBy === 'date'} 
                onChange={() => setSortBy('date')}
              />
              Новые
            </label>
            <label className="sort-option">
              <input 
                type="radio" 
                name="sort" 
                checked={sortBy === 'author'} 
                onChange={() => setSortBy('author')}
              />
              Популярные
            </label>
          </div>

          <div className="categories-section">
            <h3>Категории:</h3>
            {categories.map(category => (
              <label key={category.id} className="category-option">
                <input 
                  type="checkbox" 
                  checked={selectedCategories.includes(category.id)}
                  onChange={() => toggleCategory(category.id)}
                />
                {category.name}
              </label>
            ))}
          </div>
        </div>

        <div className="articles-list">
          {loading ? (
            <div className="loading">Загрузка статей...</div>
          ) : error ? (
            <div className="error">Ошибка: {error}</div>
          ) : articles.length > 0 ? (
            <>
              {articles.map(article => (
                <div 
                  key={article.id} 
                  id={`article-${article.id}`}
                  className="article-card"
                  style={{ minHeight: cardHeights[article.id] || 'auto' }}
                >
                  <h2>{article.title}</h2>
                  <p className="article-description">
                    {article.blocks?.find(b => b.type === 30005)?.data || 'Описание отсутствует'}
                  </p>
                  <div className="article-footer">
                    <Link to={`/articles/${article.id}`} className="read-more">
                      Читать
                    </Link>
                    <span className="article-date">
                      {new Date(article.dateOfPublication).toLocaleDateString()}
                    </span>
                  </div>
                </div>
              ))}

              <div className="pagination-wrapper">
                <div className="pagination">
                  <button 
                    onClick={() => handlePageChange(pageNumber - 1)} 
                    disabled={pageNumber === 1}
                  >
                    Назад
                  </button>
                  
                  <span>Страница {pageNumber} из {totalPages}</span>
                  
                  <button 
                    onClick={() => handlePageChange(pageNumber + 1)} 
                    disabled={pageNumber === totalPages}
                  >
                    Вперед
                  </button>
                </div>
              </div>
            </>
          ) : (
            <div className="no-articles">Статьи не найдены</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ArticlesPage;