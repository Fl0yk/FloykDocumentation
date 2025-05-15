import { useState, useEffect } from 'react';
import { useNavigate } from "react-router-dom";
import { Link } from 'react-router-dom';
import api from '../../api/axios';
import './QuestionsPage.css';

const QuestiontsPage = ({ onNewQuestionClick }) => {
  const [questions, setQuestions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(1);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    // Проверка авторизации
    const token = localStorage.getItem('accessToken');
    setIsAuthenticated(!!token);

    const fetchQuestions = async () => {
      try {
        setLoading(true);
        const response = await api.get('/Questions', {
          params: {
            PageNumber: pageNumber,
            PageSize: pageSize
          }
        });
        setQuestions(response.data.items);
        setTotalPages(response.data.totalPages);
        setError(null);
      } catch (err) {
        setError(err.response?.data?.message || 'Ошибка загрузки вопросов');
        setQuestions([]);
      } finally {
        setLoading(false);
      }
    };

    fetchQuestions();
  }, [pageNumber, pageSize]);

  const handlePageChange = (newPage) => {
    if (newPage > 0 && newPage <= totalPages) {
      setPageNumber(newPage);
    }
  };

  return (
    <div className="questions-container">
      <div className="questions-header">
        <h1>Форум вопросов и ответов</h1>
        {isAuthenticated && (
          <button 
            className="new-question-button"
            onClick={onNewQuestionClick}
          >
            Задать новый вопрос
          </button>
        )}
      </div>

      {loading ? (
        <div className="loading">Загрузка вопросов...</div>
      ) : error ? (
        <div className="error">Ошибка: {error}</div>
      ) : (
        <>
          <div className="questions-list">
            {questions.map(question => (
              <div key={question.id} className={`question-card ${question.isClosed ? 'closed' : ''}`}>
                <div className="question-content">
                  {question.isClosed && <div className="closed-badge">Закрыт</div>}
                  <h3>{question.title}</h3>
                  <p>{question.description}</p>
                  <div className="question-footer">
                    <span className="author">{question.authorPublicUsername}</span>
                    <span className="date">{new Date(question.dateOfCreation).toLocaleDateString()}</span>
                  </div>
                </div>
                <Link to={`/questions/${question.id}`} className="question-link" aria-label={`Перейти к вопросу: ${question.title}`}></Link>
              </div>
            ))}
          </div>

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
        </>
      )}
    </div>
  );
};


export default QuestiontsPage;