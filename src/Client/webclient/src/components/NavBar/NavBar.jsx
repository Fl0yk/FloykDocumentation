import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { Link } from 'react-router-dom';
import {
    HOME_ROUTE,
    ARTICLES_ROUTE,
    AUTH_ROUTE,
    FORUM_ROUTE
} from '../../utils/constants';
import React, { useEffect, useState } from 'react';

const NavBar = () => {

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [username, setUsername] = useState('');

    const updateAuthState = () => {
        console.log('update Auth state');
        const storedUsername = localStorage.getItem('username');
        console.log('username ' + storedUsername);
        if (storedUsername) {
            setIsAuthenticated(true);
            setUsername(storedUsername);
        } else {
            setIsAuthenticated(false);
            setUsername('');
        }
    };

    // Проверка состояния при загрузке компонента
    useEffect(() => {
        updateAuthState();
    }, []);

    // Подписка на изменения localStorage
    useEffect(() => {
        const handleStorageChange = () => {
            updateAuthState();
        };

        window.addEventListener('storage', handleStorageChange);

        return () => {
            window.removeEventListener('storage', handleStorageChange);
        };
    }, []);

    // Логика выхода
    const handleLogout = () => {
        localStorage.removeItem('username');
        updateAuthState();
        window.location.href = '/';
    };


    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light fixed-top w-100">
            <div className="container">
                <Link className="navbar-brand" to={HOME_ROUTE}>Knowledge Platform</Link>

                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        <li className="nav-item">
                            <Link className="nav-link" to={ARTICLES_ROUTE}>Статьи</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to={FORUM_ROUTE}>Форум</Link>
                        </li>
                    </ul>

                    <div className="d-none d-lg-flex ms-auto align-items-center">
                    {isAuthenticated ? (
                        <>
                            <span className="navbar-text me-3">Привет, {username}</span>
                            <button className="btn btn-outline-secondary" onClick={handleLogout}>
                                Выйти
                            </button>
                        </>
                    ) : (
                        <>
                            <Link to={AUTH_ROUTE + "?type=login"} className="btn btn-outline-primary me-2">
                                Войти
                            </Link>
                            <Link to={AUTH_ROUTE + "?type=register"} className="btn btn-primary">
                                Регистрация
                            </Link>
                        </>
                    )}
                </div>

                    <ul className="navbar-nav d-lg-none">
                        {isAuthenticated ? 
                        <>
                        <li className="nav-item">
                            <p>{username}</p>
                        </li> 
                        <li className="nav-item">
                            <a className="nav-link" onClick={handleLogout}>Выйти</a>
                        </li>
                        </>
                        :
                        <>
                        <li className="nav-item">
                            <Link className="nav-link" to={AUTH_ROUTE+"?type=login"}>Войти</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to={AUTH_ROUTE+"?type=register"}>Регистрация</Link>
                        </li>
                        </>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default NavBar;