# West Coast Swing Course Creator - MVP

En applikation för att skapa danskurser och hantera kursverksamheten för West Coast Swing.

## 🎯 MVP Status - FUNCTIONAL & READY TO TEST!

This is a **working MVP (Minimum Viable Product)** that demonstrates the core concept and foundation for the West Coast Swing Course Creator application. 

### ✅ Currently Implemented Features

**🔐 Authentication System**
- Secure JWT-based authentication
- User registration and login
- Protected routes and role-based access

**📚 Pattern Library**
- Browse and search through WCS patterns and exercises
- Detailed pattern information including:
  - Step-by-step instructions
  - Teaching points for instructors
  - Common mistakes to watch for
  - BPM ranges and timing (counts)
  - Prerequisites and related patterns
  - Tags for easy categorization
- Advanced filtering by:
  - Pattern type (patterns vs exercises)
  - Difficulty level (beginner, improver, intermediate, advanced)
  - Search by name, description, or tags
- Responsive design that works on desktop and mobile

**📊 Dashboard Overview**
- Library statistics and metrics
- Quick action cards for easy navigation
- Clear MVP status and roadmap

### 🚧 Next Phase Features (Coming Soon)

- **Lesson Builder**: Create structured lesson plans with drag-and-drop
- **Course Planning**: Design multi-week course curricula with progression tracking
- **Export Functionality**: Export to PDF, Markdown, and HTML formats
- **Template System**: Save and reuse lesson/course templates
- **Team Collaboration**: Share and collaborate on course materials

## 🚀 Getting Started

### Prerequisites
- Node.js (v14 or higher)
- npm

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/pownas/DanceCourseCreator.git
   cd DanceCourseCreator
   ```

2. **Install dependencies**
   ```bash
   npm install
   cd server && npm install
   cd ../client && npm install
   cd ..
   ```

3. **Initialize and seed the database**
   ```bash
   cd server && npm run db:seed
   cd ..
   ```

4. **Start the application**
   ```bash
   npm run dev
   ```

   This will start both the backend server (port 3001) and frontend client (port 3000).

5. **Access the application**
   - Open your browser to: http://localhost:3000
   - Login with demo credentials:
     - **Email:** demo@example.com
     - **Password:** password123

## 🏗️ Technical Architecture

### Backend (Server)
- **Node.js** with **Express** framework
- **TypeScript** for type safety
- **SQLite** database for simplicity (easily upgradeable to PostgreSQL)
- **JWT** authentication
- RESTful API design
- Rich data models for patterns, lessons, courses, and users

### Frontend (Client)
- **React 18** with **TypeScript**
- **Material-UI** for responsive, professional design
- **React Router** for navigation
- **Axios** for API communication
- Context-based state management for authentication

### Database Schema
The application uses a comprehensive data model that supports:
- Users and teams
- Patterns and exercises with rich metadata
- Lessons with sections and timing
- Courses with progression tracking
- Templates and versioning
- Share links and collaboration

## 📚 Sample Data

The MVP comes pre-loaded with authentic West Coast Swing content:

**Patterns:**
- Sugar Push (The fundamental WCS pattern)
- Left Side Pass
- Right Side Pass  
- Whip (Improver level)

**Exercises:**
- Anchor Exercise (timing and quality practice)
- Connection Exercise (compression/stretch awareness)

Each pattern includes detailed teaching information, common mistakes, variations, and proper metadata.

## 🎓 User Journey

1. **Login** with provided demo credentials
2. **Explore the Dashboard** to see library overview and statistics
3. **Browse Pattern Library** to see all available patterns and exercises
4. **Filter and Search** patterns by level, type, or keywords
5. **View Pattern Details** by clicking on any pattern card
6. **Test Responsive Design** by resizing your browser window

## 🛠️ Development

### Project Structure
```
DanceCourseCreator/
├── server/           # Backend API
│   ├── src/
│   │   ├── controllers/
│   │   ├── models/
│   │   ├── routes/
│   │   ├── middleware/
│   │   ├── database/
│   │   └── utils/
│   └── package.json
├── client/           # Frontend React app
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── context/
│   │   ├── api/
│   │   └── types/
│   └── package.json
└── package.json      # Root package for dev scripts
```

### Available Scripts

**Root level:**
- `npm run dev` - Start both server and client in development mode
- `npm run build` - Build both server and client for production

**Server:**
- `npm run dev` - Start server with hot reload
- `npm run build` - Compile TypeScript to JavaScript
- `npm run db:seed` - Populate database with sample data

**Client:**
- `npm start` - Start React development server
- `npm run build` - Build optimized production bundle

## 🎯 MVP Validation

This MVP successfully demonstrates:

1. **Technical Feasibility** - Full-stack TypeScript application with proper architecture
2. **User Experience** - Intuitive interface that WCS instructors can immediately understand
3. **Data Model Completeness** - Rich metadata structure that captures instructor needs
4. **Scalability Foundation** - Clean architecture ready for additional features
5. **Real-world Applicability** - Authentic dance content that instructors would actually use

## 🔮 Future Roadmap

### Phase 2: Lesson Building
- Drag-and-drop lesson builder
- Time management and validation
- Section-based organization
- Progression recommendations

### Phase 3: Course Planning  
- Multi-week course design
- Coverage tracking and analytics
- Prerequisite validation
- Student progression modeling

### Phase 4: Collaboration & Export
- PDF/Markdown export with templates
- Team sharing and collaboration
- Template marketplace
- Integration with calendar systems

## 🤝 Contributing

This MVP provides a solid foundation for further development. Key areas for contribution:

1. **UI/UX Improvements** - Enhanced drag-and-drop interfaces
2. **Additional Features** - Lesson builder, course planner, export functionality  
3. **Content Expansion** - More patterns, exercises, and educational content
4. **Mobile Optimization** - Progressive Web App (PWA) capabilities
5. **Integration** - Calendar systems, music services, video platforms

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Ready to revolutionize West Coast Swing education! 💃🕺**
