using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    public class CyberSecurityBot
    {
        
        private ResponseManager _responseManager;

        // Tracks whether the bot session is active
        private bool _isRunning;

        // Auto-property: the user's name (defaults to "Guest" until they tell us)
        public string UserName { get; private set; }

     
        public string FavouriteTopic { get; private set; }

        public CyberSecurityBot()
        {
            _responseManager = new ResponseManager();
            _isRunning = true;

            UserName = "Guest";

            FavouriteTopic = "";
        }

        public void SetUserName(string name)
        {
            UserName = name;
        }
        public void SetFavouriteTopic(string topic)
        {
            FavouriteTopic = topic;
        }

     
        public string ProcessUserInput(string input)
        {
            // Pass the current favourite topic into ResponseManager
            string response = _responseManager.ProcessInput(input, UserName, FavouriteTopic, ref _isRunning);

            // If ResponseManager detected a new favourite topic, save it here too
            if (!string.IsNullOrEmpty(_responseManager.FavouriteTopic))
            {
                FavouriteTopic = _responseManager.FavouriteTopic;
            }

            return response;
        }
    }
}
