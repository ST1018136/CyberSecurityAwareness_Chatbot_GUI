using System;
using System.Collections.Generic;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    
    public class ResponseManager
    {
        
        private Random _random;                                   // Random number generator
        private Dictionary<string, List<string>> _responses;      // Topic → list of tips
        public string ResponseType { get; private set; }          // Last detected topic

        public ResponseManager()
        {
            _random = new Random();
            _responses = new Dictionary<string, List<string>>();
            InitializeResponses();  // Load all tips
            ResponseType = "none";  // No topic selected yet
        }

        private void InitializeResponses()
        {
            _responses["password"] = new List<string> {
                "🔐 Use at least 12 characters with mixed letters, numbers, and symbols!",
                "🔑 Never reuse passwords across different accounts!",
                "🛡️ Enable Two-Factor Authentication whenever possible!",
                "🎯 Use a password manager like Bitwarden or LastPass!",
                "💪 Avoid using personal info like birthdays or pet names!",
                "⚡ Change important passwords every 3-6 months!"
            };

           
            // PHISHING PREVENTION TIPS 
          
            _responses["phishing"] = new List<string> {
                "🎣 Always check the sender's email address carefully!",
                "📧 Never click suspicious links or download unknown attachments!",
                "⚠️ Watch for urgent language demanding immediate action!",
                "🏦 In South Africa, scammers pretend to be from Capitec, FNB, or Standard Bank!",
                "🔍 Look for spelling mistakes and generic greetings like 'Dear Customer'!",
                "📞 When in doubt, call the company directly using their official number!"
            };

            // SCAM DETECTION TIPS 
        
            _responses["scam"] = new List<string> {
                "🚨 If it sounds too good to be true, it probably is!",
                "📞 Never share OTPs or PINs with anyone! No bank employee will ask for these!",
                "💰 Beware of 'Million Dollar' scams on WhatsApp and Facebook!",
                "🔒 Verify money requests by calling the person directly using a known number!",
                "📧 SARS will never ask for your banking details via email or SMS!",
                "🎭 Scammers create fake urgency - take a breath and verify first!"
            };

           
            // SAFE BROWSING TIPS 
            
            _responses["browsing"] = new List<string> {
                "🌐 Look for 'https://' and the padlock icon before entering personal info!",
                "📱 Avoid public Wi-Fi for banking or shopping! Use a VPN if you must connect!",
                "🔄 Keep your browser and operating system updated for security patches!",
                "🛡️ Use reputable antivirus software and keep it updated!",
                "🗑️ Clear your browser cache and cookies regularly to remove tracking data!",
                "🔧 Use browser extensions like uBlock Origin to block malicious ads!"
            };

           
            // PRIVACY PROTECTION TIPS 
         
            _responses["privacy"] = new List<string> {
                "🔒 Review your privacy settings on social media monthly! Set profiles to private!",
                "📸 Be careful what you share online! Posting vacation photos tells criminals your home is empty!",
                "🎭 Use different email addresses for different purposes - banking, social media, newsletters!",
                "🗑️ Shred physical documents containing personal information before throwing them away!",
                "🔐 Use encrypted messaging apps like Signal or WhatsApp for sensitive conversations!",
                "👀 Limit what apps can access your location, contacts, and camera!"
            };
        }

      
        // Processes user input and returns a response
        
        public string ProcessInput(string userInput, string userName, ref bool IsRunning)
        {
            string input = userInput.ToLower();

          
            // EXIT COMMANDS
          
            if (input == "exit" || input == "quit" || input == "byebye" || input == "goodbye")
            {
                IsRunning = false;
                string[] goodbyes = {
                    $"Goodbye, {userName}! Stay safe online! 🛡️",
                    $"See you later, {userName}! Remember to stay vigilant online! 👋",
                    $"Take care, {userName}! Keep your passwords strong and your data secure! 🔐"
                };
                return goodbyes[_random.Next(goodbyes.Length)];
            }

            // GREETINGS
      
            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey") || input.Contains("greetings"))
            {
                string[] greetings = {
                    $"Hello {userName}! How can I help you stay safe online today?",
                    $"Hi {userName}! Ready to learn about cybersecurity?",
                    $"Hey {userName}! What would you like to know about online safety?"
                };
                return greetings[_random.Next(greetings.Length)];
            }

    
            // HOW ARE YOU
      
            if (input.Contains("how are you") || input.Contains("how are u"))
            {
                string[] status = {
                    "I'm functioning like code bro! but, thanks for asking! Ready to help you stay secure! 💪",
                    "All systems operational! How can I assist you with cybersecurity today? 🤖",
                    "I'm doing great! Thanks for checking in. Let's talk about online safety!"
                };
                return status[_random.Next(status.Length)];
            }

          
            // PURPOSE 
       
            if (input.Contains("purpose") || input.Contains("what do you do") || input.Contains("what can you do"))
            {
                return "My purpose is to help you stay safe online! I provide tips on:\n\n• Password safety\n• Phishing prevention\n• Scam detection\n• Safe browsing\n• Privacy protection\n\nJust ask me about any of these topics!";
            }

            // TOPIC DETECTION - Cybersecurity topics

            // Password related
            if (input.Contains("password") || input.Contains("pass") || input.Contains("login") || input.Contains("credential"))
            {
                ResponseType = "password";
                return GetRandomResponse("password");
            }

            // Phishing related
            if (input.Contains("phish") || input.Contains("phishing"))
            {
                ResponseType = "phishing";
                return GetRandomResponse("phishing");
            }

            // Scam related
            if (input.Contains("scam") || input.Contains("fraud") || input.Contains("fake"))
            {
                ResponseType = "scam";
                return GetRandomResponse("scam");
            }

            // Browsing related
            if (input.Contains("brows") || input.Contains("internet") || input.Contains("online") || input.Contains("web"))
            {
                ResponseType = "browsing";
                return GetRandomResponse("browsing");
            }

            // Privacy related
            if (input.Contains("privacy") || input.Contains("private") || input.Contains("personal data") || input.Contains("information"))
            {
                ResponseType = "privacy";
                return GetRandomResponse("privacy");
            }


            // FOLLOW-UP - "Tell me more" or "Another tip"

            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("more tips") || input.Contains("elaborate"))
            {
                if (ResponseType != "none" && _responses.ContainsKey(ResponseType))
                    return "✨ Here's another tip:\n\n" + GetRandomResponse(ResponseType);
                return " For Sure! What topic would you like more tips on? Try asking about:\n• Password safety\n• Phishing attacks\n• Online scams\n• Safe browsing\n• Privacy protection";
            }

            // HELP - What the user can ask
 
            if (input.Contains("help") || input.Contains("what can I ask") || input.Contains("suggestions"))
            {
                return "📚 You can ask me about:\n\n🔐 Password safety tips\n🎣 Phishing attack prevention\n⚠️ Online scam detection\n🌐 Safe browsing habits\n🔒 Privacy protection\n\nJust type your question naturally, like 'Tell me about passwords' or 'How to avoid phishing?'";
            }

            // THANK YOU
   
            if (input.Contains("thank") || input.Contains("thanks"))
            {
                string[] thanks = {
                    "You're very much welcome! Stay safe online! 😊",
                    "Happy to help! Remember, cybersecurity is everyone's responsibility! 🛡️",
                    "My pleasure! Feel free to ask me anything else about online safety!"
                };
                return thanks[_random.Next(thanks.Length)];
            }

            // UNKNOWN - Default responses when no keyword matches
  
            string[] unknown = {
                "🤔 I am bamboozled and I undestand not of what you are saying. Try asking about passwords, phishing, scams, safe browsing, or privacy!",
                "💭 Can you rephrase? I specialize in cybersecurity topics like password safety and scam prevention!",
                "🔍 Ask me about password safety, phishing prevention, online scams, safe browsing, or privacy protection!",
                "📖 I'm your cybersecurity assistant. Try asking: 'How do I create a strong password?' or 'What are phishing scams?'"
            };
            return unknown[_random.Next(unknown.Length)];
        }

        // Gets a random response for a specific topic
        private string GetRandomResponse(string topic)
        {
            if (_responses.ContainsKey(topic) && _responses[topic].Count > 0)
            {
                int index = _random.Next(_responses[topic].Count);
                return _responses[topic][index];
            }
            // Fallback response
            return "🔐 Here's a fine cybersecurity tip: Always use strong, unique passwords for every account and enable two-factor authentication when possible!";
        }
    }
}