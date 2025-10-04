📚 SkillShareHub

SkillShareHub is an ASP.NET Core MVC e-learning platform where students can browse, enroll, and learn from courses created by instructors. The system includes role-based authentication (Admin, Instructor, Student) and provides an intuitive dashboard for each role.

🚀 Features
👨‍🎓 Students

Browse and enroll in available courses

View enrolled courses in dashboard

Access learning materials

👨‍🏫 Instructors

Create and manage courses

Upload course content

Track enrolled students

🛠️ Admin

Approve or reject courses submitted by instructors

Manage users (students & instructors)

View statistics:

Total number of students and instructors

Instructor-wise course count

Student enrollments per course

🖥️ Tech Stack

Backend: ASP.NET Core MVC (.NET 6 / .NET 7)

Frontend: Razor Views, Bootstrap 5

Database: Microsoft SQL Server (EF Core ORM)

Authentication: ASP.NET Core Identity (Role-based)

Hosting Options: IIS, Azure, SmarterASP.NET, Ngrok (for local demo)

⚡ Getting Started
Prerequisites

.NET SDK
 (6.0 or later)

SQL Server
 or SQL Express

Visual Studio 2022 / VS Code

Steps

1. Clone the repository

git clone https://github.com/your-username/SkillShareHub.git
cd SkillShareHub

2. Configure database connection in appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=SkillShareHub;Trusted_Connection=True;MultipleActiveResultSets=true"
}

3. Apply migrations

dotnet ef database update

4. Run the project
   
   dotnet run

5. Open in browser → https://localhost:5001

📊 Admin Demo Credentials
admin@skillsharehub.com
Admin@123

🎨 UI/UX Improvements

Gradient background for a modern look

Hover animations on course cards

Responsive Bootstrap 5 design

🤝 Contributing

Fork this repo

Create a feature branch (feature-newUI)

Commit changes

Push and create a pull request

📄 License

This project is licensed under the MIT License.
