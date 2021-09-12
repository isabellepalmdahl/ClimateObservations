using System;
using System.Collections.Generic;
using System.Text;

namespace KlimatobservationerGrupp11
{
    public class Observer
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// observer's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// observer's last name
        /// </summary>
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }

    
}
