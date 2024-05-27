import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const RequireGuest = () => {
    const { user } = useAuth();
    return (
        !user
            ? <Outlet />
            : <Navigate to="/" replace />
    )
}

export default RequireGuest;