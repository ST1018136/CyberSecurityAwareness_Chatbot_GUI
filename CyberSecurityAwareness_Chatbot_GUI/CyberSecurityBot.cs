using System;

namespace CyberSecurityAwareness_Chatbot_GUI
{
   
    public class CyberSecurityBot
    {
     
        public string UserName { get; set; }  // User's name (personalization)

        // Private field for IsRunning (used with ref parameter)
        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private ResponseManager _responseManager;  // Handles cybersecurity responses

        public CyberSecurityBot()
        {
            _isRunning = true;
            UserName = "Guest";  // Default until user introduces themselves
            _responseManager = new ResponseManager();
        }

 
        public string ProcessUserInput(string userInput)
        {
            // Check for empty input
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't catch that. Could you rephrase?";

            // Pass input to ResponseManager for processing
            return _responseManager.ProcessInput(userInput, UserName, ref _isRunning);
        }

        
        // Sets the user's name (personalization)

        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))

                // Capitalize first letter, rest lowercase
                UserName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
        }
    }
}