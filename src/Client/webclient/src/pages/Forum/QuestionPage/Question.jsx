import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const Question = () => {
    const { id } = useParams();
    const [question, setQuestion] = useState(null);
    const [answers, setAnswers] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [comment, setComment] = useState('');

    useEffect(() => {
        if(localStorage.getItem('username'))
        {
            setIsAuthenticated(true);
        }

        fetchQuestion();
    }, [id]);

    const handleCommentSubmit = (e) => {
        e.preventDefault();

        const newComment = {
            text: comment,
            questionId: id
        };

        fetch(`http://localhost:9001/api/Answers`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            },
            body: JSON.stringify(newComment),
        })
            .then(response => response.json())
            .then(data => {
                fetchQuestion();
                setComment('');
            })
            .catch(error => console.error('Ошибка добавления комментария:', error));
    };

    const fetchQuestion = () => {
         // Загрузка данных вопроса из API
         fetch(`http://localhost:9001/api/Questions/${id}`)
         .then(response => response.json())
         .then(data => {
             setQuestion(data);
             setAnswers(data.answers);
             console.log(data.answers);
         })
         .catch(error => console.error('Ошибка загрузки вопроса:', error));
    };

    if (!question) {
        return <div className="container mt-5">Загрузка...</div>;
    }

    const renderAnswers = (answers, level = 0) => {
        return answers.map(answer => (
            <div key={answer.id} style={{ marginLeft: `${level * 20}px` }} className="mt-3">
                <div className="card">
                    <div className="card-body">
                        <p>{answer.text}</p>
                        <small>Автор: {answer.authorId}</small>
                    </div>
                </div>
                {answer.childrens && renderAnswers(answer.childrens, level + 1)}
            </div>
        ));
    };

    return (
        <div className="container mt-5">
            <h1>{question.title}</h1>
            <p>{question.description}</p>
            <small>Создано: {new Date(question.dateOfCreation).toLocaleDateString()}</small>

            <hr />

            <h2>Ответы</h2>
            {answers.length > 0 ? (
                renderAnswers(answers)
            ) : (
                <p>Ответов пока нет</p>
            )}
            <hr />
            {isAuthenticated && (
                <div className="mb-4">
                    <h2>Добавить комментарий</h2>
                    <form onSubmit={handleCommentSubmit}>
                        <div className="mb-3">
                            <label htmlFor="comment" className="form-label">Комментарий</label>
                            <textarea
                                className="form-control"
                                id="comment"
                                rows="3"
                                value={comment}
                                onChange={(e) => setComment(e.target.value)}
                                required
                            ></textarea>
                        </div>
                        <button type="submit" className="btn btn-primary">Добавить</button>
                    </form>
                </div>
            )}
        </div>

        
    );
};

export default Question;