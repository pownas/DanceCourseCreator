# Dance Course Creator - .NET 8

En applikation för att skapa danskurser och hantera kursverksamheten för West Coast Swing, nu implementerad med .NET 8 Blazor WebAssembly frontend och Web API backend.

## 🎯 Status - FULLY FUNCTIONAL APPLICATION!

This is a **complete, functional application** built with modern .NET 8 technology stack, successfully converted from the original TypeScript/React/Node.js implementation.

### ✅ Implemented Features

**🔐 Authentication & Authorization**
- JWT-based authentication with secure token handling
- User registration and login system
- Role-based access control (Instructor, Editor, Reader, Admin)
- Protected routes and authorization policies

**📚 Pattern & Exercise Library**
- Complete CRUD operations for West Coast Swing patterns and exercises
- Rich metadata system including:
  - Step-by-step instructions and descriptions
  - Teaching points and common mistakes
  - BPM ranges, timing, and counts
  - Prerequisites and skill levels
  - Tags and categorization
- Advanced search and filtering capabilities:
  - Filter by pattern type (patterns vs exercises)
  - Filter by dance level (beginner, improver, intermediate, advanced)
  - Full-text search across names and descriptions
  - Tag-based filtering
- Responsive Material Design UI with MudBlazor components

**📝 Lesson Management**
- Create and manage individual lesson plans
- Structure lessons with sections and timing
- Link patterns and exercises to lessons
- Lesson planning and organization tools

**🎓 Course Management** 
- Design and manage multi-week course series
- Track course progression and coverage
- Associate lessons with courses
- Course-level planning and oversight

**📊 Database & API**
- SQLite database with Entity Framework Core
- RESTful API with comprehensive endpoints
- Automatic database creation and seeding
- Health check and monitoring endpoints
- Swagger API documentation

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022, VS Code with C# extension, or any preferred editor

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/pownas/DanceCourseCreator.git
   cd DanceCourseCreator
   ```

2. **Build the solution**
   ```bash
   dotnet build src/DanceCourseCreator.API
   dotnet build src/DanceCourseCreator.Client
   ```

3. **Start the API backend (Terminal 1)**
   ```bash
   cd src/DanceCourseCreator.API
   dotnet run
   ```
   API will be available at: https://localhost:7177

4. **Start the Blazor client (Terminal 2)**
   ```bash
   cd src/DanceCourseCreator.Client
   dotnet run
   ```
   Client application will be available at: https://localhost:5001

5. **Access the application**
   - Open your browser to: https://localhost:5001
   - The database will be automatically created with sample data on first run
   - Register a new account or use the application immediately

## 🏗️ Technical Architecture

### .NET 8 Technology Stack
- **Frontend**: Blazor WebAssembly 8.0 with MudBlazor Material Design components
- **Backend**: .NET 8 Web API with Entity Framework Core
- **Database**: SQLite with Entity Framework Core (easily upgradeable to PostgreSQL/SQL Server)
- **Authentication**: JWT with BCrypt password hashing
- **API Documentation**: Swagger/OpenAPI with interactive testing interface

### Project Structure
```
DanceCourseCreator/
├── src/
│   ├── DanceCourseCreator.API/          # .NET 8 Web API Backend
│   │   ├── Controllers/                 # API Controllers
│   │   ├── Models/                      # Entity Models
│   │   ├── Data/                        # DbContext & Database
│   │   ├── Services/                    # Business Services
│   │   ├── DTOs/                        # Data Transfer Objects
│   │   └── Program.cs                   # API Startup
│   └── DanceCourseCreator.Client/       # Blazor WebAssembly Frontend
│       ├── Pages/                       # Razor Pages/Components
│       ├── Components/                  # Reusable UI Components
│       ├── Services/                    # HTTP Client Services
│       ├── Models/                      # Client-side Models
│       ├── Layout/                      # Application Layout
│       └── Program.cs                   # Client Startup
├── legacy/                              # Original TypeScript/React code (preserved)
│   ├── server/                          # Node.js/Express backend
│   └── client/                          # React frontend
├── docs/                                # Documentation
├── Kravspecifikation.md                 # Requirements specification (Swedish)
└── DanceCourseCreator.slnx             # .NET Solution file
```

### Database Schema
The application uses a comprehensive relational data model supporting:
- **Users** - Authentication, authorization, and user profiles
- **Teams** - Group management and collaboration
- **Patterns/Exercises** - West Coast Swing dance content with rich metadata
- **Lessons** - Individual teaching sessions with sections and timing
- **Courses** - Multi-week series with progression tracking
- **Templates** - Reusable content structures
- **Share Links** - Content sharing and collaboration features

### UI Components
Built with MudBlazor for consistent Material Design:
- Responsive navigation with collapsible drawer
- Data tables with sorting, filtering, and pagination
- Forms with validation and error handling
- Modal dialogs and confirmation prompts
- Snackbar notifications and user feedback
- Light/dark theme support

## 📚 Sample Data & Content

The application comes with authentic West Coast Swing content and sample data:

**Pattern Library includes:**
- **Sugar Push** - The fundamental WCS pattern with detailed instruction
- **Left Side Pass** - Essential pattern with variations and teaching points
- **Right Side Pass** - Core pattern with timing and connection notes
- **Whip** - Improver level pattern with anchor and stretch concepts

**Exercise Collection:**
- **Anchor Exercise** - Timing, quality, and connection practice
- **Connection Exercise** - Compression and stretch awareness drills
- **Timing Exercises** - BPM practice and musical interpretation

Each pattern and exercise includes:
- Step-by-step instructions
- Teaching points and common mistakes
- Proper metadata (level, timing, prerequisites)
- Tags for easy categorization and search

## 🎓 User Experience

### Getting Started Journey
1. **Launch Application** - Access the Blazor WebAssembly interface
2. **User Registration** - Create account with secure authentication
3. **Explore Dashboard** - Overview of library statistics and quick actions
4. **Browse Pattern Library** - View all available patterns and exercises
5. **Advanced Search** - Filter by level, type, tags, or search terms
6. **Pattern Details** - View comprehensive information for each pattern
7. **Lesson Planning** - Create and organize lesson plans
8. **Course Management** - Design multi-week course series

### Key Features in Use
- **Responsive Design** - Works seamlessly on desktop, tablet, and mobile
- **Real-time Search** - Instant filtering and search results
- **Material Design** - Clean, professional interface with MudBlazor
- **Role-based Access** - Different permissions for different user types

## 🛠️ Development

### Available Commands

**API Development:**
```bash
cd src/DanceCourseCreator.API
dotnet run                    # Start API with hot reload
dotnet build                  # Build API project
dotnet test                   # Run API tests (if available)
```

**Client Development:**
```bash
cd src/DanceCourseCreator.Client
dotnet run                    # Start Blazor WebAssembly with hot reload
dotnet build                  # Build client project
```

**Solution Level:**
```bash
dotnet build                  # Build entire solution
dotnet clean                  # Clean build artifacts
```

### End-to-End Testing with Playwright

The application includes comprehensive Playwright E2E tests with automatic screenshot capture for documentation and regression testing.

**Setup and Running Tests:**
```bash
cd src/DanceCourseCreator.Tests.E2E

# First time setup - install Playwright browsers
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install chromium

# Run all tests
dotnet test

# Run specific test categories
dotnet test --filter "TestCategory=Navigation"
dotnet test --filter "TestCategory=Patterns"
dotnet test --filter "TestCategory=Courses"
```

**Test Coverage:**
- **Home & Navigation** - Main page and navigation flows
- **Pattern Library** - Browsing, filtering, and searching patterns
- **Course Creation** - Complete course creation workflow
- **Course Editing** - Modifying existing courses
- **Lessons & Templates** - Lesson and template management

See [E2E Test Documentation](src/DanceCourseCreator.Tests.E2E/README.md) for detailed information on test structure, categories, and screenshot organization.

### API Documentation & Testing

When running the API in development mode:
- **Swagger UI**: https://localhost:7177/swagger - Interactive API documentation and testing
- **Health Check**: https://localhost:7177/api/health - API status endpoint
- **OpenAPI Spec**: https://localhost:7177/swagger/v1/swagger.json - API specification

### Database Management

The application uses Entity Framework Core with SQLite:
- Database is automatically created on first run
- Located at `src/DanceCourseCreator.API/database.sqlite`
- Schema migrations handled automatically
- Sample data seeded automatically

### Development Tools & Extensions

**Visual Studio Code:**
- C# extension by Microsoft
- .NET Install Tool
- Blazor syntax highlighting

**Visual Studio 2022:**
- Full .NET 8 support
- Integrated debugging and testing
- Built-in Blazor development tools

## 🎯 Current Implementation Status

This application successfully demonstrates:

### ✅ Fully Implemented Features
1. **Complete Authentication System** - JWT-based with user registration and role management
2. **Pattern & Exercise Library** - Full CRUD operations with rich metadata and search
3. **Lesson Management** - Create, organize, and manage individual lesson plans
4. **Course Management** - Design and track multi-week course series
5. **Modern UI/UX** - Responsive Blazor WebAssembly interface with Material Design
6. **RESTful API** - Comprehensive backend with Entity Framework Core
7. **Database Integration** - SQLite with automatic setup and sample data
8. **API Documentation** - Interactive Swagger interface for testing and integration

### 🚀 Technical Achievements
1. **Successful Technology Migration** - Complete conversion from TypeScript/React/Node.js to .NET 8
2. **Modern Architecture** - Clean separation between API and client with proper dependency injection
3. **Production Ready** - Built with enterprise-grade .NET technologies and best practices
4. **Developer Experience** - Hot reload, comprehensive tooling, and clear documentation
5. **Maintainable Codebase** - Well-structured with proper separation of concerns

## 🔮 Future Enhancement Opportunities

Based on the requirements specification (see `Kravspecifikation.md`), potential areas for expansion include:

### Phase 1: Enhanced Functionality
- **Advanced Export System** - PDF generation with custom templates
- **Template Marketplace** - Share and discover lesson/course templates
- **Team Collaboration** - Enhanced sharing and permission management
- **Advanced Analytics** - Usage patterns and performance insights

### Phase 2: Integration & Automation
- **Calendar Integration** - Sync with Google Calendar, Outlook
- **Music Integration** - YouTube/Spotify playlist management
- **Mobile App** - Native mobile applications
- **Offline Capabilities** - Progressive Web App features

### Phase 3: Educational Features
- **Student Progress Tracking** - Individual student development monitoring
- **Video Integration** - Embedded instructional videos
- **Assessment Tools** - Skill evaluation and certification tracking
- **Community Features** - Forums, feedback, and instructor networking

## 🤝 Contributing

This application provides a solid foundation for West Coast Swing education technology. Areas for contribution:

### Development Priorities
1. **Feature Enhancement** - Implement advanced lesson building and course planning tools
2. **UI/UX Improvements** - Enhanced drag-and-drop interfaces and user experience
3. **Content Expansion** - More patterns, exercises, and educational content
4. **Integration Development** - Calendar systems, music services, video platforms
5. **Mobile Optimization** - Progressive Web App capabilities and mobile-specific features

### Getting Started with Development
1. Fork the repository
2. Set up the .NET 8 development environment
3. Create a feature branch
4. Make your changes with appropriate tests
5. Submit a pull request with clear description

### Code Standards
- Follow .NET coding conventions and best practices
- Use Entity Framework Core for database operations
- Implement proper error handling and validation
- Include unit tests for new functionality
- Maintain responsive design principles

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔄 Migration from Legacy

**Note**: This is the modern .NET 8 version of Dance Course Creator. The original TypeScript/React/Node.js implementation has been preserved in the `legacy/` directory for reference and historical purposes.

The migration maintained:
- ✅ All existing functionality and features
- ✅ Database schema compatibility
- ✅ API endpoint structure and contracts
- ✅ User interface design and user experience
- ✅ Authentication and authorization flow
- ✅ Data validation rules and business logic

---

**Ready to revolutionize West Coast Swing education with modern .NET technology! 💃🕺**
