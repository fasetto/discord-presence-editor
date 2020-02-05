using DiscordPresenceEditor.Commands;
using DiscordPresenceEditor.Models;
using DiscordRPC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiscordPresenceEditor
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand UpdatePresenceCommand => new RelayCommand(x => UpdatePresence(), p => CanUpdatePresence());

        public MainWindowViewModel()
        {
            PresenceConfig = new Config(Client);
            ReadConfig();
        }

        public DiscordRpcClient Client { get; set; }
        public Config PresenceConfig { get; set; }

        public async void UpdatePresence()
        {
            var presence = new RichPresence()
            {
                Details = PresenceConfig.Details,
                State = PresenceConfig.State,
                Assets = new Assets()
                {
                    LargeImageKey = PresenceConfig.ImageKey,
                    LargeImageText = PresenceConfig.ImageText,
                }
            };
            
            if (PresenceConfig.ShowElapsedTime)
                presence.Timestamps = new Timestamps(DateTime.UtcNow);

            Client.SetPresence(presence);

            StatusMessage = "Your discord presence updated successfully..";

            SaveConfig();

            await Task.Delay(5000);

            StatusMessage = "";
        }
        private void ReadConfig()
        {
            if (!File.Exists("./config.json"))
                return;

            var json = File.ReadAllText("./config.json");

            PresenceConfig = JsonConvert.DeserializeObject<Config>(json);

            Client = new DiscordRpcClient(PresenceConfig.ClientId);
            Client.Initialize();
        }

        private void SaveConfig()
        {

            var json = JsonConvert.SerializeObject(PresenceConfig, Formatting.Indented);
            File.WriteAllText("./config.json", json);
        }

        public bool CanUpdatePresence()
        {
            var canUpdate = !string.IsNullOrWhiteSpace(PresenceConfig.ClientId);
            return canUpdate;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
