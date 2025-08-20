import React from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  IconButton,
  Menu,
  MenuItem,
} from '@mui/material';
import {
  AccountCircle,
  LibraryBooks,
  School,
  Assignment,
} from '@mui/icons-material';
import { useAuth } from '../context/AuthContext';
import { useNavigate, useLocation } from 'react-router-dom';

const Navigation: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout();
    handleMenuClose();
    navigate('/login');
  };

  const isActive = (path: string) => location.pathname === path;

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 0, mr: 4 }}>
          WCS Course Creator
        </Typography>
        
        <Box sx={{ flexGrow: 1, display: 'flex', alignItems: 'center' }}>
          <Button
            color="inherit"
            startIcon={<LibraryBooks />}
            onClick={() => navigate('/patterns')}
            sx={{ 
              mr: 2,
              backgroundColor: isActive('/patterns') ? 'rgba(255,255,255,0.1)' : 'transparent'
            }}
          >
            Pattern Library
          </Button>
          
          <Button
            color="inherit"
            startIcon={<Assignment />}
            onClick={() => navigate('/lessons')}
            sx={{ 
              mr: 2,
              backgroundColor: isActive('/lessons') ? 'rgba(255,255,255,0.1)' : 'transparent'
            }}
            disabled
          >
            Lessons
          </Button>
          
          <Button
            color="inherit"
            startIcon={<School />}
            onClick={() => navigate('/courses')}
            sx={{ 
              backgroundColor: isActive('/courses') ? 'rgba(255,255,255,0.1)' : 'transparent'
            }}
            disabled
          >
            Courses
          </Button>
        </Box>

        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Typography variant="body2" sx={{ mr: 2 }}>
            Welcome, {user?.name}
          </Typography>
          <IconButton
            size="large"
            edge="end"
            aria-label="account of current user"
            aria-controls="primary-search-account-menu"
            aria-haspopup="true"
            onClick={handleProfileMenuOpen}
            color="inherit"
          >
            <AccountCircle />
          </IconButton>
        </Box>
        
        <Menu
          id="primary-search-account-menu"
          anchorEl={anchorEl}
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
          keepMounted
          transformOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
          open={Boolean(anchorEl)}
          onClose={handleMenuClose}
        >
          <MenuItem onClick={handleMenuClose}>Profile</MenuItem>
          <MenuItem onClick={handleLogout}>Logout</MenuItem>
        </Menu>
      </Toolbar>
    </AppBar>
  );
};

export default Navigation;