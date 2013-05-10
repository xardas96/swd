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
using SWD_projekt.Database;
using SWD_projekt.Core;
using System.Collections.ObjectModel;

namespace SWD_projekt.UI
{
    /// <summary>
    /// Interaction logic for UniverstyChoicePage.xaml
    /// </summary>
    public partial class UniverstyChoicePage : Page
    {
        private List<University> loadedUniversitiesList;
        private List<University> selectedUniversitiesList;
        public List<University> SelectedUniversitiesList
        {
            get
            {
                return selectedUniversitiesList;
            }
        }

        public UniverstyChoicePage()
        {
            InitializeComponent();
            loadedUniversitiesList = DBConnection.GetAllUniversities();
            selectedUniversitiesList = new List<University>();
            loadedList.ItemsSource = loadedUniversitiesList;
            selectedList.ItemsSource = selectedUniversitiesList;
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            University selected = loadedList.SelectedItem as University;
            if (selected != null)
            {
                (selectedList.ItemsSource as List<University>).Add(selected);
                (loadedList.ItemsSource as List<University>).Remove(selected);
                selectedList.Items.Refresh();
                loadedList.Items.Refresh();
            }
        }

        private void unselectButton_Click(object sender, RoutedEventArgs e)
        {
            University unselected = selectedList.SelectedItem as University;
            if (unselected != null)
            {
                (loadedList.ItemsSource as List<University>).Insert(0, unselected);
                (selectedList.ItemsSource as List<University>).Remove(unselected);
                selectedList.Items.Refresh();
                loadedList.Items.Refresh();
            }
        }
    }
}