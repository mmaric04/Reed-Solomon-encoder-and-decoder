namespace Reed_Solomon_Algorithm
{
	public class Decoder
	{
		public static List<int> InsertError(List<int> codewordPolynomial, int errorCount)
		{
			Random random = new();

			// generates unique random error positions
			HashSet<int> errorPositionsHash = new();

			while (errorPositionsHash.Count < errorCount)
			{
				errorPositionsHash.Add(random.Next(codewordPolynomial.Count));
			}

			List<int> errorPositions = new(errorPositionsHash);

			// inserts random values into positions
			foreach (int position in errorPositions)
			{
				var randomValue = random.Next(codewordPolynomial.Count + 1);
				while (randomValue == codewordPolynomial[position])
				{
					randomValue = random.Next(codewordPolynomial.Count + 1);
				};
				codewordPolynomial[position] = randomValue;
			}

			return codewordPolynomial;

			//codewordPolynomial[11] = 8;
			//codewordPolynomial[7] = 10;
			//codewordPolynomial[2] = 2;

			//return codewordPolynomial;
		}
		public static int[] SyndromeSequence(List<int> corruptedCodewordPolynomial, int t, int[] alphas)
		{
			int n = alphas.Length;
			int[] syndromeSequence = new int[2 * t];

			for (int i = 1; i <= 2 * t; i++)
			{
				int syndrome = 0;

				for (int j = 0; j < corruptedCodewordPolynomial.Count; j++)
				{
					if (corruptedCodewordPolynomial[j] != 0)
					{
						int alphaExponent = (i * (corruptedCodewordPolynomial.Count - 1 - j)) % n;
						int alphaValue = alphas[alphaExponent];

						int product = Modulo2Math.Multiply2Alphas(corruptedCodewordPolynomial[j], alphaValue, alphas);
						syndrome = Modulo2Math.Add2Alphas(syndrome, product);
					}
				}

				syndromeSequence[i - 1] = syndrome;
			}
			return syndromeSequence;

		}
		public static int[] SigmaValues(int[] syndromeSequence, int v, int[] alphas)
		{
			// create the v by v matrix
			int[,] matrixA = new int[v, v];

			for (int i = 0; i < v; i++)
			{
				for (int j = 0; j < v; j++)
				{
					if (i == 0)
						matrixA[i, j] = syndromeSequence[j];
					else
						matrixA[i, j] = syndromeSequence[i + j];
				}
			}

			// create the v by 1 result matrix
			int[] matrixB = new int[v];

			for (int i = 0; i < v; i++)
				matrixB[i] = syndromeSequence[v + i];

			// find sigma vlues
			int[,] cofactorMatrix = new int[v, v];
			int[,] inverseMatrix = new int[v, v];

			if (v == 1)
				cofactorMatrix = matrixA;
			else if (v == 2)
			{
				cofactorMatrix[0, 0] = matrixA[1, 1];
				cofactorMatrix[0, 1] = matrixA[0, 1];
				cofactorMatrix[1, 0] = matrixA[1, 0];
				cofactorMatrix[1, 1] = matrixA[0, 0];
			}
			else
			{
				for (int i = 0; i < v; i++)
				{
					for (int j = 0; j < v; j++)
					{
						int[,] minor = MatrixOperations.GetMinor(matrixA, i, j);
						cofactorMatrix[i, j] = MatrixOperations.CalculateCofactor(minor, alphas);
					}
				}
			}

			int determinant = MatrixOperations.Determinant(matrixA, alphas);
			int inverseDeterminant = MatrixOperations.InverseAlpha(determinant, alphas);

			int[] sigma = new int[v + 1];

			if (v == 1)
			{
				sigma[0] = Modulo2Math.Multiply2Alphas(inverseDeterminant, matrixB[0], alphas);
				sigma[1] = 1;
			}
			else
			{
				for (int i = 0; i < v; i++)
				{
					for (int j = 0; j < v; j++)
						inverseMatrix[i, j] = Modulo2Math.Multiply2Alphas(inverseDeterminant, cofactorMatrix[i, j], alphas);
				}
				sigma = Modulo2Math.Multiply2Matrices(inverseMatrix, matrixB, alphas);
			}
			return sigma;
		}
		public static int[] ErrorLocations(int[] sigma, int[] alphas)
		{
			List<int> errorLocations = new();
			int n = alphas.Length;

			for (int exponent = 0; exponent < n; exponent++)
			{
				int alphaValue = alphas[exponent];
				int result = PolynomialOperations.EvaluatePolynomial(sigma, alphaValue, alphas);

				if (result == 0 && exponent == 0)
					errorLocations.Add(exponent);
				else if (result == 0)
					errorLocations.Add(n - exponent);
			}

			return errorLocations.ToArray();
		}
		public static int[] ErrorValues(int[] errorLocations, int[] syndromeSequence, int[] alphas)
		{
			int v = errorLocations.Length;

			// create the v by v matrix
			int[,] matrixA = new int[v, v];

			for (int i = 0; i < v; i++)
			{
				for (int j = 0; j < v; j++)
				{
					matrixA[i, j] = alphas[(errorLocations[v - j - 1] * (i + 1)) % alphas.Length];
				}
			}

			// create the v by 1 vector
			int[] vectorB = new int[v];

			for (int i = 0; i < v; i++)
			{
				vectorB[i] = syndromeSequence[i];
			}

			int[] errorValues = new int[v];
			int determinantA = MatrixOperations.Determinant(matrixA, alphas);
			int inverseDeterminantA = MatrixOperations.InverseAlpha(determinantA, alphas);

			for (int i = 0; i < v; i++)
			{
				int[,] modifiedMatrix = MatrixOperations.ModifiedMatrix(matrixA, vectorB, i);
				int determinantM = MatrixOperations.Determinant(modifiedMatrix, alphas);
				errorValues[i] = Modulo2Math.Multiply2Alphas(determinantM, inverseDeterminantA, alphas);
			}

			return errorValues;
		}
		public static int[] ErrorPolynomial(int[] errorLocations, int[] errorValues, int n)
		{
			int[] errorPolynomial = new int[n];

			for (int i = 0; i < errorLocations.Length; i++)
			{
				int location = errorLocations[i];
				errorPolynomial[n - location - 1] = errorValues[errorLocations.Length - 1 - i];
			}
			return errorPolynomial;
		}
	}
}
