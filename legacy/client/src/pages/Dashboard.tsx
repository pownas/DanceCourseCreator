import React from 'react';
import {
  Container,
  Typography,
  Box,
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
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  const quickActions = [
    {
      title: t('dashboard.actions.browsePatternLibrary.title'),
      description: t('dashboard.actions.browsePatternLibrary.description'),
      icon: <LibraryBooks sx={{ fontSize: 40, color: 'primary.main' }} />,
      action: () => navigate('/patterns'),
      color: 'primary.light',
    },
    {
      title: t('dashboard.actions.createNewLesson.title'),
      description: t('dashboard.actions.createNewLesson.description'),
      icon: <Assignment sx={{ fontSize: 40, color: 'secondary.main' }} />,
      action: () => navigate('/lessons/new'),
      color: 'secondary.light',
      disabled: true,
    },
    {
      title: t('dashboard.actions.planCourse.title'),
      description: t('dashboard.actions.planCourse.description'),
      icon: <School sx={{ fontSize: 40, color: 'success.main' }} />,
      action: () => navigate('/courses/new'),
      color: 'success.light',
      disabled: true,
    },
  ];

  const recentStats = [
    { label: t('dashboard.stats.availablePatterns'), value: '6', color: 'primary' },
    { label: t('dashboard.stats.beginnerLevel'), value: '4', color: 'success' },
    { label: t('dashboard.stats.improverLevel'), value: '1', color: 'info' },
    { label: t('dashboard.stats.exercises'), value: '2', color: 'secondary' },
  ];

  return (
    <Container maxWidth="lg">
      <Box sx={{ py: 4 }}>
        <Typography variant="h3" gutterBottom>
          {t('dashboard.title')}
        </Typography>
        
        <Typography variant="h6" color="text.secondary" paragraph>
          {t('dashboard.subtitle')}
        </Typography>

        {/* Quick Stats */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h5" gutterBottom>
            {t('dashboard.libraryOverview')}
          </Typography>
          <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
            {recentStats.map((stat) => (
              <Box key={stat.label} sx={{ flex: '1 1 200px', minWidth: '200px' }}>
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
              </Box>
            ))}
          </Box>
        </Box>

        {/* Quick Actions */}
        <Typography variant="h5" gutterBottom>
          {t('dashboard.quickActions')}
        </Typography>
        <Box sx={{ display: 'flex', gap: 3, flexWrap: 'wrap' }}>
          {quickActions.map((action, index) => (
            <Box key={index} sx={{ flex: '1 1 300px', minWidth: '300px' }}>
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
                        label={t('dashboard.actions.comingSoon')} 
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
                    {t('dashboard.actions.getStarted')}
                  </Button>
                </CardActions>
              </Card>
            </Box>
          ))}
        </Box>

        {/* Welcome Message */}
        <Box sx={{ mt: 4, p: 3, bgcolor: 'grey.50', borderRadius: 2 }}>
          <Typography variant="h6" gutterBottom>
            {t('dashboard.gettingStarted')}
          </Typography>
          <Typography variant="body1" paragraph>
            {t('dashboard.mvpInfo.description')}
          </Typography>
          <Typography component="div" variant="body2">
            <strong>✅ {t('dashboard.mvpInfo.patternLibrary')}</strong> {t('dashboard.mvpInfo.patternLibraryDescription')}<br />
            <strong>🚧 {t('dashboard.mvpInfo.lessonBuilder')}</strong> {t('dashboard.mvpInfo.lessonBuilderDescription')}<br />
            <strong>🚧 {t('dashboard.mvpInfo.coursePlanning')}</strong> {t('dashboard.mvpInfo.coursePlanningDescription')}<br />
            <strong>🚧 {t('dashboard.mvpInfo.exportFeatures')}</strong> {t('dashboard.mvpInfo.exportFeaturesDescription')}
          </Typography>
          <Box sx={{ mt: 2 }}>
            <Button 
              variant="outlined" 
              onClick={() => navigate('/patterns')}
              startIcon={<LibraryBooks />}
            >
              {t('dashboard.mvpInfo.explorePatternLibrary')}
            </Button>
          </Box>
        </Box>
      </Box>
    </Container>
  );
};

export default Dashboard;