using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstras_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph Cities = new Graph();

            Node NewYork = new Node("New York");
            Node Miami = new Node("Miami");
            Node Chicago = new Node("Chicago");
            Node Dallas = new Node("Dallas");
            Node Denver = new Node("Denver");
            Node SanFrancisco = new Node("San Francisco");
            Node LA = new Node("Los Angeles");
            Node SanDiego = new Node("San Diego");

            Cities.Add(NewYork);
            Cities.Add(Miami);
            Cities.Add(Chicago);
            Cities.Add(Dallas);
            Cities.Add(Denver);
            Cities.Add(SanFrancisco);
            Cities.Add(LA);
            Cities.Add(SanDiego);

            NewYork.AddNeighbour(Chicago, 75);
            NewYork.AddNeighbour(Miami, 90);
            NewYork.AddNeighbour(Dallas, 125);
            NewYork.AddNeighbour(Denver, 100);

            Miami.AddNeighbour(Dallas, 50);

            Dallas.AddNeighbour(SanDiego, 90);
            Dallas.AddNeighbour(LA, 80);

            SanDiego.AddNeighbour(LA, 45);

            Chicago.AddNeighbour(SanFrancisco, 25);
            Chicago.AddNeighbour(Denver, 20);

            SanFrancisco.AddNeighbour(LA, 45);

            Denver.AddNeighbour(SanFrancisco, 75);
            Denver.AddNeighbour(LA, 100);

            DistanceCalculator c = new DistanceCalculator(Cities);
            c.Calculate(NewYork, LA);
        }


    }

    class DistanceCalculator
    {
        Dictionary<Node, int> Distances;
        Dictionary<Node, Node> Routes;
        Graph graph;
        List<Node> AllNodes;

        public DistanceCalculator(Graph g)
        {
            this.graph = g;
            this.AllNodes = g.GetNodes();
            Distances = SetDistances();
            Routes = SetRoutes();
        }

        public void Calculate(Node Source, Node Destination)
        {
            Distances[Source] = 0;

            while (AllNodes.ToList().Count != 0)
            {
                Node LeastExpensiveNode = getLeastExpensiveNode();
                ExamineConnections(LeastExpensiveNode);
                AllNodes.Remove(LeastExpensiveNode);
            }
            Print(Source, Destination);
        }

        private void Print(Node Source, Node Destination)
        {
            Console.WriteLine(string.Format("The least possible cost for flying from {0} to {1} is: {2} $", Source.getName(), Destination.getName(), Distances[Destination]));
            PrintLeg(Destination);
            Console.ReadKey();
        }

        private void PrintLeg(Node d)
        {
            if (Routes[d] == null)
                return;
            Console.WriteLine(string.Format("{0} <-- {1}", d.getName(), Routes[d].getName()));
            PrintLeg(Routes[d]);
        }

        private void ExamineConnections(Node n)
        {
            foreach (var neighbor in n.getNeighbors())
            {
                if (Distances[n] + neighbor.Value < Distances[neighbor.Key])
                {
                    Distances[neighbor.Key] = neighbor.Value + Distances[n];
                    Routes[neighbor.Key] = n;
                }
            }
        }

        private Node getLeastExpensiveNode()
        {
            Node LeastExpensive = AllNodes.FirstOrDefault();

            foreach (var n in AllNodes)
            {
                if (Distances[n] < Distances[LeastExpensive])
                    LeastExpensive = n;
            }

            return LeastExpensive;
        }

        private Dictionary<Node, int> SetDistances()
        {
            Dictionary<Node, int> Distances = new Dictionary<Node, int>();

            foreach (Node n in graph.GetNodes())
            {
                Distances.Add(n, int.MaxValue);
            }
            return Distances;
        }

        private Dictionary<Node, Node> SetRoutes()
        {
            Dictionary<Node, Node> Routes = new Dictionary<Node, Node>();

            foreach (Node n in graph.GetNodes())
            {
                Routes.Add(n, null);
            }
            return Routes;
        }
    }

    class Node
    {
        private string Name;
        private Dictionary<Node, int> Neighbors;

        public Node(string NodeName)
        {
            this.Name = NodeName;
            Neighbors = new Dictionary<Node, int>();
        }

        public void AddNeighbour(Node n, int cost)
        {
            Neighbors.Add(n, cost);
        }

        public string getName()
        {
            return Name;
        }

        public Dictionary<Node, int> getNeighbors()
        {
            return Neighbors;
        }
    }

    class Graph
    {
        private List<Node> Nodes;

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public void Add(Node n)
        {
            Nodes.Add(n);
        }

        public void Remove(Node n)
        {
            Nodes.Remove(n);
        }

        public List<Node> GetNodes()
        {
            return Nodes.ToList();
        }

        public int getCount()
        {
            return Nodes.Count;
        }
    }
}
