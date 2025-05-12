import axios from 'axios';
import { getToken } from './authService';

const API_URL = 'http://localhost:5161/api/Url';

export const getUserLastLinks = async () => {
  try {
    const token = getToken();
    if (!token) {
      throw new Error('Kullanıcı bilgisi bulunamadı. Lütfen giriş yapın.');
    }

    const response = await axios.get(
      `${API_URL}/history`,
      {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      }
    );
    
    return response.data;
  } catch (error) {
    console.error('Son linkler getirilirken hata oluştu:', error);
    throw error;
  }
}; 