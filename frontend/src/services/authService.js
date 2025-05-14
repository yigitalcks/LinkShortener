import axios from 'axios';

const API_URL = process.env.VITE_API_URL || 'http://localhost:5161/api';

export const login = async (email, password) => {
    const response = await axios.post(`${API_URL}/api/Auth/login`, {
        email,
        password
    });
    const { token } = response.data;
    localStorage.setItem('token', token);
    return token;
};

export const register = async (email, password) => {
    const response = await axios.post(`${API_URL}/api/Auth/register`, {
        email,
        password
    });
    return response.data;
};

export const logout = () => {
    localStorage.removeItem('token');
};

export const getToken = () => {
    return localStorage.getItem('token');
};

export const isAuthenticated = () => {
    return !!getToken();
};
