using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace CyberSecurityAwareness_Chatbot_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// This is the main chat window — it handles all user interaction,
    /// displays messages, and connects the UI to the CyberSecurityBot.
    /// </summary>
    public partial class MainWindow : Window
    {
        // The chatbot engine that processes all messages
        private CyberSecurityBot _chatbot;

        // Holds the path of any file the user attaches before sending
        private string _attachedFilePath = null;

        // Keeps a full record of every message in this session
        private List<string> _messageHistory;

        public MainWindow()
        {
            InitializeComponent();

            // Load the ASCII art into the background of the chat area
            LoadAsciiLogo();

            // Create the chatbot
            _chatbot = new CyberSecurityBot();

            // Start a fresh message history list
            _messageHistory = new List<string>();

            // Show the first message from CyberBot
            AddBotMessage("Yo!! I am your CyberSecurity assistant! What's your name?");

            // Placeholder text — disappears when the user clicks the input box
            UserInputTextBox.GotFocus += RemovePlaceholder;
            UserInputTextBox.LostFocus += SetPlaceholder;
        }

        private void SetPlaceholder(Object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInputTextBox.Text))
                PlaceholderText.Visibility = Visibility.Visible;
        }


        // Loads the ASCII logo into both the sidebar and the chat background.

        private void LoadAsciiLogo()
        {
            var logoDisplay = new LogoDisplay();
            string logo = logoDisplay.GetPurpleBackgroundLogo();

            // Background watermark in the centre of the chat area
            BackgroundAsciiLogo.Text = logo;
        }

        // Hides the placeholder text when the user clicks into the input box.

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserInputTextBox.Text))
                PlaceholderText.Visibility = Visibility.Collapsed;
        }


        // This Opens the CyberBot profile popup when the user clicks the contact header.

        private void ContactHeader_Click(object sender, MouseButtonEventArgs e)
        {
            ChatbotProfilePopup.IsOpen = true;
        }
        // Closes the profile popup when the user clicks "Start Conversation".

        private void CloseProfilePopup(object sender, RoutedEventArgs e)
        {
            ChatbotProfilePopup.IsOpen = false;
        }

        // This  Opens a file picker so the user can attach a file to their message.

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

        // Inserts a random emoji at the cursor's current position in the input box.

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

        // this Creates a blue message bubble on the RIGHT side of the chat (user message).

        private void AddUserMessage(string message)
        {
            // Outer bubble — blue, flat bottom-right corner = "sent" style
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(55, 151, 240)),
                CornerRadius = new CornerRadius(18, 18, 4, 18),
                Margin = new Thickness(40, 5, 10, 5),
                Padding = new Thickness(12, 8, 12, 8),
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            StackPanel panel = new StackPanel();

            // Show the attached file name above the message text if one was added
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

            // The actual message the user typed
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

            // Save to history for the History menu
            _messageHistory.Add($"You: {message}");

            // Clear the attached file after it has been sent
            _attachedFilePath = null;

            ScrollToBottom();
        }

        // Creates a grey message bubble on the  left

        private void AddBotMessage(string message)
        {
            // Outer bubble — grey, flat bottom-left corner = "received" style
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(228, 230, 235)),
                CornerRadius = new CornerRadius(18, 18, 18, 4),
                Margin = new Thickness(10, 5, 40, 5),
                Padding = new Thickness(12, 8, 12, 8),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            StackPanel panel = new StackPanel();

            // Small label above the message showing it came from CyberBot
            panel.Children.Add(new TextBlock
            {
                Text = "🛡️ CyberBot",
                FontSize = 11,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 3)
            });

            // The bot's reply text
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

            // Save to history
            _messageHistory.Add($"CyberBot: {message}");

            ScrollToBottom();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await ProcessInput();
        }

       
        // Called when the user presses Enter in the input box.
        private async void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !(Keyboard.Modifiers == ModifierKeys.Shift))
            {
                e.Handled = true;
                await ProcessInput();
            }
        }

        private async Task ProcessInput()
        {
            string input = UserInputTextBox.Text.Trim();

            // Do nothing if the box is completely empty
            if (string.IsNullOrEmpty(input)) return;

            // Clear the input box and restore the placeholder
            UserInputTextBox.Text = "";
            PlaceholderText.Visibility = Visibility.Visible;

            // Show what the user typed on the right side
            AddUserMessage(input);

            // Try to pick up the user's name from phrases
            if (_chatbot.UserName == "Guest" ||
                input.ToLower().Contains("my name is") ||
                input.ToLower().Contains("i am"))
            {
                ExtractName(input);
            }

            
            // CyberSecurityBot automatically syncs the favourite topic
            string response = _chatbot.ProcessUserInput(input);

            // Short delay to make the reply feel more natural
            await Task.Delay(400);

            // Show the bot's reply on the left side
            AddBotMessage(response);
        }

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

            // Only save it if it looks like a real single-word name
            if (!string.IsNullOrEmpty(name) && name.Length < 30 && !name.Contains(" "))
            {
                // Capitalise properly: "john" → "John"
                name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                _chatbot.SetUserName(name);
                AddBotMessage($"Nice to meet you, {name}! \n\nAsk me about passwords, scams, phishing, or safe browsing!");
            }
        }

        // Scrolls the chat panel to the very bottom so the latest message is visible.
   
        private void ScrollToBottom()
        {
            Dispatcher.Invoke(() => ChatScrollViewer.ScrollToBottom());
        }

        // Handles clicks on the side menu buttons: Home, History, Notifications, Settings.
      

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            string btn = (sender as Button)?.Name;

            switch (btn)
            {
                case "MenuHome":

                    // Clear all messages and start fresh
                    ChatMessagesPanel.Children.Clear();
                    AddBotMessage("🏠 Welcome back! How may I help you stay safe online?");
                    break;

                case "MenuHistory":

                    // Print every saved message from this session
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


        /// Handles clicks on the social media icon buttons in the right panel.

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
                else

                    findMe = "Social Media";
            }

            AddBotMessage($"Thank you for connecting with us on {findMe}! Follow us for more tips there buddy");
        }
    }
}
