import Login from './components/Login';
import Register from './components/Registation';
import { useLocation } from 'react-router-dom';

const Auth = () => {
  const location = useLocation();
  const isLogin = new URLSearchParams(location.search).get('type') === 'login';

  return isLogin ? <Login /> : <Register />;
};

export default Auth;