import { getDatabase } from './db';
import { generateId, hashPassword } from '../utils/auth';

const seedData = async () => {
  const db = getDatabase();
  
  // Initialize database first
  await db.init();
  
  console.log('Seeding database...');
  
  // Create a sample user
  const userId = generateId();
  const hashedPassword = await hashPassword('password123');
  
  await db.run(`
    INSERT OR IGNORE INTO users (id, name, email, role, hashed_password) 
    VALUES (?, ?, ?, ?, ?)
  `, [userId, 'Demo Instructor', 'demo@example.com', 'instructor', hashedPassword]);
  
  // Sample WCS patterns and exercises
  const patterns = [
    {
      id: generateId(),
      type: 'pattern',
      name: 'Sugar Push',
      aliases: ['Basic', 'Starter'],
      level: 'beginner',
      description: 'The fundamental pattern in West Coast Swing',
      steps: ['Leader steps back on 1', 'Follower steps forward on 1', 'Compress on 2', 'Stretch on 3&4', 'Anchor on 5&6'],
      counts: ['1&2', '3&4', '5&6'],
      holds: ['Two-hand hold'],
      slot: 'Linear',
      rotations: [],
      prerequisites: [],
      related: ['Left Side Pass', 'Right Side Pass'],
      teachingPoints: ['Focus on compression and stretch', 'Maintain slot direction', 'Clear anchor'],
      commonMistakes: ['Pulling too hard', 'Breaking frame', 'Rushing the timing'],
      variations: ['Single hand', 'Closed position'],
      estimatedMinutes: 10,
      bpmRange: { min: 88, max: 100 },
      tags: ['fundamental', 'basic', 'compression'],
      mediaLinks: [],
      createdBy: userId
    },
    {
      id: generateId(),
      type: 'pattern',
      name: 'Left Side Pass',
      aliases: ['LSP'],
      level: 'beginner',
      description: 'Follower passes to the left side of the leader',
      steps: ['Leader steps back on 1', 'Lead follower to left side', 'Follower travels on 3&4', 'Anchor on 5&6'],
      counts: ['1&2', '3&4', '5&6'],
      holds: ['Right to left hand'],
      slot: 'Linear',
      rotations: [],
      prerequisites: ['Sugar Push'],
      related: ['Right Side Pass', 'Sugar Push'],
      teachingPoints: ['Clear directional lead', 'Maintain connection during travel', 'Slot integrity'],
      commonMistakes: ['Leading too early', 'Follower stepping out of slot', 'Losing connection'],
      variations: ['Hand change on 4', 'Open position'],
      estimatedMinutes: 15,
      bpmRange: { min: 90, max: 105 },
      tags: ['fundamental', 'travel', 'pass'],
      mediaLinks: [],
      createdBy: userId
    },
    {
      id: generateId(),
      type: 'pattern',
      name: 'Right Side Pass',
      aliases: ['RSP'],
      level: 'beginner',
      description: 'Follower passes to the right side of the leader',
      steps: ['Leader steps back on 1', 'Lead follower to right side', 'Follower travels on 3&4', 'Anchor on 5&6'],
      counts: ['1&2', '3&4', '5&6'],
      holds: ['Left to right hand'],
      slot: 'Linear',
      rotations: [],
      prerequisites: ['Sugar Push', 'Left Side Pass'],
      related: ['Left Side Pass', 'Sugar Push'],
      teachingPoints: ['Opposite direction from LSP', 'Hand change technique', 'Smooth transition'],
      commonMistakes: ['Confusing with LSP', 'Poor hand connection', 'Rushing the pass'],
      variations: ['No hand change', 'Extended travel'],
      estimatedMinutes: 15,
      bpmRange: { min: 90, max: 105 },
      tags: ['fundamental', 'travel', 'pass'],
      mediaLinks: [],
      createdBy: userId
    },
    {
      id: generateId(),
      type: 'pattern',
      name: 'Whip',
      aliases: ['Basic Whip'],
      level: 'improver',
      description: 'Follower performs a controlled rotation',
      steps: ['Setup like sugar push', 'Lead into whip position on 3', 'Follower rotates on 4', 'Complete rotation on 5&6'],
      counts: ['1&2', '3&4', '5&6'],
      holds: ['Two-hand to one-hand'],
      slot: 'Linear',
      rotations: ['180 degree turn'],
      prerequisites: ['Sugar Push', 'Left Side Pass', 'Right Side Pass'],
      related: ['Whip variations', 'Tuck Turn'],
      teachingPoints: ['Control the rotation speed', 'Maintain slot', 'Proper frame'],
      commonMistakes: ['Over-rotating', 'Losing slot', 'Too fast rotation'],
      variations: ['Inside roll', 'Outside roll', 'Double whip'],
      estimatedMinutes: 20,
      bpmRange: { min: 95, max: 110 },
      tags: ['rotation', 'improver', 'whip'],
      mediaLinks: [],
      createdBy: userId
    },
    {
      id: generateId(),
      type: 'exercise',
      name: 'Anchor Exercise',
      aliases: ['Anchor Drill'],
      level: 'beginner',
      description: 'Practicing the anchor step timing and quality',
      steps: ['Step on 5', 'Replace weight on &', 'Step forward on 6'],
      counts: ['5&6'],
      holds: [],
      slot: 'Linear',
      rotations: [],
      prerequisites: [],
      related: ['Sugar Push', 'Connection Exercise'],
      teachingPoints: ['Clear weight changes', 'Stay in slot', 'Quality over speed'],
      commonMistakes: ['Rushing the timing', 'Poor weight placement', 'Moving out of slot'],
      variations: ['With music', 'Eyes closed', 'Different speeds'],
      estimatedMinutes: 8,
      bpmRange: { min: 85, max: 95 },
      tags: ['technique', 'anchor', 'timing'],
      mediaLinks: [],
      createdBy: userId
    },
    {
      id: generateId(),
      type: 'exercise',
      name: 'Connection Exercise',
      aliases: ['Frame Drill', 'Compression/Stretch'],
      level: 'beginner',
      description: 'Building awareness of compression and stretch',
      steps: ['Face partner in frame', 'Practice compression', 'Practice stretch', 'Feel the elasticity'],
      counts: [],
      holds: ['Two-hand hold'],
      slot: 'Stationary',
      rotations: [],
      prerequisites: [],
      related: ['Sugar Push', 'Anchor Exercise'],
      teachingPoints: ['Feel the resistance', 'Maintain frame', 'Breath and relax'],
      commonMistakes: ['Too much tension', 'Collapsing frame', 'Not following'],
      variations: ['One hand', 'Eyes closed', 'With movement'],
      estimatedMinutes: 10,
      bpmRange: { min: 80, max: 90 },
      tags: ['technique', 'connection', 'frame'],
      mediaLinks: [],
      createdBy: userId
    }
  ];
  
  // Insert patterns
  for (const pattern of patterns) {
    await db.run(`
      INSERT OR IGNORE INTO patterns_exercises (
        id, type, name, aliases, level, description, steps, counts, holds, slot,
        rotations, prerequisites, related, teaching_points, common_mistakes,
        variations, estimated_minutes, bpm_range_min, bpm_range_max, tags,
        media_links, created_by
      ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    `, [
      pattern.id, pattern.type, pattern.name, JSON.stringify(pattern.aliases),
      pattern.level, pattern.description, JSON.stringify(pattern.steps),
      JSON.stringify(pattern.counts), JSON.stringify(pattern.holds), pattern.slot,
      JSON.stringify(pattern.rotations), JSON.stringify(pattern.prerequisites),
      JSON.stringify(pattern.related), JSON.stringify(pattern.teachingPoints),
      JSON.stringify(pattern.commonMistakes), JSON.stringify(pattern.variations),
      pattern.estimatedMinutes, pattern.bpmRange.min, pattern.bpmRange.max,
      JSON.stringify(pattern.tags), JSON.stringify(pattern.mediaLinks), pattern.createdBy
    ]);
  }
  
  console.log('Database seeded successfully!');
  console.log(`Created user: demo@example.com / password123`);
  console.log(`Created ${patterns.length} patterns and exercises`);
};

// Run if called directly
if (require.main === module) {
  seedData().catch(console.error);
}

export default seedData;