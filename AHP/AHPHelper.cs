using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SWD_projekt.AHP
{
    public static class AHPHelper
    {
        private static Dictionary<int, double> randomConsistencyIndices = new Dictionary<int, double>();
        private static Dictionary<int, string> ahpMatrixValues = new Dictionary<int, string>();

        public static void Init()
        {
            randomConsistencyIndices.Add(3, 0.58);
            randomConsistencyIndices.Add(4, 0.9);
            randomConsistencyIndices.Add(5, 1.12);
            randomConsistencyIndices.Add(6, 1.24);
            randomConsistencyIndices.Add(7, 1.32);
            randomConsistencyIndices.Add(8, 1.41);
            randomConsistencyIndices.Add(9, 1.45);
            randomConsistencyIndices.Add(10, 1.49);

            ahpMatrixValues.Add(1, (string)Application.Current.FindResource("AHPone"));
            ahpMatrixValues.Add(3, (string)Application.Current.FindResource("AHPthree"));
            ahpMatrixValues.Add(5, (string)Application.Current.FindResource("AHPfive"));
            ahpMatrixValues.Add(7, (string)Application.Current.FindResource("AHPseven"));
            ahpMatrixValues.Add(9, (string)Application.Current.FindResource("AHPnine"));
        }

        public static double GetRandomConsistencyIndex(int MatrixDim)
        {
            return randomConsistencyIndices[MatrixDim];
        }

        public static Dictionary<int, string> GetAHPMatrixValues()
        {
            return ahpMatrixValues;
        }
    }
}