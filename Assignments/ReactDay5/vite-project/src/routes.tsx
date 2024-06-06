import React from 'react';
import { Navigate, RouteObject, useRoutes } from 'react-router-dom';
import RequireGuest from './HOC/RequireGuest';
import Login from './guest/pages/Login';
import NotFound from './guest/pages/NotFound';
import { routeNames } from './routesConstants';
import HomeLayout from './shared/layouts/HomeLayout';
import Home from './shared/pages/Home';
import BookList from './user/pages/BookPages/BookList';
import { CategoryList } from './user/pages/CategoryPages';
import RequireAuth from './HOC/RequireAuth';
import { RoleName, useAuth } from './contexts/AuthContext';
import { Error401, Error403, Error500 } from './shared/pages/ErrorPages';

// const Login = lazy(() => import('./guest/pages/Login'))

const commonRoutes: RouteObject[] = [
    {
        element: <HomeLayout />,
        children: [
            {
                path: routeNames.index,
                element: <Home />
            }
        ]
    },
    {
        path: routeNames.notFound,
        element: <NotFound />
    },
    {
        path: routeNames.unauthorized,
        element: <Error401 />
    },
    {
        path: routeNames.forbidden,
        element: <Error403 />
    },
    {
        path: routeNames.serverError,
        element: <Error500 />
    },
    {
        path: '*',
        element: <Navigate to={routeNames.notFound} replace />,
    },
]

const guestRoutes: RouteObject[] = [
    {
        element: <RequireGuest />,
        children: [
            {
                path: routeNames.login,
                element: <Login />
            }
        ]
    },
    ...commonRoutes
]

const userRoutes: RouteObject[] = [
    ...commonRoutes
]

const adminRoutes: RouteObject[] = [
    {
        element: <RequireAuth allowedRoles={[RoleName.Admin]} />,
        children: [
            {
                element: <HomeLayout />,
                children: [
                    {
                        path: routeNames.bookList,
                        element: <BookList />
                    },
                    {
                        path: routeNames.categoryList,
                        element: <CategoryList />
                    }
                ]
            }
        ]
    },
    ...commonRoutes
]

const AppRouter: React.FC = () => {
    const {user} = useAuth();
    const getRoutes = () :RouteObject[]  => {
        switch (user?.roles[0].name) {
            case RoleName.Admin:
                return adminRoutes;
            case RoleName.User:
                return userRoutes;
            default:
                return guestRoutes;
        }
    }
    const routes = useRoutes(getRoutes());

    return (routes);
};

export default AppRouter;
