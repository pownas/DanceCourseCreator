"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.authorize = exports.authenticate = void 0;
const auth_1 = require("../utils/auth");
const db_1 = require("../database/db");
const authenticate = async (req, res, next) => {
    try {
        const token = req.header('Authorization')?.replace('Bearer ', '');
        if (!token) {
            return res.status(401).json({ error: 'Access denied. No token provided.' });
        }
        const decoded = (0, auth_1.verifyToken)(token);
        const db = (0, db_1.getDatabase)();
        const user = await db.get('SELECT id, name, email, role, team_id FROM users WHERE id = ?', [decoded.userId]);
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
    }
    catch (error) {
        res.status(401).json({ error: 'Invalid token.' });
    }
};
exports.authenticate = authenticate;
const authorize = (roles) => {
    return (req, res, next) => {
        if (!req.user || !roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Access denied.' });
        }
        next();
    };
};
exports.authorize = authorize;
//# sourceMappingURL=auth.js.map