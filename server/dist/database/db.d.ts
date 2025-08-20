import sqlite3 from 'sqlite3';
export declare class Database {
    private db;
    constructor(dbPath?: string);
    init(): Promise<void>;
    run(sql: string, params?: any[]): Promise<sqlite3.RunResult>;
    get(sql: string, params?: any[]): Promise<any>;
    all(sql: string, params?: any[]): Promise<any[]>;
    close(): void;
}
export declare const getDatabase: () => Database;
//# sourceMappingURL=db.d.ts.map