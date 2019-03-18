using System;

namespace Trains.Services
{
    public class TrainService
    {        
        private string readGraphData(string filename)
        {
            BufferedReader dataFile = new BufferedReader(new FileReader(filename));
            String line, data;
            data = "";

            while ((line = dataFile.readLine()) != null)
            {
                data += line;
            }

            return data;
        }

        private double tr(char v)
        {
            return Char.GetNumericValue(v) - 10;
        }

        public void createMap(string filename)
        {
            string data;
            try
            {
                data = readGraphData(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read graph data");
                Console.WriteLine(e.Message);
                return;
            }

            string[] edges = data.Split("[\\s]*,[\\s]*");
            int i;
            for (i = 0; i < edges.Length; i++)
            {
                int weight = int.Parse(edges[i].Substring(2));
                try
                {
                    Map.AddEdge(tr(edges[i][0]), tr(edges[i][1]), weight);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //answers questions 1-5
        public object routeDistance(string route)
        {
            int i, rd = 0;

            try
            {
                for (i = 0; i < (route.Length - 1);)
                {
                    rd += Map.Distance(tr(route[i]), tr(route[++i]));
                }
            }
            catch (Exception e)
            {
                return "No such route";
            }

            return rd;
        }
        
        //answers questions 8,9
        public object shortestPathDist(char start, char end)
        {
            try
            {
                return Map.ShortestPath(tr(start), tr(end));
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //answers question 10
        public object numberOfPaths_Distance(char start, char end, int maxDist)
        {
            try
            {
                return Map.AllPathsDistance(tr(start), tr(end), maxDist).length;
            }
            catch (Exception e) { return e.Message; }
        }

        //answers question 6
        public object numberOfPaths_MaxHops(char start, char end, int maxHops)
        {
            try
            {
                return Map.AllPathsHops(tr(start), tr(end), maxHops).length;
            }
            catch (Exception e) { return e.Message; }
        }

        //answers question 7
        public object numberOfPaths_ExactHops(char start, char end, int hops)
        {
            int[] pathLengths;
            try
            {
                pathLengths = Map.AllPathsHops(tr(start), tr(end), hops);
            }
            catch (Exception e)
            {

                return e.Message;
            }

            int i, paths = 0;               //count paths that are exactly hops long
            for (i = 0; i < pathLengths.Length; i++)
            {
                if (pathLengths[i] == hops)
                {
                    paths += 1;
                }
            }

            return paths;
        }

    }
}
