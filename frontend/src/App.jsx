import React from 'react';
import { Routes, Route, Navigate } from 'react-router';
import Login from './components/Login';
import Home from './components/Home';
import PrivateRoute from './components/PrivateRoute';

function App() {
    return (
      <div className="App">
        <h1 style={{ textAlign: 'center' }}>URL Kısaltıcı Uygulaması</h1>
        <Routes>
          {/* Eğer token varsa ana sayfada /url'e yönlendir, yoksa /login */}
          <Route
            path="/"
            element={
              localStorage.getItem('token') ? (
                <Navigate replace to="/url" />
              ) : (
                <Navigate replace to="/login" />
              )
            }
          />
          <Route path="/login" element={<Login />} />
          {/* PrivateRoute, altındaki route’lara erişimi kontrol eder */}
          <Route element={<PrivateRoute />}>
            <Route path="/url" element={<Home />} />
          </Route>
        </Routes>
      </div>
    );
  }

export default App;
