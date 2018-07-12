using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Amadeus {
    public class Game {
        public int PlanetCount { get; set; }
        public int EdgeCount { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Planet> Planets { get; set; }

        internal void Loop()
        {
            WriteDebug();
            var myPlanet = this.Planets.Where(p => !p.MyUnits.Equals(0)).ToList();
            var otherPlanet = this.Planets.Where(p => p.MyUnits.Equals(0)).ToList();
            for (int i = 0; i < 5; i++)
            {
                var plan = otherPlanet.Find(o => o.CanAssign.Equals(1));                    
                Console.WriteLine(plan?.ID ?? 0);
                myPlanet.Add(plan);
                otherPlanet.Remove(plan);
            }
            Console.WriteLine("NONE");
        }

        private void WriteDebug()
        {
            Console.Error.WriteLine(PlanetCount + " " + EdgeCount);
            Console.Error.WriteLine("Edges");
            foreach(var edge in Edges)
                Console.Error.WriteLine(edge.PlanetA + " " + edge.PlanetB);
            Console.Error.WriteLine("Planetes");
            foreach (var planet in Planets)
                Console.Error.WriteLine(planet.MyUnits + " " + planet.MyTolerance +
                    " " + planet.OtherUnits + " " + planet.OtherTolerance + " " + 
                    planet.CanAssign + " " + planet.ID);
        }
    }
    public class Edge {
        public int PlanetA { get; set; }
        public int PlanetB { get; set; }
    }
    public class Planet {
        public int ID { get; set; }
        public int MyUnits { get; set; }
        public int MyTolerance { get; set; }
        public int OtherUnits { get; set; }
        public int OtherTolerance { get; set; }
        public int CanAssign { get; set; }
    }
    class Player {
        static void Main(string[] args)
        {
            Game game = new Game();
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            game.PlanetCount = int.Parse(inputs[0]);
            game.EdgeCount = int.Parse(inputs[1]);
            game.Edges = new List<Edge>();
            for (int i = 0; i < game.EdgeCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int planetA = int.Parse(inputs[0]);
                int planetB = int.Parse(inputs[1]);
                game.Edges.Add( new Edge {PlanetA = planetA, PlanetB = planetB});
            }

            // game loop
            while (true)
            {
                game.Planets = new List<Planet>();
                for (int i = 0; i < game.PlanetCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    game.Planets.Add( new Planet {
                        MyUnits = int.Parse(inputs[0]),
                        MyTolerance = int.Parse(inputs[1]),
                        OtherUnits = int.Parse(inputs[2]),
                        OtherTolerance = int.Parse(inputs[3]),
                        CanAssign = int.Parse(inputs[4]),
                        ID = i
                    });
                }
                game.Planets = game.Planets.OrderBy(p => p.MyUnits).ToList();
                game.Loop();
            }
        }
    }
}