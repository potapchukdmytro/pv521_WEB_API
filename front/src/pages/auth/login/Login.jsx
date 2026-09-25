import { useState } from "react";
import { api } from "../../../api";
import { useNavigate } from "react-router";
import "./Login.css";

export default function Login() {
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);

    const data = {
      email: formData.get("email"),
      password: formData.get("password"),
    };

    try {
        await api.post("auth/login", data);
        navigate("/");
    } catch (error) {
    }
  };

  return (
    <main className="login-page">
      <div className="login-card">
        <div className="login-logo">
          <div className="login-logo-icon">B</div>
          <span>Bookly</span>
        </div>

        <div className="login-header">
          <h1>Вхід</h1>
          <p>Увійдіть у свій обліковий запис</p>
        </div>

        <form className="login-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email</label>

            <div className="input-wrapper">
              <svg
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
              >
                <rect x="3" y="5" width="18" height="14" rx="2" />
                <path d="m3 7 9 6 9-6" />
              </svg>

              <input
                id="email"
                name="email"
                type="email"
                placeholder="example@gmail.com"
                required
              />
            </div>
          </div>

          <div className="form-group">
            <div className="password-label">
              <label htmlFor="password">Пароль</label>

              <a href="/forgot-password">
                Забули пароль?
              </a>
            </div>

            <div className="input-wrapper">
              <svg
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
              >
                <rect x="3" y="11" width="18" height="10" rx="2" />
                <path d="M7 11V7a5 5 0 0 1 10 0v4" />
              </svg>

              <input
                id="password"
                name="password"
                type={showPassword ? "text" : "password"}
                placeholder="Введіть пароль"
                required
              />

              <button
                type="button"
                className="password-toggle"
                onClick={() => setShowPassword(!showPassword)}
                aria-label="Показати пароль"
              >
                {showPassword ? (
                  <svg
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                  >
                    <path d="m3 3 18 18" />
                    <path d="M10.6 10.6a2 2 0 0 0 2.8 2.8" />
                    <path d="M9.9 4.2A10.5 10.5 0 0 1 12 4c5 0 9 4 10 8a11.7 11.7 0 0 1-2 3.9" />
                    <path d="M6.6 6.6C4.6 8 3.3 10 2 12c1.5 5 5.5 8 10 8a10.6 10.6 0 0 0 4.1-.8" />
                  </svg>
                ) : (
                  <svg
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                  >
                    <path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12Z" />
                    <circle cx="12" cy="12" r="3" />
                  </svg>
                )}
              </button>
            </div>
          </div>

          <label className="remember-me">
            <input type="checkbox" />
            <span>Запам'ятати мене</span>
          </label>

          <button type="submit" className="login-button">
            Увійти
          </button>
        </form>

        <div className="login-divider">
          <span></span>
          <p>або</p>
          <span></span>
        </div>

        <p className="register-text">
          Ще немає акаунта?
          <a href="/register"> Зареєструватися</a>
        </p>
      </div>
    </main>
  );
}