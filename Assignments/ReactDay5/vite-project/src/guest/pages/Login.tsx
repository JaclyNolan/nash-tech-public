import React from 'react';
import { Container, Typography, Box, Button } from '@mui/material';
import LoginForm from '../components/LoginForm';
import NoStyleLink from '../../shared/components/NoStyleLink';

// ----------------------------------------------------------------------

const Login: React.FC = () => {
    return (
        <Container maxWidth="sm">
            <NoStyleLink to={"/"}>
                <Button
                    variant="contained"
                    sx={{ mt: 3 }}
                >
                    Home
                </Button>
            </NoStyleLink>
            <Box sx={{ my: 4, textAlign: 'center' }}>
                <Typography variant="h4" gutterBottom>
                    Sign In
                </Typography>
                <Typography sx={{ color: 'text.secondary', mb: 3 }}>
                    Enter your credentials to continue
                </Typography>
                <LoginForm />
            </Box>
        </Container>
    );
};

export default Login;
