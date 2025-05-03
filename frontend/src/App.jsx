import React from 'react';
import { Routes, Route, Navigate } from 'react-router';
import Login from './components/Login';
import Register from './components/Register';
import Home from './components/Home';
import PrivateRoute from './components/PrivateRoute';
import PublicRoute from './components/PublicRoute';
import { isAuthenticated } from './services/authService';

function App() {
  return (
    <div className="App">
      <h1>URL Kısaltıcı Uygulaması</h1>
      <Routes>
        {/* Ana sayfa yönlendirmesi */}
        <Route
          path="/"
          element={
            isAuthenticated() ? (
              <Navigate replace to="/url" />
            ) : (
              <Navigate replace to="/login" />
            )
          }
        />

        {/* Public Routes - sadece giriş yapmamış kullanıcılar için */}
        <Route element={<PublicRoute />}>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>

        {/* Private Routes - sadece giriş yapmış kullanıcılar için */}
        <Route element={<PrivateRoute />}>
          <Route path="/url" element={<Home />} />
        </Route>
      </Routes>
    </div>
  );
}

export default App;
