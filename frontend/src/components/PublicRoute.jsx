import React from 'react';
import { Navigate, Outlet } from 'react-router';
import { isAuthenticated } from '../services/authService';

const PublicRoute = () => {
  // Kullanıcı giriş yapmışsa Home sayfasına yönlendir
  if (isAuthenticated()) {
    return <Navigate to="/url" replace />;
  }
  
  // Giriş yapmamışsa istenen sayfayı göster
  return <Outlet />;
};

export default PublicRoute;