# Changelog

All notable changes to the DanceCourseCreator project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] - 2025-11-29

### Added
- **DanceCourseCreator.Web** - New Blazor Server web application replacing the WebAssembly client
  - Server-side rendering with SignalR for interactive components
  - Improved startup performance and lower client requirements
  - Full .NET API access without WebAssembly constraints
  - Better SEO support with server-side rendering
- Error boundaries around MudBlazor theme provider to prevent full app crashes
- Fallback to default MudBlazor theme if custom theme fails to load
- Application startup lifecycle logging to track start times
- Aspire service discovery configuration for API communication
- Hosted service to display clean startup completion message in AppHost

### Changed
- **Migrated from Blazor WebAssembly to Blazor Server**
  - All pages, components, and services moved to `DanceCourseCreator.Web`
  - Updated namespaces from `DanceCourseCreator.Client` to `DanceCourseCreator.Web`
  - Removed WebAssembly-specific dependencies and code
- AppHost now references `DanceCourseCreator.Web` instead of `DanceCourseCreator.Client`
- API client configuration updated to use Aspire service discovery with fallback to localhost
- Database seeding now checks for existing data to prevent duplicate key errors on restart

### Fixed
- **MudBlazor theme IndexOutOfRangeException** - Expanded `Shadows.Elevation` array to all 25 required levels (0-24)
- Build warnings suppressed:
  - Removed unused `hasError` fields from Pages (Patterns, Templates, Home, Courses, Lessons)
  - Fixed MudBlazor analyzer warnings for `Title` attribute on `MudIconButton` components
  - Fixed MudBlazor analyzer warnings for `Checked` and `CheckedChanged` on `MudSwitch` components
  - Removed obsolete `WithOpenApi()` usage in favor of updated API documentation methods
- API startup crash due to duplicate `LessonSection` IDs in database seeder
- Logging configuration to suppress noisy messages:
  - DataProtection key storage warnings
  - Aspire port forwarding informational messages
  - DCP (Distributed Application Control Plane) warnings
- Health check endpoints configured properly for Aspire dashboard resource tracking

### Removed
- **DanceCourseCreator.Client** (WebAssembly project) - Replaced by `DanceCourseCreator.Web`
  - WebAssembly-specific dependencies removed
  - Browser-side rendering replaced with server-side rendering

## Technical Details

### Architecture Changes
- **Before**: Blazor WebAssembly (runs entirely in browser)
- **After**: Blazor Server (UI updates via SignalR, logic on server)

### Build Status
- ✅ 0 Errors
- ✅ 0 Warnings
- All projects compile successfully

### Projects Structure
```
DanceCourseCreator.sln
├── DanceCourseCreator.API          (Backend API)
├── DanceCourseCreator.Web          (Blazor Server - NEW)
├── DanceCourseCreator.AppHost      (Aspire orchestration)
├── DanceCourseCreator.ServiceDefaults
└── DanceCourseCreator.Tests.E2E
```

### Migration Notes
- All UI components, pages, and services successfully migrated
- MudBlazor theme with 25 elevation levels (Material Design standard)
- Authentication and authorization state management preserved
- Custom theme with WCAG 2.1 AA compliant colors maintained
- Error handling improved with proper error boundaries
