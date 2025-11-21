# ?? Blogy - Modern Blog Platform with AI Integration

[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![C# Version](https://img.shields.io/badge/C%23-13.0-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)

A professional blog management platform built with **ASP.NET Core 9.0**, featuring **AI-powered content generation** and **automated comment moderation** using OpenAI APIs.

![Blogy Platform](https://via.placeholder.com/1200x400/4A90E2/ffffff?text=Blogy+-+AI-Powered+Blog+Platform)

## ?? Key Features

### ?? AI Integration
- **AI Content Generation**: Automatically generate blog articles using OpenAI GPT models
- **Smart Comment Moderation**: Real-time toxicity detection with OpenAI Moderation API
- **Automated About Text**: AI-generated company descriptions

### ?? Role-Based Access Control
- **Admin Panel**: Full system control, user management, content moderation
- **Writer Panel**: Create and manage own blog posts
- **User Panel**: Profile management, commenting system

### ?? Blog Management
- Create, read, update, delete blog posts
- Multiple image support (cover + 2 additional images)
- Category and tag organization
- SEO-friendly URLs
- Pagination support

### ?? Advanced Comment System
- User authentication required
- AI-powered toxicity filtering
- Rate limit handling with fallback mechanism
- Comment approval workflow

### ?? Dashboard & Analytics
- Blog statistics
- Category distribution charts
- Daily activity tracking
- User engagement metrics

## ??? Architecture

The project follows **N-Tier Architecture** pattern with clean separation of concerns:

```
Blogy/
??? ?? Blogy.Entity/           # Domain Models & Entities
?   ??? Entities/
?       ??? Blog, Category, Tag
?       ??? Comment, AppUser, AppRole
?       ??? About, TeamMember, ContactInfo
?
??? ?? Blogy.DataAccess/       # Data Layer
?   ??? Context/               # EF Core DbContext
?   ??? Repositories/          # Repository Pattern
?   ??? Migrations/            # Database Migrations
?
??? ?? Blogy.Business/         # Business Logic Layer
?   ??? DTOs/                  # Data Transfer Objects
?   ??? Services/              # Business Services
?   ??? Validators/            # FluentValidation Rules
?   ??? Mappings/              # AutoMapper Profiles
?
??? ?? Blogy.WebUI/            # Presentation Layer
    ??? Areas/                 # Role-based Areas (Admin, Writer, User)
    ??? Controllers/           # MVC Controllers
    ??? Views/                 # Razor Views
    ??? ViewComponents/        # Reusable UI Components
```

## ??? Technologies & Patterns

### Backend
- **Framework**: ASP.NET Core 9.0 MVC
- **ORM**: Entity Framework Core (Code First)
- **Authentication**: ASP.NET Core Identity
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Pagination**: PagedList.Core
- **AI Integration**: OpenAI SDK (Chat & Moderation APIs)

### Design Patterns
- Repository Pattern
- Service Layer Pattern
- DTO Pattern
- Dependency Injection
- Area-based Architecture

### Frontend
- Razor Views
- Bootstrap 5
- AOS (Animate On Scroll)
- jQuery
- Bootstrap Icons

## ?? Database Schema

### Core Entities
- **Blog**: Title, Description, Images, Category, Tags, Writer, Comments
- **Category**: Name, Description, Blog Count
- **Tag**: Name, Blog Count (Many-to-Many with Blog)
- **Comment**: Content, User, Blog, Timestamp
- **AppUser**: Extended Identity User (Name, Surname, Image)
- **AppRole**: Custom Roles (Admin, Writer, User)

### CMS Entities
- **About**: Company information
- **TeamMember**: Team profiles
- **FooterInfo**: Footer content
- **ContactInfo**: Contact details
- **ContactMessage**: User inquiries
- **Social**: Social media links

## ?? Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB or Express)
- OpenAI API Key ([Get one here](https://platform.openai.com/api-keys))

### Installation

1. **Clone the repository**
    ```bash
    git clone https://github.com/emreokanbaskaya1/4-AcademyBlogProject.git
    cd 4-AcademyBlogProject
    ```

2. **Configure OpenAI API**

    Edit `Blogy.WebUI/appsettings.json`:
    ```json
    {
      "OpenAISettings": {
        "ApiKey": "your-openai-api-key-here",
        "Model": "gpt-4",
        "MaxTokens": 500
      }
    }
    ```

3. **Update Database Connection String**

    In `appsettings.json`:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogyDb;Trusted_Connection=true"
      }
    }
    ```

4. **Apply Migrations**
    ```bash
    cd Blogy.WebUI
    dotnet ef database update
    ```

5. **Run the Application**
    ```bash
    dotnet run
    ```

6. **Access the Application**
    - Main Site: `https://localhost:5001`
    - Admin Panel: `https://localhost:5001/Admin/Dashboard`

### Default Credentials
After first migration, create users via Register page and assign roles through Admin panel.

## ?? Project Structure Details

### Blogy.Entity
Domain models following DDD principles with `BaseEntity` for common properties (Id, CreatedDate, UpdatedDate, IsDeleted).

### Blogy.DataAccess
- **Generic Repository**: `IGenericRepository<T>` with common CRUD operations
- **Specific Repositories**: Custom queries for complex operations
- **EF Core Context**: `AppDbContext` with Identity integration

### Blogy.Business
- **Service Layer**: Encapsulates business logic
- **DTOs**: Separate models for Create, Update, and Result operations
- **Validators**: FluentValidation rules for input validation
- **AI Services**:
  - `OpenAIService`: Article generation
  - `ToxicityService`: Comment moderation with retry logic

### Blogy.WebUI
- **Areas**: Separate admin/writer/user interfaces
- **ViewComponents**: Reusable UI elements (Footer, Blog Lists)
- **Filters**: Global validation exception handling
- **Pagination**: PagedList.Core integration

## ?? Key Features Implementation

### AI-Powered Content Generation
```csharp
// Generate blog article from keywords
var article = await _openAIService.GenerateArticleAsync(
    keywords: "ASP.NET Core, AI",
    shortDescription: "Modern web development"
);
```

### Smart Comment Moderation
```csharp
// Check comment toxicity before posting
var result = await _toxicityService.AnalyzeCommentAsync(commentText);
if (result.IsToxic) {
    // Block comment and notify user
}
```

### Rate Limit Handling
Implements exponential backoff with fallback mechanism when OpenAI rate limits are reached.

## ?? Features Breakdown

### Public Features
- ? Browse blog posts with pagination
- ? Filter by category
- ? View blog details with comments
- ? Sidebar with popular posts, categories, tags
- ? Contact form
- ? About page with team members

### User Features
- ? Register and login
- ? Post comments (AI-moderated)
- ? View own comments
- ? Update profile
- ? Change password

### Writer Features
- ? Create blog posts (manual or AI-generated)
- ? Edit/delete own posts
- ? Manage own comments
- ? Upload images

### Admin Features
- ? Full CRUD for all entities
- ? User management
- ? Role assignment
- ? Comment moderation
- ? Dashboard with statistics
- ? CMS content management

## ?? Security Features

- ASP.NET Core Identity authentication
- Role-based authorization
- Cookie-based authentication
- Password hashing
- CSRF protection
- XSS prevention (Razor encoding)
- SQL injection prevention (EF Core parameterization)

## ?? Validation

FluentValidation rules implemented for:
- Blog creation/update (title length, required fields)
- Category management
- Comment content (length, required)
- User registration (email format, password strength)

## ?? Performance Optimizations

- Async/await throughout the application
- EF Core Include() for eager loading
- Pagination to limit query results
- Rate limiting for OpenAI API calls
- Kestrel configuration for large headers

## ?? Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? Author

**Emre Okan Baþkaya**

- GitHub: [@emreokanbaskaya1](https://github.com/emreokanbaskaya1)
- LinkedIn: [Emre Okan Baþkaya](https://linkedin.com/in/emre-okan-baþkaya)

## ?? Acknowledgments

- Built as part of C# Academy Bootcamp Project
- OpenAI for providing powerful AI APIs
- ASP.NET Core community for excellent documentation

## ?? Support

For support, open an issue in the repository or contact via GitHub.

---

? **If you find this project useful, please consider giving it a star!** ?

## ?? Screenshots

### Home Page
![Home Page](https://via.placeholder.com/800x450/4A90E2/ffffff?text=Home+Page)

### Blog Listing
![Blog List](https://via.placeholder.com/800x450/4A90E2/ffffff?text=Blog+Listing)

### Admin Dashboard
![Admin Dashboard](https://via.placeholder.com/800x450/4A90E2/ffffff?text=Admin+Dashboard)

### AI Content Generation
![AI Generation](https://via.placeholder.com/800x450/4A90E2/ffffff?text=AI+Content+Generation)

---

**Project Status**: ? Active Development | Last Updated: December 2024
