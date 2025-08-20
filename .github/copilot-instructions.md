# DanceCourseCreator

A Swedish dance course creation application for West Coast Swing, designed to help instructors plan, structure, and share lesson and course plans by selecting and combining patterns/exercises per course session.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Current Repository State

**CRITICAL**: This repository is currently in documentation-only phase. No application code has been implemented yet.

- Repository contains: Requirements specification (Kravspecifikation.md), README.md, LICENSE, .gitignore
- The .gitignore indicates this will be a .NET/Visual Studio project (C#)
- No build system, project files, or source code exists yet
- No CI/CD pipelines are set up

## Working Effectively

### Repository Setup and Navigation
- **Current files to reference**:
  - `Kravspecifikation.md` - Comprehensive requirements specification in Swedish (16KB+)
  - `README.md` - Basic project description
  - `.gitignore` - Configured for .NET/Visual Studio projects

### Understanding the Project Requirements
- **ALWAYS** read `Kravspecifikation.md` first when working on this project
- The application will be a web-based SPA/SSR for creating West Coast Swing dance course materials
- Key features: pattern/exercise library, lesson builder, course planning, templates, export/sharing
- Target users: Dance instructors, course coordinators, dance schools
- Technical direction: Web app (SPA/SSR), REST/GraphQL API, relational database

### When Code Is Added

Since no code exists yet, these instructions will apply once development begins:

#### Expected .NET Development Setup
Based on the .gitignore configuration, expect the following setup commands:
- Install .NET SDK: `dotnet --version` (verify installation) -- Current environment has .NET 8.0.119
- Create project structure: `dotnet new sln -n DanceCourseCreator`
- Build: `dotnet build` -- NEVER CANCEL. Simple builds take ~15 seconds, complex builds may take 5-10 minutes. Set timeout to 15+ minutes.
- Test: `dotnet test` -- NEVER CANCEL. Allow 2-5 minutes for test execution. Set timeout to 10+ minutes.
- Run: `dotnet run` (for console apps) or `dotnet watch run` (for web apps with hot reload)

#### Alternative Technology Stacks
If a different technology stack is chosen:
- **Node.js/TypeScript**: Look for `package.json`, use `npm install` (~1-2 minutes), `npm run build`, `npm run test`
- **Python**: Look for `requirements.txt` or `pyproject.toml`, use `pip install -r requirements.txt`
- **Docker**: Look for `Dockerfile` or `docker-compose.yml`

**Environment Validation**: Current environment includes .NET 8.0.119, Node.js v20.19.4, Python 3.12.3

## Validation

### Current Validation Steps
- Verify all documentation files are present and readable: `file Kravspecifikation.md README.md LICENSE .gitignore`
- Check that .gitignore excludes appropriate build artifacts and IDE files
- Ensure requirements specification is comprehensive and up-to-date: `wc -c Kravspecifikation.md` (should be ~16KB)
- Verify no project files exist yet: `find . -name "*.csproj" -o -name "*.sln" -o -name "package.json"` (should return empty)
- Check available development tools: `dotnet --version && node --version && python3 --version`

### Future Validation Steps (when code exists)
- ALWAYS manually test the core user workflows after making changes:
  1. Creating a new pattern/exercise in the library
  2. Building a lesson plan with multiple patterns
  3. Creating a multi-session course plan
  4. Exporting lesson plans to PDF/Markdown
  5. Sharing lesson plans with other instructors
- Build and run the application completely before validating changes
- Test both web interface and API endpoints if applicable
- Verify database migrations and seed data work correctly

## Common Tasks

### Current Repository Contents
```
.
├── .git/
├── .github/
│   └── copilot-instructions.md
├── .gitignore (7KB+ .NET/Visual Studio configuration)
├── Kravspecifikation.md (16KB+ comprehensive requirements)
├── LICENSE (MIT License)
└── README.md (Basic description in Swedish)
```

### Key Information from Requirements Document

#### Application Purpose
- Create and manage lesson plans for West Coast Swing dance courses
- Support pattern/exercise library with rich metadata
- Enable course progression planning and template reuse
- Provide export capabilities (PDF/Markdown/HTML)

#### Core Entities (from data model section)
- **Patterns/Exercises**: Sugar Push, Left Side Pass, Right Side Pass, Whip, Tuck Turn, connection drills, etc.
- **Lessons**: 75-90 minute sessions with structured sections
- **Courses**: Multi-week series with progression tracking  
- **Templates**: Reusable lesson and course structures
- **Users/Teams**: Role-based access (Instructor, Editor, Reader, Admin)

**Pattern Examples in Requirements**: The document contains detailed examples of specific West Coast Swing patterns including Sugar Push, Left Side Pass, Right Side Pass, and Whip variations. Use `grep -n "Sugar Push\|Left Side Pass\|Whip" Kravspecifikation.md` to find specific references.

#### Technical Architecture (proposed)
- Frontend: Responsive web app (SPA/SSR), possible PWA
- Backend: REST/GraphQL API with relational database
- Authentication: JWT/OAuth2 with role-based access
- Export service: Server-side PDF/Markdown rendering

### Development Priorities (from requirements checklist)
1. Pattern/exercise library with metadata (FR-001..004)
2. Lesson builder with time sections and validation (FR-010..015)  
3. Course planning with progression coverage (FR-020..023)
4. Templates, versioning, comments (FR-030..032)
5. Export and sharing capabilities (FR-050..052)
6. Import functionality and team permissions (FR-060, FR-070..071)
7. Reports and insights (FR-080..082)

## Important Notes

- **Language**: Requirements and documentation are in Swedish
- **Domain**: West Coast Swing dance instruction (specialized terminology)
- **No Code Yet**: All instructions above are preparatory for future development
- **Technology Stack**: Not finalized, but .gitignore suggests .NET preference

## Next Steps for Development

When beginning implementation:
1. Choose and set up the technology stack
2. Create project structure and basic scaffolding
3. Set up database schema based on data model in requirements
4. Implement core entities (patterns, lessons, courses)
5. Build basic CRUD operations and API endpoints
6. Create frontend interface for lesson planning
7. Add export functionality
8. Implement user authentication and permissions

REMEMBER: This is currently a planning-phase repository. Always check for actual code files before assuming build/test commands will work.