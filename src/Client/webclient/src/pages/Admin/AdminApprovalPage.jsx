import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../../api/axios';
import './AdminApprovalPage.css';

const AdminApprovalPage = () => {
  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadArticles = async () => {
      try {
        setLoading(true);
        const response = await api.get('/articles/approve');
        setArticles(response.data);
      } catch (err) {
        console.error('Ошибка загрузки статей:', err);
        setError('Не удалось загрузить статьи для модерации');
      } finally {
        setLoading(false);
      }
    };

    loadArticles();
  }, []);

  if (loading) {
    return <div className="loading">Загрузка...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="admin-approval-page">
      <h1>Модерация статей</h1>
      
      {articles.length === 0 ? (
        <div className="no-articles">Нет статей, требующих одобрения</div>
      ) : (
        <div className="articles-table-container">
          <table className="articles-table">
            <thead>
              <tr>
                <th>Автор</th>
                <th>Название статьи</th>
                <th>Краткое описание</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {articles.map(article => (
                <tr key={article.id}>
                  <td>{article.authorUsername}</td>
                  <td>
                    <Link to={`/articles/${article.id}`} className="article-link">
                      {article.title}
                    </Link>
                  </td>
                  <td>{article.shortDescription}</td>
                  <td>
                    <Link 
                      to={`/articles/${article.id}`} 
                      className="review-button"
                    >
                      Просмотреть
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default AdminApprovalPage;