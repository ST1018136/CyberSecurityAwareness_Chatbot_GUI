using System;
using System.Collections.Generic;

namespace CyberSecurityAwareness_Chatbot_GUI
{
  
    public class ResponseManager
    {

        private Random _random;

        private Dictionary<string, List<string>> _responses;

        public string ResponseType { get; private set; }

        
        public string FavouriteTopic { get; private set; }

        
      
        public ResponseManager()
        {
            _random = new Random();
            _responses = new Dictionary<string, List<string>>();

            // Set defaults so these are never null
            ResponseType = "none";
            FavouriteTopic = "";

            //  Password tips 

            _responses["password"] = new List<string> {
                "🔐 Use at least 12 characters with mixed letters, numbers, and symbols!",
                "🔑 Never reuse the same password across different accounts!",
                "🛡️ Enable Two-Factor Authentication (2FA) wherever possible!",
                "🎯 Use a password manager to generate and store strong passwords!",
                "💪 Avoid personal info like birthdays or pet names in your passwords!"
            };

            // Phishing tips 

            _responses["phishing"] = new List<string> {
                "🎣 Always check the sender's email address carefully before clicking anything!",
                "📧 Never click suspicious links or download unknown attachments!",
                "⚠️ Watch for urgent language demanding money or personal info!",
                "🏦 In South Africa, scammers often pretend to be from Capitec, FNB, or Standard Bank!",
                "🔍 Look for spelling mistakes and generic greetings like 'Dear Customer'!"
            };

            // Scam tips 

            _responses["scam"] = new List<string> {
                "🚨 If it sounds too good to be true, it most likely is!",
                "📞 Never share OTPs or PINs with anyone — not even your bank!",
                "💰 Beware of 'Million Dollar' lottery scams on social media!",
                "🔒 Always verify money requests by calling the person directly!",
                "📧 SARS will never ask for your banking details via email!"
            };

            // Safe browsing tips 

            _responses["browsing"] = new List<string> {
                "🌐 Always look for 'https://' and the padlock icon before entering your info!",
                " Avoid using public Wi-Fi for banking — use a VPN instead!",
                " Keep your browser and operating system updated for the latest security patches!",
                " Install antivirus software and make sure it stays updated!",
                " Clear your browser cache and cookies regularly to protect your data!"
            };

            // Sentimental response

            _responses["worried"] = new List<string>
            {
                " It is completely normal to feel that way, Scammers are very " +
                "convincing, Let me give you Tips on how to stay calm!",
                "Look out for emails that demand money from you!!",
                 "Do not click links or ads that look too good to be true, like 'you have won'"
            };

            // Privacy tips

            _responses["privacy"] = new List<string> {
                "🔒 Review your social media privacy settings at least once a month!",
                "📸 Think carefully about what personal information you share online!",
                "🎭 Use different email addresses for different purposes (work, shopping, personal)!",
                "🗑️ Shred any physical documents that contain personal information!",
                "🔐 Use encrypted messaging apps like Signal or WhatsApp for sensitive chats!"
            };
        }

        // Main method — reads the user's input and returns the correct response.
        // Now accepts the favouriteTopic so it can refer back to it in replies.

        /// <param name="userInput">
        /// <param name="userName">
        /// <param name="favouriteTopic">
        /// <param name="isRunning">
        public string ProcessInput(string userInput, string userName, string favouriteTopic, ref bool isRunning)
        {
            // Save the favourite topic 
            FavouriteTopic = favouriteTopic;

            string input = userInput.ToLower().Trim();

            //Empty input 
            if (string.IsNullOrWhiteSpace(input))
            {
                return "✏️ It looks like you didn't type anything. Please ask me a question!";
            }

            //  Exit commands
            if (input == "exit" || input == "quit" || input == "bye")
            {
                isRunning = false;
                return $"Goodbye, {userName}! Stay safe online! 🛡️";
            }

            // Detect "I am interested in [topic]"
            // This is the memory recall trigger — saves the topic for later
            if (input.Contains("interested in"))
            {
                return HandleInterestDetection(input, userName);
            }

            // Greeting
            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
            {
                return $"Hello {userName}! How can I help you stay safe online today? 😊";
            }

            //How are you
            if (input.Contains("how are you"))
            {
                return "I am functioning like a normal Chatbot would bro and ready to help you stay secure! 💪";
            }

            // Purpose
            if (input.Contains("purpose") || input.Contains("what do you do"))
            {
                return "My purpose only is to help you stay safe online! I can give tips on " +
                       "passwords, phishing, scams, safe browsing, and privacy.";
            }

            // Password
            if (input.Contains("password") || input.Contains("pass"))
            {
                ResponseType = "password";
                return GetResponseWithMemory("password", userName, favouriteTopic);
            }

            // --- Phishing ---
            if (input.Contains("phishing"))
            {
                ResponseType = "phishing";
                return GetResponseWithMemory("phishing", userName, favouriteTopic);
            }

            // --- Scam ---
            if (input.Contains("scam"))
            {
                ResponseType = "scam";
                return GetResponseWithMemory("scam", userName, favouriteTopic);
            }

            //Safe browsing 
            if (input.Contains("brows") || input.Contains("internet") || input.Contains("online"))
            {
                ResponseType = "browsing";
                return GetResponseWithMemory("browsing", userName, favouriteTopic);
            }
            // Sentiment
            if (input.Contains("worried") || input.Contains("curious") || input.Contains("frustrated"))
                {
                ResponseType = "worried";
                return GetResponseWithMemory("worried", userName, favouriteTopic);
            }

            // Privacy 
            if (input.Contains("privacy") || input.Contains("private"))
            {
                ResponseType = "privacy";
                return GetResponseWithMemory("privacy", userName, favouriteTopic);
            }

            //ell me more / another tip
            // Continues the last topic without the user having to repeat themselves
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("more tips") || input.Contains("explain more"))
            {
                if (ResponseType != "none" && _responses.ContainsKey(ResponseType))
                    return $"✨ Here is another {ResponseType} tip, {userName}:\n" + GetRandomResponse(ResponseType);

                return " okay Sure! What topic would you like more tips on? " +
                       "Passwords, phishing, scams, browsing, or privacy?";
            }

            //  What is my favourite topic (memory recall test)
            if (input.Contains("what do you remember") || input.Contains("my favourite") || input.Contains("what did i tell you"))
            {
                return GetMemoryRecallResponse(userName, favouriteTopic);
            }

            // Help 
            if (input.Contains("help") || input.Contains("what can i ask"))
            {
                return "You can ask me about:\n" +
                       "• 🔐 Password safety\n" +
                       "• 🎣 Phishing attacks\n" +
                       "• 🚨 Online scams\n" +
                       "• 🌐 Safe browsing\n" +
                       "• 🔒 Privacy protection\n\n" +
                       "Just type your question naturally!";
            }

            // Unknown input fallback 
            string[] unknown = {
                $"🤔 I am not sure I understand my good buddy, {userName}. Try asking " +
                $"about passwords, phishing, scams, or safe browsing!",
                "💭 Can you rephrase that? I specialise in cybersecurity topics!",
                "🔍 Ask me about password safety, phishing prevention, or online privacy!"
            };
            return unknown[_random.Next(unknown.Length)];
        }

        // Detects which topic the user is interested in from their message,
     
        private string HandleInterestDetection(string input, string userName)
        {
            // Check which topic keyword appears after "interested in"
            string detectedTopic = "";

            if (input.Contains("password")) detectedTopic = "password";
            else if (input.Contains("phish")) detectedTopic = "phishing";
            else if (input.Contains("scam")) detectedTopic = "scam";
            else if (input.Contains("brows") ||
                     input.Contains("internet")) detectedTopic = "browsing";
            else if (input.Contains("privacy")) detectedTopic = "privacy";

            if (!string.IsNullOrEmpty(detectedTopic))
            {
                // Save it so it can be referred back to later
                FavouriteTopic = detectedTopic;
                ResponseType = detectedTopic;

                // This Confirm to the user that the bot has remembered it,
                // then immediately give a tip on that topic
                return $"✅ Got it, {userName}! I will remember that you are interested in " +
                       $"{detectedTopic}. It is a crucial part of staying safe online!\n\n" +
                       $"Here is a tip to get you well settled:\n" +
                       GetRandomResponse(detectedTopic);
            }

            // Could not identify a specific topic
            return "That sounds interesting! Could you tell me which topic specifically? " +
                   "Passwords, phishing, scams, browsing, or privacy?";
        }

        /// <summary>
        /// Returns a tip for the requested topic.
        /// If the topic matches the user's favourite, adds a personalised intro line.
        /// This is how the bot "refers back" to what the user told it earlier.
        /// </summary>
        private string GetResponseWithMemory(string topic, string userName, string favouriteTopic)
        {
            string tip = GetRandomResponse(topic);

            // If this topic matches the user's saved favourite, add a personal reference
            if (!string.IsNullOrEmpty(favouriteTopic) && favouriteTopic == topic)
            {
                return $"💡 As someone interested in {topic}, {userName}, here is something important:\n\n{tip}";
            }

            // If the user has a different favourite topic, gently remind them of it
            if (!string.IsNullOrEmpty(favouriteTopic) && favouriteTopic != topic)
            {
                return $"{tip}\n\n🔔 By the way, {userName}, don't forget you were also " +
                       $"interested in {favouriteTopic} — type '{favouriteTopic}' anytime for more tips on that!";
            }

            // No favourite topic saved yet — just return the normal tip
            return tip;
        }
        // Returns a response when the user asks what the bot remembers about them.

        private string GetMemoryRecallResponse(string userName, string favouriteTopic)
        {
            if (!string.IsNullOrEmpty(favouriteTopic))
            {
                return $"🧠 I remember that you are {userName} and that you are particularly " +
                       $"interested in {favouriteTopic}!\n\n" +
                       $"Here is a fresh {favouriteTopic} tip for you:\n" +
                       GetRandomResponse(favouriteTopic);
            }

            return $"🧠 I know your name is {userName}! You have not told me your favourite " +
                   $"topic yet — try saying something like 'I am interested in privacy' and I will remember it!";
        }

        // Picks a random response from the given topic's list.

        private string GetRandomResponse(string topic)
        {
            if (_responses.ContainsKey(topic) && _responses[topic].Count > 0)
            {
                int index = _random.Next(_responses[topic].Count);
                return _responses[topic][index];
            }

            // Fallback if somehow the topic is not found
            return "Here is my personal cybersecurity tip: Always use strong, unique passwords for every account!!!";
        }
    }
}
