# Dance Course Creator - .NET 8 Version

En applikation för att skapa danskurser och hantera kursverksamheten för West Coast Swing, nu konverterad till .NET 8 med Blazor WebAssembly frontend och Web API backend.

## 🚀 .NET 8 Architecture

This application has been successfully converted from the original TypeScript/React/Node.js stack to:

- **Frontend**: Blazor WebAssembly 8.0 with MudBlazor UI components
- **Backend**: .NET 8 Web API with Entity Framework Core
- **Database**: SQLite with Entity Framework Core
- **Authentication**: JWT with BCrypt password hashing

## 🏗️ Project Structure

```
DanceCourseCreator/
├── src/
│   ├── DanceCourseCreator.API/          # .NET 8 Web API Backend
│   │   ├── Controllers/                 # API Controllers
│   │   ├── Models/                      # Entity Models
│   │   ├── Data/                        # DbContext
│   │   ├── Services/                    # Business Services
│   │   └── DTOs/                        # Data Transfer Objects
│   └── DanceCourseCreator.Client/       # Blazor WebAssembly Frontend
│       ├── Pages/                       # Razor Pages
│       ├── Components/                  # Reusable Components
│       ├── Services/                    # HTTP Client Services
│       ├── Models/                      # Client-side Models
│       └── Layout/                      # App Layout
├── DanceCourseCreator.sln              # .NET Solution
└── legacy/                             # Original TypeScript/React code
    ├── server/                         # Node.js/Express backend
    ├── client/                         # React frontend
    └── package.json                    # Root package file
```

## 🛠️ Development

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/pownas/DanceCourseCreator.git
   cd DanceCourseCreator
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the API (Terminal 1)**
   ```bash
   cd src/DanceCourseCreator.API
   dotnet run
   ```
   API will be available at: https://localhost:7177

4. **Run the Blazor Client (Terminal 2)**
   ```bash
   cd src/DanceCourseCreator.Client
   dotnet run
   ```
   Client will be available at: https://localhost:5001

### API Documentation

When running in development mode, Swagger documentation is available at:
- https://localhost:7177/swagger

### Health Check

API health endpoint:
- https://localhost:7177/api/health

## 🎯 Converted Features

### ✅ Completed
- User authentication and registration with JWT
- Pattern/Exercise library with CRUD operations
- Role-based authorization (Instructor, Editor, Reader, Admin)
- Responsive Material Design UI with MudBlazor
- Protected routes and navigation
- Search and filtering capabilities
- SQLite database with Entity Framework Core
- RESTful API with proper error handling

### 🔄 Available for Extension
- Lesson planning and management
- Course creation and tracking
- Template system
- Export functionality (PDF/Markdown)
- Team collaboration features
- Advanced reporting and analytics

## 🔐 Authentication

The application uses JWT authentication with the following default roles:
- **Instructor**: Can create and manage their own patterns
- **Editor**: Can edit patterns created by others
- **Reader**: Read-only access
- **Admin**: Full system access

## 📊 Database

The application uses SQLite with Entity Framework Core. The database is automatically created on first run with the following main entities:

- Users (Authentication and authorization)
- Teams (Group management)
- Patterns/Exercises (Dance content)
- Lessons (Individual sessions)
- Courses (Multi-week series)
- Templates (Reusable content)
- Share Links (Content sharing)

## 🎨 UI Components

The application uses MudBlazor for consistent Material Design components:
- Navigation with responsive drawer
- Data tables with sorting and filtering
- Forms with validation
- Modal dialogs
- Snackbar notifications
- Theme support

## 🔄 Migration from Legacy

The conversion from TypeScript/React/Node.js to .NET 8 maintained:
- ✅ All existing functionality
- ✅ Database schema compatibility
- ✅ API endpoint structure
- ✅ User interface design
- ✅ Authentication flow
- ✅ Data validation rules

## 📄 License

MIT License - se [LICENSE](LICENSE) filen för detaljer.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if needed
5. Submit a pull request

---

**Note**: This is the .NET 8 version of Dance Course Creator. The original TypeScript/React/Node.js version is preserved in the `legacy/` directory for reference.