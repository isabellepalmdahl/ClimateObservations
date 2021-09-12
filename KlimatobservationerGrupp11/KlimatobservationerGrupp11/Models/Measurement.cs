using System;
using System.Collections.Generic;
using System.Text;
using KlimatobservationerGrupp11.Repositories;
using KlimatobservationerGrupp11.Models;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Measurement
    /// </summary>
    public class Measurement
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Value of Measurement
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Foreign key to table Observation
        /// </summary>
        public int? Observation_ID { get; set; }

        /// <summary>
        /// Foreign key to table Category
        /// </summary>
        public int? Category_ID { get; set; }

        /// <summary>
        /// connects measurement to category
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// list of measurements
        /// </summary>
        public List<Measurement> Measurements { get; set; } = new List<Measurement>();


        public override string ToString()
        {
            return $"{Value} ({Category.Unit.Abbreviation}) {Category.Name}"; 
        }
    }
}
