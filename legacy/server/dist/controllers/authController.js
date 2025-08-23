"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getProfile = exports.login = exports.register = void 0;
const auth_1 = require("../utils/auth");
const db_1 = require("../database/db");
const register = async (req, res) => {
    try {
        const { name, email, password, role = 'instructor' } = req.body;
        if (!name || !email || !password) {
            return res.status(400).json({ error: 'Name, email, and password are required' });
        }
        const db = (0, db_1.getDatabase)();
        // Check if user already exists
        const existingUser = await db.get('SELECT id FROM users WHERE email = ?', [email]);
        if (existingUser) {
            return res.status(400).json({ error: 'User with this email already exists' });
        }
        const userId = (0, auth_1.generateId)();
        const hashedPassword = await (0, auth_1.hashPassword)(password);
        await db.run('INSERT INTO users (id, name, email, role, hashed_password) VALUES (?, ?, ?, ?, ?)', [userId, name, email, role, hashedPassword]);
        const token = (0, auth_1.generateToken)(userId);
        res.status(201).json({
            user: { id: userId, name, email, role },
            token
        });
    }
    catch (error) {
        console.error('Registration error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.register = register;
const login = async (req, res) => {
    try {
        const { email, password } = req.body;
        if (!email || !password) {
            return res.status(400).json({ error: 'Email and password are required' });
        }
        const db = (0, db_1.getDatabase)();
        const user = await db.get('SELECT id, name, email, role, team_id, hashed_password FROM users WHERE email = ?', [email]);
        if (!user) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }
        const isValidPassword = await (0, auth_1.comparePassword)(password, user.hashed_password);
        if (!isValidPassword) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }
        const token = (0, auth_1.generateToken)(user.id);
        res.json({
            user: {
                id: user.id,
                name: user.name,
                email: user.email,
                role: user.role,
                teamId: user.team_id
            },
            token
        });
    }
    catch (error) {
        console.error('Login error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};
exports.login = login;
const getProfile = async (req, res) => {
    res.json({ user: req.user });
};
exports.getProfile = getProfile;
//# sourceMappingURL=authController.js.map