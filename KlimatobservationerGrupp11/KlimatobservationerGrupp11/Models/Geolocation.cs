using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Geolocation
    /// </summary>
    public class Geolocation
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Latitude for geolocation
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude for geolocation
        /// </summary>
        public double Longitude{ get; set; }
        /// <summary>
        /// Foreign Key from table Area
        /// </summary>
        public int? Area_ID { get; set; }

        /// <summary>
        /// connects Geolocation to Area
        /// </summary>
        public Area Area { get; set; }

        public override string ToString()
        {
            return $"{Area.Country.Name} - {Area.Name} - {ID}";
        }
    }
}
