import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import { login } from '../services/authService';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            await login(email, password);
            navigate('/url');
        } catch (error) {
            setError('Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
        }
    };

    return (
        <div className="auth-container">
          <div className="auth-card">
            <h2>Giriş Yap</h2>
            <form onSubmit={handleLogin}>
              <div className="form-group">
                <label htmlFor="email">Email:</label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="Email adresinizi giriniz"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="password">Şifre:</label>
                <input
                  type="password"
                  id="password"
                  name="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder="Şifrenizi giriniz"
                  required
                />
              </div>
              <button type="submit" className="primary-button">Giriş Yap</button>
            </form>
            {error && (
              <div className="error-message">
                <p>{error}</p>
              </div>
            )}
            <div className="auth-link">
              <p>
                Hesabınız yok mu?{' '}
                <a href="/register" onClick={(e) => { e.preventDefault(); navigate('/register'); }}>
                  Kayıt Olun
                </a>
              </p>
            </div>
          </div>
        </div>
    );
};

export default Login;
