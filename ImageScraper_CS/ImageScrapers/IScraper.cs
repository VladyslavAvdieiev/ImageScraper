using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScrapers
{
    public interface IScraper {
        IList<char> Alphabet { get; }
        void StartDownloading();
    }
}
