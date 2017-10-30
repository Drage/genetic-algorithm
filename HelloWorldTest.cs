using System;

class HelloWorldTest
{
	static string target = "Hello World!";
	static Random random = new Random();

	static char GetRandomLetter()
	{
		return (char)random.Next(32, 122);
	}

	static int LevenshteinDistance(string a, string b)
    {
		if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) 
			return 0;

		int lengthA = a.Length;
		int lengthB = b.Length;
		int[,] distances = new int[lengthA + 1, lengthB + 1];
		for (int i = 0; i <= lengthA; distances[i, 0] = i++);
		for (int j = 0; j <= lengthB; distances[0, j] = j++);

		for (int i = 1; i <= lengthA; i++)
		{
			for (int j = 1; j <= lengthB; j++)
			{
				int cost = b[j - 1] == a[i - 1] ? 0 : 1;
				distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1), distances[i - 1, j - 1] + cost);
			}
		}
		return distances[lengthA, lengthB];
    }

	static float EvaluateFitness(char[] genes)
	{
		return 1 - (float)LevenshteinDistance(new string(genes), target) / (float)target.Length;
	}

	static int Main(string[] args)
	{
		GenericAlgorithm<char> ga = new GenericAlgorithm<char>(300, target.Length, GetRandomLetter, EvaluateFitness);

		do
		{
			ga.Iterate(1.0f, 0.2f);
			Console.WriteLine("{0}: {1}", ga.CurrentGeneration, new string(ga.AlphaGenes));
		}
		while (ga.MaxFitness < 1);

		return 0;
	}
}