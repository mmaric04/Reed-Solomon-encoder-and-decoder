namespace Reed_Solomon_Algorithm
{
	public class MatrixOperations
	{
		public static int[,] GetMinor(int[,] matrix, int row, int col)
		{
			int v = matrix.GetLength(0);

			int[,] minor = new int[v - 1, v - 1];

			int minorRow = 0, minorCol = 0;
			for (int i = 0; i < v; i++)
			{
				if (i == row) continue;
				minorCol = 0;
				for (int j = 0; j < v; j++)
				{
					if (j == col) continue;
					minor[minorRow, minorCol] = matrix[i, j];
					minorCol++;
				}
				minorRow++;
			}

			return minor;
		}
		public static int CalculateCofactor(int[,] matrix, int[] alphas)
		{
			int v = matrix.GetLength(0);
			int determinant = 0;

			if (v == 1)
			{
				determinant = matrix[0, 0];
			}
			else if (v == 2)
			{
				determinant = Modulo2Math.Add2Alphas(Modulo2Math.Multiply2Alphas(matrix[0, 0], matrix[1, 1], alphas),
					 Modulo2Math.Multiply2Alphas(matrix[0, 1], matrix[1, 0], alphas));
			}
			else
			{
				for (int i = 0; i < v; i++)
				{
					for (int j = 0; j < v; j++)
					{
						int[,] minor = GetMinor(matrix, i, j);
						int minorDeterminant = CalculateCofactor(minor, alphas);
						int term = Modulo2Math.Multiply2Alphas(matrix[i, j], minorDeterminant, alphas);
						determinant = Modulo2Math.Add2Alphas(determinant, term);
					}
				}
			}
			return determinant;
		}
		public static int Determinant(int[,] matrix, int[] alphas)
		{
			int v = matrix.GetLength(0);
			int determinant = 0;
			int[,] cofactorMatrix = new int[v, v];

			if (v == 1 || v == 2)
			{
				determinant = CalculateCofactor(matrix, alphas);
			}
			else
			{
				for (int i = 0; i < v; i++)
				{
					for (int j = 0; j < v; j++)
					{

						int[,] minor = GetMinor(matrix, i, j);
						cofactorMatrix[i, j] = CalculateCofactor(minor, alphas);
						int product = Modulo2Math.Multiply2Alphas(cofactorMatrix[i, j], matrix[i, j], alphas);
						determinant = Modulo2Math.Add2Alphas(determinant, product);

					}
				}
			}
			return determinant;
		}
		public static int[,] ModifiedMatrix(int[,] matrix, int[] vector, int i)
		{
			int v = matrix.GetLength(0);
			int[,] modifiedMatrix = new int[v, v];

			for (int row = 0; row < v; row++)
			{
				for (int col = 0; col < v; col++)
				{
					if (col == i)
					{
						modifiedMatrix[row, col] = vector[row];
					}
					else
					{
						modifiedMatrix[row, col] = matrix[row, col];
					}
				}
			}

			return modifiedMatrix;
		}
		public static int InverseAlpha(int determinant, int[] alphas)
		{
			if (determinant == 0)
				throw new Exception("The method: 'matrix system for solving linear equations', cannot be used for solving this particular problem.");
			else
			{
				int exponent = Array.IndexOf(alphas, determinant);
				int inverseAlpha;
				if (exponent == 0)
				{
					return determinant;
				}
				else
					inverseAlpha = alphas[alphas.Length - exponent];

				return inverseAlpha;
			}
		}
	}
}
