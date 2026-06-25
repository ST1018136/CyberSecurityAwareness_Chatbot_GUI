# 🛡️ CyberSecurity Awareness Chatbot

A comprehensive WPF-based cybersecurity awareness application that combines an interactive chatbot, educational quiz system, task management, and engaging visual elements to promote online safety.

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Installation Guide](#installation-guide)
- [Database Setup](#database-setup)
- [How to Use](#how-to-use)
- [Application Features in Detail](#application-features-in-detail)
- [Class Diagram](#class-diagram)
- [Screenshots](#screenshots)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📌 Overview

The **CyberSecurity Awareness Chatbot** is an educational desktop application designed to help users learn about cybersecurity best practices through interactive conversations, quizzes, and task management. Built with **Windows Presentation Foundation (WPF)** and **.NET Framework 4.7.2**, the application provides a user-friendly interface with modern UI design and engaging animations.

**Purpose:** To raise cybersecurity awareness by providing accessible, interactive learning tools that cover essential topics like password safety, phishing detection, online scams, and privacy protection.

 YouTube Video Link: https://youtu.be/Bst6Fry-myg?si=HQI1VHAudQHRXFjH
---

## ✨ Features

### 🤖 AI-Powered Chatbot
- **Natural Language Processing**: Detects user intent and provides relevant cybersecurity tips
- **Personalized Experience**: Greets users by name after introduction
- **Topic Detection**: Automatically identifies topics like passwords, phishing, scams, and more
- **Context-Aware Responses**: Maintains conversation context for follow-up questions
- **Voice Greeting**: Plays a welcome audio file when the application starts

### 🧠 Cybersecurity Quiz
- **11 Interactive Questions**: Mix of multiple-choice and true/false questions
- **Instant Feedback**: Get immediate explanations for correct and incorrect answers
- **Score Tracking**: Real-time score updates as you progress
- **30-Minute Timer**: Time limit to add challenge
- **Progress Tracking**: Shows current question number and total questions
- **Celebration Effects**: Balloon animations when scoring 8/11 or higher

### 📋 Task Management System
- **Add Tasks**: Create tasks with titles and descriptions
- **Set Reminders**: Add reminder dates for important tasks
- **Complete Tasks**: Mark tasks as done (with visual strikethrough)
- **Delete Tasks**: Remove unwanted tasks
- **Persistent Storage**: MySQL database integration for task persistence
- **Task List View**: Clean, organized display of all tasks

### 🎈 Visual Celebrations
- **Balloon Animations**: Colorful floating balloons for quiz achievements
- **Smooth Animations**: 60 FPS rendering for fluid motion
- **Randomized Elements**: Different colors, sizes, and speeds each time

### 📊 Activity Logging
- **User Activity Tracking**: Logs all major actions (task additions, quiz starts, etc.)
- **Timestamped Records**: Each entry includes date and time
- **Recent Activity View**: View last 10 activities
- **Database Storage**: Persistent activity history

### 🌐 Social Features
- **Social Media Integration**: Connect with Facebook, Instagram, Twitter, WhatsApp
- **Contact Information**: Display phone, email, and website
- **Location Info**: Shows location details
- **Security Badges**: Visual trust indicators

### 🎨 Modern UI Design
- **Gradient Color Schemes**: Professional purple gradient theme
- **Glass Effect**: Modern translucent elements
- **3D Drop Shadows**: Depth effects for buttons and cards
- **Responsive Layout**: Three-column design with side menu
- **Emoji Support**: Easy emoji insertion in chat
- **File Attachment**: Upload and share files in chat

---

## 🛠️ Technologies Used

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET Framework | 4.7.2 | Application framework |
| WPF | 4.7.2 | UI framework |
| C# | 7.3+ | Programming language |
| MySQL | 8.0+ | Database management |
| MySql.Data | 9.7.0 | MySQL connector |
| MahApps.Metro.IconPacks | 6.2.1 | Icon library |
| FontAwesome.WPF | 4.7.0 | Additional icons |
| Visual Studio | 2019/2022 | IDE |
| Git | Latest | Version control |

### NuGet Packages Used
```xml
<package id="MySql.Data" version="9.7.0" />
<package id="MahApps.Metro.IconPacks" version="6.2.1" />
<package id="FontAwesome.WPF" version="4.7.0.9" />
<package id="System.Configuration.ConfigurationManager" version="8.0.0" />
