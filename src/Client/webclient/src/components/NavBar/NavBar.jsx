import { Link } from 'react-router-dom';
import {
  HOME_ROUTE,
  ARTICLES_ROUTE,
  LOGIN_ROUTE,
  REGISTER_ROUTE,
  FORUM_ROUTE
} from '../../utils/constants';
import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import './NavBar.css';

const NavBar = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState('');
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const updateAuthState = () => {
    const token = localStorage.getItem('accessToken');
    
    if (token) {
      try {
        const payload = jwtDecode(token);
        const publicUsername = payload?.PublicUsername || payload?.publicusername;
        
        if (publicUsername) {
          setIsAuthenticated(true);
          setUsername(publicUsername);
          return;
        }
      } catch (err) {
        console.error('Ошибка при декодировании токена:', err);
      }
    }

    setIsAuthenticated(false);
    setUsername('');
  };

  useEffect(() => {
    updateAuthState();

    const handleStorageChange = () => {
      updateAuthState();
    };

    window.addEventListener('storage', handleStorageChange);
    return () => {
      window.removeEventListener('storage', handleStorageChange);
    };
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    updateAuthState();
    window.location.href = '/';
  };

  const toggleMobileMenu = () => {
    setMobileMenuOpen(!mobileMenuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link className="navbar-brand" to={HOME_ROUTE}>Knowledge Platform</Link>

        <button 
          className="navbar-toggle" 
          onClick={toggleMobileMenu}
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggle-icon"></span>
          <span className="navbar-toggle-icon"></span>
          <span className="navbar-toggle-icon"></span>
        </button>

        <div className={`navbar-menu ${mobileMenuOpen ? 'open' : ''}`}>
          <ul className="navbar-links">
            <li className="navbar-item">
              <Link className="navbar-link" to={ARTICLES_ROUTE}>Статьи</Link>
            </li>
            <li className="navbar-item">
              <Link className="navbar-link" to={FORUM_ROUTE}>Форум</Link>
            </li>
          </ul>

          <div className="navbar-auth">
            {isAuthenticated ? (
              <>
                <span className="navbar-username">Привет, {username}</span>
                <button className="navbar-button logout" onClick={handleLogout}>
                  Выйти
                </button>
              </>
            ) : (
              <>
                <Link to={LOGIN_ROUTE} className="navbar-button login">
                  Войти
                </Link>
                <Link to={REGISTER_ROUTE} className="navbar-button register">
                  Регистрация
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default NavBar;