using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    public class QuizManager
    {

        private List<QuizQuestion> _questions;
        private int _currentQuestionIndex;
        private int _score;
        private bool _quizActive;
        private DateTime _startTimer;

        // New questions Displayed
        public event Action<string> OnQuestionChanged; //

        // Score total
        public event Action<int, int> OnScoreUpdated;

        // Answers is correct
        public event Action<bool, string> OnAnswerChecked;

        // quiz feedback
        public event Action<int, string> OnQuizCompleted;

        public QuizManager()
        {
            InitializeQuestions();
            ResetQuiz();
           
        }

        // Creates the questions
        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
              
                // MULTIPLE CHOICE QUESTIONS (1-6
                // Question 1: Password Safety
                new QuizQuestion
                {
                    Question = "What is the best way to create a strong password?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "Use your birthday",
                        "Use a combination of letters, numbers, and symbols",
                        "Use your pet's name",
                        "Use 'password123456'"
                    },
                    CorrectAnswerIndex = 1, // Index 1 = second option 
                    Explanation = "A strong password should use a mix of uppercase/lowercase letters," +
                    " numbers, and special characters. Avoid personal information!"
                },

                // Question 2: Phishing Emails
                new QuizQuestion
                {
                    Question = "What should you do if you receive a suspicious email asking for your bank details?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "Reply with your details",
                        "Click the link to verify",
                        "Delete the email and report it",
                        "Forward it to your friends"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Never share personal information via email." +
                    " Legitimate companies will never ask for your bank details this way."
                },

                // Question 3: Phishing Definition
                new QuizQuestion
                {
                    Question = "What does 'phishing' refer to in cybersecurity?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "A type of computer virus",
                        "A fishing technique",
                        "A fraudulent attempt to obtain sensitive information",
                        "A type of firewall"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Phishing is a cyber attack where scammers" +
                    " trick victims into revealing sensitive information like passwords or credit card numbers."
                },

                // Question 4: Safe Browsing
                new QuizQuestion
                {
                    Question = "Which of these is NOT a safe browsing practice?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "Using HTTPS websites",
                        "Downloading from unknown sources",
                        "Using a VPN on public Wi-Fi",
                        "Keeping browser updated"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Downloading from unknown sources can expose your" +
                    " computer to malware and viruses. Always use trusted sources."
                },

                // Question 5: Two-Factor Authentication
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "A single password",
                        "An extra security layer requiring two verification methods",
                        "A type of antivirus",
                        "A biometric scanner"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA adds an extra layer of security by requiring" +
                    " something you know (password) and something you have (phone, token, and more.)."
                },

                // Question 6: Social Engineering
                new QuizQuestion
                {
                    Question = "What is social engineering in cybersecurity?",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<string> {
                        "A type of programming",
                        "Manipulating people to reveal sensitive information",
                        "Building social media apps",
                        "Network design"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering is a non-technical attack where hackers manipulate people into breaking security procedures."
                },

                
                // TRUE/FALSE QUESTIONS (7-11)
                

                // Question 7: Password Reuse
                new QuizQuestion
                {
                    Question = "You should use the same password for all your accounts.",
                    Type = QuestionType.TrueFalse,
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1, // Index 1 = "False"
                    Explanation = "Using the same password across multiple accounts is dangerous." +
                    " If one account is breached, all your accounts become vulnerable."
                },

                // Question 8: Public Wi-Fi Safety
                new QuizQuestion
                {
                    Question = "Public Wi-Fi is safe for online banking.",
                    Type = QuestionType.TrueFalse,
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi networks are unsecured and can be intercepted by hackers." +
                    " Use a VPN or your mobile data for sensitive transactions."
                },

                // Question 9: Suspicious Links
                new QuizQuestion
                {
                    Question = "Clicking on suspicious links is safe as long as you don't enter any information.",
                    Type = QuestionType.TrueFalse,
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Suspicious links can install malware on your device even without entering any" +
                    " information. Never click on unknown links."
                },

                // Question 10: Software Updates
                new QuizQuestion
                {
                    Question = "Updating your software regularly helps protect against security threats.",
                    Type = QuestionType.TrueFalse,
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 0, // Index 0 = "True"
                    Explanation = "Software updates often include security patches that fix known vulnerabilities." +
                    " Always keep your software updated!"
                },

                // Question 11: OTP Sharing
                new QuizQuestion
                {
                    Question = "You should share your OTP (One-Time Password) with your bank if they ask for it.",
                    Type = QuestionType.TrueFalse,
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "NEVER share your OTP with anyone! Banks will never ask for your OTP." +
                    " This is a common scam tactic."
                }
            };
        }

        // starts the quiz and restes the timer
        public void StartQuiz()
        {
            ResetQuiz();

            _quizActive = true;
            _startTimer = DateTime.Now;

            OnQuestionChanged?.Invoke(GetCurrentQuestionText());

        }

            //Gets the current Question
            public string GetCurrentQuestionText()
        {
            if (_currentQuestionIndex < _questions.Count)
                return _questions[_currentQuestionIndex].Question;
            return "";
        }

        // Gets the current question object
        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentQuestionIndex < _questions.Count)
                return _questions[_currentQuestionIndex];
            return null;
        }

        // returns the true and false
        public bool CheckAnswer(int selectedIndex)
        {
            var question = GetCurrentQuestion();
            if (question == null) return false;

            bool isCorrect = selectedIndex == question.CorrectAnswerIndex;

            if (isCorrect)
                _score++; // Increment score if correct

            // Notify listeners about score and answer result
            OnScoreUpdated?.Invoke(_score, _currentQuestionIndex + 1);
            OnAnswerChecked?.Invoke(isCorrect, question.Explanation);

            return isCorrect;
        }

        // Moves the next question or ends it
        public bool NextQuestion()
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _questions.Count)
            {
                // Quiz is complete
                _quizActive = false;
                int totalQuestions = _questions.Count;

                // Determine feedback based on score
                string feedback = _score >= 8 ? "🌟 Great job! You're a cybersecurity pro!" :
                                  _score >= 5 ? "👍 Good effort! Keep learning!" :
                                  "📚 Keep learning bro! Cybersecurity is important!";

                OnQuizCompleted?.Invoke(_score, feedback);
                return false;
            }

            // Show next question
            OnQuestionChanged?.Invoke(GetCurrentQuestionText());
            return true;
        }

        // resets the quiz to the reset state
        public void ResetQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = false;
            // Shuffle questions for variety each time
            _questions = _questions.OrderBy(q => Guid.NewGuid()).ToList();
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;
        public bool IsQuizActive() => _quizActive;
        public TimeSpan GetElapsedTime() => DateTime.Now - _startTimer;
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
    }

    // Question type of questions
    public enum QuestionType
    {
        MultipleChoice, // 4 Options
        TrueFalse // 2 options
    }
}
