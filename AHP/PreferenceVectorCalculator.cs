using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWD_projekt.AHP
{
    public class PreferenceVectorCalculator
    {
        private List<AHPMatrix> matrices;
        private int rankingVectorSize;
        private List<double> rankingVector;
        public List<double> RankingVector
        {
            get
            {
                return rankingVector;
            }
        }

        public PreferenceVectorCalculator(List<AHPMatrix> matrices)
        {
            this.matrices = matrices;
            rankingVectorSize = matrices[1].MatrixDim;
            rankingVector = new List<double>();
        }

        public void CalculateRankingVector()
        {
            int cryteriaMatrixDim = matrices[0].MatrixDim;
            double[] cryteriaMatrixRowAverages = matrices[0].RowAverages;
            for (int i = 0; i < rankingVectorSize; i++)
            {
                rankingVector.Add(0);
                for (int j = 0; j < cryteriaMatrixDim; j++)
                    rankingVector[i] += cryteriaMatrixRowAverages[j] * matrices[j + 1].RowAverages[i];
            }
        }

        public int[] GetPreferenceVector()
        {
            int[] preferenceVector = new int[rankingVectorSize];
            List<double> sortedRankingVector = rankingVector.OrderBy(d => -d).ToList();
            for (int i = 0; i < preferenceVector.Length; i++)
                preferenceVector[i] = rankingVector.IndexOf(sortedRankingVector[i]);
            return preferenceVector;
        }
    }
}