using DiscordRPC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordPresenceEditor.Models
{
    public class Config: INotifyPropertyChanged
    {
        private string _clientId;
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                OnPropertyChanged();

                _client = new DiscordRpcClient(_clientId);
                _client.Initialize();
            }
        }

        private string _state;
        public string State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private string _details;
        public string Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged();
            }
        }

        private string _imageKey;
        public string ImageKey
        {
            get => _imageKey;
            set
            {
                _imageKey = value;
                OnPropertyChanged();
            }
        }

        private string _imageText;
        public string ImageText
        {
            get => _imageText;
            set
            {
                _imageText = value;
                OnPropertyChanged();
            }
        }

        private bool _showElapsedTime;
        public bool ShowElapsedTime
        {
            get => _showElapsedTime;
            set
            {
                _showElapsedTime = value;
                OnPropertyChanged();
            }
        }

        private DiscordRpcClient _client;

        public Config(DiscordRpcClient client)
        {
            _client = client;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
