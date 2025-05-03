import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import { register } from '../services/authService';

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        
        if (password !== confirmPassword) {
            setError('Şifreler eşleşmiyor.');
            return;
        }
        
        try {
            await register(email, password);
            navigate('/login');
        } catch (error) {
            setError('Kayıt işlemi başarısız oldu. Lütfen bilgilerinizi kontrol edin.');
        }
    };

    return (
        <div className="auth-container">
          <div className="auth-card">
            <h2>Hesap Oluştur</h2>
            <form onSubmit={handleRegister}>
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
              <div className="form-group">
                <label htmlFor="confirmPassword">Şifre Tekrar:</label>
                <input
                  type="password"
                  id="confirmPassword"
                  name="confirmPassword"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  placeholder="Şifrenizi tekrar giriniz"
                  required
                />
              </div>
              <button type="submit" className="primary-button">Kayıt Ol</button>
            </form>
            {error && (
              <div className="error-message">
                <p>{error}</p>
              </div>
            )}
            <div className="auth-link">
              <p>
                Zaten hesabınız var mı?{' '}
                <a href="/login" onClick={(e) => { e.preventDefault(); navigate('/login'); }}>
                  Giriş Yapın
                </a>
              </p>
            </div>
          </div>
        </div>
    );
};

export default Register;