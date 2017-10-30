using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GenericAlgorithm<T>
{
	public class Genome: IComparable<Genome>
	{
		private T[] genes;

		private enum Encoding { Permutation, Values };
		private Encoding encoding;

		private Func<T[], float> fitnessFunction;
		private Func<T> randomFunction;

		private static Random random = new Random();

		public float Fitness
		{
			get { return fitnessFunction(genes); }
		}

		public T[] Genes
		{
			get { return genes; }
		}

		private Genome() { }

		public Genome(T[] valueSet, Func<T[], float> fitnessFunc)
		{
			encoding = Encoding.Permutation;
			fitnessFunction = fitnessFunc;
			genes = valueSet.OrderBy(x => random.Next()).ToArray();  
		}

		public Genome(int size, Func<T> randomFunc, Func<T[], float> fitnessFunc)
		{
			encoding = Encoding.Values;
			fitnessFunction = fitnessFunc;
			randomFunction = randomFunc;

			genes = new T[size];
			for (int i = 0; i < size; i++)
				genes[i] = randomFunction();
		}

		public Genome(Genome parent1, Genome parent2)
		{
			encoding = parent1.encoding;
			fitnessFunction = parent1.fitnessFunction;
			randomFunction = parent1.randomFunction;

			int size = parent1.genes.Length;
			int crossover = random.Next(size);
			genes = new T[size];

			switch (encoding)
			{
				case Encoding.Values:
					for (int i = 0; i < size; i++)
					{
						if (i < crossover)
							genes[i] = parent1.genes[i];
						else
							genes[i] = parent2.genes[i];
					}
					break;
				
				case Encoding.Permutation:
					for (int i = 0; i < size; i++)
					{
						if (i < crossover)
							genes[i] = parent1.genes[i];
						else
						{
							for (int j = 0; j < size; j++)
							{
								if (!genes.Contains(parent2.genes[j]))
								{
									genes[i] = parent2.genes[j];
									break;
								}
							}
						}
					}
					break;
			}
		}

		public void Mutate()
		{
			switch (encoding)
			{
				case Encoding.Values:
					genes[random.Next(genes.Length)] = randomFunction();
					break;

				case Encoding.Permutation:
					int a = random.Next(genes.Length);
					int b = random.Next(genes.Length);
					T temp = genes[a];
					genes[a] = genes[b];
					genes[b] = temp;
					break;
			}
		}

		public int CompareTo(Genome other)
		{
			return this.Fitness.CompareTo(other.Fitness);
		}

		public Genome Clone()
		{
			Genome newGenome = new Genome();
			newGenome.genes = (T[])genes.Clone();
			newGenome.encoding = encoding;
			newGenome.fitnessFunction = fitnessFunction;
			newGenome.randomFunction = randomFunction;
			return newGenome;
		}
	}

	private Genome[] population;
	private int generation = 0;
	private Random random = new Random();

	public float MaxFitness
	{
		get	{ return population[0].Fitness; }
	}

	public T[] AlphaGenes
	{
		get	{ return population[0].Genes; }
	}

	public int CurrentGeneration
	{
		get { return generation; }
	}

	private float totalFitness;
	private int totalFitnessCalcGen = -1;
	public float TotalFitness
	{
		get
		{
			if (generation != totalFitnessCalcGen)
				totalFitness = population.Sum(x => x.Fitness);
			return totalFitness;
		}
	}

	public GenericAlgorithm(int populationSize, int genomeSize, Func<T> randomFunc, Func<T[], float> fitnessFunc)
	{
		population = new Genome[populationSize];
		for (int i = 0; i < populationSize; i++)
			population[i] = new Genome(genomeSize, randomFunc, fitnessFunc);
	}

	public GenericAlgorithm(int populationSize, T[] valueSet, Func<T[], float> fitnessFunc)
	{
		population = new Genome[populationSize];
		for (int i = 0; i < populationSize; i++)
			population[i] = new Genome(valueSet, fitnessFunc);
	}

	public void Iterate(float crossover = 1.0f, float mutation = 0.05f, float elitism = 0.01f)
	{
		Genome[] newPopulation = new Genome[population.Length];
		Parallel.For(0, population.Length, i => newPopulation[i] = IterateGenome(i, crossover, mutation, elitism));
		population = newPopulation.OrderByDescending(x => x).ToArray();
		generation++;
	}

	private Genome IterateGenome(int i, float crossover, float mutation, float elitism)
	{
		if (i < elitism * population.Length)
			return population[i].Clone();
		else
		{
			Genome newGenome;

			if (random.NextDouble() > crossover)
				newGenome = RouletteSelect().Clone();
			else
				newGenome = new Genome(RouletteSelect(), RouletteSelect());

			if (random.NextDouble() > mutation)
				newGenome.Mutate();

			return newGenome;
		}
	}

	private Genome RouletteSelect()
	{
		float target = (float)(random.NextDouble() * TotalFitness);
		float sum = 0.0f;
		for (int i = 0; i < population.Length; i++)
		{
			sum += population[i].Fitness;
			if (sum >= target)
				return population[i];
		}
		return population.Last();
	}	
}
