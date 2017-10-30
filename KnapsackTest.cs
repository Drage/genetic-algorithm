using System;
using System.Collections.Generic;
using System.Linq;

class KnapsackTest
{
	struct Item
	{
		public int weight;
		public int value;

		public Item(int weight, int value)
		{
			this.weight = weight;
			this.value = value;
		}
	}

	static Item[] items = new Item[25]
		{	
			new Item(45, 86),
			new Item(64, 79),
			new Item(50, 17),
			new Item(85, 36),
			new Item(10, 41),
			new Item(16, 94),
			new Item(44, 53),
			new Item(92, 55),
			new Item(43, 16),
			new Item(99, 35),
			new Item(98, 30),
			new Item(71, 18),
			new Item(40, 49),
			new Item(79, 26),
			new Item(26, 53),
			new Item(17, 75),
			new Item(18, 91),
			new Item(84, 77),
			new Item(68, 30),
			new Item(40, 96),
			new Item(95, 22),
			new Item(52, 97),
			new Item(90, 60),
			new Item(14, 74),
			new Item(96, 41)
		};

	static int maxWeight = 500;

	static Random random = new Random();

	static bool GetRandomNumber()
	{
		return random.Next(100) > 50;
	}

	static float EvaluateFitness(bool[] genes)
	{
		int sumWeight = 0;
		int sumValue = 0;

		for (int i = 0; i < genes.Length; i++)
		{
			if (genes[i])
			{
				sumWeight += items[i].weight;
				sumValue += items[i].value;
			}
		}
		
		return sumWeight > maxWeight ? 0 : sumValue;
	}

	static int Main(string[] args)
	{
		GenericAlgorithm<bool> ga = new GenericAlgorithm<bool>(100, items.Length, GetRandomNumber, EvaluateFitness);

		for (int i = 0; i < 100; i++)
		{
			ga.Iterate();
			Console.WriteLine("{0}: {1}", ga.CurrentGeneration, ga.MaxFitness);
		}

		Console.WriteLine();
		Console.WriteLine("Weight: " + items.Zip(ga.AlphaGenes, (i, g) => g ? i.weight : 0).Sum());
		Console.WriteLine("Value: " + items.Zip(ga.AlphaGenes, (i, g) => g ? i.value : 0).Sum());

		return 0;
	}
}