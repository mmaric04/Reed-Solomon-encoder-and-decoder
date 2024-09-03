namespace Reed_Solomon_Algorithm
{
	public class Encoder
	{
		// encoder- methods that don't depend on the message polynomial
		public static int GetPrimitivePolynomial(int m) => m switch
		{
			3 => 0b1011,  // X^3 + X + 1  
			4 => 0b10011,
			5 => 0b100101,
			6 => 0b1000011,
			7 => 0b10001001,
			8 => 0b100011101,
			_ => throw new ArgumentException($"Invalid input"),
		};
		public static int[] GenerateAlphas(int m, int primitivePolynomial)
		{
			int[] alphas = new int[(1 << m) - 1]; // allocates space for n ints
			alphas[0] = 1;

			for (int i = 1; i < alphas.Length; i++)
			{
				alphas[i] = alphas[i - 1] << 1; // one bit shift to the left of the previous alpha value
				if ((alphas[i] & (1 << m)) != 0) // if alpha element exceeds the m bit size
				{
					alphas[i] = Modulo2Math.Add2Alphas(alphas[i], primitivePolynomial);
				}
			}

			return alphas;
		}
		public static Dictionary<int, char> MapAlphasToChars(int[] alphas)
		{
			Dictionary<int, char> map = new Dictionary<int, char>();
			char currentChar = 'A';

			for (int i = 0; i < alphas.Length; i++)
			{
				map[i] = currentChar;
				currentChar++;
			}
			map[alphas.Length] = '0';

			return map;
		}
		public static Dictionary<char, int> MapCharsToAlphas(Dictionary<int, char> alphaToCharMap)
		{
			Dictionary<char, int> map = new Dictionary<char, int>();

			foreach (var entry in alphaToCharMap)
			{
				map[entry.Value] = entry.Key;
			}

			return map;
		}
		public static int[] GeneratorPolynomial(int t, int[] alphas)
		{
			// highest order term of a generator polynomial will always have a coefficient of value 1 
			int[] generatorPolynomial = [1];

			for (int i = 1; i <= 2 * t; i++)
			{
				int[] term = [alphas[i], 1];
				generatorPolynomial = Modulo2Math.Multiply2Polynomials(generatorPolynomial, term, alphas);
			}

			return generatorPolynomial;
		}


		// encoder- methods that depend on message polynomial
		public static int[]? TranslateUserInput(string userInput, Dictionary<char, int> charToAlphaMap, int[] alphas)
		{
			List<int> alphaValues = new();
			bool isUserInputValid = true;

			foreach (char ch in userInput)
			{
				if (charToAlphaMap.ContainsKey(ch))
				{
					if (ch == '0')
						alphaValues.Add(0);
					else
						alphaValues.Add(alphas[charToAlphaMap[ch]]);
				}
				else
				{
					isUserInputValid = false;
					Console.WriteLine($"Warning: Character '{ch}' is not valid and will be ignored.");
				}
			}

			return isUserInputValid ? alphaValues.ToArray() : null;
		}
		public static int[] GetRedundancyPolynomial(int[] messagePolynomial, int[] generatorPolynomial, int[] alphas)
		{
			int redundancyLength = generatorPolynomial.Length - 1;
			int messageLength = messagePolynomial.Length;

			int[] redundancyPolynomial = new int[redundancyLength];

			for (int i = 0; i < messageLength; i++)
			{
				int feedback = messagePolynomial[i] ^ redundancyPolynomial[redundancyLength - 1];

				for (int j = redundancyLength - 1; j >= 0; j--)
				{
					if (j == 0)
						redundancyPolynomial[j] = Modulo2Math.Multiply2Alphas(feedback, generatorPolynomial[j], alphas);
					else
						redundancyPolynomial[j] = redundancyPolynomial[j - 1] ^ Modulo2Math.Multiply2Alphas(feedback, generatorPolynomial[j], alphas);
				}

			}
			return redundancyPolynomial;
		}
	}
}
