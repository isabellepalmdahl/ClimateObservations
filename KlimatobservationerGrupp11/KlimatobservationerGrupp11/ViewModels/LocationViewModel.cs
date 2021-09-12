using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// class created to join all three location tables.
    /// </summary>
    public class LocationViewModel
    {
        public Geolocation Geolocation { get; set; }
        public Area Area { get; set; }
        public Country Country { get; set; }

        public override string ToString()
        {
            return $"{Country.Name} - {Area.Name} - {Geolocation.ID}";
        }
    }
}
