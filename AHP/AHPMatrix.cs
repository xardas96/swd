using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SWD_projekt.AHP
{
    public class AHPMatrix
    {
        private double[,] matrix;
        private int matrixDim;
        public int MatrixDim
        {
            get
            {
                return matrixDim;
            }
        }
        private double[] columnSums;
        private double[] rowAverages;
        public double[] RowAverages
        {
            get
            {
                return rowAverages;
            }
        }

        public AHPMatrix(double[,] matrix)
        {
            this.matrix = matrix;
            matrixDim = matrix.GetUpperBound(0) + 1;
            for (int i = 0; i < matrixDim; i++)
                for (int j = 0; j < matrixDim; j++)
                    if (matrix[i,j] == 0)
                        matrix[i,j] = 1 / matrix[j,i];
            columnSums = new double[matrixDim];
            rowAverages = new double[matrixDim];
            Normalize();
        }

        private void Normalize()
        {
            for (int i = 0; i < matrixDim; i++)
                for (int j = 0; j < matrixDim; j++)
                    columnSums[i] += matrix[j, i];
            for (int i = 0; i < matrixDim; i++)
                for (int j = 0; j < matrixDim; j++)
                    matrix[i, j] /= columnSums[j];
            for (int i = 0; i < matrixDim; i++)
            {
                for (int j = 0; j < matrixDim; j++)
                    rowAverages[i] += matrix[i, j];
                rowAverages[i] /= matrixDim;
            }
        }

        public bool CheckConsistency()
        {
            bool output = true;
            if (matrixDim > 2)
            {
                double lambdaMax = 0;
                for (int i = 0; i < matrixDim; i++)
                    lambdaMax += columnSums[i] * rowAverages[i];
                double ci = (lambdaMax - matrixDim) / (matrixDim - 1);
                double ri = AHPHelper.GetRandomConsistencyIndex(matrixDim);
                output = ci / ri <= 0.1;
            }
            return output;
        }

        public override string ToString()
        {
            string mat = "";
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                    mat += matrix[i, j] + " ";
                mat += "\n";
            }
            return mat;
        }
    }
}