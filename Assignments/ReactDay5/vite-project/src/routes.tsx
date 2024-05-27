import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, RouteObject, useRoutes } from 'react-router-dom';
import Login from './guest/pages/Login';
import RequireGuest from './guest/components/RequireGuest';
import HomeLayout from './shared/layouts/HomeLayout';
import Home from './shared/pages/Home';
import NotFound from './guest/pages/NotFound';
import { routeNames } from './routesConstants';

const commonRoutes: RouteObject[] = [
    {
        element: <RequireGuest/>,
        children: [
            {
                path: routeNames.login,
                element: <Login/>
            }
        ]
    },
    {
        element: <HomeLayout/>,
        children: [
            {
                path: routeNames.index,
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
  const routes = useRoutes(commonRoutes);

  return (routes);
};

export default AppRouter;
