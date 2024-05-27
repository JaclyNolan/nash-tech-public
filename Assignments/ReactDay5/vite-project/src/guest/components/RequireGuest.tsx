import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const RequireGuest = () => {
    const { user } = useAuth();

    console.log(user);

    return (
        !user
            ? <Outlet />
            : <Navigate to="/" replace />
    )
}

export default RequireGuest;