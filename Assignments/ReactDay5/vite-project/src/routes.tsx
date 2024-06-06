import React from 'react';
import { Navigate, RouteObject, useRoutes } from 'react-router-dom';
import RequireAuth from './HOC/RequireAuth';
import RequireGuest from './HOC/RequireGuest';
import { RoleName, useAuth } from './contexts/AuthContext';
import Login from './guest/pages/Login';
import { routeNames } from './routesConstants';
import HomeLayout from './shared/layouts/HomeLayout';
import SimpleLayout from './shared/layouts/SimpleLayout';
import { Error401, Error403, Error404, Error500 } from './shared/pages/ErrorPages';
import Home from './shared/pages/Home';
import BookList from './admin/pages/BookPages/BookList';
import { CategoryList } from './admin/pages/CategoryPages';

// const Login = lazy(() => import('./guest/pages/Login'))

const commonRoutes: RouteObject[] = [
    {
        element: <RequireGuest />,
        children: [
            {
                path: routeNames.login,
                element: <Login />
            }
        ]
    },
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
        element: <SimpleLayout/>,
        children: [
            {
                path: routeNames.notFound,
                element: <Error404 />
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
        ]
    },
    {
        path: '*',
        element: <Navigate to={routeNames.notFound} replace />,
    },
]

const guestRoutes: RouteObject[] = [
    {
        element: <RequireAuth />,
        children: [
            {
                path: routeNames.index,
                element: <Navigate to={routeNames.login} replace/>
            },
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
    const { user } = useAuth();
    const getRoutes = (): RouteObject[] => {
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
