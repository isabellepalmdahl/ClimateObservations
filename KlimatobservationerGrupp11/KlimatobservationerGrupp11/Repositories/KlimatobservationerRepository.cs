using System;
using System.Collections.Generic;
using System.Text;
using KlimatobservationerGrupp11.Models;
using Npgsql;


namespace KlimatobservationerGrupp11.Repositories
{
    public class KlimatobservationerRepository
    {

        private static readonly string connectionString = "Server=localhost;Port=5432;Database=ClimateObservations;User ID=demouser;Password=DemoUser;";

        #region Observer

        #region Read Observer

        /// <summary>
        /// gets one observer from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>observer</returns>
        public Observer GetObserver(int id)
        {

            string stmt = "SELECT * FROM observer WHERE id = @id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();

            Observer observer = null;

            while (reader.Read())
            {
                observer = new Observer
                {
                    ID = (int)reader["id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"],


                };

            }

            return observer;
        }

        /// <summary>
        /// gets list of observers from database
        /// </summary>
        /// <returns>list of observers</returns>
        public List<Observer> GetObservers()
        {
            string stmt = "SELECT * FROM observer where id = @id ORDER BY lastname";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();
            
            Observer observer = null;
            var observers = new List<Observer>();
           
            while (reader.Read())
            {
                observer = new Observer
                {
                    ID = (int)reader["id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"],

                };
               
                observers.Add(observer);
            }
            return observers;
        }
        #endregion

        #region Create Observer
        /// <summary>
        /// creates new observer
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>observer</returns>
        public Observer AddObserver(Observer observer)
        {
            string stmt = "INSERT INTO observer(firstname, lastname) VALUES (@FirstName, @LastName) RETURNING Id";

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("FirstName", observer.FirstName);
                command.Parameters.AddWithValue("LastName", observer.LastName);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    observer.ID = (int)reader["Id"];
                        
                }

                return observer;

            }

            catch (PostgresException exception)
            {
                string errorCode = exception.SqlState;
                throw new Exception("Du måste ange observantens namn", exception);
            }


        }

        #endregion

        #region Update Observer

        /// <summary>
        /// updates details of existing observer
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>observer</returns>
        public int UpdateObserver(Observer observer) 
        {
            string stmt = "UPDATE observer SET firstname = @firstname, lastname = @lastname WHERE id = @id";
            int result = 0;
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            try
            {

                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("FirstName", observer.FirstName);
                command.Parameters.AddWithValue("LastName", observer.LastName);
                command.Parameters.AddWithValue("id", observer.ID);
                result = command.ExecuteNonQuery();

                return result;

            }

            catch (PostgresException exception)
            {
                string errorCode = exception.SqlState;
                throw new Exception("Update failed....", exception);
            }

        }

        #endregion

        #region Delete Observer

        /// <summary>
        /// deletes selected observer from list and database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>number of deleted posts</returns>
        public int DeleteObserver(int id)
        {
            string stmt = "DELETE FROM observer WHERE id = @id";
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                command.Parameters.AddWithValue("id", id);

                return command.ExecuteNonQuery();
            }
            catch (PostgresException exception)
            {
                throw new Exception("The observer could not be removed from the database.", exception);
            }
        }

        /// <summary>
        /// method to check if observer has previous observations
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="observations"></param>
        /// <returns>bool</returns>
        public bool ObserverHasObservations(Observer observer, List<ObservationViewModel> observations)
        {
            
            foreach  (ObservationViewModel observationViewModel in observations)
            {
                if(observationViewModel.Observer.ID == observer.ID)
                {
                    return true;

                }
            }

            return false;

        }

        #endregion

        #endregion

        #region Observation

        #region Read Observation

        #region GetObservation
        /// <summary>
        /// Gets observation
        /// </summary>
        /// <param name="id"></param>
        /// <returns>observation</returns>
        public Observation GetObservation(int id)
        {

            string stmt = "SELECT * FROM observation WHERE id = @id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();

            Observation observation = null;

            while (reader.Read())
            {
                observation = new Observation
                {
                    ID = (int)reader["id"],
                    Date = (DateTime)reader["date"],
                    Observer_ID = (int?)reader["observer_id"],
                    Geolocation_ID = (int?)reader["geolocation_id"]


                };

            }

            return observation;
        }

        #endregion

        #region Get Observation ViewModel and Get Observer Observations

        /// <summary>
        /// Gets a list of all observations stored as observationviewmodels
        /// </summary>
        /// <returns>list of observations</returns>
        public List<ObservationViewModel> GetObservationViewModel()
        {
            string stmt = "SELECT observer.id AS observerid, observer.firstname, observer.lastname, " +
            "unit.id AS unitid, unit.type, unit.abbreviation, " +
            "category.id AS categoryid, category.name AS categoryname, category.basecategory_id, category.unit_id, " +
            "measurement.id AS measurementid, measurement.value, measurement.observation_id, measurement.category_id," +
            "observation.id AS observationid, observation.date, observation.observer_id, observation.geolocation_id, " +
            "geolocation.id as geolocationid, geolocation.latitude, geolocation.longitude, geolocation.area_id, " +
            "area.id AS areaid, area.name AS areaname, area.country_id, " +
            "country.id AS countryid, country.name AS countryname " +
            "FROM observation JOIN measurement ON observation.id = measurement.observation_id JOIN category ON category.id = measurement.category_id JOIN geolocation ON geolocation.id = observation.geolocation_id JOIN area ON geolocation.area_id = area.id JOIN country ON country.id = area.country_id JOIN observer ON observer.id = observation.observer_id JOIN unit ON unit.id = category.unit_id WHERE observation.id > 12 ORDER BY observation.id;";

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            ObservationViewModel observationViewModel = null;

            var observations = new List<ObservationViewModel>();
            var measurements = new List<Measurement>();
            var geolocations = new List<Geolocation>();

            int observationid = 0;

            while (reader.Read())
            {
                if (observationid != (int)reader["observation_id"])
                {

                    observationViewModel = new ObservationViewModel
                    {
                        Observer = new Observer
                        {
                            ID = (int)reader["observerid"],
                            FirstName = (string)reader["firstname"],
                            LastName = (string)reader["lastname"]
                        },

                        Geolocation = new Geolocation
                        {
                            ID = (int)reader["geolocationid"],
                            Latitude = (double)reader["latitude"],
                            Longitude = (double)reader["longitude"],
                            Area_ID = (int?)reader["area_id"]
                        },

                        Area = new Area
                        {
                            ID = (int)reader["areaid"],
                            Name = (string)reader["areaname"],
                            Country_ID = (int?)reader["country_id"]
                        },

                        Country = new Country
                        {
                            ID = (int)reader["countryid"],
                            Name = (string)reader["countryname"]
                        },

                        Date = (DateTime)reader["date"],

                    };

                    var measurement = new Measurement
                    {
                        ID = (int)reader["measurementid"],
                        Value = (double)reader["value"],
                        Category_ID = (int)reader["category_id"],
                        Observation_ID = (int)reader["observation_id"],

                        Category = new Category
                        {
                            ID = (int)reader["categoryid"],
                            Name = (string)reader["categoryname"],
                            Basecategory_ID = (int?)reader["basecategory_id"],
                            Unit_ID = (int?)reader["unit_id"],

                            Unit = new Unit
                            {
                                ID = (int)reader["unitid"],
                                Type = (string)reader["type"],
                                Abbreviation = (string)reader["abbreviation"],
                            },
                        },

                    };

                    var geolocation = new Geolocation
                    {
                        ID = (int)reader["geolocationid"],
                        Latitude = (double)reader["latitude"],
                        Longitude = (double)reader["longitude"],
                        Area_ID = (int?)reader["area_id"]
                    };

                    observationViewModel.Measurements.Add(measurement);
                    observations.Add(observationViewModel);
                    observationViewModel.Geolocations.Add(geolocation);
                }

                else
                {
                    var measurement = new Measurement
                    {
                        ID = (int)reader["measurementid"],
                        Value = (double)reader["value"],
                        Category_ID = (int)reader["category_id"],
                        Observation_ID = (int)reader["observation_id"],

                        Category = new Category
                        {
                            ID = (int)reader["categoryid"],
                            Name = (string)reader["categoryname"],
                            Basecategory_ID = (int?)reader["basecategory_id"],
                            Unit_ID = (int?)reader["unit_id"],

                            Unit = new Unit
                            {
                                ID = (int)reader["unitid"],
                                Type = (string)reader["type"],
                                Abbreviation = (string)reader["abbreviation"],
                            },
                        },

                    };

                    observationViewModel.Measurements.Add(measurement);
                }

                observationid = (int)reader["observation_id"];

            }

            return observations;
        }

        /// <summary>
        /// Gets a list of all observations made by selected observer, stored as observationviewmodels
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>list of observations</returns>
        public List<ObservationViewModel> GetObserverObservations(Observer observer)
        {
            string stmt = "SELECT observer.id AS observerid, observer.firstname, observer.lastname, " +
            "unit.id AS unitid, unit.type, unit.abbreviation, " +
            "category.id AS categoryid, category.name AS categoryname, category.basecategory_id, category.unit_id, " +
            "measurement.id AS measurementid, measurement.value, measurement.observation_id, measurement.category_id," +
            "observation.id AS observationid, observation.date, observation.observer_id, observation.geolocation_id, " +
            "geolocation.id as geolocationid, geolocation.latitude, geolocation.longitude, geolocation.area_id, " +
            "area.id AS areaid, area.name AS areaname, area.country_id, " +
            "country.id AS countryid, country.name AS countryname " +
            "FROM observation JOIN measurement ON observation.id = measurement.observation_id JOIN category ON category.id = measurement.category_id JOIN geolocation ON geolocation.id = observation.geolocation_id JOIN area ON geolocation.area_id = area.id JOIN country ON country.id = area.country_id JOIN observer ON observer.id = observation.observer_id JOIN unit ON unit.id = category.unit_id WHERE observer.id = @observer.id ORDER BY observation.date;";

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("observer.id", observer.ID);

            using var reader = command.ExecuteReader();

            ObservationViewModel observationViewModel = null;

            var observations = new List<ObservationViewModel>();

            int observationid = 0;

            while (reader.Read())
            {
                if (observationid != (int)reader["observation_id"])
                {
                    observationViewModel = new ObservationViewModel
                    {
                        Observer = new Observer
                        {
                            ID = (int)reader["observerid"],
                            FirstName = (string)reader["firstname"],
                            LastName = (string)reader["lastname"]
                        },

                        Geolocation = new Geolocation
                        {
                            ID = (int)reader["geolocationid"],
                            Latitude = (double)reader["latitude"],
                            Longitude = (double)reader["longitude"],
                            Area_ID = (int?)reader["area_id"]
                        },

                        Area = new Area
                        {
                            ID = (int)reader["areaid"],
                            Name = (string)reader["areaname"],
                            Country_ID = (int?)reader["country_id"]
                        },

                        Country = new Country
                        {
                            ID = (int)reader["countryid"],
                            Name = (string)reader["countryname"]
                        },

                        Date = (DateTime)reader["date"],

                    };

                    var measurement = new Measurement
                    {
                        ID = (int)reader["measurementid"],
                        Value = (double)reader["value"],
                        Category_ID = (int)reader["category_id"],
                        Observation_ID = (int)reader["observation_id"],

                        Category = new Category
                        {
                            ID = (int)reader["categoryid"],
                            Name = (string)reader["categoryname"],
                            Basecategory_ID = (int?)reader["basecategory_id"],
                            Unit_ID = (int?)reader["unit_id"],

                            Unit = new Unit
                            {
                                ID = (int)reader["unitid"],
                                Type = (string)reader["type"],
                                Abbreviation = (string)reader["abbreviation"],
                            },
                        },
                    };

                    var geolocation = new Geolocation
                    {
                        ID = (int)reader["geolocationid"],
                        Latitude = (double)reader["latitude"],
                        Longitude = (double)reader["longitude"],
                        Area_ID = (int?)reader["area_id"]
                    };

                    observationViewModel.Measurements.Add(measurement);
                    observations.Add(observationViewModel);
                    observationViewModel.Geolocations.Add(geolocation);

                }
                
                else
                {
                    var measurement = new Measurement
                    {
                        ID = (int)reader["measurementid"],
                        Value = (double)reader["value"],
                        Category_ID = (int)reader["category_id"],
                        Observation_ID = (int)reader["observation_id"],

                        Category = new Category
                        {
                            ID = (int)reader["categoryid"],
                            Name = (string)reader["categoryname"],
                            Basecategory_ID = (int?)reader["basecategory_id"],
                            Unit_ID = (int?)reader["unit_id"],

                            Unit = new Unit
                            {
                                ID = (int)reader["unitid"],
                                Type = (string)reader["type"],
                                Abbreviation = (string)reader["abbreviation"],
                            },
                        },
                    };

                    observationViewModel.Measurements.Add(measurement);
                }

                observationid = (int)reader["observation_id"];

            }

            return observations;
        }

        #endregion

        #endregion

        #region Create Observation
        /// <summary>
        /// adds a new observationviewmodel
        /// </summary>
        /// <param name="observationViewModel"></param>
        /// <returns>observationviewmodel</returns>
        public ObservationViewModel AddObservation(ObservationViewModel observationViewModel) 
        {           
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                string stmt = "INSERT INTO observation(date, observer_id, geolocation_id) VALUES (@date, @observer_id, @geolocation_id) RETURNING id;";

                using var command = new NpgsqlCommand(stmt, conn);
         
                command.Parameters.AddWithValue("date", observationViewModel.Date);
                command.Parameters.AddWithValue("observer_id", observationViewModel.Observer.ID);
                command.Parameters.AddWithValue("geolocation_id", observationViewModel.Geolocation.ID);

                int observationID = (int)command.ExecuteScalar();


                foreach(Measurement measurement in observationViewModel.Measurements)
                {
                    stmt = "INSERT INTO measurement(value, observation_id, category_id) VALUES(@value, @observation_id, @category_id);";

                    command.CommandText = stmt;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("value", measurement.Value);
                    command.Parameters.AddWithValue("observation_id", observationID);
                    command.Parameters.AddWithValue("category_id", measurement.Category_ID);

                   command.ExecuteNonQuery();
                }

                transaction.Commit();
                return observationViewModel;

            }
            catch (PostgresException exception)
            {
                transaction.Rollback();
                string errorCode = exception.SqlState;
                throw new Exception("Error message", exception);
            }

        }

        #endregion

        #region Edit Observation

        /// <summary>
        /// edits an observation from the observationviewmodel list
        /// </summary>
        /// <param name="observationViewModel"></param>
        /// <returns>observationviewmodel</returns>
        public ObservationViewModel EditObservation(ObservationViewModel observationViewModel, Measurement measurement, Geolocation geolocation)
        {

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var transaction = conn.BeginTransaction();

            try
            {
                string stmt = "UPDATE observation SET date = @date, geolocation_id = @geolocation_id WHERE id = @id;"; // kommer den att veta vilken ID?

                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("id", measurement.Observation_ID);
                command.Parameters.AddWithValue("date", observationViewModel.Date);
                command.Parameters.AddWithValue("geolocation_id", geolocation.ID);

                command.ExecuteNonQuery();


                stmt = "UPDATE measurement SET value = @value, category_id = @category_id WHERE id = @id;"; // kommer den att veta vilken ID?

                command.CommandText = stmt;
                command.Parameters.Clear();

                foreach (Measurement measurement1 in observationViewModel.Measurements)
                {

                    command.Parameters.AddWithValue("id", measurement.ID);
                    command.Parameters.AddWithValue("value", measurement.Value);
                    command.Parameters.AddWithValue("category_id", measurement.Category_ID);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
                return observationViewModel;

            }

            catch (PostgresException exception)
            {
                transaction.Rollback();
                string errorCode = exception.SqlState;
                throw new Exception("Error message", exception);
            }

        }

        #endregion

        #endregion

        #region Import Geolocation and Gategory to Lists

        /// <summary>
        /// Only used to display Geolocation-Area-Country in the location combobox cbolocations.
        /// </summary>
        /// <returns>List of Geolocations with their Areas and Countries</returns>
        public List<Geolocation> GetLocations()
        {
            string stmt = "SELECT geolocation.id AS geolocation, area.name AS area, country.name AS country FROM geolocation JOIN area ON geolocation.area_id = area.id JOIN country ON country.id = area.country_id ORDER BY geolocation.id, area.name";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();

            Geolocation geolocation = null;
            var geolocations = new List<Geolocation>();

            while (reader.Read())
            {
                geolocation = new Geolocation
                {
                    ID = (int)reader["geolocation"],

                    Area = new Area
                    {
                        Name = (string)reader["area"],

                        Country = new Country
                        {
                            Name = (string)reader["country"]
                        }
                    },

                };

                geolocations.Add(geolocation);
            }
            return geolocations;
        }



        /// <summary>
        /// Used for displaying category, basecategory and unit in category combobox cboCategories.
        /// </summary>
        /// <returns>List of Categories with their Basecategories and Units</returns>
        public List<Category> GetCategories()
        {
            string stmt = "SELECT a.id AS categoryid, a.name AS categoryname, a.basecategory_id AS basecategory_id, a.unit_id AS unit_id, b.id AS basecategoryid, b.name AS basecategoryname, unit.id AS unitid, unit.abbreviation FROM category a JOIN category b ON a.basecategory_id = b.id JOIN unit ON unit.id = a.unit_id GROUP BY a.id, a.name, b.id, unit.id, unit.abbreviation ORDER BY b.name, a.name, unit.abbreviation;";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();
            

            Category category = null;
            var categories = new List<Category>();

            while (reader.Read())
            {
                category = new Category
                {
                    Unit = new Unit
                    {
                        ID = (int)reader["unit_id"],
                        Abbreviation = (string)reader["abbreviation"]
                    },

                    BaseCategory = new Category
                    {
                        Name = (string)reader["basecategoryname"],
                        ID = (int)reader["basecategoryid"],

                    },

                        Name = (string)reader["categoryname"],
                        ID = (int)reader["categoryid"],
                        Unit_ID = (int)reader["unit_id"],
                        Basecategory_ID = (int)reader["basecategory_id"]


                };

                categories.Add(category);

            }

            return categories;
        }

        #endregion

    }

}
