# genetic-algorithm
Solves optimisation problems using a genetic algorithm

## Usage
### 1. Create an instance of the GeneticAlgorithm class
- The GeneticAlgorithm may be initialised so that each genome comprises a list of values or a permutation of a given set of items
- The type of the values or items to be ordered is defined by the generic type parameter `T`
- The fitness function should return a higher value for genomes that provide a more optimal solution

#### Values
```
GenericAlgorithm<T> ga = new GenericAlgorithm<T>(populationSize, genomeSize, randomFunc, fitnessFunc);
```
Parameter | Type | Description
--- | --- | ---
populationSize | `int` | The number of genomes
genomeSize | `int` | The number of genes in each genome
randomFunc | `Func<T>` | A function to intialise the genes of each genome
fitnessFunc | `Func<T[], float>` | A function to evaluate the fitness of a genome

#### Permutation
```
GenericAlgorithm<T> ga = new GenericAlgorithm<T>(populationSize, valueSet, fitnessFunc);
```
Parameter | Type | Description
--- | --- | ---
populationSize | `int` | The number of genomes
valueSet | `T[]` | The list of items the genome is a permutation of
fitnessFunc | `Func<T[], float>` | A function to evaluate the fitness of a genome

### 2. Iterate
Generate a new generation of the population until a condition is met e.g. a given number of iterations or a minimum fitness
```
for (int i = 0; i < 1000; i++)
{
    ga.Iterate(crossover, mutation, elitism);
    Console.WriteLine("{0}: {1}", ga.CurrentGeneration, ga.MaxFitness);
}
```
Parameter | Type | Description
--- | --- | ---
crossover | `float` | The percentage of genomes that will be recombined into child genomes
mutation | `float` | The percentage chance of a gene in each genome being randomly modified
elitism | `float` | The percentage of the genomes with highest fitness to be preserved for the next iteration without modification

### 3. Results
Get the 'alpha' genome (highest fitness) from the GeneticAlgorithm
```
Console.WriteLine("Fitness: " + ga.MaxFitness);
Console.WriteLine("Genes:");
ga.AlphaGenes.ToList().ForEach(i => Console.WriteLine(i));
```
