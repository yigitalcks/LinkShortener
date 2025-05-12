import axios from 'axios';
import { getToken } from './authService';

export const BASE_API_URL = process.env.VITE_API_URL || 'http://localhost:5161';

export const getUserLastLinks = async () => {
  try {
    const token = getToken();
    if (!token) {
      throw new Error('Kullanıcı bilgisi bulunamadı. Lütfen giriş yapın.');
    }

    const response = await axios.get(
      `${BASE_API_URL}/api/Url/history`,
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

export const createShortLink = async (originalUrl, customKey = null) => {
  try {
    const token = getToken();
    if (!token) {
      throw new Error('Kullanıcı bilgisi bulunamadı. Lütfen giriş yapın.');
    }

    const payload = { url: originalUrl };
    if (customKey) {
      payload.customKey = customKey;
    }

    const response = await axios.post(
      `${BASE_API_URL}/api/Url`, // Endpoint for creating links
      payload,
      {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      }
    );
    
    // Assuming the backend returns the key directly in response.data or response.data.key
    // Adjust based on your actual backend response structure
    return response.data.key || response.data; 
  } catch (error) {
    console.error('Link oluşturulurken hata oluştu:', error);
    // Re-throw the error so the component can handle it (e.g., show error messages)
    throw error; 
  }
}; 
