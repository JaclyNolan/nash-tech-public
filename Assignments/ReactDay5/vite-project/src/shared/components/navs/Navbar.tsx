import { AppBar, Avatar, Box, Button, Container, Divider, IconButton, Menu, MenuItem, Toolbar, Tooltip, Typography } from "@mui/material";
import { FC, useState } from "react";
import { RoleName, useAuth } from "../../../contexts/AuthContext";
import NoStyleLink from "../NoStyleLink";
import { useNavigate } from "react-router-dom";
import { routeNames } from "../../../routesConstants";
import { adminNavItem, guestNavItem, NavItem, userNavItem } from "./navItems";
import Logo from "../logo";
import Iconify from "../iconify";

const settings = ['Profile', 'Account', 'Dashboard'];

const Navbar: FC = () => {
    const navigate = useNavigate();
    const { user, setUserCredential } = useAuth();
    const [anchorElNav, setAnchorElNav] = useState<null | HTMLElement>(null);
    const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

    const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    const handleLogout = () => {
        setUserCredential(null)
        return navigate(routeNames.login)
    }

    const renderAvatar = () => {
        if (user) {
            return (<Box sx={{ flexGrow: 0 }}>
                <Tooltip title="Open settings">
                    <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                        <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg" />
                    </IconButton>
                </Tooltip>
                <Menu
                    sx={{ mt: '45px' }}
                    id="menu-appbar"
                    anchorEl={anchorElUser}
                    anchorOrigin={{
                        vertical: 'top',
                        horizontal: 'right',
                    }}
                    keepMounted
                    transformOrigin={{
                        vertical: 'top',
                        horizontal: 'right',
                    }}
                    open={Boolean(anchorElUser)}
                    onClose={handleCloseUserMenu}
                >
                    <MenuItem disableRipple>
                        <Typography textAlign="center">{user.email}</Typography>
                    </MenuItem>

                    <Divider variant="middle" component="menu" />
                    {settings.map((setting) => (
                        <MenuItem key={setting} onClick={handleCloseUserMenu}>
                            <Typography textAlign="center">{setting}</Typography>
                        </MenuItem>
                    ))}
                    <MenuItem key="Logout" onClick={handleLogout}>
                        <Typography textAlign="center">Logout</Typography>
                    </MenuItem>
                </Menu>
            </Box>)
        }

        return (
            <Box sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
                <NoStyleLink to={routeNames.login} style={{ textDecoration: 'none' }}>
                    <Button sx={{ my: 2, color: 'white', display: 'block' }}>
                        Login
                    </Button>
                </NoStyleLink>

                <NoStyleLink to={"/register"} style={{ textDecoration: 'none' }}>
                    <Button sx={{ my: 2, color: 'white', display: 'block' }}>
                        Register
                    </Button>
                </NoStyleLink>
            </Box>
        )
    }

    const pages = (): NavItem[] => {
        switch (user?.roles[0].name) {
            case RoleName.Admin:
                return adminNavItem;
            case RoleName.User:
                return userNavItem;
            default:
                return guestNavItem;
        }
    }

    return (
        <AppBar position="static">
            <Container maxWidth="xl">
                <Toolbar disableGutters>
                    <Logo sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} />
                    <Typography
                        variant="h6"
                        noWrap
                        component="a"
                        href="#app-bar-with-responsive-menu"
                        sx={{
                            mr: 2,
                            display: { xs: 'none', md: 'flex' },
                            fontFamily: 'monospace',
                            fontWeight: 700,
                            letterSpacing: '.3rem',
                            color: 'inherit',
                            textDecoration: 'none',
                        }}
                    >
                        LOGO
                    </Typography>

                    <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
                        <IconButton
                            size="large"
                            aria-label="account of current user"
                            aria-controls="menu-appbar"
                            aria-haspopup="true"
                            onClick={handleOpenNavMenu}
                            color="inherit"
                        >
                            <Iconify icon={'eva:menu-outline'} />
                        </IconButton>
                        <Menu
                            id="menu-appbar"
                            anchorEl={anchorElNav}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left',
                            }}
                            keepMounted
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            open={Boolean(anchorElNav)}
                            onClose={handleCloseNavMenu}
                            sx={{
                                display: { xs: 'block', md: 'none' },
                            }}
                        >
                            {pages().map((page) => (
                                <NoStyleLink key={page.name} to={page.to}>
                                    <MenuItem onClick={handleCloseNavMenu}>
                                        <Typography color="black" textAlign="center">{page.name}</Typography>
                                    </MenuItem>
                                </NoStyleLink>
                            ))}
                            {!user &&

                                <Box sx={{ flexGrow: 0, display: { xs: 'block', md: 'none' } }}>
                                    <Divider variant="middle" component="menu" />
                                    <NoStyleLink to={routeNames.login}>
                                        <MenuItem>
                                            <Typography color="black" textAlign="center">Login</Typography>

                                        </MenuItem>
                                    </NoStyleLink>

                                    <NoStyleLink to={"/register"}>
                                        <MenuItem>
                                            <Typography color="black" textAlign="center">Register</Typography>
                                        </MenuItem>
                                    </NoStyleLink>
                                </Box>}
                        </Menu>
                    </Box>
                    <Logo sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} />
                    <Typography
                        variant="h5"
                        noWrap
                        component="a"
                        href="#app-bar-with-responsive-menu"
                        sx={{
                            mr: 2,
                            display: { xs: 'flex', md: 'none' },
                            flexGrow: 1,
                            fontFamily: 'monospace',
                            fontWeight: 700,
                            letterSpacing: '.3rem',
                            color: 'inherit',
                            textDecoration: 'none',
                        }}
                    >
                        LOGO
                    </Typography>
                    <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
                        {pages().map((page) => (
                            <NoStyleLink key={page.name} to={page.to}>
                                <Button
                                    onClick={handleCloseNavMenu}
                                    sx={{ my: 2, color: 'white', display: 'block' }}
                                >
                                    {page.name}
                                </Button>
                            </NoStyleLink>
                        ))}
                    </Box>
                    {renderAvatar()}
                </Toolbar>
            </Container>
        </AppBar>
    )
}

export default Navbar;