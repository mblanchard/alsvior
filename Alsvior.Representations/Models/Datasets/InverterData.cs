﻿using Alsvior.Representations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Representations.Models
{
    public class InverterData: ILocatable
    {
        #region Coord Properties
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        #endregion Coord Properties

        public double Performance { get; set; }


    }
}
