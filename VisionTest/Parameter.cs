using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTest
{
    [Serializable]
    public class Parameter
    {
        public LineParameter lineParameter { get; set; }
        public GrayParameter grayParameter { get; set; }
        public Parameter()
        {
            lineParameter = new LineParameter();
            grayParameter= new GrayParameter();
        }
        
    }
    public class LineParameter
    {
        public double Row { get; set; }
        public double Column { get; set; }
        public double Angle { get; set; }

    }
    public class GrayParameter
    {
        public double MinGray { get; set; }
        public double MaxGray { get; set; }
    }


}
