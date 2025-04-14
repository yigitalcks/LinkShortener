import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import axios from 'axios';
//import api from '../services/api';
import { logout, getToken } from '../services/authService';

const Home = () => {
    const [url, setUrl] = useState('');
    const [shortUrl, setShortUrl] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const token = getToken();
        if (!token) {
            setError('Kullanıcı bilgisi bulunamadı. Lütfen giriş yapın.');
            return;
        }

        try {
            const response = await axios.post(
                'http://localhost:5161/api/Url',
                { url },  
                {
                  headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                  },
                }
              );
            const shortCode = response.data.key
            setShortUrl(`http://localhost:5161/${shortCode}`);
            setError('');
        } catch (error) {
            setError('URL kısaltma işlemi başarısız oldu.');
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div style={{ maxWidth: '500px', margin: '2rem auto', padding: '1rem', border: '1px solid #ccc' }}>
          <h2>URL Kaydet</h2>
          <button onClick={handleLogout} style={{ marginBottom: '1rem' }}>Çıkış Yap</button>
          <form onSubmit={handleSubmit}>
            <div style={{ marginBottom: '1rem' }}>
              <label htmlFor="url">URL:</label>
              <input
                type="text"
                id="url"
                name="url"
                value={url}
                onChange={(e) => setUrl(e.target.value)}
                placeholder="https://www.ornek.com"
                style={{ width: '100%', padding: '0.5rem', marginTop: '0.5rem' }}
                required
              />
            </div>
            <button type="submit" style={{ padding: '0.5rem 1rem' }}>Kaydet</button>
          </form>
          {shortUrl && (
            <div style={{ marginTop: '1rem' }}>
              <h3>Kayıt Başarılı!</h3>
                <a href={shortUrl} target="_blank" rel="noopener noreferrer">
                    {shortUrl}
                </a>
            </div>
          )}
          {error && (
            <div style={{ marginTop: '1rem', color: 'red' }}>
              <h3>Hata:</h3>
              <p>{error}</p>
            </div>
          )}
        </div>
      );
};

export default Home;
