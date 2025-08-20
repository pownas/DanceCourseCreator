import React from 'react';
import {
  Container,
  Typography,
  Box,
  Grid,
  Card,
  CardContent,
  CardActions,
  Button,
  Chip,
} from '@mui/material';
import {
  LibraryBooks,
  Assignment,
  School,
  Add as AddIcon,
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const { user } = useAuth();

  const quickActions = [
    {
      title: 'Browse Pattern Library',
      description: 'Explore available West Coast Swing patterns and exercises',
      icon: <LibraryBooks sx={{ fontSize: 40, color: 'primary.main' }} />,
      action: () => navigate('/patterns'),
      color: 'primary.light',
    },
    {
      title: 'Create New Lesson',
      description: 'Build a lesson plan with patterns and exercises',
      icon: <Assignment sx={{ fontSize: 40, color: 'secondary.main' }} />,
      action: () => navigate('/lessons/new'),
      color: 'secondary.light',
      disabled: true,
    },
    {
      title: 'Plan Course',
      description: 'Design a multi-week course curriculum',
      icon: <School sx={{ fontSize: 40, color: 'success.main' }} />,
      action: () => navigate('/courses/new'),
      color: 'success.light',
      disabled: true,
    },
  ];

  const recentStats = [
    { label: 'Available Patterns', value: '6', color: 'primary' },
    { label: 'Beginner Level', value: '4', color: 'success' },
    { label: 'Improver Level', value: '1', color: 'info' },
    { label: 'Exercises', value: '2', color: 'secondary' },
  ];

  return (
    <Container maxWidth="lg">
      <Box sx={{ py: 4 }}>
        <Typography variant="h3" gutterBottom>
          Welcome to WCS Course Creator
        </Typography>
        
        <Typography variant="h6" color="text.secondary" paragraph>
          Plan, structure, and share your West Coast Swing lessons and courses
        </Typography>

        {/* Quick Stats */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom>
            Library Overview
          </Typography>
          <Grid container spacing={2}>
            {recentStats.map((stat) => (
              <Grid item xs={6} md={3} key={stat.label}>
                <Card sx={{ textAlign: 'center', py: 2 }}>
                  <CardContent sx={{ pb: 1 }}>
                    <Typography variant="h4" color={`${stat.color}.main`}>
                      {stat.value}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {stat.label}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Box>

        {/* Quick Actions */}
        <Typography variant="h5" gutterBottom>
          Quick Actions
        </Typography>
        <Grid container spacing={3}>
          {quickActions.map((action, index) => (
            <Grid item xs={12} md={4} key={index}>
              <Card 
                sx={{ 
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  opacity: action.disabled ? 0.6 : 1,
                }}
              >
                <CardContent sx={{ flexGrow: 1, textAlign: 'center', py: 3 }}>
                  <Box sx={{ mb: 2 }}>
                    {action.icon}
                  </Box>
                  <Typography variant="h6" gutterBottom>
                    {action.title}
                    {action.disabled && (
                      <Chip 
                        label="Coming Soon" 
                        size="small" 
                        color="default" 
                        sx={{ ml: 1 }}
                      />
                    )}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    {action.description}
                  </Typography>
                </CardContent>
                <CardActions sx={{ justifyContent: 'center', pb: 2 }}>
                  <Button 
                    variant="contained" 
                    onClick={action.action}
                    disabled={action.disabled}
                  >
                    Get Started
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* Welcome Message */}
        <Box sx={{ mt: 4, p: 3, bgcolor: 'grey.50', borderRadius: 2 }}>
          <Typography variant="h6" gutterBottom>
            Getting Started
          </Typography>
          <Typography variant="body1" paragraph>
            This is your MVP (Minimum Viable Product) for the West Coast Swing Course Creator. 
            Currently available features:
          </Typography>
          <Typography component="div" variant="body2">
            <strong>✅ Pattern Library:</strong> Browse and view detailed information about WCS patterns and exercises<br />
            <strong>🚧 Lesson Builder:</strong> Coming soon - Create structured lesson plans<br />
            <strong>🚧 Course Planning:</strong> Coming soon - Design multi-week course curricula<br />
            <strong>🚧 Export Features:</strong> Coming soon - Export to PDF and Markdown
          </Typography>
          <Box sx={{ mt: 2 }}>
            <Button 
              variant="outlined" 
              onClick={() => navigate('/patterns')}
              startIcon={<LibraryBooks />}
            >
              Explore Pattern Library
            </Button>
          </Box>
        </Box>
      </Box>
    </Container>
  );
};

export default Dashboard;