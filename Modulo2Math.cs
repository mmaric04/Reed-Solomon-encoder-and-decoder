namespace Reed_Solomon_Algorithm
{
	public class Modulo2Math
	{
		public static int Add2Alphas(int a, int b)
		{
			return (a ^ b);
		}
		public static int Multiply2Alphas(int a, int b, int[] alphas)
		{
			if (a == 0 || b == 0 || a == -1 || b == -1)
			{
				return 0;
			}

			int indexA = Array.IndexOf(alphas, a);
			int indexB = Array.IndexOf(alphas, b);
			int resultIndex = (indexA + indexB) % alphas.Length;

			return alphas[resultIndex];
		}
		public static int[] Add2Polynomials(int[] poly1, int[] poly2)
		{
			int n = poly1.Length;
			int[] result = new int[n];

			for (int i = 0; i < n; i++)
				result[i] = Add2Alphas(poly2[i], poly1[i]);

			return result;
		}
		public static int[] Multiply2Polynomials(int[] poly1, int[] poly2, int[] alphas)
		{
			int[] result = new int[poly1.Length + poly2.Length - 1];

			for (int i = 0; i < poly1.Length; i++)
			{
				for (int j = 0; j < poly2.Length; j++)
				{
					// if two terms with same exponents exist, add them together
					result[i + j] ^= Multiply2Alphas(poly1[i], poly2[j], alphas);

				}
			}

			return result;
		}
		public static int[] Multiply2Matrices(int[,] matrixA, int[] matrixB, int[] alphas)
		{
			int rowsA = matrixA.GetLength(0);
			int colsA = matrixA.GetLength(1);

			int[] result = new int[rowsA + 1];

			for (int i = 0; i < rowsA; i++)
			{
				result[i] = 0;
				for (int j = 0; j < colsA; j++)
				{
					int mul = Multiply2Alphas(matrixA[i, j], matrixB[j], alphas);
					result[i] = Add2Alphas(result[i], mul);
				}
			}

			result[rowsA] = 1; // the last sigma value will always be 1 
			return result;
		}
	}
}
