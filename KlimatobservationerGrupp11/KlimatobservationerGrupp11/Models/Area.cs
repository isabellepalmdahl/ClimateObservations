using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Area in which geolocation is located
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of Area
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Foreign Key to Country of Area
        /// </summary>
        public int? Country_ID { get; set; }
        /// <summary>
        /// connects with country
        /// </summary>
        public Country Country { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
