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
    /// Interaction logic for CryeriaChoicePage.xaml
    /// </summary>
    public partial class CryteriaChoicePage : Page
    {
        private List<Cryteria> loadedCryteriaList;
        private List<Cryteria> selectedCryteriaList;
        public List<Cryteria> SelectedCryteriaList
        {
            get
            {
                return selectedCryteriaList;
            }
        }

        public CryteriaChoicePage()
        {
            InitializeComponent();
            loadedCryteriaList = DBConnection.GetAllCryteria();
            selectedCryteriaList = new List<Cryteria>();
            loadedList.ItemsSource = loadedCryteriaList;
            selectedList.ItemsSource = selectedCryteriaList;
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            Cryteria selected = loadedList.SelectedItem as Cryteria;
            if (selected != null)
            {
                (selectedList.ItemsSource as List<Cryteria>).Add(selected);
                (loadedList.ItemsSource as List<Cryteria>).Remove(selected);
                selectedList.Items.Refresh();
                loadedList.Items.Refresh();
            }
        }

        private void unselectButton_Click(object sender, RoutedEventArgs e)
        {
            Cryteria unselected = selectedList.SelectedItem as Cryteria;
            if (unselected != null)
            {
                (loadedList.ItemsSource as List<Cryteria>).Insert(0, unselected);
                (selectedList.ItemsSource as List<Cryteria>).Remove(unselected);
                selectedList.Items.Refresh();
                loadedList.Items.Refresh();
            }
        }
    }
}