import { Request, Response, NextFunction } from 'express';
import { verifyToken } from '../utils/auth';
import { getDatabase } from '../database/db';

export interface AuthenticatedRequest extends Request {
  user?: {
    id: string;
    name: string;
    email: string;
    role: string;
    teamId?: string;
  };
}

export const authenticate = async (req: AuthenticatedRequest, res: Response, next: NextFunction) => {
  try {
    const token = req.header('Authorization')?.replace('Bearer ', '');
    
    if (!token) {
      return res.status(401).json({ error: 'Access denied. No token provided.' });
    }

    const decoded = verifyToken(token);
    const db = getDatabase();
    
    const user = await db.get(
      'SELECT id, name, email, role, team_id FROM users WHERE id = ?',
      [decoded.userId]
    );

    if (!user) {
      return res.status(401).json({ error: 'Invalid token.' });
    }

    req.user = {
      id: user.id,
      name: user.name,
      email: user.email,
      role: user.role,
      teamId: user.team_id
    };

    next();
  } catch (error) {
    res.status(401).json({ error: 'Invalid token.' });
  }
};

export const authorize = (roles: string[]) => {
  return (req: AuthenticatedRequest, res: Response, next: NextFunction) => {
    if (!req.user || !roles.includes(req.user.role)) {
      return res.status(403).json({ error: 'Access denied.' });
    }
    next();
  };
};