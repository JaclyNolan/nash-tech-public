import React, { useState, ChangeEvent, FormEvent } from 'react';
// import { useNavigate } from 'react-router-dom';
// @mui
import { Link, Stack, IconButton, InputAdornment, TextField, Checkbox, FormControlLabel } from '@mui/material';
import { LoadingButton } from '@mui/lab';
// components
import Iconify from './../../shared/components/iconify';
import { URLConstants } from '../../common/Constants';
import axiosInstance from '../../axiosInstance';
import { useAuth } from '../../contexts/AuthContext';

// ----------------------------------------------------------------------

export interface LoginSchema {
    email: string,
    password: string
}

export interface LoginResponse {
    tokenType: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}

const LoginForm: React.FC = () => {
    const [showPassword, setShowPassword] = useState<boolean>(false);
    const { setUser, setUserCredential } = useAuth();
    const [isFetching, setIsFetching] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setIsFetching(true);
        try {
            const data = new FormData(event.currentTarget);
            const payload = {
                email: data.get('email') as string,
                password: data.get('password') as string,
            };

            const response: LoginResponse = await axiosInstance.post(URLConstants.LOGIN_ENDPOINT, payload);
            setUser({email: payload.email});
            setUserCredential(response);
        } catch (error) {
            setError("Login failed. Please check your credentials");
            console.error(error);
        } finally {
            setIsFetching(false);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <Stack spacing={3}>
                <TextField
                    name="email"
                    label="Email address"
                    value={email}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
                    required
                    autoFocus
                />

                <TextField
                    name="password"
                    label="Password"
                    value={password}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
                    required
                    type={showPassword ? 'text' : 'password'}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">
                                <IconButton onClick={() => setShowPassword(!showPassword)} edge="end">
                                    <Iconify icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'} />
                                </IconButton>
                            </InputAdornment>
                        ),
                    }}
                />
            </Stack>
            <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ my: 2 }}>
                <FormControlLabel
                    value="remember_me"
                    control={<Checkbox />}
                    label="Remember me"
                    labelPlacement="end"
                />
                <Link variant="subtitle2" underline="hover">
                    Forgot password?
                </Link>
            </Stack>

            {error && (
                <div style={{ color: 'red', textAlign: 'center', marginBottom: '10px' }}>{error}</div>
            )}

            <LoadingButton loading={isFetching} fullWidth size="large" type="submit" variant="contained">
                Login
            </LoadingButton>
        </form>
    );
};

export default LoginForm;
