using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScrapers
{
    public interface IScraper {
        /// <summary>
        /// Represents the alphabet used in generating image names.
        /// </summary>
        IList<char> Alphabet { get; }
        /// <summary>
        /// Downloads random images from different clouds.
        /// </summary>
        void StartDownloading();
    }
}
