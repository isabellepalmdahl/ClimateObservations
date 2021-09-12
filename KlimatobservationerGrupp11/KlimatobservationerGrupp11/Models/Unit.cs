using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Units in which the categories are measured
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unit description
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Unit abbreviation (cm, mm, m/s etc)
        /// </summary>
        public string Abbreviation { get; set; }


    }
}
