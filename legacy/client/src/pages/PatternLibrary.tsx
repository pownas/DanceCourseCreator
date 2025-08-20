import React, { useState, useEffect, useCallback } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  CardContent,
  Chip,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Fab,
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
} from '@mui/icons-material';
import { PatternOrExercise } from '../types';
import { patternsAPI } from '../api';
import { useTranslation } from 'react-i18next';

const PatternLibrary: React.FC = () => {
  const { t } = useTranslation();
  const [patterns, setPatterns] = useState<PatternOrExercise[]>([]);
  const [filteredPatterns, setFilteredPatterns] = useState<PatternOrExercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [typeFilter, setTypeFilter] = useState('');
  const [levelFilter, setLevelFilter] = useState('');
  const [dialogOpen, setDialogOpen] = useState(false);
  const [selectedPattern, setSelectedPattern] = useState<PatternOrExercise | null>(null);

  useEffect(() => {
    loadPatterns();
  }, []);

  const loadPatterns = async () => {
    try {
      setLoading(true);
      const data = await patternsAPI.getAll();
      setPatterns(data);
    } catch (error) {
      console.error('Failed to load patterns:', error);
    } finally {
      setLoading(false);
    }
  };

  const filterPatterns = useCallback(() => {
    let filtered = patterns;

    if (searchTerm) {
      filtered = filtered.filter(
        (pattern) =>
          pattern.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          pattern.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
          pattern.tags.some(tag => tag.toLowerCase().includes(searchTerm.toLowerCase()))
      );
    }

    if (typeFilter) {
      filtered = filtered.filter(pattern => pattern.type === typeFilter);
    }

    if (levelFilter) {
      filtered = filtered.filter(pattern => pattern.level === levelFilter);
    }

    setFilteredPatterns(filtered);
  }, [patterns, searchTerm, typeFilter, levelFilter]);

  useEffect(() => {
    filterPatterns();
  }, [filterPatterns]);

  const getLevelColor = (level: string) => {
    switch (level) {
      case 'beginner':
        return 'success';
      case 'improver':
        return 'info';
      case 'intermediate':
        return 'warning';
      case 'advanced':
        return 'error';
      default:
        return 'default';
    }
  };

  const getTypeColor = (type: string) => {
    return type === 'pattern' ? 'primary' : 'secondary';
  };

  const handleViewPattern = (pattern: PatternOrExercise) => {
    setSelectedPattern(pattern);
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setSelectedPattern(null);
  };

  if (loading) {
    return (
      <Container>
        <Typography>{t('patternLibrary.loadingPatterns')}</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg">
      <Box sx={{ py: 4 }}>
        <Typography variant="h4" gutterBottom>
          {t('patternLibrary.title')}
        </Typography>

        {/* Filters */}
        <Box sx={{ mb: 3 }}>
          <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', alignItems: 'center' }}>
            <Box sx={{ flex: '1 1 300px', minWidth: '300px' }}>
              <TextField
                fullWidth
                label={t('patternLibrary.searchPlaceholder')}
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                variant="outlined"
                size="small"
              />
            </Box>
            <Box sx={{ flex: '0 0 150px' }}>
              <FormControl fullWidth size="small">
                <InputLabel>{t('patternLibrary.filters.type')}</InputLabel>
                <Select
                  value={typeFilter}
                  label={t('patternLibrary.filters.type')}
                  onChange={(e) => setTypeFilter(e.target.value)}
                >
                  <MenuItem value="">{t('patternLibrary.filters.all')}</MenuItem>
                  <MenuItem value="pattern">{t('patternLibrary.filters.patterns')}</MenuItem>
                  <MenuItem value="exercise">{t('patternLibrary.filters.exercises')}</MenuItem>
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ flex: '0 0 150px' }}>
              <FormControl fullWidth size="small">
                <InputLabel>{t('patternLibrary.filters.level')}</InputLabel>
                <Select
                  value={levelFilter}
                  label={t('patternLibrary.filters.level')}
                  onChange={(e) => setLevelFilter(e.target.value)}
                >
                  <MenuItem value="">{t('patternLibrary.filters.all')}</MenuItem>
                  <MenuItem value="beginner">{t('patternLibrary.filters.beginner')}</MenuItem>
                  <MenuItem value="improver">{t('patternLibrary.filters.improver')}</MenuItem>
                  <MenuItem value="intermediate">{t('patternLibrary.filters.intermediate')}</MenuItem>
                  <MenuItem value="advanced">{t('patternLibrary.filters.advanced')}</MenuItem>
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ flex: '1 1 200px' }}>
              <Typography variant="body2" color="text.secondary">
                {t('patternLibrary.showingResults', { filtered: filteredPatterns.length, total: patterns.length })}
              </Typography>
            </Box>
          </Box>
        </Box>

        {/* Pattern Grid */}
        <Box sx={{ display: 'flex', gap: 3, flexWrap: 'wrap' }}>
          {filteredPatterns.map((pattern) => (
            <Box key={pattern.id} sx={{ flex: '1 1 350px', minWidth: '350px', maxWidth: '400px' }}>
              <Card 
                sx={{ 
                  height: '100%', 
                  cursor: 'pointer',
                  '&:hover': { elevation: 4 }
                }}
                onClick={() => handleViewPattern(pattern)}
              >
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                    <Chip 
                      label={pattern.type} 
                      color={getTypeColor(pattern.type) as any}
                      size="small"
                    />
                    <Chip 
                      label={pattern.level} 
                      color={getLevelColor(pattern.level) as any}
                      size="small"
                    />
                  </Box>
                  
                  <Typography variant="h6" gutterBottom>
                    {pattern.name}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    {pattern.description}
                  </Typography>
                  
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="caption" color="text.secondary">
                      {pattern.estimatedMinutes} min • {pattern.bpmRange.min}-{pattern.bpmRange.max} BPM
                    </Typography>
                  </Box>
                  
                  <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                    {pattern.tags.slice(0, 3).map((tag) => (
                      <Chip key={tag} label={tag} size="small" variant="outlined" />
                    ))}
                    {pattern.tags.length > 3 && (
                      <Chip label={`+${pattern.tags.length - 3}`} size="small" variant="outlined" />
                    )}
                  </Box>
                </CardContent>
              </Card>
            </Box>
          ))}
        </Box>

        {filteredPatterns.length === 0 && (
          <Box sx={{ textAlign: 'center', py: 4 }}>
            <Typography variant="h6" color="text.secondary">
              No patterns found
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Try adjusting your search criteria
            </Typography>
          </Box>
        )}

        {/* Floating Action Button for Adding New Pattern */}
        <Fab
          color="primary"
          aria-label="add"
          sx={{
            position: 'fixed',
            bottom: 16,
            right: 16,
          }}
          onClick={() => {/* TODO: Open create pattern dialog */}}
        >
          <AddIcon />
        </Fab>

        {/* Pattern Detail Dialog */}
        <Dialog 
          open={dialogOpen} 
          onClose={handleCloseDialog}
          maxWidth="md"
          fullWidth
        >
          {selectedPattern && (
            <>
              <DialogTitle>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                  <Typography variant="h5">{selectedPattern.name}</Typography>
                  <Box>
                    <Chip 
                      label={selectedPattern.type} 
                      color={getTypeColor(selectedPattern.type) as any}
                      sx={{ mr: 1 }}
                    />
                    <Chip 
                      label={selectedPattern.level} 
                      color={getLevelColor(selectedPattern.level) as any}
                    />
                  </Box>
                </Box>
              </DialogTitle>
              <DialogContent>
                <Box sx={{ mb: 2 }}>
                  <Typography variant="body1">{selectedPattern.description}</Typography>
                </Box>

                {selectedPattern.steps.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="h6" gutterBottom>{t('patternLibrary.details.steps')}</Typography>
                    {selectedPattern.steps.map((step, index) => (
                      <Typography key={index} variant="body2">
                        {index + 1}. {step}
                      </Typography>
                    ))}
                  </Box>
                )}

                {selectedPattern.teachingPoints.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="h6" gutterBottom>{t('patternLibrary.details.teachingPoints')}</Typography>
                    {selectedPattern.teachingPoints.map((point, index) => (
                      <Typography key={index} variant="body2">
                        • {point}
                      </Typography>
                    ))}
                  </Box>
                )}

                {selectedPattern.commonMistakes.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="h6" gutterBottom>{t('patternLibrary.details.commonMistakes')}</Typography>
                    {selectedPattern.commonMistakes.map((mistake, index) => (
                      <Typography key={index} variant="body2" color="error">
                        • {mistake}
                      </Typography>
                    ))}
                  </Box>
                )}

                <Box sx={{ mb: 2 }}>
                  <Typography variant="h6" gutterBottom>{t('patternLibrary.details.details')}</Typography>
                  <Typography variant="body2">
                    <strong>{t('patternLibrary.details.duration')}:</strong> {selectedPattern.estimatedMinutes} {t('patternLibrary.details.minutes')}
                  </Typography>
                  <Typography variant="body2">
                    <strong>{t('patternLibrary.details.bpmRange')}:</strong> {selectedPattern.bpmRange.min}-{selectedPattern.bpmRange.max}
                  </Typography>
                  {selectedPattern.counts.length > 0 && (
                    <Typography variant="body2">
                      <strong>{t('patternLibrary.details.counts')}:</strong> {selectedPattern.counts.join(', ')}
                    </Typography>
                  )}
                </Box>

                {selectedPattern.tags.length > 0 && (
                  <Box>
                    <Typography variant="h6" gutterBottom>{t('patternLibrary.details.tags')}</Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                      {selectedPattern.tags.map((tag) => (
                        <Chip key={tag} label={tag} size="small" variant="outlined" />
                      ))}
                    </Box>
                  </Box>
                )}
              </DialogContent>
              <DialogActions>
                <Button onClick={handleCloseDialog}>{t('patternLibrary.details.close')}</Button>
                <Button variant="contained" startIcon={<EditIcon />}>
                  {t('patternLibrary.details.edit')}
                </Button>
              </DialogActions>
            </>
          )}
        </Dialog>
      </Box>
    </Container>
  );
};

export default PatternLibrary;