using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amadeus;
using System;
using System.Collections.Generic;

namespace Amadeus.Tests
{
    [TestClass]
    public class AmadeusTests
    {
        Game game;

        [TestInitialize]
        public void SetUp() {
            game = new Game();
            game.Edges = new List<Edge>();
            game.Planets = new List<Planet>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Init("Test1.txt");

            Assert.IsTrue(true);
        }

        private void Init(string fileName)
        {
            int counter = 0;  
            string line;
            string[] inputs;  
            System.IO.StreamReader file =   
                new System.IO.StreamReader(@"..\..\..\..\Amadeus.Tests\" + fileName);  
            while((line = file.ReadLine()) != null)  
            {
                if(counter == 0)
                {
                    inputs = line.Split(' ');
                    game.PlanetCount = int.Parse(inputs[0]);
                    game.EdgeCount = int.Parse(inputs[1]);
                }  else if (counter < game.EdgeCount + 1) {
                    inputs = line.Split(' ');
                    game.Edges.Add( new Edge { PlanetA = int.Parse(inputs[0]),
                        PlanetB = int.Parse(inputs[1])});
                } else {
                    inputs = line.Split(' ');
                    game.Planets.Add( new Planet {
                        MyUnits = int.Parse(inputs[0]),
                        MyTolerance = int.Parse(inputs[1]),
                        OtherUnits = int.Parse(inputs[2]),
                        OtherTolerance = int.Parse(inputs[3]),
                        CanAssign = int.Parse(inputs[4])
                    });
                }
                counter++;
            }
            file.Close();  
        }
    }
}
