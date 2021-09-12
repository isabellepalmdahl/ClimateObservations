using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KlimatobservationerGrupp11.Repositories;
using KlimatobservationerGrupp11.Models;
using Microsoft.VisualBasic;
    
namespace KlimatobservationerGrupp11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KlimatobservationerRepository db;
        Observer observer;
        ObservationViewModel observationViewModel;
        Category category;
        Measurement measurement;
        List<Measurement> measurements = new List<Measurement>();
        
      

        public MainWindow()
        {
            InitializeComponent();
            observer = new Observer();
            db = new KlimatobservationerRepository();
            category = new Category();
            measurement = new Measurement();
            observationViewModel = new ObservationViewModel();
            cboLocation.ItemsSource = db.GetLocations();
            cboCategory.ItemsSource = db.GetCategories();
            lstObservers.ItemsSource = db.GetObservers();


        }

        #region Read, Update and Delete Observers

        /// <summary>
        /// gets all observers 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnObserverGetList_Click(object sender, RoutedEventArgs e)
        {
            var observer = db.GetObserver(1);
            var observers = db.GetObservers();

            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = observers;

        }

        /// <summary>
        /// adds observer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddObserver_Click(object sender, RoutedEventArgs e)
        {
            var observers = db.GetObservers();
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;

            observer = new Observer
            {

                FirstName = firstName,
                LastName = lastName
            };

            observer = db.AddObserver(observer);

            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = db.GetObservers();

        }
        /// <summary>
        /// updates observer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateObserver_Click(object sender, RoutedEventArgs e)
        {
            observer = (Observer)lstObservers.SelectedItem;
            string FirstName = txtFirstName.Text;
            string LastName = txtLastName.Text;

            int selectedIndex = lstObservers.SelectedIndex;
            Observer selectedItem = (Observer)lstObservers.SelectedItem;

            selectedItem.FirstName = FirstName;
            selectedItem.LastName = LastName;

            db.UpdateObserver(observer);

            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = db.GetObservers();

        }

        /// <summary>
        /// deletes observer if no registered observations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteObserver_Click(object sender, RoutedEventArgs e) 
        {

            observer = (Observer)lstObservers.SelectedItem;
            List<ObservationViewModel> observations = db.GetObservationViewModel();

            if (db.ObserverHasObservations(observer, observations) == false)
            {

                string message = $"Are you sure that you want to delete {lstObservers.SelectedItem}?";
                string title = "Delete observer";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show(message, title, buttons);

                if (result == MessageBoxResult.Yes)
                {
                    db.DeleteObserver(observer.ID);
                    MessageBox.Show("The observer has been deleted.");
                }
                else
                {
                    MessageBox.Show("The observer has not been deleted.");

                }

            }
            else
            {
                MessageBox.Show("This observer has made observations and therefore cannot be deleted.",
                    "Critical Warning",
                    MessageBoxButton.OKCancel);

            }

            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = db.GetObservers();

        }

        #endregion

        #region Read, Add and Edit Observations

        /// <summary>
        /// gets list of all observations made by selected observer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstObservers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Observer observer = (Observer)lstObservers.SelectedItem;
            var observation = db.GetObservation(1);
            var observations = db.GetObserverObservations(observer);

            txtFirstName.Text = observer.FirstName;
            txtLastName.Text = observer.LastName;

            EnableAddObservation();
            EnableGetObservations();

            lstObservations.ItemsSource = observations;
        }


        /// <summary>
        /// gets list of all observations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetObservations_Click(object sender, RoutedEventArgs e)
        {
            Observer observer = (Observer)lstObservers.SelectedItem;
            var observation = db.GetObservation(1);
            var observations = db.GetObserverObservations(observer);

            lstObservations.ItemsSource = observations;
        }

        /// <summary>
        /// selects observation and populates fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstObservations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<ObservationViewModel> observations = db.GetObservationViewModel();
            ObservationViewModel observationViewModel = (ObservationViewModel)lstObservations.SelectedItem;

            dtePicker.SelectedDate = observationViewModel.Date;

            // THIS IS EMERGENCY CODING AT ITS WORST BUT RAN OUT OF TIME TO FIND BETTER SOLUTION. THE BIRD FLIES.
            int index = observationViewModel.Geolocation.ID;

            if (index <= 40)
            {
                cboLocation.SelectedIndex = index - 1;
            }
            else
            {
                cboLocation.SelectedIndex = index - 2;
            }

            lstMeasurements.ItemsSource = null;
            lstMeasurements.ItemsSource = observationViewModel.Measurements;

        }

        /// <summary>
        /// adds measurement to list of measurements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMeasurement_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(txtAddValue.Text);
            Category category = (Category)cboCategory.SelectedItem;
            
            AddMeasurement(value, category);
            
            lstMeasurements.ItemsSource = null;
            lstMeasurements.ItemsSource = measurements;

            txtAddValue.Clear();
            cboCategory.SelectedItem = null;

        }

        /// <summary>
        /// method for adding measurement
        /// </summary>
        /// <param name="value"></param>
        /// <param name="category"></param>
        public void AddMeasurement(int value, Category category)
        {

            measurement = new Measurement
            {
                Value = value,
                Category_ID = category.ID,
                Category = category
               
            };

            measurements.Add(measurement);

        }
        
        /// <summary>
        /// gets selected measurement into combobox and textbox, enables edit observation and update list buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMeasurements_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {          
            EnableEditObservation();
            EnableUpdateList();

            measurement = (Measurement)lstMeasurements.SelectedItem;

            cboCategory.SelectedItem = measurement; // cannot get Category to show in the combobox, will look at it further.
            txtAddValue.Text = measurement.Value.ToString();
        }

        /// <summary>
        /// updates the selected measurement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            
            int value = int.Parse(txtAddValue.Text);
            Category category = (Category)cboCategory.SelectedItem;

            Measurement selectedItem = (Measurement)lstMeasurements.SelectedItem;

            selectedItem.Category_ID = category.ID;
            selectedItem.Value = value;

            
            txtAddValue.Clear();
            cboCategory.SelectedItem = null;

            //this update does not show in the list but is committed to database so when program is ran again it displays correctly with updated value.
            
        }

        /// <summary>
        /// adds new observation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddObservation_Click(object sender, RoutedEventArgs e)
        {
            var observations = db.GetObservationViewModel();

            Observer observer = (Observer)lstObservers.SelectedItem;
            Geolocation geolocation = (Geolocation)cboLocation.SelectedItem;
            DateTime date;

            if (dtePicker.SelectedDate == null)
            {
                date = DateTime.Now;

            }
            else
            {
                date = (DateTime)dtePicker.SelectedDate;
            }

            observationViewModel = new ObservationViewModel
            {
                Observer = observer,
                Geolocation = geolocation,
                Date = date,
                Measurements = measurements,  
               
            };

            db.AddObservation(observationViewModel);

            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = db.GetObserverObservations(observer);

            dtePicker.SelectedDate = null;
            cboLocation.SelectedItem = null;
            lstMeasurements.ItemsSource = null;

        }

        /// <summary>
        /// edits observation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditObservation_Click(object sender, RoutedEventArgs e)
        {
            var observations = db.GetObservationViewModel();

            ObservationViewModel observationViewModel = (ObservationViewModel)lstObservations.SelectedItem;
            Geolocation geolocation = (Geolocation)cboLocation.SelectedItem;
            //geolocation = observationViewModel.Geolocation;
            DateTime date;
            Measurement measurement = (Measurement)lstMeasurements.SelectedItem;

            if (dtePicker.SelectedDate == null)
            {
                date = DateTime.Now;
            }
            else
            {
                date = (DateTime)dtePicker.SelectedDate;
            }

            db.EditObservation(observationViewModel, measurement, geolocation);
            lstObservers.SelectedItem = observer;

            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = db.GetObserverObservations(observer);
            MessageBox.Show("The observation has been edited and saved.");
            btnEditObservation.IsEnabled = false;
            
        }

        #endregion

        #region Help methods for enabling buttons in UI

        /// <summary>
        /// enables add observation button
        /// </summary>
        private void EnableAddObservation()
        {
            if (lstObservers.SelectedItem == null)
            {
                btnAddObservation.IsEnabled = false;
            }
            else
            {
                btnAddObservation.IsEnabled = true;
                lblSelectObserverInfo.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// enables edit observation button
        /// </summary>
        private void EnableEditObservation()
        {
            if (lstObservations.SelectedItem == null)
            {
                btnEditObservation.IsEnabled = false;
            }
            else
            {
                btnEditObservation.IsEnabled = true;
                lblSelectObservationInfo.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// enables get observation button
        /// </summary>
        private void EnableGetObservations()
        {
            if (lstObservers.SelectedItem == null)
            {
                btnGetObservations.IsEnabled = false;
            }
            else
            {
                btnGetObservations.IsEnabled = true;
                lblSelectObserverInfo.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// enables update list button
        /// </summary>
        private void EnableUpdateList()
        {
            if (lstMeasurements.SelectedItem == null)
            {
                btnUpdateList.IsEnabled = false;
            }
            else
            {
                btnUpdateList.IsEnabled = true;
                
            }
        }

        #endregion


    }
}
