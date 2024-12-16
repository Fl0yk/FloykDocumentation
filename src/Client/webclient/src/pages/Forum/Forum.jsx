import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

const Forum = () => {
    const [questions, setQuestions] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');

    useEffect(() => {
        // Проверка авторизации пользователя (пример для симуляции)
        if(localStorage.getItem('username'))
        {
            setIsAuthenticated(true);
        }

        fetchQuestions();
    }, []);


    const handleSubmit = (e) => {
        e.preventDefault();

        const newQuestion = {
            title,
            description,
            dateOfCreation: new Date().toISOString(),
        };

        fetch('http://localhost:9001/api/Questions', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            },
            body: JSON.stringify(newQuestion),
        })
            .then(response => response.json())
            .then(data => {
                fetchQuestions();
            })
            .catch(error => console.error('Ошибка добавления вопроса:', error));
    };

    const fetchQuestions = () => {
        fetch('http://localhost:9001/api/Questions?pagesize=100&pagenumber=1')
        .then(response => {
            console.log(response);
            if (!response.ok) {
                console.log('response not Ok ');
                throw new Error(`Ошибка сервера: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => { 
            console.log(data);
            setQuestions(data.items);
        })
        .catch(error => {
            console.error('Ошибка загрузки вопросов:', error);
            setError(error.message);
        });
    };

    return (
        <div className="container mt-5">
            <h1>Форум</h1>

            {isAuthenticated && (
                <div className="mb-4">
                    <h2>Добавить вопрос</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="title" className="form-label">Заголовок</label>
                            <input
                                type="text"
                                className="form-control"
                                id="title"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                                required
                            />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="description" className="form-label">Описание</label>
                            <textarea
                                className="form-control"
                                id="description"
                                rows="3"
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                required
                            ></textarea>
                        </div>
                        <button type="submit" className="btn btn-primary">Добавить</button>
                    </form>
                </div>
            )}

            <ul className="list-group">
                {questions.map(question => (
                    <li key={question.id} className="list-group-item">
                        <Link to={`/question/${question.id}`} className="text-decoration-none">
                            <h5>{question.title}</h5>
                            <p>{question.description}</p>
                            <small>Создано: {new Date(question.dateOfCreation).toLocaleDateString()}</small>
                        </Link>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Forum;