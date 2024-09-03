namespace Reed_Solomon_Algorithm
{
	public class PolynomialOperations
	{
		public static int Power(int[] sigma, int alphaValue, int exponent, int[] alphas)
		{
			if (exponent == sigma.Length - 1)
				return 1;
			else
				return alphas[(Array.IndexOf(alphas, alphaValue) * (sigma.Length - 1 - exponent)) % alphas.Length];
		}
		public static int EvaluatePolynomial(int[] sigma, int alphaValue, int[] alphas)
		{
			int result = 0;
			for (int i = sigma.Length - 1; i >= 0; i--)
			{
				int term = Modulo2Math.Multiply2Alphas(sigma[i], Power(sigma, alphaValue, i, alphas), alphas);
				result = Modulo2Math.Add2Alphas(result, term);
			}
			return result;
		}
	}
}
