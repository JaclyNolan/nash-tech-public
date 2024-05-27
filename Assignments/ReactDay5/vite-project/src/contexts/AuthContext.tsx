import { createContext, FC, ReactNode, useContext, useEffect, useState } from "react";

export interface AuthContextType {
    user: User | null,
    setUser: (user: User | null) => void,
    userCredential: UserCredential | null,
    setUserCredential: (userCredential: UserCredential | null) => void,
}

export interface User {
    email: string,
    // role: Role
}

export interface UserCredential {
    tokenType: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}

// enum Role {
//     admin = "admin",
//     user = "user"
// }

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// AuthProvider component to provide AuthContext to children
export const AuthProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(() => {
        // Retrieve user data from localStorage on initial load
        const storedUser = localStorage.getItem('user');
        return storedUser ? JSON.parse(storedUser) : null;
    });

    const [userCredential, setUserCredential] = useState<UserCredential | null>(() => {
        // Retrieve user credential from localStorage on initial load
        const storedUserCredential = localStorage.getItem('userCredential');
        return storedUserCredential ? JSON.parse(storedUserCredential) : null;
    });

    useEffect(() => {
        // Store user data in localStorage whenever it changes
        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
        } else {
            localStorage.removeItem('user');
        }
    }, [user]);

    useEffect(() => {
        // Store user credential in localStorage whenever it changes
        if (userCredential) {
            localStorage.setItem('userCredential', JSON.stringify(userCredential));
        } else {
            localStorage.removeItem('userCredential');
        }
    }, [userCredential]);

    return (
        <AuthContext.Provider value={{ user, setUser, userCredential, setUserCredential }}>
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