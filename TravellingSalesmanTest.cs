using System;
using System.Collections.Generic;
using System.Linq;

// Generation: 1000, Fitness: 0.8388569
// Canterbury, London, Bristol, Cardiff, Exeter, Falmouth, Swansea, Birmingham, Liverpool, Manchester, Leeds, Hull, Newcastle, Carlisle, Edinburgh, Glasgow

class TravellingSalesmanTest
{
	struct City
	{
		public string name;
		public double latitude;
		public double longitude;

		public City(string name, double latitude, double longitude)
		{
			this.name = name;
			this.latitude = latitude;
			this.longitude = longitude;
		}
	}

	static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
	{
		double r = 6371; // radius of the earth in km
		double dLat = DegToRad(latitude2 - latitude1);
		double dLon = DegToRad(longitude2 - longitude1);
		double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(DegToRad(latitude1)) * Math.Cos(DegToRad(latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return r * c;
	}

	static double DegToRad(double deg)
	{
		return deg * (System.Math.PI / 180);
	}

	static float EvaluateFitness(City[] cities)
	{
		double distance = 0;
		for (int i = 1; i < cities.Length; i++)
			distance += GetDistance(cities[i - 1].latitude, cities[i - 1].longitude, cities[i].latitude, cities[i].longitude);
		return 1 - (float)distance / 10000.0f;
	}

	static int Main(string[] args)
	{
		List<City> cities = new List<City>
            {
                new City("Birmingham", 52.486125, -1.890507),
                new City("Bristol", 51.460852, -2.588139),
                new City("London", 51.512161, -0.116215),
                new City("Leeds", 53.803895, -1.549931),
                new City("Manchester", 53.478239, -2.258549),
                new City("Liverpool", 53.409532, -3.000126),
                new City("Hull", 53.751959, -0.335941),
                new City("Newcastle", 54.980766, -1.615849),
                new City("Carlisle", 54.892406, -2.923222),
                new City("Edinburgh", 55.958426, -3.186893),
                new City("Glasgow", 55.862982, -4.263554),
                new City("Cardiff", 51.488224, -3.186893),
                new City("Swansea", 51.624837, -3.94495),
                new City("Exeter", 50.726024, -3.543949),
                new City("Falmouth", 50.152266, -5.065556),
                new City("Canterbury", 51.289406, 1.075802)
            };

		GenericAlgorithm<City> ga = new GenericAlgorithm<City>(200, cities.ToArray(), EvaluateFitness);

		for (int i = 0; i < 1000; i++)
		{
			ga.Iterate();
			Console.WriteLine("{0}: {1}", ga.CurrentGeneration, ga.MaxFitness);
		}

		Console.WriteLine("Fitness: " + ga.MaxFitness);
		Console.WriteLine("Order:");
		ga.AlphaGenes.ToList().ForEach(i => Console.WriteLine(i.name));

		return 0;
	}
}