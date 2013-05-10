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
using SWD_projekt.AHP;
using SWD_projekt.Core;

namespace SWD_projekt.UI
{
    /// <summary>
    /// Interaction logic for CryteriaEvaluationControl.xaml
    /// </summary>
    public partial class CryteriaEvaluationControl : UserControl
    {
        private Cryteria c1;
        private Cryteria c2;

        public CryteriaEvaluationControl(Cryteria c1, Cryteria c2)
        {
            InitializeComponent();
            this.c1 = c1;
            this.c2 = c2;
            cryteria1Label.Content = c1;
            cryteria2Label.Content = c2;
            ahpValuesBox.ItemsSource = AHPHelper.GetAHPMatrixValues().Values;
        }

        public Tuple<int, int, int> GetCryteriaPairEvaluation()
        {
            var evaluation = AHPHelper.GetAHPMatrixValues().SingleOrDefault(ev => ev.Value.Equals(ahpValuesBox.SelectedItem as string));
            return new Tuple<int, int, int>(c1.ID, c2.ID, evaluation.Key);
        }
    }
}