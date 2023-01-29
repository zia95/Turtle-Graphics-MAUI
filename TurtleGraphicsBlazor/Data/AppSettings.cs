using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleGraphicsBlazor.Data
{
    public class AppSettings
    {
        public const int TurtleImageIdMin = 0;
        public const int TurtleImageIdMax = 6;

        public const int TurtleImageSizeMin = 0;
        public const int TurtleImageSizeMax = 3;

        public const int PenSizeMin = 1;
        public const int PenSizeMax = 20;

        public const int TurtleSpeedSlow =  500;
        public const int TurtleSpeedMedium =  300;
        public const int TurtleSpeedFast =  150;

        public const int VolumeMin = 0;
        public const int VolumeMax = 100;

        public int TurtleImageId { get; set; }
        public int TurtleImageSize { get; set; }
        public int PenSize { get;set; }

		public string CanvasColor { get; set; }
        public int TurtleSpeed { get; set; }
        public int Volume { get; set; }


        public bool IsValid()
        {
            if(TurtleImageId is >= TurtleImageIdMin and <= TurtleImageIdMax &&
                TurtleImageSize is >= TurtleImageSizeMin and <= TurtleImageSizeMax &&
                PenSize is >= PenSizeMin and <= PenSizeMax &&
                !string.IsNullOrWhiteSpace(CanvasColor) &&
                TurtleSpeed is TurtleSpeedSlow or TurtleSpeedMedium or TurtleSpeedFast &&
                Volume is >= VolumeMin and <= VolumeMax)
            {
                return true;
            }
            return false;
        }
    }
}
