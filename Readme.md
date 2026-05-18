## Purpose

Literary Lounge is a full-stack web application designed for book lovers and quote enthusiasts. It provides a secure platform where users can:

Manage personal book collections with ISBN validation

Save and organize favorite quotes from their readings

Share literary discoveries within a community-driven platform

Access their library from anywhere with secure authentication

The application serves as a digital sanctuary for readers who want to preserve, organize, and share their literary journey.


## Functional Requirements

### User Authentication
User registration with password strength validation

Secure login with JWT token authentication

Session persistence across browser tabs

Logout functionality with history cleanup

### Book Management
Create, Read, Update, and Delete books

ISBN-10 and ISBN-13 validation using regex

Publish year validation (1000 to current year)

Prevent duplicate books (by ISBN or Title/Author combination)

ISBN immutable after creation

View detailed book information

### Quote Management
Create, Read, Update, and Delete quotes

Store quote text, author, and source information

Prevent duplicate quotes

View detailed quote information

### *User Interface
Responsive design (Desktop, Tablet, Mobile)

Dark/Light mode toggle with persistence

Card-based layout on mobile devices

Table-based layout on desktop devices

Bootstrap 5 styling

Font Awesome icons

### Security 
JWT token-based authentication

Password hashing with BCrypt

Protected API endpoints with [Authorize] attribute

HTTP interceptor for automatic token injection

Route guards for protected pages


## Backend

| Layer/Component          | Technology           | Purpose                        |
|--------------------------|----------------------|--------------------------------|
| API Framework            | .NET 9.0.313         | Web API backend                |
| Programming Language     | C# 9.0.313           | Application logic              |
| Authentication           | JWT                  | Secure user authentication     |
| Password Hashing         | BCrypt               | Secure password storage        |
| Data Persistence         | JSON                 | Storing books and quotes data  |


## Frontend

| Technology     | Version      | Purpose                |
|----------------|--------------|------------------------|
| Angular        | 20.3.26      | Frontend Framework     |
| Bootstrap      | 5.3.8        | UI Components          |
| Font Awesome   | 4.7.0        | Icons                  |
| TypeScript     | 5.9.2        | Programming Language   |


## Validation Rules

**Book Validation**
- **Title**: Required
- **Author**: Required
- **ISBN**: Must be 10 or 13 digits (with or without hyphens)
- **Publish Year**: Must be between 1000 and the current year
- No duplicate ISBN or Title/Author combination allowed

**Quote Validation**
- **Text**: Required
- **Author**: Required
- No duplicate text/author combination


**Password Validation**
- Minimum 8 characters
- At least one uppercase letter (A-Z)
- At least one lowercase letter (a-z)
- At least one number (0-9)
- At least one special character (!@#$%^&*)

