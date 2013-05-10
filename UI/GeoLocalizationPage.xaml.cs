using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SWD_projekt.Core;
using SWD_projekt.GeoLocalization;
using SWD_projekt.Database;
using System.ComponentModel;

namespace SWD_projekt.UI
{
    /// <summary>
    /// Interaction logic for GeoLocalizationPage.xaml
    /// </summary>
    public partial class GeoLocalizationPage : Page
    {
        private readonly BackgroundWorker coordinatesDownloader = new BackgroundWorker();
        private List<University> selectedUniversities;
        private MainWindow mw;

        public GeoLocalizationPage(MainWindow mw)
        {
            InitializeComponent();
            InitializeWorkers();
            this.mw = mw;
        }

        public void Init(List<University> selectedUniversities)
        {
            this.selectedUniversities = selectedUniversities;
        }

        public void SetDistanceFromAddressInUniversities()
        {
            mw.DisableAll();
            coordinatesDownloader.RunWorkerAsync(addressBox.Text);
        }

        public bool GetReverseDistance()
        {
            return largeDistanceBox.IsChecked.Value;
        }

        private void coordinatesDownloader_DoWork(object sender, DoWorkEventArgs e)
        {
            double[] addressLoc = GeoLocalizator.GetGeolocalization(e.Argument as string);
            foreach (University u in selectedUniversities)
            {
                string uniLocStr = DBConnection.GetCryteriaValueForUniversity(u.ID, Cryteria.CITY_DISTANCE_CRYTERIA);
                string[] split = uniLocStr.Split(';');
                double[] uniLoc = new double[] {
                    Convert.ToDouble(split[0]), Convert.ToDouble(split[1]) 
                };
                double distance = Math.Sqrt(Math.Pow(addressLoc[0] - uniLoc[0], 2) + Math.Pow(addressLoc[1] - uniLoc[1], 2));
                u.Distance = distance;
            }  
        }

        private void coordinatesDownloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mw.EnableAll();
        }

        private void InitializeWorkers()
        {
            coordinatesDownloader.DoWork += new DoWorkEventHandler(coordinatesDownloader_DoWork);
            coordinatesDownloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(coordinatesDownloader_RunWorkerCompleted);
        }
    }
}