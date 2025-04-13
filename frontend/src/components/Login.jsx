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
            navigate('/');
        } catch (error) {
            setError('Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
        }
    };

    return (
        <div style={{ maxWidth: '400px', margin: '2rem auto', padding: '1rem', border: '1px solid #ccc' }}>
          <h2>Giriş Yap</h2>
          <form onSubmit={handleLogin}>
            <div style={{ marginBottom: '1rem' }}>
              <label htmlFor="email">Email:</label>
              <input
                type="email"
                id="email"
                name="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Email adresinizi giriniz"
                style={{ width: '100%', padding: '0.5rem', marginTop: '0.5rem' }}
                required
              />
            </div>
            <div style={{ marginBottom: '1rem' }}>
              <label htmlFor="password">Şifre:</label>
              <input
                type="password"
                id="password"
                name="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Şifrenizi giriniz"
                style={{ width: '100%', padding: '0.5rem', marginTop: '0.5rem' }}
                required
              />
            </div>
            <button type="submit" style={{ padding: '0.5rem 1rem' }}>Giriş Yap</button>
          </form>
          {error && (
            <div style={{ marginTop: '1rem', color: 'red' }}>
              <p>{error}</p>
            </div>
          )}
        </div>
      );
};

export default Login;
