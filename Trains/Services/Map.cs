using System;
using System.Collections.Generic;

namespace Trains.Services
{
    public static class Map
    {

        private static int GetClosest(Situacao[] situacao)
        {
            int i, smallest = 0;
            int smallestIndex = -1;         //the default return value
            for (i = 0; i < 27; i++)
            {
                if (situacao[i] == null)
                {
                    continue;    //unitialized Situacao
                }

                if (situacao[i].Status == false)
                {   //found a candidate
                    //if its the first candidate, or smaller then prev candidate
                    if (smallestIndex == -1 || situacao[i].Distancia < smallest)
                    {
                        smallest = situacao[i].Distancia;
                        smallestIndex = i;
                    }
                }
            }
            return smallestIndex;
        }

        public static int ShortestPath(int start, int end)
        {
            var Situacao = new Situacao[27];
            Situacao[start] = new Situacao { Distancia = 0, Status = false };
            int current = start;

            do
            {
                LinkedList neighborList = getNeighbors(current);    //get neighbors
                ListIterator i = neighborList.listIterator(0);
                while (i.hasNext())                                 //for each neighbor
                {
                    Ponto neighbor = (Ponto)i.next();
                    int DistanciaThroughCurrent = Situacao[current].Distancia + neighbor.Distancia;
                    if (Situacao[neighbor.Valor] == null)              //if we've never seen it before
                    {                                               // add it to Situacao list
                        Situacao[neighbor.Valor] = new Situacao { Distancia = DistanciaThroughCurrent, Status = false };
                    }
                    //if we've found a shorter path
                    else if (Situacao[neighbor.Valor].Distancia > DistanciaThroughCurrent)
                    {
                        Situacao[neighbor.Valor].Distancia = DistanciaThroughCurrent; //change Distancia
                    }
                }  //done going through neighbors

                Situacao[current].Status = true;    //we should never come back to the current node

                if (current == end)              // This only happens on the first iteration since
                {
                    Situacao[current] = null;     // it is the loop exit condition, so if its true then
                }
                // current == start == end.  We want to find this node again
                // so we pretend we haven't seen it yet

                current = GetClosest(Situacao);   //the next node is the closest onea
                if (current == -1)
                {
                    throw new Exception("No such path");  //if no more next nodes
                }
            } while (current != end);

            return Situacao[end].Distancia;
        }

        private static int[] listToArray(LinkedList list)
        {
            int[] array = new int[list.size()];     //create return list
            int i;
            for (i = 0; !list.isEmpty(); i++)
            {
                array[i] = ((int)list.removeFirst()).intValue();
            }

            return array;
        }

        private static int[] CountPaths(int start, int end, int maxDistancia, bool countHops)
        {
            List<Situacao> situacoes = new List<Situacao>();
            LinkedList Distancias = new LinkedList();

            Situacao current = new Situacao { Ponto = start, Distancia = 0 };      //initialize current

            /* note: this loop is organized counter-intuitively.  this is to insure that
             * start is never current when we're checking if we've found a path. otherwise
             * when start==end we would always find a Distancia-0 path and there's never one */
            while (true)
            {
                LinkedList neighborList = getNeighbors(current.Ponto);   //get neighbor list
                ListIterator i = neighborList.listIterator(0);
                while (i.hasNext())
                {
                    Ponto neighbor = (Ponto)i.next();                 //for each neighbor

                    int DistanciaFromStart = current.Distancia;
                    if (countHops)
                    {
                        DistanciaFromStart += 1;
                    }
                    else
                    {
                        DistanciaFromStart += neighbor.Distancia;
                    }

                    if (DistanciaFromStart <= maxDistancia)        //if within range, add to queue
                    {
                        situacoes.Add(new Situacao { Ponto = neighbor.Valor, Distancia = DistanciaFromStart });
                    }
                }

                try
                {
                    current = (Situacao)situacoes.removeFirst();  //try to get next node to explore
                }
                catch { break; }         //break if none

                if (current.Ponto == end)                    //if we found another path to end
                {
                    Distancias.add(current.Distancia);   //take down its Distancia
                }
            }

            return listToArray(Distancias);
        }

        public static int[] AllPathsDistancia(int start, int end, int maxDistancia)
        {
            return CountPaths(start, end, maxDistancia, false);
        }

        public static int[] AllPathsHops(int start, int end, int maxHops)
        {
            return CountPaths(start, end, maxHops, true);
        }

        private static void CheckValid(int v)
        {

            if (v < 0 || v >= 27)
            {
                throw new Exception("Invalid node");
            }

        }


        public static void AddEdge(int v1, int v2, int dist)
        {
            CheckValid(v1); CheckValid(v2);
            if (dist < 0)
            {
                throw new Exception("Distance must be >= 0");
            }

            if (nodeList[v1] == null)
            {
                nodeList[v1] = new LinkedList();
            }

            Ponto edge = new Ponto(v2, dist);
            nodeList[v1].add(edge);         //this does not check whether an edge v1-v2 is already
        }                           //in the list, so we could get two with different wieghts if 
                                    //the input is careless

        public static int Distance(int v1, int v2)
        {
            CheckValid(v1); CheckValid(v2);
            ListIterator i = nodeList[v1].listIterator(0);
            while (i.hasNext())

            {
                Ponto current = (Ponto)i.next();          //get the next edge in the adjacency-list
                if (current.node == v2)
                {
                    return current.distance;
                }
            }
            throw new Exception("No such edge");
        }

        public static LinkedList GetNeighbors(int v)
        {
            CheckValid(v);
            if (nodeList[v] == null)
            {
                nodeList[v] = new LinkedList();
            }

            return nodeList[v];
        }
    }
}
