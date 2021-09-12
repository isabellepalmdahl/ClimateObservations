using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Class created to combine all classes needed for an Observationviewmodel
    /// </summary>
    public class ObservationViewModel
    {

        public Observer Observer { get; set; }
        public Unit Unit { get; set; }
        public Observation Observation { get; set; }
        public Geolocation Geolocation { get; set; }
        public Area Area { get; set; }
        public Country Country { get; set; }
        public List<Measurement> Measurements { get; set; } = new List<Measurement>();
        public List<Geolocation> Geolocations { get; set; } = new List<Geolocation>();
        public DateTime Date { get; set; }


        public override string ToString()
        {
            return $"{Date.ToShortDateString()}, {Area.Name}, {Country.Name}";
        }

    }

  
}
