import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import axios from 'axios';
import { logout, getToken } from '../services/authService';

const Home = () => {
    const [url, setUrl] = useState('');
    const [customUrl, setCustomUrl] = useState('');
    const [useCustomUrl, setUseCustomUrl] = useState(false);
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
            const payload = { url };
            
            // Eğer custom URL kullanımı aktifse ve bir değer girilmişse ekle
            if (useCustomUrl && customUrl.trim() !== '') {
                const trimmedCustomUrl = customUrl.trim();

                // Regex kontrolü
                const regex = /^[A-Za-z0-9_-]+$/;
                if (!regex.test(trimmedCustomUrl)) {
                    setError('Özel URL yalnızca harf, rakam, alt çizgi (_) ve kısa çizgi (-) içerebilir.');
                    return;
                }

                // Uzunluk kontrolü
                if (trimmedCustomUrl.length < 3 || trimmedCustomUrl.length > 15) {
                    setError('Özel URL en az 3, en fazla 15 karakterden oluşmalıdır.');
                    return;
                }

                payload.customKey = trimmedCustomUrl;
            }
            
            const response = await axios.post(
                'http://localhost:5161/api/Url',
                payload,  
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
            if (error.response && error.response.status === 400) {
                setError('Bu custom URL zaten kullanılıyor veya geçerli değil.');
            } else {
                setError('URL kısaltma işlemi başarısız oldu.');
            }
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div className="auth-container">
          <div className="auth-card">
            <div className="header-with-button">
              <h2>URL Kaydet</h2>
              <button onClick={handleLogout} className="secondary-button">Çıkış Yap</button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label htmlFor="url">URL:</label>
                <input
                  type="text"
                  id="url"
                  name="url"
                  value={url}
                  onChange={(e) => setUrl(e.target.value)}
                  placeholder="https://www.ornek.com"
                  required
                />
              </div>
              
              <div className="form-group checkbox-group">
                <input
                  type="checkbox"
                  id="useCustomUrl"
                  checked={useCustomUrl}
                  onChange={() => setUseCustomUrl(!useCustomUrl)}
                />
                <label htmlFor="useCustomUrl">Özel URL kullan</label>
              </div>
              
              {useCustomUrl && (
                <div className="form-group">
                  <label htmlFor="customUrl">Özel URL:</label>
                  <div className="custom-url-input">
                    <span className="custom-url-prefix">http://localhost:5161/</span>
                    <input
                      type="text"
                      id="customUrl"
                      name="customUrl"
                      value={customUrl}
                      onChange={(e) => setCustomUrl(e.target.value)}
                      placeholder="ozel-url"
                    />
                  </div>
                </div>
              )}
              
              <button type="submit" className="primary-button">Kaydet</button>
            </form>
            {shortUrl && (
              <div className="success-message">
                <h3>Kayıt Başarılı!</h3>
                <a href={shortUrl} target="_blank" rel="noopener noreferrer">
                  {shortUrl}
                </a>
              </div>
            )}
            {error && (
              <div className="error-message">
                <h3>Hata:</h3>
                <p>{error}</p>
              </div>
            )}
          </div>
        </div>
    );
};

export default Home;
