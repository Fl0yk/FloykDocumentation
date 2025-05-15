import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import api from "../../api/axios";
import './ConfirmPage.css';

export default function ConfirmPage() {
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState({
    loading: true,
    message: "Проверка ссылки подтверждения...",
    isSuccess: false
  });
  const navigate = useNavigate();

  useEffect(() => {
    const userId = searchParams.get("userId");
    const token = searchParams.get("token");

    if (!userId || !token) {
      setStatus({
        loading: false,
        message: "Неверная ссылка подтверждения. Пожалуйста, проверьте письмо еще раз.",
        isSuccess: false
      });
      return;
    }

    api.get("Identity/registration/confirm", {
      params: { userId, token }
    })
      .then((response) => {
        localStorage.setItem("accessToken", response.data.jwtToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        localStorage.setItem("publicUsername", response.data.publicUsername);
        
        window.dispatchEvent(new Event('storage'));

        setStatus({
          loading: false,
          message: "Регистрация успешно подтверждена!",
          isSuccess: true
        });
        setTimeout(() => {
          navigate("/", { replace: true });
        }, 3000);
      })
      .catch((err) => {
        setStatus({
          loading: false,
          message: err.response?.data?.message || "Ошибка подтверждения. Ссылка недействительна или уже использована.",
          isSuccess: false
        });
      });
  }, [searchParams, navigate]);

  return (
    <div className="confirm-container">
      <div className="confirm-card">
        <h2 className="confirm-title">
          {status.isSuccess ? 'Успешное подтверждение!' : 'Подтверждение регистрации'}
        </h2>
        
        <div className={`status-message ${status.isSuccess ? 'success' : 'error'}`}>
          {status.loading ? (
            <div className="loading-spinner"></div>
          ) : (
            <p>{status.message}</p>
          )}
        </div>

        {status.isSuccess && (
          <p className="redirect-message">Вы будете перенаправлены на главную страницу...</p>
        )}

        {!status.loading && !status.isSuccess && (
          <button 
            className="confirm-button"
            onClick={() => navigate('/register')}
          >
            Попробовать снова
          </button>
        )}
      </div>
    </div>
  );
}