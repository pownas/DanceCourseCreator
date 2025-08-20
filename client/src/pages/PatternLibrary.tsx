import React, { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Box,
  Card,
  CardContent,
  Chip,
  TextField,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Fab,
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  FilterList as FilterIcon,
} from '@mui/icons-material';
import { PatternOrExercise } from '../types';
import { patternsAPI } from '../api';

const PatternLibrary: React.FC = () => {
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

  useEffect(() => {
    filterPatterns();
  }, [patterns, searchTerm, typeFilter, levelFilter]);

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

  const filterPatterns = () => {
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
  };

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
        <Typography>Loading patterns...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg">
      <Box sx={{ py: 4 }}>
        <Typography variant="h4" gutterBottom>
          Pattern & Exercise Library
        </Typography>

        {/* Filters */}
        <Box sx={{ mb: 3 }}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Search patterns..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                variant="outlined"
                size="small"
              />
            </Grid>
            <Grid item xs={6} md={2}>
              <FormControl fullWidth size="small">
                <InputLabel>Type</InputLabel>
                <Select
                  value={typeFilter}
                  label="Type"
                  onChange={(e) => setTypeFilter(e.target.value)}
                >
                  <MenuItem value="">All</MenuItem>
                  <MenuItem value="pattern">Patterns</MenuItem>
                  <MenuItem value="exercise">Exercises</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={6} md={2}>
              <FormControl fullWidth size="small">
                <InputLabel>Level</InputLabel>
                <Select
                  value={levelFilter}
                  label="Level"
                  onChange={(e) => setLevelFilter(e.target.value)}
                >
                  <MenuItem value="">All</MenuItem>
                  <MenuItem value="beginner">Beginner</MenuItem>
                  <MenuItem value="improver">Improver</MenuItem>
                  <MenuItem value="intermediate">Intermediate</MenuItem>
                  <MenuItem value="advanced">Advanced</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} md={4}>
              <Typography variant="body2" color="text.secondary">
                Showing {filteredPatterns.length} of {patterns.length} items
              </Typography>
            </Grid>
          </Grid>
        </Box>

        {/* Pattern Grid */}
        <Grid container spacing={3}>
          {filteredPatterns.map((pattern) => (
            <Grid item xs={12} md={6} lg={4} key={pattern.id}>
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
            </Grid>
          ))}
        </Grid>

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
                    <Typography variant="h6" gutterBottom>Steps</Typography>
                    {selectedPattern.steps.map((step, index) => (
                      <Typography key={index} variant="body2">
                        {index + 1}. {step}
                      </Typography>
                    ))}
                  </Box>
                )}

                {selectedPattern.teachingPoints.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="h6" gutterBottom>Teaching Points</Typography>
                    {selectedPattern.teachingPoints.map((point, index) => (
                      <Typography key={index} variant="body2">
                        • {point}
                      </Typography>
                    ))}
                  </Box>
                )}

                {selectedPattern.commonMistakes.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="h6" gutterBottom>Common Mistakes</Typography>
                    {selectedPattern.commonMistakes.map((mistake, index) => (
                      <Typography key={index} variant="body2" color="error">
                        • {mistake}
                      </Typography>
                    ))}
                  </Box>
                )}

                <Box sx={{ mb: 2 }}>
                  <Typography variant="h6" gutterBottom>Details</Typography>
                  <Typography variant="body2">
                    <strong>Duration:</strong> {selectedPattern.estimatedMinutes} minutes
                  </Typography>
                  <Typography variant="body2">
                    <strong>BPM Range:</strong> {selectedPattern.bpmRange.min}-{selectedPattern.bpmRange.max}
                  </Typography>
                  {selectedPattern.counts.length > 0 && (
                    <Typography variant="body2">
                      <strong>Counts:</strong> {selectedPattern.counts.join(', ')}
                    </Typography>
                  )}
                </Box>

                {selectedPattern.tags.length > 0 && (
                  <Box>
                    <Typography variant="h6" gutterBottom>Tags</Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                      {selectedPattern.tags.map((tag) => (
                        <Chip key={tag} label={tag} size="small" variant="outlined" />
                      ))}
                    </Box>
                  </Box>
                )}
              </DialogContent>
              <DialogActions>
                <Button onClick={handleCloseDialog}>Close</Button>
                <Button variant="contained" startIcon={<EditIcon />}>
                  Edit
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