import React, { useState } from 'react';
import { getUserLastLinks, BASE_API_URL } from '../services/linkService';
import { convertUtcToLocalFormattedString } from '../utils/dateUtils';
import '../styles/LinkHistoryPanel.css';

const LinkHistoryPanel = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [links, setLinks] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const togglePanel = () => {
    setIsOpen(!isOpen);
    if (!isOpen && links.length === 0) {
      fetchLinks();
    }
  };

  const fetchLinks = async () => {
    setIsLoading(true);
    setError('');
    try {
      const data = await getUserLastLinks();
      setLinks(data);
    } catch (err) {
      setError('Linkler yüklenirken bir hata oluştu.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="link-history-container">
      <button 
        className="link-history-toggle"
        onClick={togglePanel}
      >
        {isOpen ? 'Son Linklerimi Gizle' : 'Son Linklerimi Göster'}
      </button>
      
      {isOpen && (
        <div className="link-history-panel">
          <h3>Son 10 Linkiniz</h3>
          <button onClick={fetchLinks} className="refresh-button">Güncelle</button>
          
          {isLoading && <div className="loading">Yükleniyor...</div>}
          
          {error && <div className="error">{error}</div>}
          
          {!isLoading && !error && links.length === 0 && (
            <div className="no-links">Henüz kaydedilmiş link bulunamadı.</div>
          )}
          
          {!isLoading && !error && links.length > 0 && (
            <ul className="link-list">
              {links.map((link) => (
                <li key={link.id} className="link-item">
                  <div className="link-details">
                    <div className="link-short">
                      <span className="short-label">Kısa Link:</span>
                      <a 
                        href={`${BASE_API_URL}/${link.key}`} 
                        target="_blank" 
                        rel="noopener noreferrer"
                      >
                        {`${BASE_API_URL}/${link.key}`}
                      </a>
                    </div>
                    <div className="link-date">
                      <span className="date-label">Oluşturulma:</span> 
                      {convertUtcToLocalFormattedString(link.createdAt)}
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default LinkHistoryPanel;