"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const patternsController_1 = require("../controllers/patternsController");
const auth_1 = require("../middleware/auth");
const router = express_1.default.Router();
// All routes require authentication
router.use(auth_1.authenticate);
router.post('/', (0, auth_1.authorize)(['instructor', 'editor', 'admin']), patternsController_1.createPatternOrExercise);
router.get('/', patternsController_1.getAllPatternsAndExercises);
router.get('/:id', patternsController_1.getPatternOrExercise);
router.put('/:id', (0, auth_1.authorize)(['instructor', 'editor', 'admin']), patternsController_1.updatePatternOrExercise);
router.delete('/:id', (0, auth_1.authorize)(['instructor', 'editor', 'admin']), patternsController_1.deletePatternOrExercise);
exports.default = router;
//# sourceMappingURL=patterns.js.map