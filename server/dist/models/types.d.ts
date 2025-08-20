export interface User {
    id: string;
    name: string;
    email: string;
    role: 'instructor' | 'editor' | 'reader' | 'admin';
    teamId?: string;
    hashedPassword: string;
    createdAt: Date;
    updatedAt: Date;
}
export interface Team {
    id: string;
    name: string;
    createdAt: Date;
    updatedAt: Date;
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
    bpmRange: {
        min: number;
        max: number;
    };
    tags: string[];
    mediaLinks: string[];
    createdBy: string;
    updatedAt: Date;
}
export interface LessonSection {
    type: 'warmup' | 'technique' | 'patterns' | 'combination' | 'repetition' | 'social';
    items: string[];
    notes: string;
}
export interface Lesson {
    id: string;
    courseId?: string;
    date?: Date;
    duration: number;
    sections: LessonSection[];
    totalEstimatedMinutes: number;
    notes: string;
    version: number;
    createdBy: string;
    reviewers: string[];
    history: string[];
    createdAt: Date;
    updatedAt: Date;
}
export interface Course {
    id: string;
    name: string;
    level: 'beginner' | 'improver' | 'intermediate' | 'advanced';
    durationWeeks: number;
    goals: string[];
    themesByWeek: string[];
    lessons: string[];
    coverageMetrics: string;
    repetitionPlan: string;
    createdBy: string;
    createdAt: Date;
    updatedAt: Date;
}
export interface Template {
    id: string;
    scope: 'lesson' | 'course';
    name: string;
    content: string;
    owner: string;
    team?: string;
    createdAt: Date;
    updatedAt: Date;
}
export interface ShareLink {
    id: string;
    resourceId: string;
    type: 'lesson' | 'course';
    visibility: 'public' | 'private';
    expiresAt?: Date;
    token: string;
    createdBy: string;
    createdAt: Date;
}
//# sourceMappingURL=types.d.ts.map