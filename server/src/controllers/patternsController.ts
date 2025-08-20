import { Response } from 'express';
import { generateId } from '../utils/auth';
import { getDatabase } from '../database/db';
import { AuthenticatedRequest } from '../middleware/auth';
import { PatternOrExercise } from '../models/types';

export const createPatternOrExercise = async (req: AuthenticatedRequest, res: Response) => {
  try {
    const {
      type, name, aliases = [], level, description = '', steps = [], counts = [],
      holds = [], slot = '', rotations = [], prerequisites = [], related = [],
      teachingPoints = [], commonMistakes = [], variations = [], estimatedMinutes = 0,
      bpmRange = { min: 90, max: 110 }, tags = [], mediaLinks = []
    } = req.body;

    if (!type || !name || !level) {
      return res.status(400).json({ error: 'Type, name, and level are required' });
    }

    const id = generateId();
    const db = getDatabase();

    await db.run(`
      INSERT INTO patterns_exercises (
        id, type, name, aliases, level, description, steps, counts, holds, slot,
        rotations, prerequisites, related, teaching_points, common_mistakes,
        variations, estimated_minutes, bpm_range_min, bpm_range_max, tags,
        media_links, created_by
      ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    `, [
      id, type, name, JSON.stringify(aliases), level, description,
      JSON.stringify(steps), JSON.stringify(counts), JSON.stringify(holds), slot,
      JSON.stringify(rotations), JSON.stringify(prerequisites), JSON.stringify(related),
      JSON.stringify(teachingPoints), JSON.stringify(commonMistakes),
      JSON.stringify(variations), estimatedMinutes, bpmRange.min, bpmRange.max,
      JSON.stringify(tags), JSON.stringify(mediaLinks), req.user!.id
    ]);

    const pattern = await getPatternOrExerciseById(id);
    res.status(201).json(pattern);
  } catch (error) {
    console.error('Create pattern/exercise error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

export const getPatternOrExerciseById = async (id: string): Promise<PatternOrExercise | null> => {
  const db = getDatabase();
  const row = await db.get('SELECT * FROM patterns_exercises WHERE id = ?', [id]);
  
  if (!row) return null;

  return {
    id: row.id,
    type: row.type,
    name: row.name,
    aliases: JSON.parse(row.aliases || '[]'),
    level: row.level,
    description: row.description,
    steps: JSON.parse(row.steps || '[]'),
    counts: JSON.parse(row.counts || '[]'),
    holds: JSON.parse(row.holds || '[]'),
    slot: row.slot,
    rotations: JSON.parse(row.rotations || '[]'),
    prerequisites: JSON.parse(row.prerequisites || '[]'),
    related: JSON.parse(row.related || '[]'),
    teachingPoints: JSON.parse(row.teaching_points || '[]'),
    commonMistakes: JSON.parse(row.common_mistakes || '[]'),
    variations: JSON.parse(row.variations || '[]'),
    estimatedMinutes: row.estimated_minutes,
    bpmRange: { min: row.bpm_range_min, max: row.bpm_range_max },
    tags: JSON.parse(row.tags || '[]'),
    mediaLinks: JSON.parse(row.media_links || '[]'),
    createdBy: row.created_by,
    updatedAt: new Date(row.updated_at)
  };
};

export const getPatternOrExercise = async (req: AuthenticatedRequest, res: Response) => {
  try {
    const { id } = req.params;
    const pattern = await getPatternOrExerciseById(id);
    
    if (!pattern) {
      return res.status(404).json({ error: 'Pattern/exercise not found' });
    }

    res.json(pattern);
  } catch (error) {
    console.error('Get pattern/exercise error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

export const getAllPatternsAndExercises = async (req: AuthenticatedRequest, res: Response) => {
  try {
    const { type, level, tags, search } = req.query;
    const db = getDatabase();
    
    let query = 'SELECT * FROM patterns_exercises WHERE 1=1';
    const params: any[] = [];

    if (type) {
      query += ' AND type = ?';
      params.push(type);
    }

    if (level) {
      query += ' AND level = ?';
      params.push(level);
    }

    if (search) {
      query += ' AND (name LIKE ? OR description LIKE ?)';
      params.push(`%${search}%`, `%${search}%`);
    }

    if (tags) {
      const tagList = Array.isArray(tags) ? tags : [tags];
      for (const tag of tagList) {
        query += ' AND tags LIKE ?';
        params.push(`%"${tag}"%`);
      }
    }

    query += ' ORDER BY name';

    const rows = await db.all(query, params);
    
    const patterns = rows.map(row => ({
      id: row.id,
      type: row.type,
      name: row.name,
      aliases: JSON.parse(row.aliases || '[]'),
      level: row.level,
      description: row.description,
      steps: JSON.parse(row.steps || '[]'),
      counts: JSON.parse(row.counts || '[]'),
      holds: JSON.parse(row.holds || '[]'),
      slot: row.slot,
      rotations: JSON.parse(row.rotations || '[]'),
      prerequisites: JSON.parse(row.prerequisites || '[]'),
      related: JSON.parse(row.related || '[]'),
      teachingPoints: JSON.parse(row.teaching_points || '[]'),
      commonMistakes: JSON.parse(row.common_mistakes || '[]'),
      variations: JSON.parse(row.variations || '[]'),
      estimatedMinutes: row.estimated_minutes,
      bpmRange: { min: row.bpm_range_min, max: row.bpm_range_max },
      tags: JSON.parse(row.tags || '[]'),
      mediaLinks: JSON.parse(row.media_links || '[]'),
      createdBy: row.created_by,
      updatedAt: new Date(row.updated_at)
    }));

    res.json(patterns);
  } catch (error) {
    console.error('Get all patterns/exercises error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

export const updatePatternOrExercise = async (req: AuthenticatedRequest, res: Response) => {
  try {
    const { id } = req.params;
    const existingPattern = await getPatternOrExerciseById(id);
    
    if (!existingPattern) {
      return res.status(404).json({ error: 'Pattern/exercise not found' });
    }

    // Check if user can edit this pattern
    if (existingPattern.createdBy !== req.user!.id && req.user!.role !== 'admin') {
      return res.status(403).json({ error: 'Access denied' });
    }

    const {
      name, aliases, level, description, steps, counts, holds, slot,
      rotations, prerequisites, related, teachingPoints, commonMistakes,
      variations, estimatedMinutes, bpmRange, tags, mediaLinks
    } = req.body;

    const db = getDatabase();

    await db.run(`
      UPDATE patterns_exercises SET
        name = ?, aliases = ?, level = ?, description = ?, steps = ?,
        counts = ?, holds = ?, slot = ?, rotations = ?, prerequisites = ?,
        related = ?, teaching_points = ?, common_mistakes = ?, variations = ?,
        estimated_minutes = ?, bpm_range_min = ?, bpm_range_max = ?, tags = ?,
        media_links = ?, updated_at = CURRENT_TIMESTAMP
      WHERE id = ?
    `, [
      name || existingPattern.name,
      JSON.stringify(aliases || existingPattern.aliases),
      level || existingPattern.level,
      description || existingPattern.description,
      JSON.stringify(steps || existingPattern.steps),
      JSON.stringify(counts || existingPattern.counts),
      JSON.stringify(holds || existingPattern.holds),
      slot || existingPattern.slot,
      JSON.stringify(rotations || existingPattern.rotations),
      JSON.stringify(prerequisites || existingPattern.prerequisites),
      JSON.stringify(related || existingPattern.related),
      JSON.stringify(teachingPoints || existingPattern.teachingPoints),
      JSON.stringify(commonMistakes || existingPattern.commonMistakes),
      JSON.stringify(variations || existingPattern.variations),
      estimatedMinutes || existingPattern.estimatedMinutes,
      (bpmRange?.min) || existingPattern.bpmRange.min,
      (bpmRange?.max) || existingPattern.bpmRange.max,
      JSON.stringify(tags || existingPattern.tags),
      JSON.stringify(mediaLinks || existingPattern.mediaLinks),
      id
    ]);

    const updatedPattern = await getPatternOrExerciseById(id);
    res.json(updatedPattern);
  } catch (error) {
    console.error('Update pattern/exercise error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};

export const deletePatternOrExercise = async (req: AuthenticatedRequest, res: Response) => {
  try {
    const { id } = req.params;
    const pattern = await getPatternOrExerciseById(id);
    
    if (!pattern) {
      return res.status(404).json({ error: 'Pattern/exercise not found' });
    }

    // Check if user can delete this pattern
    if (pattern.createdBy !== req.user!.id && req.user!.role !== 'admin') {
      return res.status(403).json({ error: 'Access denied' });
    }

    const db = getDatabase();
    await db.run('DELETE FROM patterns_exercises WHERE id = ?', [id]);

    res.json({ message: 'Pattern/exercise deleted successfully' });
  } catch (error) {
    console.error('Delete pattern/exercise error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
};