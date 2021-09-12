using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11.Models
{
     /// <summary>
     /// Observations
     /// </summary>
    public class Observation
    {
       /// <summary>
       /// Primary Key
       /// </summary>
        public int ID { get; set; }
       /// <summary>
       /// Date of Observation - defaults to today's date if left blank
       /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Foreign Key from table Observer
        /// </summary>
        public int? Observer_ID { get; set; }
        /// <summary>
        /// Foreign Key from table Geolocation
        /// </summary>
        public int? Geolocation_ID { get; set; }


    }
}
