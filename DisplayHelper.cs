namespace Reed_Solomon_Algorithm
{
	public class DisplayHelper
	{
		public static void DisplayAlphas(int[] alphas, int m)
		{
			string zeroes = new('0', m);
			Console.WriteLine($"0 => {zeroes}");

			for (int i = 0; i < alphas.Length; i++)
			{
				Console.WriteLine($"alpha^{i} => {Convert.ToString(alphas[i], 2).PadLeft(m, '0')}");
			}
		}
		public static void DisplayAlphaToCharMap(Dictionary<int, char> map, int n)
		{
			Console.WriteLine("0 => 0");

			foreach (var entry in map)
			{
				if (entry.Key == n) { }
				else
					Console.WriteLine($"alpha^{entry.Key} => {entry.Value}");
			}
		}
		public static void DisplayGeneratorPolynomial(int[] generatorPolynomial, Dictionary<int, char> alphaToCharMap, int[] alphas)
		{
			Console.WriteLine("\nGenerator Polynomial with alphas:");

			for (int i = generatorPolynomial.Length - 1; i >= 0; i--)
			{
				if (generatorPolynomial[i] != 0)
				{
					int alphaIndex = Array.IndexOf(alphas, generatorPolynomial[i]);

					Console.Write($"α^{alphaIndex}");

					if (i != 0)
						Console.Write($"X^{i} + ");

				}
			}

			Console.WriteLine("\n");
			Console.WriteLine("Generator Polynomial with symbols:");

			for (int i = generatorPolynomial.Length - 1; i >= 0; i--)
			{
				if (generatorPolynomial[i] != 0)
				{
					int alphaIndex = Array.IndexOf(alphas, generatorPolynomial[i]);
					char alphaChar = alphaToCharMap[alphaIndex];
					Console.Write($"{alphaChar}");
					if (i != 0)
					{
						Console.Write($"X^{i} + ");
					}
				}
			}
			Console.WriteLine("\n");
		}
		public static void DisplayUserInputToAlpha(int[] messagePolynomialValues)
		{
			Console.WriteLine("User input translated to alpha exponents:");
			foreach (int value in messagePolynomialValues)
			{
				if (value == 0)
					Console.WriteLine($"0");
				else
					Console.WriteLine($"alpha^{value}");
			}
		}
		public static void DisplayCodeword(int[] codewordValues, Dictionary<int, char> alphaToCharMap, int[] alphas)
		{
			foreach (int value in codewordValues)
			{
				if (value == 0) { }
				else
					Console.Write(alphaToCharMap[Array.IndexOf(alphas, value)] + " ");
			}
			Console.WriteLine();
		}
		public static void DisplaySyndromeSequence(int[] syndromeSequence, int[] alphas)
		{
			Console.WriteLine("\nSyndrome Sequence:");
			for (int i = 0; i <= syndromeSequence.Length - 1; i++)
			{
				int alphaIndex = Array.IndexOf(alphas, syndromeSequence[i]);
				if (alphaIndex == -1)
					Console.Write($"S{i + 1} => 0\t");
				else
					Console.Write($"S{i + 1} => alpha^{alphaIndex}\t");
			}
			Console.WriteLine("\n");
		}
		public static void DisplayErrorLocations(int[] errorLocations)
		{
			Console.WriteLine("\nError locations: ");

			for (int i = errorLocations.Length - 1; i >= 0; i--)
				Console.Write($"beta{errorLocations.Length - i} => {errorLocations[i]}\t");
			Console.WriteLine("");
		}
		public static void DisplayDecodedCodeword(int[] codewordValues, Dictionary<int, char> alphaToCharMap, int k, int[] alphas)
		{
			for (int i = 0; i < k; i++)
			{
				if (codewordValues[i] == 0) { }
				else
					Console.Write(alphaToCharMap[Array.IndexOf(alphas, codewordValues[i])] + " ");
			}
			Console.WriteLine();
		}
	}
}
