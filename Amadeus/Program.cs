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
            //WriteDebug();
            var myPlanet = this.Planets.Where(p => !p.MyUnits.Equals(0)).ToList();
            var otherPlanet = this.Planets.Where(p => p.MyUnits.Equals(0)).ToList();
            var ret = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                otherPlanet = otherPlanet.OrderBy( o => o.MyUnits).ToList();
                // conquerir les planetes non conquisent
                var plan = otherPlanet.Find(o => o.CanAssign.Equals(1)
                    && o.MyUnits.Equals(0) 
                    && o.OtherUnits.Equals(0)
                );
                // recuperer des planetes deja conquises
                if(plan == null)
                {
                    var edges = Edges.Where(e=> myPlanet.Any(p => p.ID == e.PlanetA)).ToList();
                    otherPlanet = otherPlanet.OrderBy( o => edges.Where(e => e.PlanetB == o.ID).ToList().Count()).ToList();
                    plan = otherPlanet.Find(o => o.CanAssign.Equals(1)
                        && o.MyTolerance >= o.OtherTolerance
                        && o.MyUnits < o.OtherUnits);
                }

                // charger mes planetes faible
                if(plan == null)
                {
                    myPlanet = myPlanet.OrderBy(p => p.MyUnits).ThenBy(p => p.MyTolerance).ToList();
                    plan = myPlanet.FirstOrDefault();
                }

                ret.Add(plan.ID);
                plan.MyUnits++;
                if(plan.MyUnits > plan.OtherUnits)
                {
                    myPlanet.Add(plan);
                    otherPlanet.Remove(plan);
                }
            }

            ret.ForEach(
                i => System.Console.WriteLine(i)
            );
            Console.WriteLine("NONE");
        }

        internal int nbMyUnitOfPlanetAndAround(Planet planete)
        {
            var edges = Edges.Where(e => e.PlanetA == planete.ID).ToList();
            var nb = planete.MyUnits;
            foreach (var item in edges)
            {
                nb += Planets.Find(p => p.ID == item.PlanetB).MyUnits;
            }
            return nb;
        }
        internal int nbOtherUnitOfPlanetAndAround(Planet planete)
        {
            var edges = Edges.Where(e => e.PlanetA == planete.ID).ToList();
            var nb = planete.OtherUnits;
            foreach (var item in edges)
            {
                nb += Planets.Find(p => p.ID == item.PlanetB).OtherUnits;
            }
            return nb;
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