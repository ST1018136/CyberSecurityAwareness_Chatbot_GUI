using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadingTask = System.Threading.Tasks.Task;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Timers;
using System.Windows.Media.Animation;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    
    public partial class MainWindow : Window
    {
        // Core components
        private CyberSecurityBot _chatbot; // Main chatbot logic
        private string _attachedFilePath = null; // Path of attached file
        private List<string> _messageHistory; // Stores chat history

        // Database and utilities
        private DatabaseHelper _dbHelper;
        private QuizManager _quizManager;
        private NLPsimulatorscs _nlpSimulator;

        // Quiz related fields
        private System.Timers.Timer _quizTimer;
        private bool _isQuizActive = false;
        private int _quizSecondsRemaining = 1800; // 30 minutes

        // Balloon animation fields
        private List<Balloon> _balloons = new List<Balloon>();
        private Random _random = new Random();

        public List<ThreadingTask> CurrentTasks { get; private set; }

        // Constructor: initializes all components and sets up the UI
        public MainWindow()
        {
            InitializeComponent();

            // Load the ASCII art into the background of the chat area
            LoadAsciiLogo();

            // Create the chatbot
            _chatbot = new CyberSecurityBot();

            // Start a fresh message history list
            _messageHistory = new List<string>();

            // Initialize database, quiz, and NLP components
            _dbHelper = new DatabaseHelper();
            _quizManager = new QuizManager();
            _nlpSimulator = new NLPsimulatorscs();

            // Subscribe to quiz events
            _quizManager.OnQuestionChanged += QuizManager_OnQuestionChanged;
            _quizManager.OnScoreUpdated += QuizManager_OnScoreUpdated;
            _quizManager.OnAnswerChecked += QuizManager_OnAnswerChecked;
            _quizManager.OnQuizCompleted += QuizManager_OnQuizCompleted;

            // Initialize task list
            CurrentTasks = new List<ThreadingTask>();

            // Show the first message from CyberBot
            AddBotMessage("Yo!! I am your CyberSecurity assistant! What's your name?");

            // Placeholder text — disappears when the user clicks the input box
            UserInputTextBox.GotFocus += RemovePlaceholder;
            UserInputTextBox.LostFocus += SetPlaceholder;

            // Test database connection and load tasks
            try
            {
                var tasks = _dbHelper.GetTasks();
                AddBotMessage($" Database connected! Found {tasks.Count} tasks.");
                RefreshTaskList();
            }
            catch (Exception ex)
            {
                AddBotMessage($" Database error: {ex.Message}");
            }
        }

        // Shows/hides the placeholder text in the input box
        private void SetPlaceholder(Object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInputTextBox.Text))
                PlaceholderText.Visibility = Visibility.Visible;
        }

        // Loads the ASCII art logo into the background
        private void LoadAsciiLogo()
        {
            var logoDisplay = new LogoDisplay();
            string logo = logoDisplay.GetPurpleBackgroundLogo();
            BackgroundAsciiLogo.Text = logo;
        }

        // Hides the placeholder when user starts typing
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserInputTextBox.Text))
                PlaceholderText.Visibility = Visibility.Collapsed;
        }

        // Opens the chatbot profile popup
        private void ContactHeader_Click(object sender, MouseButtonEventArgs e)
        {
            ChatbotProfilePopup.IsOpen = true;
        }

        // Closes the profile popup
        private void CloseProfilePopup(object sender, RoutedEventArgs e)
        {
            ChatbotProfilePopup.IsOpen = false;
        }

        // Opens file dialog to attach a file
        private void BtnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "All Files (*.*)|*.*|Images (*.png;*.jpg)|*.png;*.jpg|Documents (*.pdf;*.docx)|*.pdf;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                _attachedFilePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(_attachedFilePath);
                AddBotMessage($"📎 File attached: {fileName}\nYou can type your message and send it with this file.");
            }
        }

        // Inserts a random emoji at the cursor position
        private void BtnEmoji_Click(object sender, RoutedEventArgs e)
        {
            string[] emojis = { "😊", "😂", "❤️", "👍", "🔥", "🎉", "🔒", "🛡️", "⚠️", "💡" };
            Random rand = new Random();
            string emoji = emojis[rand.Next(emojis.Length)];

            int cursorPos = UserInputTextBox.CaretIndex;
            UserInputTextBox.Text = UserInputTextBox.Text.Insert(cursorPos, emoji);
            UserInputTextBox.CaretIndex = cursorPos + emoji.Length;
            UserInputTextBox.Focus();
        }

        // Adds a user message bubble to the chat
        private void AddUserMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(55, 151, 240)),
                CornerRadius = new CornerRadius(18, 18, 4, 18),
                Margin = new Thickness(40, 5, 10, 5),
                Padding = new Thickness(12, 8, 12, 8),
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            StackPanel panel = new StackPanel();

            // Show attached file if present
            if (!string.IsNullOrEmpty(_attachedFilePath))
            {
                string fileName = System.IO.Path.GetFileName(_attachedFilePath);
                panel.Children.Add(new TextBlock
                {
                    Text = $"📎 {fileName}",
                    FontSize = 11,
                    Foreground = Brushes.LightYellow,
                    Margin = new Thickness(0, 0, 0, 5)
                });
            }

            panel.Children.Add(new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 14,
                MaxWidth = 350,
                TextWrapping = TextWrapping.Wrap
            });

            bubble.Child = panel;
            ChatMessagesPanel.Children.Add(bubble);
            _messageHistory.Add($"You: {message}");
            _attachedFilePath = null;
            ScrollToBottom();
        }

        // Adds a bot message bubble to the chat
        private void AddBotMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(228, 230, 235)),
                CornerRadius = new CornerRadius(18, 18, 18, 4),
                Margin = new Thickness(10, 5, 40, 5),
                Padding = new Thickness(12, 8, 12, 8),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "🛡️ CyberBot",
                FontSize = 11,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 3)
            });

            panel.Children.Add(new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                FontSize = 14,
                MaxWidth = 350,
                TextWrapping = TextWrapping.Wrap
            });

            bubble.Child = panel;
            ChatMessagesPanel.Children.Add(bubble);
            _messageHistory.Add($"CyberBot: {message}");
            ScrollToBottom();
        }

        // Handles send button click
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await ProcessInput();
        }

        // Handles Enter key press in input box
        private async void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !(Keyboard.Modifiers == ModifierKeys.Shift))
            {
                e.Handled = true;
                await ProcessInput();
            }
        }

        // Processes user input - main logic handler
        private async ThreadingTask ProcessInput()
        {
            string input = UserInputTextBox.Text.Trim();

            if (string.IsNullOrEmpty(input)) return;

            UserInputTextBox.Text = "";
            PlaceholderText.Visibility = Visibility.Visible;

            AddUserMessage(input);

            // Check if user is introducing themselves
            if (_chatbot.UserName == "Guest" ||
                input.ToLower().Contains("my name is") ||
                input.ToLower().Contains("i am"))
            {
                ExtractName(input);
            }

            // Analyze intent using NLP
            string intent = _nlpSimulator.AnalyzeIntent(input);

            // Route to appropriate handler based on intent
            switch (intent)
            {
                case "add_task":
                    await HandleAddTask(input);
                    break;
                case "show_tasks":
                    ShowTasks();
                    break;
                case "start_quiz":
                    StartQuiz();
                    break;
                case "show_log":
                    ShowActivityLog();
                    break;
                case "password_help":
                    AddBotMessage("🔐 Password tips:\n• Use at least 12 characters\n• Mix letters, numbers, symbols\n• Never reuse passwords\n• Enable 2FA");
                    break;
                case "phishing_help":
                    AddBotMessage("🎣 Phishing prevention:\n• Check sender email carefully\n• Never click suspicious links\n• Look for spelling mistakes\n• Verify with the company directly");
                    break;
                default:
                    string response = _chatbot.ProcessUserInput(input);
                    await ThreadingTask.Delay(400);
                    AddBotMessage(response);
                    break;
            }
        }

        // Extracts the user's name from their input
        private void ExtractName(string input)
        {
            string lower = input.ToLower();
            string name = "";

            if (lower.Contains("my name is"))
            {
                int idx = lower.IndexOf("my name is") + 10;
                if (idx < input.Length) name = input.Substring(idx).Trim();
            }
            else if (lower.Contains("i am") && !lower.Contains("interested in"))
            {
                int idx = lower.IndexOf("i am") + 4;
                if (idx < input.Length) name = input.Substring(idx).Trim();
            }
            else if (lower.StartsWith("i'm "))
            {
                name = input.Substring(4).Trim();
            }

            // Validate and set the name
            if (!string.IsNullOrEmpty(name) && name.Length < 30 && !name.Contains(" "))
            {
                name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                _chatbot.SetUserName(name);
                AddBotMessage($"Nice to meet you, {name}! \n\nAsk me about passwords, scams, phishing, or safe browsing!");
            }
        }

        // Scrolls the chat view to the bottom
        private void ScrollToBottom()
        {
            Dispatcher.Invoke(() => ChatScrollViewer.ScrollToBottom());
        }

        // Handles menu button clicks
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            string btn = (sender as Button)?.Name;

            switch (btn)
            {
                case "MenuHome":
                    ChatMessagesPanel.Children.Clear();
                    AddBotMessage("🏠 Welcome back! How may I help you stay safe online?");
                    break;

                case "MenuHistory":
                    AddBotMessage("📜 Conversation history:");
                    foreach (var msg in _messageHistory)
                    {
                        ChatMessagesPanel.Children.Add(new TextBlock
                        {
                            Text = msg,
                            FontSize = 11,
                            Foreground = Brushes.Gray,
                            Margin = new Thickness(20, 2, 20, 2)
                        });
                    }
                    ScrollToBottom();
                    break;

                case "MenuNotifications":
                    AddBotMessage("🔔 No new security alerts. You're still safe buddy!!!");
                    break;

                case "MenuSettings":
                    AddBotMessage("⚙️ Settings: Notifications ON | Security Alerts ON | Auto-save ON");
                    break;
            }
        }

        // Handles social media button clicks
        private void SocialMedia_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string findMe = "";

            if (clickedButton != null)
            {
                if (clickedButton.Name == "BtnFacebook") findMe = "Facebook";
                else if (clickedButton.Name == "BtnInstagram") findMe = "Instagram";
                else if (clickedButton.Name == "BtnTwitter") findMe = "Twitter";
                else if (clickedButton.Name == "BtnWhatsApp") findMe = "WhatsApp";
                else findMe = "Social Media";
            }

            AddBotMessage($"Thank you for connecting with us on {findMe}! Follow us for more tips there buddy");
        }

        // ---- TASK METHODS ----

        // Handles adding a new task
        private async ThreadingTask HandleAddTask(string input)
        {
            string taskDetails = _nlpSimulator.ExtractTaskDetails(input);
            DateTime? reminderDate = _nlpSimulator.ExtractReminderDate(input);

            string taskTitle = taskDetails.Length > 50 ? taskDetails.Substring(0, 50) : taskDetails;

            int taskId = _dbHelper.InsertTask(taskTitle, taskDetails, reminderDate);
            _dbHelper.LogActivity("Task Added", $"Task '{taskTitle}' added");

            AddBotMessage($"✅ Task '{taskTitle}' added successfully!");
            if (reminderDate.HasValue)
            {
                AddBotMessage($"⏰ Reminder set for {reminderDate.Value.ToShortDateString()}");
            }
            RefreshTaskList();
        }

        // Displays all tasks
        private void ShowTasks()
        {
            var tasks = _dbHelper.GetTasks();
            if (tasks.Count == 0)
            {
                AddBotMessage("📋 You have no tasks yet. Add one by saying 'Add task'!");
                return;
            }

            AddBotMessage($"📋 You have {tasks.Count} tasks:");
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "✅" : "⏳";
                AddBotMessage($"   {status} {task.Title}");
            }
        }

        // Displays the activity log
        private void ShowActivityLog()
        {
            var logs = _dbHelper.GetActivityLog(10);
            if (logs.Count == 0)
            {
                AddBotMessage("📋 No activity logged yet.");
                return;
            }

            AddBotMessage("📜 Recent Activity Log:");
            foreach (var log in logs)
            {
                AddBotMessage($"   • {log.Timestamp.ToShortTimeString()} - {log.Action}: {log.Details}");
            }
        }

        // Refreshes the task list UI
        private void RefreshTaskList()
        {
            if (TaskListPanel == null) return;

            TaskListPanel.Children.Clear();
            var tasks = _dbHelper.GetTasks();

            if (tasks.Count == 0)
            {
                TaskListPanel.Children.Add(new TextBlock
                {
                    Text = "No tasks yet. Add one above!",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    FontSize = 14,
                    Margin = new Thickness(10)
                });
                return;
            }

            foreach (var task in tasks)
            {
                Border taskBorder = new Border
                {
                    Background = task.IsCompleted ? new SolidColorBrush(Color.FromRgb(40, 80, 40)) :
                                                    new SolidColorBrush(Color.FromRgb(42, 42, 62)),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(0, 5, 0, 5)
                };

                Grid taskGrid = new Grid();
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                StackPanel textPanel = new StackPanel();
                textPanel.Children.Add(new TextBlock
                {
                    Text = task.Title,
                    Foreground = task.IsCompleted ? System.Windows.Media.Brushes.Gray :
                                                    System.Windows.Media.Brushes.White,
                    FontSize = 14,
                    FontWeight = task.IsCompleted ? FontWeights.Normal : FontWeights.SemiBold,
                    TextDecorations = task.IsCompleted ? TextDecorations.Strikethrough : null
                });

                if (!string.IsNullOrEmpty(task.Description))
                {
                    textPanel.Children.Add(new TextBlock
                    {
                        Text = task.Description,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        FontSize = 11
                    });
                }

                if (task.ReminderDate.HasValue)
                {
                    textPanel.Children.Add(new TextBlock
                    {
                        Text = $"⏰ {task.ReminderDate.Value.ToShortDateString()}",
                        Foreground = System.Windows.Media.Brushes.Gold,
                        FontSize = 11
                    });
                }

                taskGrid.Children.Add(textPanel);
                Grid.SetColumn(textPanel, 0);

                // Complete button (only for incomplete tasks)
                if (!task.IsCompleted)
                {
                    Button completeBtn = new Button
                    {
                        Content = "✅",
                        Background = System.Windows.Media.Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                        Cursor = System.Windows.Input.Cursors.Hand,
                        Margin = new Thickness(5, 0, 5, 0),
                        Tag = task.Id
                    };
                    completeBtn.Click += CompleteTask_Click;
                    taskGrid.Children.Add(completeBtn);
                    Grid.SetColumn(completeBtn, 1);
                }

                // Delete button
                Button deleteBtn = new Button
                {
                    Content = "❌",
                    Background = System.Windows.Media.Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = task.Id
                };
                deleteBtn.Click += DeleteTask_Click;
                taskGrid.Children.Add(deleteBtn);
                Grid.SetColumn(deleteBtn, 2);

                taskBorder.Child = taskGrid;
                TaskListPanel.Children.Add(taskBorder);
            }
        }

        // Marks a task as complete
        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int taskId = (int)btn.Tag;
            _dbHelper.CompleteTask(taskId);
            _dbHelper.LogActivity("Task Completed", $"Task ID {taskId} completed");
            AddBotMessage("✅ Task marked as completed! Great job!");
            RefreshTaskList();
        }

        // Deletes a task
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int taskId = (int)btn.Tag;
            _dbHelper.DeleteTask(taskId);
            _dbHelper.LogActivity("Task Deleted", $"Task ID {taskId} deleted");
            AddBotMessage("🗑️ Task deleted successfully!");
            RefreshTaskList();
        }

        // Refreshes tasks manually
        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
            AddBotMessage("🔄 Tasks refreshed!");
        }

        // Handles Enter key in task input
        private void TaskInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTaskButton_Click(sender, e);
            }
        }

        // Adds a task from the task panel
        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskText = TaskInputBox.Text.Trim();
            if (string.IsNullOrEmpty(taskText)) return;

            await HandleAddTask(taskText);
            TaskInputBox.Clear();
        }

        // ---- QUIZ METHODS ----

        // Starts the cybersecurity quiz
        private void StartQuiz()
        {
            if (_quizManager.IsQuizActive())
            {
                AddBotMessage("⚠️ A quiz is already in progress!");
                return;
            }

            _quizManager.StartQuiz();
            _isQuizActive = true;
            _quizSecondsRemaining = 1800;
            QuizPopup.IsOpen = true;
            QuizScore.Text = "Score: 0/11";
            QuizFeedback.Text = "";

            // Start the quiz timer
            _quizTimer = new System.Timers.Timer(1000);
            _quizTimer.Elapsed += QuizTimer_Elapsed;
            _quizTimer.Start();

            _dbHelper.LogActivity("Quiz Started", "User started the cybersecurity quiz");
        }

        // Handles quiz timer tick
        private void QuizTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _quizSecondsRemaining--;
                TimeSpan time = TimeSpan.FromSeconds(_quizSecondsRemaining);
                QuizTimer.Text = $"⏱️ {time.Minutes:D2}:{time.Seconds:D2}";

                if (_quizSecondsRemaining <= 0)
                {
                    _quizTimer.Stop();
                    _isQuizActive = false;
                    QuizPopup.IsOpen = false;
                    AddBotMessage("⏰ Time's up! The quiz has ended.");
                }
            });
        }

        // Updates the quiz question display
        private void QuizManager_OnQuestionChanged(string question)
        {
            Dispatcher.Invoke(() =>
            {
                QuizQuestionText.Text = question;
                ResetQuizOptionColors();
                QuizFeedback.Text = "";
                SetQuizOptionsEnabled(true);

                var currentQuestion = _quizManager.GetCurrentQuestion();
                if (currentQuestion != null)
                {
                    var options = currentQuestion.Options;
                    QuizOption1.Content = options.Count > 0 ? $"A: {options[0]}" : "A";
                    QuizOption2.Content = options.Count > 1 ? $"B: {options[1]}" : "B";
                    QuizOption3.Content = options.Count > 2 ? $"C: {options[2]}" : "C";
                    QuizOption4.Content = options.Count > 3 ? $"D: {options[3]}" : "D";
                }
            });
        }

        // Updates the quiz score display
        private void QuizManager_OnScoreUpdated(int score, int total)
        {
            Dispatcher.Invoke(() =>
            {
                QuizScore.Text = $"Score: {score}/{total}";
            });
        }

        // Shows feedback for an answered question
        private void QuizManager_OnAnswerChecked(bool isCorrect, string explanation)
        {
            Dispatcher.Invoke(() =>
            {
                QuizFeedback.Text = explanation;
                QuizFeedback.Foreground = isCorrect ?
                    new SolidColorBrush(Colors.LightGreen) :
                    new SolidColorBrush(Colors.OrangeRed);
            });
        }

        // Handles quiz completion
        private void QuizManager_OnQuizCompleted(int score, string feedback)
        {
            Dispatcher.Invoke(() =>
            {
                _isQuizActive = false;
                _quizTimer?.Stop();
                QuizPopup.IsOpen = false;
                _dbHelper.LogActivity("Quiz Completed", $"Score: {score}/11 - {feedback}");
                AddBotMessage($"🏆 Quiz Complete! You scored {score}/11!");
                AddBotMessage($"💬 {feedback}");

                // Celebrate with balloons if score is good
                if (score >= 8)
                {
                    StartBalloonCelebration();
                }
            });
        }

        // Handles quiz option button clicks
        private void QuizOption_Click(object sender, RoutedEventArgs e)
        {
            if (!_isQuizActive) return;

            Button btn = sender as Button;
            int selectedIndex = int.Parse(btn.Tag.ToString());
            var question = _quizManager.GetCurrentQuestion();

            if (question == null) return;

            bool isCorrect = _quizManager.CheckAnswer(selectedIndex);

            ResetQuizOptionColors();
            btn.Background = isCorrect ?
                new SolidColorBrush(Colors.Green) :
                new SolidColorBrush(Colors.Red);

            // Show the correct answer if user got it wrong
            if (!isCorrect)
            {
                foreach (Button option in new[] { QuizOption1, QuizOption2, QuizOption3, QuizOption4 })
                {
                    int index = int.Parse(option.Tag.ToString());
                    if (index == question.CorrectAnswerIndex)
                    {
                        option.Background = new SolidColorBrush(Colors.Green);
                    }
                }
            }

            SetQuizOptionsEnabled(false);

            // Move to next question after a delay
            ThreadingTask.Delay(1500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_quizManager.NextQuestion())
                    {
                        SetQuizOptionsEnabled(true);
                    }
                });
            });
        }

        // Enables or disables quiz options
        private void SetQuizOptionsEnabled(bool enabled)
        {
            QuizOption1.IsEnabled = enabled;
            QuizOption2.IsEnabled = enabled;
            QuizOption3.IsEnabled = enabled;
            QuizOption4.IsEnabled = enabled;
        }

        // Resets quiz option colors to default
        private void ResetQuizOptionColors()
        {
            foreach (Button option in new[] { QuizOption1, QuizOption2, QuizOption3, QuizOption4 })
            {
                option.Background = new SolidColorBrush(Color.FromRgb(58, 58, 85));
                option.BorderBrush = new SolidColorBrush(Color.FromRgb(74, 0, 224));
            }
        }

        // ---- BALLOON METHODS ----

        // Starts the balloon celebration animation
        private void StartBalloonCelebration()
        {
            BalloonCanvas.Visibility = Visibility.Visible;
            _balloons.Clear();
            BalloonCanvas.Children.Clear();

            Color[] colors = {
                Colors.Red, Colors.Orange, Colors.Yellow, Colors.Green,
                Colors.Blue, Colors.Indigo, Colors.Violet, Colors.Pink,
                Colors.Cyan, Colors.Magenta, Colors.Gold
            };

            // Create 30 balloons at random positions
            for (int i = 0; i < 30; i++)
            {
                double x = _random.NextDouble() * BalloonCanvas.ActualWidth;
                if (x < 10) x = 10;
                double y = BalloonCanvas.ActualHeight + _random.NextDouble() * 100;
                Color color = colors[_random.Next(colors.Length)];
                double size = 30 + _random.NextDouble() * 30;

                var balloon = new Balloon(x, y, color, size);
                BalloonCanvas.Children.Add(balloon.String);
                BalloonCanvas.Children.Add(balloon.Body);
                _balloons.Add(balloon);
            }

            // Start animation
            CompositionTarget.Rendering += AnimateBalloons;

            // Stop after 10 seconds
            ThreadingTask.Delay(10000).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    CompositionTarget.Rendering -= AnimateBalloons;
                    BalloonCanvas.Visibility = Visibility.Collapsed;
                    BalloonCanvas.Children.Clear();
                    _balloons.Clear();
                });
            });
        }

        // Animates balloons floating upward with sway
        private void AnimateBalloons(object sender, EventArgs e)
        {
            foreach (var balloon in _balloons)
            {
                if (balloon.Body == null || balloon.String == null) continue;

                double newY = Canvas.GetTop(balloon.Body) - balloon.Speed;
                Canvas.SetTop(balloon.Body, newY);
                Canvas.SetTop(balloon.String, newY);

                double sway = Math.Sin(newY / 50) * 0.5;
                Canvas.SetLeft(balloon.Body, balloon.XPosition + sway);
                Canvas.SetLeft(balloon.String, balloon.XPosition + sway);

                // Reset balloon when it goes off screen
                if (newY < -100)
                {
                    double x = _random.NextDouble() * BalloonCanvas.ActualWidth;
                    if (x < 10) x = 10;
                    double y = BalloonCanvas.ActualHeight + 50;
                    Canvas.SetTop(balloon.Body, y);
                    Canvas.SetTop(balloon.String, y);
                    Canvas.SetLeft(balloon.Body, x);
                    Canvas.SetLeft(balloon.String, x);
                    balloon.XPosition = x;
                }
            }
        }
    }
}