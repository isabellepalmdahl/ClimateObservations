using System;
using System.Collections.Generic;
using System.Text;
using KlimatobservationerGrupp11.Repositories;

namespace KlimatobservationerGrupp11.Models
{
    /// <summary>
    /// Category of Observation (Animal, Weather, Tree etc)
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of observation (hare, snow etc)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// refers back to its base category - Rabbit to Animal, Birch to Tree etc. FK from own table
        /// </summary>
        public int? Basecategory_ID { get; set; }
        /// <summary>
        /// used to get a Category's Basecategory information
        /// </summary>
        public Category BaseCategory { get; set; }
        /// <summary>
        /// Foreign Key to the unit in which the category is masured (cm, mm etc)
        /// </summary>
        public int? Unit_ID { get; set; }

        /// <summary>
        /// connects category to unit
        /// </summary>
        public Unit Unit { get; set; }


        public override string ToString()
        {
            return $"{Name} ({BaseCategory.Name}) in {Unit.Abbreviation}";
        }
     
    }

}

