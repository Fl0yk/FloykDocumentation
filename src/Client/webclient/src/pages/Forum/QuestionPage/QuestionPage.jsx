import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../../../api/axios';
import { jwtDecode } from 'jwt-decode';
import './QuestionPage.css';

const QuestionPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [question, setQuestion] = useState(null);
  const [answers, setAnswers] = useState([]);
  const [newAnswer, setNewAnswer] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [editingAnswerId, setEditingAnswerId] = useState(null);
  const [editedAnswerText, setEditedAnswerText] = useState('');

  useEffect(() => {
    // Проверка авторизации
    const token = localStorage.getItem('accessToken');
    setIsAuthenticated(!!token);
    if (token) {
      try {
        const payload = jwtDecode(token);
        const publicUsername = payload?.PublicUsername || payload?.publicusername;
        console.log(payload);
        if (publicUsername) {
          setCurrentUser({
        username: publicUsername
      });
        }
      } catch (err) {
        console.error('Ошибка при декодировании токена:', err);
      }
    }

    // Загрузка вопроса и ответов
    const fetchData = async () => {
      try {
        setIsLoading(true);
        const questionRes = await api.get(`/Questions/${id}`);
        console.log(questionRes.data);
        setQuestion(questionRes.data);
        setAnswers(questionRes.data.answers);
      } catch (err) {
        console.log(err);
        setError(err.response?.data?.message || 'Ошибка загрузки данных');
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleAnswerSubmit = async (e) => {
    e.preventDefault();
    if (!newAnswer.trim()) return;

    try {
      const response = await api.post('/Answers', {
        text: newAnswer,
        questionId: id
      });
      setAnswers([...answers, response.data]);
      setNewAnswer('');
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка при отправке ответа');
    }
  };

  const handleCloseQuestion = async () => {
    try {
      await api.post(`/Questions/close/${id}`);
      setQuestion({ ...question, isClosed: true });
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка при закрытии вопроса');
    }
  };

  const startEditingAnswer = (answer) => {
    setEditingAnswerId(answer.id);
    setEditedAnswerText(answer.text);
  };

  const cancelEditing = () => {
    setEditingAnswerId(null);
    setEditedAnswerText('');
  };

  const saveEditedAnswer = async (answerId) => {
    try {
      await api.put('/Answers', {
        id: answerId,
        text: editedAnswerText
      });
      
      setAnswers(answers.map(answer => 
        answer.id === answerId ? { ...answer, text: editedAnswerText } : answer
      ));
      cancelEditing();
    } catch (err) {
      setError(err.response?.data?.message || 'Ошибка при сохранении изменений');
    }
  };

  if (isLoading) return <div className="loading">Загрузка...</div>;
  if (error) return <div className="error">Ошибка: {error}</div>;
  if (!question) return <div className="not-found">Вопрос не найден</div>;

  const isQuestionAuthor = currentUser && question.authorId === currentUser.id;

  return (
    <div className="question-page">
      <div className="question-header">
        <h1>{question.title}</h1>
        {question.isAuthor && !question.isClosed && (
          <button 
            className="close-question-button"
            onClick={handleCloseQuestion}
          >
            Закрыть вопрос
          </button>
        )}
        {question.isClosed && (
          <span className="closed-badge">Закрыт</span>
        )}
      </div>

      <div className="question-content">
        <div className="question-meta">
          <span>Автор: {question.publicAuthorUsername}</span>
          <span>Дата: {new Date(question.dateOfCreation).toLocaleString()}</span>
        </div>
        <p className="question-text">{question.description}</p>
      </div>

      <div className="answers-section">
        <h2>Ответы ({answers.length})</h2>
        
        {answers.length === 0 ? (
          <p className="no-answers">Пока нет ответов</p>
        ) : (
          <div className="answers-list">
            {answers.map(answer => (
              <div key={answer.id} className="answer-card">
                <div className="answer-meta">
                  <span>{answer.publicAuthorUsername}</span>
                  <span>{new Date(answer.timeOfCreation).toLocaleString()}</span>
                  {answer.isAuthor && (
                    <div className="answer-actions">
                      {editingAnswerId === answer.id ? (
                        <>
                          <button onClick={() => saveEditedAnswer(answer.id)}>Сохранить</button>
                          <button onClick={cancelEditing}>Отмена</button>
                        </>
                      ) : (
                        <button onClick={() => startEditingAnswer(answer)}>Редактировать</button>
                      )}
                    </div>
                  )}
                </div>
                {editingAnswerId === answer.id ? (
                  <textarea
                    value={editedAnswerText}
                    onChange={(e) => setEditedAnswerText(e.target.value)}
                    rows={3}
                  />
                ) : (
                  <p className="answer-text">{answer.text}</p>
                )}
              </div>
            ))}
          </div>
        )}

        {isAuthenticated && !question.isClosed && (
          <form onSubmit={handleAnswerSubmit} className="answer-form">
            <h3>Ваш ответ</h3>
            <textarea
              value={newAnswer}
              onChange={(e) => setNewAnswer(e.target.value)}
              placeholder="Напишите ваш ответ..."
              required
              rows={5}
            />
            <button type="submit" className="submit-answer-button">
              Отправить ответ
            </button>
          </form>
        )}

        {question.isClosed && (
          <p className="closed-message">Вопрос закрыт для новых ответов</p>
        )}
      </div>
    </div>
  );
};

export default QuestionPage;