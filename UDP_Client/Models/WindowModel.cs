using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UDP_Client.Models
{
    internal class WindowModel : INotifyPropertyChanged, IDisposable
    {
        private int serwerPort = 8080;

        private string serverAddress = "127.0.0.1";

        private string message = string.Empty;

        private readonly UdpClient udpClient = new();

        private IPEndPoint? endPoint;

        private bool disposedValue;

        private IPEndPoint? PEndPoint => endPoint ??= new(IPAddress.Parse(Address), Port);

        public int Port
        {
            get => serwerPort;
            set
            {
                serwerPort = value;
                endPoint = null;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => serverAddress;
            set
            {
                serverAddress = value;
                endPoint = null;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private async void sendMessageAsync(object o)
        {
            UdpReceiveResult receiveData;
            string message = o?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(message)) return;
            DialogMessages.Add(new()
            {
                Color = Colors.LightGreen,
                Message = message,
                DTime = DateTime.Now,
                Name = "Client",
                HAlignment = HorizontalAlignment.Left
            });
            byte[] data = Encoding.Unicode.GetBytes(message);
            udpClient?.Send(data, data.Length, PEndPoint);
            try { receiveData = await udpClient.ReceiveAsync(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Message = string.Empty;
                return;
            }
            string response = Encoding.Unicode.GetString(receiveData.Buffer);
            DialogMessages.Add(new()
            {
                Color = Colors.LightPink,
                Message = response,
                DTime = DateTime.Now,
                Name = "Server",
                HAlignment = HorizontalAlignment.Right
            });
            Message = string.Empty;
        }

        public ObservableCollection<DialogMessage> DialogMessages { get; set; } = new();

        public RelayCommand Send => new((o) => sendMessageAsync(o), (o) => !string.IsNullOrEmpty(Message) && IPAddress.TryParse(Address, out _) && Port > 1024);

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? prop = null) => PropertyChanged?.Invoke(this, new(prop));

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udpClient.Close();
                    udpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
