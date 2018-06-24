using System;
using System.Collections.Generic;
using System.Text;

namespace RoverExcercise.Models
{
    public class MarsGrid
    {
        public MarsGrid()
        {
            XMinBoundary = 0;
            YMinBoundary = 0;
        }
        public int XMinBoundary { get; private set; }
        public int YMinBoundary { get; private set; }

        public int XMaxBoundary { get; set; }
        public int YMaxBoundary { get; set; }

    }
}
