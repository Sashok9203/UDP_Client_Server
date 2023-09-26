using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UDP_Client.Models
{
    internal class DialogMessage
    {
        public DateTime DTime { get; set; }
        public string Message { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public string Time => DTime.ToShortTimeString();
        public HorizontalAlignment HAlignment { get; set; }
    }
}
