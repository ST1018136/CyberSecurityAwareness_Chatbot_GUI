# CyberSecurityAwareness_Chatbot_GUI
CyberBot - Cybersecurity Awareness Chatbot
📖 Overview
CyberBot is an interactive desktop application designed to educate users about cybersecurity best practices. The chatbot provides information on password safety, phishing detection, scam prevention, safe browsing habits, and privacy protection in an engaging, conversational format.

YouTube Video Link:
https://youtu.be/Bst6Fry-myg?si=HQI1VHAudQHRXFjH


✨ Features
Core Functionality
Interactive Chat Interface - Instagram-style chat bubbles with user/bot messages

Keyword Recognition - Automatically detects cybersecurity topics and provides relevant responses

Random Responses - Multiple variations for each topic to keep conversations fresh

Conversation Memory - Remembers user name and interests for personalized interactions

Sentiment Detection - Adjusts responses based on user's mood (worried, frustrated, curious)

Voice Greeting - Optional audio greeting when the application starts

UI Features
3-Column Layout - Side menu, chat area, and social media footer

Gradient Side Menu - Blue/pink/purple gradient design

Profile Popup - Click contact name to view chatbot profile

File Attachment - Attach files to messages

Emoji Picker - Quick emoji insertion in messages

3D Shadow Effects - Modern visual design with depth

🚀 Installation
Prerequisites
Windows 10 or 11

Visual Studio 2022 (or later)

.NET Framework 4.7.2

Steps
Clone or download the project

Open the solution

text
Double-click CyberSecurityAwareness_Chatbot_GUI.sln
Build the solution

text
Press Ctrl+Shift+B
Run the application

text
Press F5
📁 Project Structure
text
CyberSecurityAwareness_Chatbot_GUI/
├── MainWindow.xaml              # GUI design
├── MainWindow.xaml.cs           # GUI code-behind
├── CyberSecurityBot.cs          # Main bot logic
├── ResponseManager.cs           # Keyword detection & responses
├── AudioPlayer.cs               # Voice greeting functionality
├── LogoDisplay.cs               # ASCII art logos
├── App.xaml                     # Application settings
├── App.xaml.cs                  # Application entry point
└── Resources/
    └── greeting.wav             # Voice greeting audio file (optional)
🎮 How to Use
Basic Commands
You Can Ask	Example
Password tips	"Tell me about password safety"
Phishing prevention	"How to spot phishing emails?"
Scam detection	"What are common online scams?"
Safe browsing	"How to browse safely?"
Privacy protection	"Give me privacy tips"
Conversation Features
Feature	How to Use
Tell your name	"My name is John" or "I'm Sarah"
Get another tip	"Tell me more" or "Another tip"
Ask for help	"What can I ask?" or "Help"
Exit	"Exit" or "Goodbye"
Navigation
Menu Icon	Function
🏠 Home	Reset conversation
📜 History	View chat history
🔔 Notifications	Check security alerts
⚙️ Settings	View settings
🎨 Customization Guide
Change Colors
In MainWindow.xaml, modify the gradient colors:

xml
<LinearGradientBrush x:Key="SideMenuGradient" StartPoint="0,0" EndPoint="0,1">
    <GradientStop Color="#4A00E0" Offset="0.0"/>  <!-- Change this -->
    <GradientStop Color="#8E2DE2" Offset="0.3"/>  <!-- Change this -->
    <GradientStop Color="#FF6B9D" Offset="0.6"/>  <!-- Change this -->
    <GradientStop Color="#C850C0" Offset="1.0"/>  <!-- Change this -->
</LinearGradientBrush>
Change Icons
Replace emojis in the XAML (look for Text="🏠" etc.) with any emoji you prefer.

Change Contact Information
In MainWindow.xaml, find COLUMN 3 and update:

xml
<Run Text="📱 "/><Run Text="+27 123 456 789"/>     <!-- Phone -->
<Run Text="✉️ "/><Run Text="support@cyberbot.com"/> <!-- Email -->
<Run Text="🌐 "/><Run Text="www.cyberbot.com"/>     <!-- Website -->
Add New Response Topics
In ResponseManager.cs, add to the InitializeResponses() method:

csharp
_responses["yourtopic"] = new List<string> {
    "Response 1",
    "Response 2",
    "Response 3"
};
Then add keyword detection in ProcessInput():

csharp
if (input.Contains("yourkeyword"))
{
    ResponseType = "yourtopic";
    return GetRandomResponse("yourtopic");
}
🛠️ Troubleshooting
Issue	Solution
Grid not showing	Remove AllowsTransparency="False" from Window
Voice not working	Ensure greeting.wav is in Resources folder
Icons not displaying	Use emojis instead of Font Awesome
Build errors	Clean solution (Build → Clean) then rebuild
Third column hidden	Check margin values, remove large numbers
📝 System Requirements
Component	Requirement
OS	Windows 10/11
Framework	.NET Framework 4.7.2
RAM	2GB minimum
Disk Space	50MB
Visual Studio	2022 (for development)
🤝 Contributing
Fork the project

Create a feature branch

Commit your changes

Push to the branch

Open a Pull Request

📄 License
This project is for educational purposes as part of a cybersecurity awareness assignment.

🙏 Acknowledgments
Font Awesome Icons (optional package)

Material Design inspiration

Cybersecurity best practices from industry standards

📞 Support
For issues or questions:

Email: support@cyberbot.com

Phone: +27 123 456 789

Author:
[Sefako Siphosethu Mongalo]
[ST10181363]

Diploma in Software Development
[Programming 2A [Rosebank College Braamfontien]
