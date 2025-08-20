export declare const generateId: () => string;
export declare const hashPassword: (password: string) => Promise<string>;
export declare const comparePassword: (password: string, hash: string) => Promise<boolean>;
export declare const generateToken: (userId: string) => string;
export declare const verifyToken: (token: string) => {
    userId: string;
};
export declare const generateShareToken: () => string;
//# sourceMappingURL=auth.d.ts.map