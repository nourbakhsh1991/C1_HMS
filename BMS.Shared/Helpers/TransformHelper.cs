using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Helpers
{
    public static class TransformHelper
    {
        public static Matrix<float> GetScaleMatrix(float xScale, float yScale)
        {
            float[,] x = {{ xScale, 0 , 0 },
                          { 0, yScale, 0 } ,
                          { 0, 0, 1} };
            return Matrix<float>.Build.DenseOfArray(x);
        }

        public static Matrix<float> GetRotationMatrix(float rotate)
        {
            float[,] x = {{ MathF.Cos(rotate), -MathF.Sin(rotate) , 0 },
                          { MathF.Sin(rotate), MathF.Cos(rotate), 0 } ,
                          { 0, 0, 1} };
            return Matrix<float>.Build.DenseOfArray(x);
        }

        public static Matrix<float> GetTranslateMatrix(float xValue, float yValue)
        {
            float[,] x = {{ 1, 0 , xValue },
                          { 0, 1, yValue } ,
                          { 0, 0, 1} };
            return Matrix<float>.Build.DenseOfArray(x);
        }
    }
}
