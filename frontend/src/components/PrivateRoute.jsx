import React from 'react';
import { Navigate, Outlet } from 'react-router';
import { isAuthenticated } from '../services/authService';

const PrivateRoute = () => {
  // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }
  
  // Giriş yapmışsa istenen sayfayı göster
  return <Outlet />;
};

export default PrivateRoute;
