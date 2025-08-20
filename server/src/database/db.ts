import sqlite3 from 'sqlite3';
import { promisify } from 'util';
import path from 'path';

export class Database {
  private db: sqlite3.Database;

  constructor(dbPath: string = 'database.sqlite') {
    this.db = new sqlite3.Database(path.resolve(dbPath));
  }

  async init(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.db.serialize(() => {
        // Users table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS users (
            id TEXT PRIMARY KEY,
            name TEXT NOT NULL,
            email TEXT UNIQUE NOT NULL,
            role TEXT NOT NULL CHECK (role IN ('instructor', 'editor', 'reader', 'admin')),
            team_id TEXT,
            hashed_password TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
          )
        `);

        // Teams table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS teams (
            id TEXT PRIMARY KEY,
            name TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
          )
        `);

        // Patterns and Exercises table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS patterns_exercises (
            id TEXT PRIMARY KEY,
            type TEXT NOT NULL CHECK (type IN ('pattern', 'exercise')),
            name TEXT NOT NULL,
            aliases TEXT, -- JSON array
            level TEXT NOT NULL CHECK (level IN ('beginner', 'improver', 'intermediate', 'advanced')),
            description TEXT,
            steps TEXT, -- JSON array
            counts TEXT, -- JSON array
            holds TEXT, -- JSON array
            slot TEXT,
            rotations TEXT, -- JSON array
            prerequisites TEXT, -- JSON array
            related TEXT, -- JSON array
            teaching_points TEXT, -- JSON array
            common_mistakes TEXT, -- JSON array
            variations TEXT, -- JSON array
            estimated_minutes INTEGER,
            bpm_range_min INTEGER,
            bpm_range_max INTEGER,
            tags TEXT, -- JSON array
            media_links TEXT, -- JSON array
            created_by TEXT NOT NULL,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (created_by) REFERENCES users(id)
          )
        `);

        // Lessons table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS lessons (
            id TEXT PRIMARY KEY,
            course_id TEXT,
            date DATETIME,
            duration INTEGER NOT NULL, -- minutes
            sections TEXT, -- JSON array of sections
            total_estimated_minutes INTEGER,
            notes TEXT,
            version INTEGER DEFAULT 1,
            created_by TEXT NOT NULL,
            reviewers TEXT, -- JSON array
            history TEXT, -- JSON array
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (created_by) REFERENCES users(id),
            FOREIGN KEY (course_id) REFERENCES courses(id)
          )
        `);

        // Courses table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS courses (
            id TEXT PRIMARY KEY,
            name TEXT NOT NULL,
            level TEXT NOT NULL CHECK (level IN ('beginner', 'improver', 'intermediate', 'advanced')),
            duration_weeks INTEGER,
            goals TEXT, -- JSON array
            themes_by_week TEXT, -- JSON array
            lessons TEXT, -- JSON array of lesson IDs
            coverage_metrics TEXT, -- JSON string
            repetition_plan TEXT, -- JSON string
            created_by TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (created_by) REFERENCES users(id)
          )
        `);

        // Templates table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS templates (
            id TEXT PRIMARY KEY,
            scope TEXT NOT NULL CHECK (scope IN ('lesson', 'course')),
            name TEXT NOT NULL,
            content TEXT, -- JSON string
            owner TEXT NOT NULL,
            team TEXT,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (owner) REFERENCES users(id),
            FOREIGN KEY (team) REFERENCES teams(id)
          )
        `);

        // Share links table
        this.db.run(`
          CREATE TABLE IF NOT EXISTS share_links (
            id TEXT PRIMARY KEY,
            resource_id TEXT NOT NULL,
            type TEXT NOT NULL CHECK (type IN ('lesson', 'course')),
            visibility TEXT NOT NULL CHECK (visibility IN ('public', 'private')),
            expires_at DATETIME,
            token TEXT UNIQUE NOT NULL,
            created_by TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (created_by) REFERENCES users(id)
          )
        `, (err) => {
          if (err) {
            reject(err);
          } else {
            resolve();
          }
        });
      });
    });
  }

  async run(sql: string, params: any[] = []): Promise<sqlite3.RunResult> {
    return new Promise((resolve, reject) => {
      this.db.run(sql, params, function(err) {
        if (err) {
          reject(err);
        } else {
          resolve(this);
        }
      });
    });
  }

  async get(sql: string, params: any[] = []): Promise<any> {
    return new Promise((resolve, reject) => {
      this.db.get(sql, params, (err, row) => {
        if (err) {
          reject(err);
        } else {
          resolve(row);
        }
      });
    });
  }

  async all(sql: string, params: any[] = []): Promise<any[]> {
    return new Promise((resolve, reject) => {
      this.db.all(sql, params, (err, rows) => {
        if (err) {
          reject(err);
        } else {
          resolve(rows);
        }
      });
    });
  }

  close(): void {
    this.db.close();
  }
}

let dbInstance: Database | null = null;

export const getDatabase = (): Database => {
  if (!dbInstance) {
    dbInstance = new Database();
  }
  return dbInstance;
};