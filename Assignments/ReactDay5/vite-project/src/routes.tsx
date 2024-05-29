import React, { lazy } from 'react';
import { RouteObject, useRoutes } from 'react-router-dom';
import Login from './guest/pages/Login';
import RequireGuest from './guest/components/RequireGuest';
import HomeLayout from './shared/layouts/HomeLayout';
import Home from './shared/pages/Home';
import NotFound from './guest/pages/NotFound';
import { routeNames } from './routesConstants';
import BookList from './user/pages/BookPages/BookList';

// const Login = lazy(() => import('./guest/pages/Login'))

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
            },
            {
                path: routeNames.bookList,
                element: <BookList/>
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
