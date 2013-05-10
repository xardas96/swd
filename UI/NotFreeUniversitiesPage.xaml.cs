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

namespace SWD_projekt.UI
{
    /// <summary>
    /// Interaction logic for NotFreeUniversitiesPage.xaml
    /// </summary>
    public partial class NotFreeUniversitiesPage : Page
    {
        private List<University> selectedUniversities;
        private List<University> wrongUniversities;

        public NotFreeUniversitiesPage()
        {
            InitializeComponent();
        }

        public void Init(List<University> selectedUniversities, List<University> wrongUniversities)
        {
            this.selectedUniversities = selectedUniversities;
            this.wrongUniversities = wrongUniversities;
            foreach(University u in wrongUniversities)
                label1.Content += u.ToString() + System.Environment.NewLine;
        }

        public bool GetNoWrong()
        {
            return noWrongBox.IsChecked.Value;
        }
    }
}
