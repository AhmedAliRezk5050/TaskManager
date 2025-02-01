# Task Manager

This project is a Task Management System designed to help teams and individuals efficiently track, organize, and manage tasks in a Kanban-style board (similar to Jira or Trello).


## Tech Stack

**Client:** Angular, Primeng, TailwindCSS

**Server:** ASP.NET WEB API 6


## Features

- Drag-and-Drop Tasks – Move tasks between "To Do," "In Progress," and "Done" columns
- Task  CRUD Operations – Create, Edit, Delete tasks with a simple UI
- User Authentication – Secure login and registration with JWT
- Responsive Design – Works seamlessly on desktop and mobile

## Run Locally

Clone the project

```bash
  git clone https://github.com/AhmedAliRezk5050/TaskManager
```

## Instructions for Filling appsettings.json

The `appsettings.json` file is used to configure the application settings. Please fill out the following fields:

1. **Jwt:Key**: The secret key used for JWT authentication. Example: `your-secret-key`
2. **Jwt:Issuer**: The issuer of the JWT token. Example: `your-issuer`
3. **Jwt:Audience**: The audience of the JWT token. Example: `your-audience`
4. **AdminInfo:UserName**: The username of the admin user. Example: `admin`
5. **AdminInfo:Password**: The password of the admin user. Example: `password`

Ensure that all fields are filled out according to the specified format.