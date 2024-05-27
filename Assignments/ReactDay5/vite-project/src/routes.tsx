import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, RouteObject, useRoutes } from 'react-router-dom';
// import Dashboard from './pages/Dashboard';
// import Profile from './pages/Profile';
// import NotFound from './pages/NotFound';
import { useAuth } from './contexts/AuthContext';
import Login from './guest/pages/Login';
import RequireGuest from './guest/components/RequireGuest';
import HomeLayout from './shared/layouts/HomeLayout';
import Home from './shared/pages/Home';
import NotFound from './guest/pages/NotFound';

const commonRoutes: RouteObject[] = [
    {
        element: <RequireGuest/>,
        children: [
            {
                path: "/login",
                element: <Login/>
            }
        ]
    },
    {
        element: <HomeLayout/>,
        children: [
            {
                path: "/",
                element: <Home/>
            }
        ]
    },
    {
        path: '*',
        element: <NotFound />,
    },
]

const AppRouter: React.FC = () => {
  const { user } = useAuth();
  const routes = useRoutes(commonRoutes);

  return (routes);
};

export default AppRouter;
