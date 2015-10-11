using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Scanner.Models
{
    public class CropControlPointsData
    {
        public CropControlPointsData()
        {

        }

        public Point LeftTopPoint { get; set; }

        public Point RightTopPoint { get; set; }

        public Point LeftBottomPoint { get; set; }

        public Point RightBottomPoint { get; set; }

        public Point LeftTopLinePoint { get; set; }

        public Point RightTopLinePoint { get; set; }

        public Point LeftBottomLinePoint { get; set; }

        public Point RightBottomLinePoint { get; set; }
    }
}
