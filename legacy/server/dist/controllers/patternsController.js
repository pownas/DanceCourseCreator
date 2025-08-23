"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.deletePatternOrExercise = exports.updatePatternOrExercise = exports.getAllPatternsAndExercises = exports.getPatternOrExercise = exports.getPatternOrExerciseById = exports.createPatternOrExercise = void 0;
const auth_1 = require("../utils/auth");
const db_1 = require("../database/db");
const createPatternOrExercise = async (req, res) => {
    try {
        const { type, name, aliases = [], level, description = '', steps = [], counts = [], holds = [], slot = '', rotations = [], prerequisites = [], related = [], teachingPoints = [], commonMistakes = [], variations = [], estimatedMinutes = 0, bpmRange = { min: 90, max: 110 }, tags = [], mediaLinks = [] } = req.body;
        if (!type || !name || !level) {
            return res.status(400).json({ error: 'Type, name, and level are required' });
        }
        const id = (0, auth_1.generateId)();
        const db = (0, db_1.getDatabase)();
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
            JSON.stringify(tags), JSON.stringify(mediaLinks), req.user.id
        ]);
        const pattern = await (0, exports.getPatternOrExerciseById)(id);
        res.status(201).json(pattern);
    }
    catch (error) {
        console.error('Create pattern/exercise error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.createPatternOrExercise = createPatternOrExercise;
const getPatternOrExerciseById = async (id) => {
    const db = (0, db_1.getDatabase)();
    const row = await db.get('SELECT * FROM patterns_exercises WHERE id = ?', [id]);
    if (!row)
        return null;
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
exports.getPatternOrExerciseById = getPatternOrExerciseById;
const getPatternOrExercise = async (req, res) => {
    try {
        const { id } = req.params;
        const pattern = await (0, exports.getPatternOrExerciseById)(id);
        if (!pattern) {
            return res.status(404).json({ error: 'Pattern/exercise not found' });
        }
        res.json(pattern);
    }
    catch (error) {
        console.error('Get pattern/exercise error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.getPatternOrExercise = getPatternOrExercise;
const getAllPatternsAndExercises = async (req, res) => {
    try {
        const { type, level, tags, search } = req.query;
        const db = (0, db_1.getDatabase)();
        let query = 'SELECT * FROM patterns_exercises WHERE 1=1';
        const params = [];
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
    }
    catch (error) {
        console.error('Get all patterns/exercises error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.getAllPatternsAndExercises = getAllPatternsAndExercises;
const updatePatternOrExercise = async (req, res) => {
    try {
        const { id } = req.params;
        const existingPattern = await (0, exports.getPatternOrExerciseById)(id);
        if (!existingPattern) {
            return res.status(404).json({ error: 'Pattern/exercise not found' });
        }
        // Check if user can edit this pattern
        if (existingPattern.createdBy !== req.user.id && req.user.role !== 'admin') {
            return res.status(403).json({ error: 'Access denied' });
        }
        const { name, aliases, level, description, steps, counts, holds, slot, rotations, prerequisites, related, teachingPoints, commonMistakes, variations, estimatedMinutes, bpmRange, tags, mediaLinks } = req.body;
        const db = (0, db_1.getDatabase)();
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
        const updatedPattern = await (0, exports.getPatternOrExerciseById)(id);
        res.json(updatedPattern);
    }
    catch (error) {
        console.error('Update pattern/exercise error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.updatePatternOrExercise = updatePatternOrExercise;
const deletePatternOrExercise = async (req, res) => {
    try {
        const { id } = req.params;
        const pattern = await (0, exports.getPatternOrExerciseById)(id);
        if (!pattern) {
            return res.status(404).json({ error: 'Pattern/exercise not found' });
        }
        // Check if user can delete this pattern
        if (pattern.createdBy !== req.user.id && req.user.role !== 'admin') {
            return res.status(403).json({ error: 'Access denied' });
        }
        const db = (0, db_1.getDatabase)();
        await db.run('DELETE FROM patterns_exercises WHERE id = ?', [id]);
        res.json({ message: 'Pattern/exercise deleted successfully' });
    }
    catch (error) {
        console.error('Delete pattern/exercise error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.deletePatternOrExercise = deletePatternOrExercise;
//# sourceMappingURL=patternsController.js.map