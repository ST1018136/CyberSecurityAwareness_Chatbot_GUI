using System;
using System.Media;
using System.IO;
using ThreadingTask = System.Threading.Tasks.Task;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    public class AudioPlayer
    {
        public bool IsAudioAvailable { get; private set; } = true;

        public void PlayVoiceGreeting()
        {
            try
            {
                string[] paths = {
                    "greeting.wav",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greeting.wav")
                };

                string found = null;
                foreach (var p in paths)
                {
                    if (File.Exists(p))
                    {
                        found = p;
                        break;
                    }
                }

                if (found == null)
                {
                    IsAudioAvailable = false;
                    return;
                }

                using (SoundPlayer player = new SoundPlayer(found))
                {
                    player.PlaySync();
                }

                IsAudioAvailable = true;
            }
            catch (Exception)
            {
                IsAudioAvailable = false;
            }
        }

        public async ThreadingTask PlayVoiceGreetingAsync()
        {
            await ThreadingTask.Run(() => PlayVoiceGreeting());
        }
    }
}