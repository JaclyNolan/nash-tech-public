import { Navigate, Outlet } from 'react-router-dom';
import { RoleName, useAuth, User } from '../contexts/AuthContext';
import { routeNames } from '../routesConstants';

interface RequireAuthProps {
    allowedRoles?: RoleName[]
}

const RequireAuth = ({ allowedRoles = [] }: RequireAuthProps) => {
    const { user } = useAuth();

    const redirect = (user: User | null) => {
        if (user) return <Navigate to={routeNames.unauthorized} replace />
        return <Navigate to={routeNames.login} replace />
    }

    return (
        (user?.roles[0] && allowedRoles.includes(user.roles[0].name))
            ? <Outlet />
            : redirect(user)
    )
}

export default RequireAuth;