using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    public class NLPsimulatorscs
    {

        private Dictionary<string, List<string>> _intentPatterns;
        private Dictionary<string, string> _intentResponses;

        public event Action<string> OnIntentDetected; //fires when intent is identified

        public NLPsimulatorscs()
        {
            InitializePatterns();
        }

        // sets up a keyword pattern for intent
        private void InitializePatterns()
        {
            // Dictionary mapping intent names to lists of keywords/phrases
            _intentPatterns = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                // Task Management Intents
                ["add_task"] = new List<string> {
                    "add task", "create task", "new task", "remind me to",
                    "add reminder", "set reminder", "remember to",
                    "make a task", "add a task", "create a task", "task to"
                },
                ["show_tasks"] = new List<string> {
                    "show tasks", "view tasks", "list tasks", "my tasks",
                    "what tasks", "show my tasks", "display tasks", "tasks"
                },
                ["complete_task"] = new List<string> {
                    "complete task", "mark done", "finish task", "task done",
                    "complete", "done", "mark as complete", "task complete"
                },
                ["delete_task"] = new List<string> {
                    "delete task", "remove task", "cancel task", "delete",
                    "remove", "cancel"
                },

                // Quiz Intents
                ["start_quiz"] = new List<string> {
                    "start quiz", "play quiz", "take quiz", "cyber quiz",
                    "security quiz", "quiz me", "test me", "quiz",
                    "begin quiz", "start test"
                },

                // Activity Log Intents
                ["show_log"] = new List<string> {
                    "show log", "activity log", "what have you done",
                    "show history", "view log", "display log", "log"
                },

                // Help Intents
                ["password_help"] = new List<string> {
                    "password help", "password tips", "strong password",
                    "create password", "password advice", "passwords"
                },
                ["phishing_help"] = new List<string> {
                    "phishing help", "phishing tips", "avoid phishing",
                    "spot phishing", "phishing advice", "phishing"
                }
            };

            // Predefined responses for each intent
            _intentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["add_task"] = "I'll help you add a task. What task would you like to add?",
                ["show_tasks"] = "Let me retrieve your tasks for you.",
                ["complete_task"] = "I'll mark that task as completed.",
                ["delete_task"] = "I'll remove that task for you.",
                ["start_quiz"] = "Great! Let's test your cybersecurity knowledge!",
                ["show_log"] = "Here's what I've been doing for you:",
                ["password_help"] = "Here are some password safety tips:",
                ["phishing_help"] = "Here are some phishing prevention tips:"
            };
        }

        // analyses user input
        public string AnalyzeIntent(string userInput)
        {
            string lowerInput = userInput.ToLower();

            // Check each intent pattern
            foreach (var pattern in _intentPatterns)
            {
                foreach (var keyword in pattern.Value)
                {
                    if (lowerInput.Contains(keyword.ToLower()))
                    {
                        OnIntentDetected?.Invoke(pattern.Key);
                        return pattern.Key;
                    }
                }
            }

            // FALLBACK KEYWORD DETECTION 

            // Task-related keywords
            if (lowerInput.Contains("task") || lowerInput.Contains("remind") || lowerInput.Contains("remember"))
                return "add_task";

            // Quiz-related keywords
            if (lowerInput.Contains("quiz") || lowerInput.Contains("test") || lowerInput.Contains("game"))
                return "start_quiz";

            // Log-related keywords
            if (lowerInput.Contains("log") || lowerInput.Contains("history") || lowerInput.Contains("done"))
                return "show_log";

            // Password-related keywords
            if (lowerInput.Contains("password") || lowerInput.Contains("pass"))
                return "password_help";

            // Phishing-related keywords
            if (lowerInput.Contains("phish") || lowerInput.Contains("scam"))
                return "phishing_help";

            // No intent detected
            return "unknown";
        }

        public string GetIntentResponse(string intent)
        {
            if (_intentResponses.ContainsKey(intent))
                return _intentResponses[intent];
            return "I'm not sure how to help with that. Could you rephrase?";
        }

        // Checks if the input starts with a command word

        public bool IsCommand(string input)
        {
            string[] commandWords = {
                "add", "create", "remind", "show", "view", "list", "delete",
                "remove", "complete", "done", "finish", "start", "play", "take"
            };
            foreach (var word in commandWords)
            {
                if (input.ToLower().StartsWith(word))
                    return true;
            }
            return false;
        }
        // extracts the task description from input
        public string ExtractTaskDetails(string input)
        {
            string[] patterns = { "remind me to", "add task", "create task", "task to", "remember to" };
            foreach (var pattern in patterns)
            {
                if (input.ToLower().Contains(pattern))
                {
                    int index = input.ToLower().IndexOf(pattern) + pattern.Length;
                    if (index < input.Length)
                        return input.Substring(index).Trim();
                }
            }
            return input; // Return original if no pattern matches
        }

        // extracts the remainder date from user input
        public DateTime? ExtractRemainderDate(string input)
        {
            // pattern 1
            Regex dayRegex = new Regex(@"(\d+)\s*(day|days)", RegexOptions.IgnoreCase);
            var match = dayRegex.Match(input);
            if (match.Success)
            {
                int days = int.Parse(match.Groups[1].Value);
                return DateTime.Now.AddDays(days);
            }

            // pattern 2
            Regex weekRegex = new Regex(@"(\d+)\s*(week|weeks)", RegexOptions.IgnoreCase);
            match = weekRegex.Match(input);
            if (match.Success)
            {
                int weeks = int.Parse(match.Groups[1].Value);
                return DateTime.Now.AddDays(weeks * 7);
            }

            // pattern 3 dates
            Regex dateRegex = new Regex(@"(\d{1,2})/(\d{1,2})/(\d{4})");
            match = dateRegex.Match(input);
            if (match.Success)
            {
                int day = int.Parse(match.Groups[1].Value);
                int month = int.Parse(match.Groups[2].Value);
                int year = int.Parse(match.Groups[3].Value);
                try
                {
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return null; // Invalid date
                }
            }

            return null; // No date found
        }

        // determines the task operation type
        public string GetTaskIntent(string input)
        {
            if (input.ToLower().Contains("complete") || input.ToLower().Contains("done"))
                return "complete";

            if (input.ToLower().Contains("delete") || input.ToLower().Contains("remove"))
                return "delete";

            if (input.ToLower().Contains("show") || input.ToLower().Contains("view") ||
                input.ToLower().Contains("list"))
                return "show";

            return "add";
        }

        internal DateTime? ExtractReminderDate(string input)
        {
            throw new NotImplementedException();
        }
    }

}