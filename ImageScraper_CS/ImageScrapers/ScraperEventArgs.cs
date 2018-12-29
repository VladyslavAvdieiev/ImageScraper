using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScrapers
{
    public class ScraperEventArgs : EventArgs {
        public string Message { get; }
        public string Image { get; }
        public ScraperEventArgs(string message, string image) {
            Message = message;
            Image = image;
        }
    }
}
