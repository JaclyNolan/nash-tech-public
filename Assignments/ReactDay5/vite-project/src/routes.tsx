import React from 'react';
import { RouteObject, useRoutes } from 'react-router-dom';
import RequireGuest from './guest/components/RequireGuest';
import Login from './guest/pages/Login';
import NotFound from './guest/pages/NotFound';
import { routeNames } from './routesConstants';
import HomeLayout from './shared/layouts/HomeLayout';
import Home from './shared/pages/Home';
import BookList from './user/pages/BookPages/BookList';
import { CategoryList } from './user/pages/CategoryPages';

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
            },
            {
                path: routeNames.categoryList,
                element: <CategoryList/>
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
