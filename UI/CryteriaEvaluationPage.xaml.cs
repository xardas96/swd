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
using SWD_projekt.AHP;

namespace SWD_projekt.UI
{
    /// <summary>
    /// Interaction logic for CryteriaEvaluationPage.xaml
    /// </summary>
    public partial class CryteriaEvaluationPage : Page
    {
        private List<Cryteria> selectedCryteria;

        public CryteriaEvaluationPage()
        {
            InitializeComponent();
        }

        public void Init(List<Cryteria> selectedCryteria)
        {
            this.selectedCryteria = selectedCryteria;
            stackPanel1.Children.Clear();
            for (int i = 0; i < selectedCryteria.Count; i++)
                for (int j = i + 1; j < selectedCryteria.Count; j++)
                    if(selectedCryteria[i].ID != Cryteria.CITY_DISTANCE_CRYTERIA && selectedCryteria[i].ID != Cryteria.FREE_UNIVERSITY_CRYTERIA
                        && selectedCryteria[j].ID != Cryteria.CITY_DISTANCE_CRYTERIA && selectedCryteria[j].ID != Cryteria.FREE_UNIVERSITY_CRYTERIA)
                        stackPanel1.Children.Add(new CryteriaEvaluationControl(selectedCryteria[i], selectedCryteria[j]));
        }

        public AHPMatrix GetCryteriaEvaluationMatrix()
        {
            double[,] cryteriaEvals = new double[selectedCryteria.Count, selectedCryteria.Count];
            for (int i = 0; i <= cryteriaEvals.GetUpperBound(0); i++)
                for (int j = 0; j <= cryteriaEvals.GetUpperBound(1); j++)
                    if (i == j)
                        cryteriaEvals[i, j] = 1;
            foreach (CryteriaEvaluationControl c in stackPanel1.Children)
            {
                Tuple<int, int, int> t = c.GetCryteriaPairEvaluation();
                cryteriaEvals[t.Item1 - 1, t.Item2 - 1] = t.Item3;
            }
            return new AHPMatrix(cryteriaEvals);
        }
    }
}