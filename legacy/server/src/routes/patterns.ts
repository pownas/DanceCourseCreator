import express from 'express';
import {
  createPatternOrExercise,
  getPatternOrExercise,
  getAllPatternsAndExercises,
  updatePatternOrExercise,
  deletePatternOrExercise
} from '../controllers/patternsController';
import { authenticate, authorize } from '../middleware/auth';

const router = express.Router();

// All routes require authentication
router.use(authenticate);

router.post('/', authorize(['instructor', 'editor', 'admin']), createPatternOrExercise);
router.get('/', getAllPatternsAndExercises);
router.get('/:id', getPatternOrExercise);
router.put('/:id', authorize(['instructor', 'editor', 'admin']), updatePatternOrExercise);
router.delete('/:id', authorize(['instructor', 'editor', 'admin']), deletePatternOrExercise);

export default router;