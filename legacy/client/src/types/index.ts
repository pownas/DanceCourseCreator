export interface User {
  id: string;
  name: string;
  email: string;
  role: 'instructor' | 'editor' | 'reader' | 'admin';
  teamId?: string;
}

export interface AuthResponse {
  user: User;
  token: string;
}

export interface PatternOrExercise {
  id: string;
  type: 'pattern' | 'exercise';
  name: string;
  aliases: string[];
  level: 'beginner' | 'improver' | 'intermediate' | 'advanced';
  description: string;
  steps: string[];
  counts: string[];
  holds: string[];
  slot: string;
  rotations: string[];
  prerequisites: string[];
  related: string[];
  teachingPoints: string[];
  commonMistakes: string[];
  variations: string[];
  estimatedMinutes: number;
  bpmRange: { min: number; max: number };
  tags: string[];
  mediaLinks: string[];
  createdBy: string;
  updatedAt: Date;
}

export interface CreatePatternRequest {
  type: 'pattern' | 'exercise';
  name: string;
  aliases?: string[];
  level: 'beginner' | 'improver' | 'intermediate' | 'advanced';
  description?: string;
  steps?: string[];
  counts?: string[];
  holds?: string[];
  slot?: string;
  rotations?: string[];
  prerequisites?: string[];
  related?: string[];
  teachingPoints?: string[];
  commonMistakes?: string[];
  variations?: string[];
  estimatedMinutes?: number;
  bpmRange?: { min: number; max: number };
  tags?: string[];
  mediaLinks?: string[];
}