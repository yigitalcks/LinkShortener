import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import { logout, getToken } from '../services/authService';
import { createShortLink, BASE_API_URL } from '../services/linkService';
import LinkHistoryPanel from './LinkHistoryPanel';

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
            const payloadUrl = url;
            let payloadCustomKey = null;
            
            if (useCustomUrl && customUrl.trim() !== '') {
                const trimmedCustomUrl = customUrl.trim();
                const regex = /^[A-Za-z0-9_-]+$/;
                if (!regex.test(trimmedCustomUrl)) {
                    setError('Özel URL yalnızca harf, rakam, alt çizgi (_) ve kısa çizgi (-) içerebilir.');
                    return;
                }
                if (trimmedCustomUrl.length < 3 || trimmedCustomUrl.length > 15) {
                    setError('Özel URL en az 3, en fazla 15 karakterden oluşmalıdır.');
                    return;
                }
                payloadCustomKey = trimmedCustomUrl;
            }
            
            const shortCode = await createShortLink(payloadUrl, payloadCustomKey);

            setShortUrl(`${BASE_API_URL}/${shortCode}`);
            setError('');
        } catch (error) {
            setError('URL kısaltma işlemi başarısız oldu. Lütfen tekrar deneyin.');
            console.error('Kısaltma hatası:', error);
            
            setShortUrl('');
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
                    <span className="custom-url-prefix">{`${BASE_API_URL}/`}</span>
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
            
            {/* Link History Panel */}
            <LinkHistoryPanel />
          </div>
        </div>
    );
};

export default Home;
