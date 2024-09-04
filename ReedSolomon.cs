namespace Reed_Solomon_Algorithm
{
    class ReedSolomon
    {
        public static void Main()
        {
            bool isValid = false;
            int m = 0, n = 0, k = 0, t = 0;

            while (!isValid)
            {
                Console.Write("What is the number of bits that will make out your symbols (3 to 8): ");
                if (int.TryParse(Console.ReadLine(), out m) && m >= 3 && m <= 8)
                {
                    n = (int)Math.Pow(2, m) - 1;

                    Console.Write($"How many symbols do you want your R-S code to correct? (0 to {n / 2}): ");

                    if (int.TryParse(Console.ReadLine(), out t) && t <= n / 2 && t >= 0)
                    {
                        k = n - 2 * t;
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine($"Please choose the number of error-correcting symbols to be between (0 and {n / 2})!\n");
                        isValid = false;
                    }

                }
                else
                {
                    Console.WriteLine("Please enter a number between 3 and 8!");
                    isValid = false;
                }
            }

            Console.WriteLine($"\nYou want a {m} bit R-S({n}, {k}) code.");

            int primaryPolynomial = Encoder.GetPrimitivePolynomial(m); // from a table of some already previously known primitive polynomials

            int[] alphas = Encoder.GenerateAlphas(m, primaryPolynomial);

            // maps
            Dictionary<int, char> alphaToCharMap = Encoder.MapAlphasToChars(alphas);
            Dictionary<char, int> charToAlphaMap = Encoder.MapCharsToAlphas(alphaToCharMap);

            //DisplayHelper.DisplayAlphas(alphas, m);
            //Console.WriteLine("\n");
            //DisplayHelper.DisplayAlphaToCharMap(alphaToCharMap, n);

            int[] generatorPolynomial = Encoder.GeneratorPolynomial(t, alphas);
            //DisplayHelper.DisplayGeneratorPolynomial(generatorPolynomial, alphaToCharMap, alphas);

            string? userInput;
            isValid = false;

            Console.Write("\nList of allowed symbols for the message: \n");
            DisplayHelper.DisplayAlphaToCharMap(alphaToCharMap, n);

            while (!isValid)
            {
                Console.Write("\nEnter a message that you want to encode: ");
                userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine($"Message cannot be empty.");
                    isValid = false;
                }
                else if (userInput.Length != k)
                {
                    Console.WriteLine($"Please type a message that is {k} symbols long!");
                    isValid = false;
                }
                else
                {
                    int[]? messagePolynomial = Encoder.TranslateUserInput(userInput, charToAlphaMap, alphas);
                    isValid = messagePolynomial != null;

                    if (isValid)
                    {
                        int[] redundancyPolynomial = new int[generatorPolynomial.Length - 1];

                        if (n != k)
                            redundancyPolynomial = Encoder.GetRedundancyPolynomial(messagePolynomial!, generatorPolynomial, alphas); 
                        

                        // combine the message and redundancy polynomials to form the final codeword polynomial
                        List<int> codewordPolynomial = new List<int>(messagePolynomial!);
                        codewordPolynomial.AddRange(redundancyPolynomial.Reverse());

                        // display the codeword- end of encoder
                        Console.Write("\nEncoded codeword at the sender device: ");
                        DisplayHelper.DisplayCodeword(codewordPolynomial.ToArray(), alphaToCharMap, alphas);

                        // generates a random error between 0 and t + 1 (included), so it can also show when the decoder wont be able to decode a codeword
                        Random random = new();
                        int errorCount = random.Next(0, t + 2);
                        //int errorCount = 3;

                        List<int> corruptedCodewordPolynomial = new List<int>();
                        corruptedCodewordPolynomial = Decoder.InsertError(codewordPolynomial, errorCount);

                        Console.Write("\nEncoded codeword at the receiver device: ");
                        DisplayHelper.DisplayCodeword(corruptedCodewordPolynomial.ToArray(), alphaToCharMap, alphas);
                        Console.WriteLine($"\nNumber of errors that occured is {errorCount}.");

                        int[] syndromeSequence = new int[2*t];
                        syndromeSequence = Decoder.SyndromeSequence(corruptedCodewordPolynomial, t, alphas);
                        //DisplayHelper.DisplaySyndromeSequence(syndromeSequence, alphas);

                        bool hasError = false;
                        foreach(int syndromeSymbol in syndromeSequence)
                        {
                            if (syndromeSymbol != 0)
                            {
                                hasError = true;
                                break;
                            }

                        }

                        if (hasError || n == k)
                        {
                            // finding error locations
                            int[] sigma, errorLocations, errorValues, errorPolynomial, decodedCodewordPolynomial;

                            if (errorCount > t)
                                Console.WriteLine("\nThis R-S code isn't capable of correcting this many errors!");                     
                            else if(n == k)
                                Console.WriteLine("\nThis R-S code isn't capable of correcting any number of errors!");
                            else
                            {
                                try
                                {
                                    sigma = Decoder.SigmaValues(syndromeSequence, errorCount, alphas);
                                    errorLocations = Decoder.ErrorLocations(sigma, alphas);
                                    //DisplayHelper.DisplayErrorLocations(errorLocations);
                                    errorValues = Decoder.ErrorValues(errorLocations, syndromeSequence, alphas);
                                    errorPolynomial = Decoder.ErrorPolynomial(errorLocations, errorValues, n);
                                    decodedCodewordPolynomial = Modulo2Math.Add2Polynomials(errorPolynomial, corruptedCodewordPolynomial.ToArray());
                                    Console.Write("\nDecoded codeword: ");
                                    DisplayHelper.DisplayDecodedCodeword(decodedCodewordPolynomial, alphaToCharMap, k, alphas);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                               
                            } 
                        }
                        else
                        {
                            Console.WriteLine("\nNot a single error occured during transmission!");
                            Console.Write("\nCodeword is: ");
                            DisplayHelper.DisplayDecodedCodeword(corruptedCodewordPolynomial.ToArray(), alphaToCharMap, k, alphas);
                        }

                    }
                }
            }
        }
    }
}


