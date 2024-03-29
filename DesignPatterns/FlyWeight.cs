﻿using Newtonsoft.Json;

namespace DesignPatterns.FlyWeight
{
    public class FlyWeight
    {
        private Car _sharedState;

        public FlyWeight(Car car)
        {
            _sharedState = car;
        }

        public void Operation(Car uniqueState)
        {
            string s = JsonConvert.SerializeObject(_sharedState);
            string u = JsonConvert.SerializeObject(uniqueState);
            Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state");
        }
    }

    public class Car
    { 
        public string Owner { get; set; }
        public string Number { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }

    public class FlyWeightFactory
    {
        private List<Tuple<FlyWeight, string>> flyWeights = new List<Tuple<FlyWeight, string>>();  

        public FlyWeightFactory(params Car[] args)
        {
            foreach (Car car in args)
                flyWeights.Add(new Tuple<FlyWeight, string>(new FlyWeight(car), GetKey(car)));
        }

        public string GetKey(Car key)
        {
            List<string> list = new List<string>();

            list.Add(key.Model);
            list.Add(key.Color);
            list.Add(key.Company);

            if (key.Owner != null && key.Number != null)
            {
                list.Add(key.Owner);
                list.Add(key.Number);
            }

            list.Sort();

            return string.Join("_", list);
        }

        public FlyWeight GetFlyWeight(Car sharedState)
        {
            string key = GetKey(sharedState);

            if (flyWeights.Where(t => t.Item2 == key).Count() == 0)
            {
                Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                flyWeights.Add(new Tuple<FlyWeight, string>(new FlyWeight(sharedState), key));
            }
            else
            {
                Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }

            return flyWeights.Where(t => t.Item2 == key).FirstOrDefault().Item1;
        }

        public void listFlyweights()
        {
            var count = flyWeights.Count;
            Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
            foreach (var flyweight in flyWeights)
            {
                Console.WriteLine(flyweight.Item2);
            }
        }
    }

    public class Client
    {
        public void Main()
        {
            var factory = new FlyWeightFactory(
                new Car { Company = "Chevrolet", Model = "Camaro2018", Color = "pink" },
                new Car { Company = "Mercedes Benz", Model = "C300", Color = "black" },
                new Car { Company = "Mercedes Benz", Model = "C500", Color = "red" },
                new Car { Company = "BMW", Model = "M5", Color = "red" },
                new Car { Company = "BMW", Model = "X6", Color = "white" }
            );
            factory.listFlyweights();

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "M5",
                Color = "red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",
                Color = "red"
            });

            factory.listFlyweights();
        }

        public static void addCarToPoliceDatabase(FlyWeightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");

            var flyweight = factory.GetFlyWeight(new Car
            {
                Color = car.Color,
                Model = car.Model,
                Company = car.Company
            });

            flyweight.Operation(car);
        }
    }
}


namespace DesignPatterns.FlyWeightV2
{
    public abstract class Unit
    { 
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public Image Picture { get; protected set; }
    }

    public class Image
    { 
        public static Image Load(string str) => new Image();
    }

    class Client
    {
        public void Main()
        {
            var list = ParserHTML();
        }
        public List<Unit> ParserHTML()
        {
            var units = new List<Unit>();
            for (int i = 0; i < 150; i++)
                units.Add(new Dragon());
            for (int i = 0; i < 500; i++)
                units.Add(new Goblin());

            Console.WriteLine("Dragons and Goblins has been parsed from HTML page");
            return units;
        }
    }

    class UnitImagesFactory
    {
        public static Dictionary<Type, Image> Images = new Dictionary<Type, Image>();
        public static Image CreateDragonImage()
        {
            if (!Images.ContainsKey(typeof(Dragon)))
                Images.Add(typeof(Dragon), Image.Load("Dragon.jpg"));
            return Images[typeof(Dragon)];
        }

        public static Image CreateGoblinImage()
        {
            if (!Images.ContainsKey(typeof(Goblin)))
                Images.Add(typeof(Goblin), Image.Load("Goblin.jpg"));
            return Images[typeof(Goblin)];
        }
    }

    class Dragon : Unit
    {
        public Dragon()
        {
            Name = "Dragon";
            Health = 8;
            // Picture = Image.Load("Dragon.jpg");
            Picture = UnitImagesFactory.CreateDragonImage();
        }
    }

    class Goblin : Unit
    {
        public Goblin()
        {
            Name = "Goblin";
            Health = 8;
            // Picture = Image.Load("Goblin.jpg");
            Picture = UnitImagesFactory.CreateGoblinImage();
        }
    }
}