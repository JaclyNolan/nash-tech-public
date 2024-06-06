import { createContext, FC, ReactNode, useContext, useEffect, useState } from "react";
import axiosInstance from "../axiosInstance";
import { LocalStorageConstants, URLConstants } from "../common/constants";
import { useNavigate } from "react-router-dom";

export interface AuthContextType {
    user: User | null,
    userCredential: UserCredential | null,
    setUserCredential: (userCredential: UserCredential | null) => void,
}

export interface User {
    id: string,
    email: string,
    name: string,
    roles: Role[]
}

export interface Role {
    id: string
    name: RoleName,
    description: string | null
}

export enum RoleName {
    Admin = "Admin",
    User = "User"
}

export interface UserCredential {
    tokenType: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// AuthProvider component to provide AuthContext to children
export const AuthProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [userCredential, _setUserCredential] = useState<UserCredential | null>(() => {
        // Retrieve user credential from localStorage on initial load
        const storedUserCredential = localStorage.getItem(LocalStorageConstants.USER_CREDENTIAL);
        return storedUserCredential ? JSON.parse(storedUserCredential) : null;
    });
    const [isUserFetching, setIsUserFetching] = useState<boolean>(false);
    const navigate = useNavigate();

    const setUserCredential = (userCredential: UserCredential | null) => {
        // Store user credential in localStorage whenever it changes
        _setUserCredential(userCredential);
        if (userCredential) {
            localStorage.setItem(LocalStorageConstants.USER_CREDENTIAL, JSON.stringify(userCredential));
        } else {
            localStorage.removeItem(LocalStorageConstants.USER_CREDENTIAL);
            setUser(null);
        }
    }

    useEffect(() => {
        console.log(user, userCredential);
        (async () => {
            if (userCredential && user === null) {
            setIsUserFetching(true);
                await axiosInstance.get(URLConstants.ACCOUNT_INFO_ENDPOINT)
                    .then((response) => {
                        // Store the fetched user data in AuthContext
                        setUser(response.data as User);
                    })
                    .catch((error) => {
                        console.log(error);
                        setUser(null);
                        setUserCredential(null);
                        return navigate('/login');
                    })
            }
            setIsUserFetching(false)
        })()
    }, [userCredential, user])

    if (isUserFetching) return (<p>Loading...</p>)

    return (
        <AuthContext.Provider value={{ user, userCredential, setUserCredential }}>
            {children}
        </AuthContext.Provider>
    );
};

// Custom hook to use the AuthContext
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};