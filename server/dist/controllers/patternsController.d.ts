import { Response } from 'express';
import { AuthenticatedRequest } from '../middleware/auth';
import { PatternOrExercise } from '../models/types';
export declare const createPatternOrExercise: (req: AuthenticatedRequest, res: Response) => Promise<Response<any, Record<string, any>> | undefined>;
export declare const getPatternOrExerciseById: (id: string) => Promise<PatternOrExercise | null>;
export declare const getPatternOrExercise: (req: AuthenticatedRequest, res: Response) => Promise<Response<any, Record<string, any>> | undefined>;
export declare const getAllPatternsAndExercises: (req: AuthenticatedRequest, res: Response) => Promise<void>;
export declare const updatePatternOrExercise: (req: AuthenticatedRequest, res: Response) => Promise<Response<any, Record<string, any>> | undefined>;
export declare const deletePatternOrExercise: (req: AuthenticatedRequest, res: Response) => Promise<Response<any, Record<string, any>> | undefined>;
//# sourceMappingURL=patternsController.d.ts.map