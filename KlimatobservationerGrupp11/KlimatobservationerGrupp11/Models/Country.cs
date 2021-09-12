using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Country in which the areas are located
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of Country
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
