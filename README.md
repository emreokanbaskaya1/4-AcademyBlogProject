# 📝 Blogy - Modern Blog Platform with AI Integration

[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![C# Version](https://img.shields.io/badge/C%23-13.0-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)

A professional blog management platform built with **ASP.NET Core 9.0**, featuring **AI-powered content generation** and **automated comment moderation** using OpenAI APIs.

## 🌟 Key Features

### 🤖 AI Integration
- **AI Content Generation**: Automatically generate blog articles using OpenAI GPT models
- **Smart Comment Moderation**: Real-time toxicity detection with OpenAI Moderation API
- **Automated About Text**: AI-generated company descriptions

### 👥 Role-Based Access Control
- **Admin Panel**: Full system control, user management, content moderation
- **Writer Panel**: Create and manage own blog posts
- **User Panel**: Profile management, commenting system

### 📚 Blog Management
- Create, read, update, delete blog posts
- Multiple image support (cover + 2 additional images)
- Category and tag organization
- SEO-friendly URLs
- Pagination support

### 💬 Advanced Comment System
- User authentication required
- AI-powered toxicity filtering
- Rate limit handling with fallback mechanism
- Comment approval workflow

### 📊 Dashboard & Analytics
- Blog statistics
- Category distribution charts
- Daily activity tracking
- User engagement metrics

## 🏗️ Architecture

The project follows **N-Tier Architecture** pattern with clean separation of concerns:

```
Blogy/
├── 🎯 Blogy.Entity/           # Domain Models & Entities
│   └── Entities/
│       ├── Blog, Category, Tag
│       ├── Comment, AppUser, AppRole
│       └── About, TeamMember, ContactInfo
│
├── 💾 Blogy.DataAccess/       # Data Layer
│   ├── Context/               # EF Core DbContext
│   ├── Repositories/          # Repository Pattern
│   └── Migrations/            # Database Migrations
│
├── 💼 Blogy.Business/         # Business Logic Layer
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Services/              # Business Services
│   ├── Validators/            # FluentValidation Rules
│   └── Mappings/              # AutoMapper Profiles
│
└── 🎨 Blogy.WebUI/            # Presentation Layer
    ├── Areas/                 # Role-based Areas (Admin, Writer, User)
    ├── Controllers/           # MVC Controllers
    ├── Views/                 # Razor Views
    └── ViewComponents/        # Reusable UI Components
```

## 🛠️ Technologies & Patterns

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

## 📦 Database Schema

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

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB or Express)
- OpenAI API Key ([Get one here](https://platform.openai.com/api-keys))


```

## 🎯 Project Structure Details

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

## 🔑 Key Features Implementation

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

## 📊 Features Breakdown

### Public Features
- ✅ Browse blog posts with pagination
- ✅ Filter by category
- ✅ View blog details with comments
- ✅ Sidebar with popular posts, categories, tags
- ✅ Contact form
- ✅ About page with team members

### User Features
- ✅ Register and login
- ✅ Post comments (AI-moderated)
- ✅ View own comments
- ✅ Update profile
- ✅ Change password

### Writer Features
- ✅ Create blog posts (manual or AI-generated)
- ✅ Edit/delete own posts
- ✅ Manage own comments
- ✅ Upload images

### Admin Features
- ✅ Full CRUD for all entities
- ✅ User management
- ✅ Role assignment
- ✅ Comment moderation
- ✅ Dashboard with statistics
- ✅ CMS content management

## 🔒 Security Features

- ASP.NET Core Identity authentication
- Role-based authorization
- Cookie-based authentication
- Password hashing
- CSRF protection
- XSS prevention (Razor encoding)
- SQL injection prevention (EF Core parameterization)

## 🧪 Validation

FluentValidation rules implemented for:
- Blog creation/update (title length, required fields)
- Category management
- Comment content (length, required)
- User registration (email format, password strength)

## 📈 Performance Optimizations

- Async/await throughout the application
- EF Core Include() for eager loading
- Pagination to limit query results
- Rate limiting for OpenAI API calls
- Kestrel configuration for large headers

## 👤 Author

**Emre Okan Başkaya**

- GitHub: [@emreokanbaskaya1](https://github.com/emreokanbaskaya1)
- LinkedIn: [Emre Okan Başkaya](https://linkedin.com/in/emre-okan-baskaya)


---

## Project Video


---

