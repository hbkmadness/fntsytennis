using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    public class DrawsTree
    {
        public MatchNode root;

        public DrawsTree(int rounds, List<Tuple<TennisDB.TennisPlayer,TennisDB.TennisPlayer>> list)
        {
            root = new MatchNode(null,rounds);
            Queue<MatchNode> constructQueue = new Queue<MatchNode>();
            constructQueue.Enqueue(root);

            for (int i = 1; i < rounds; i++)
            {
                Queue<MatchNode> newConstructQueue = new Queue<MatchNode>();
                while (constructQueue.Count != 0)
                {
                    MatchNode current = constructQueue.Dequeue();
                    current.leftChild = new MatchNode(current, current.round - 1);
                    current.rightChild = new MatchNode(current, current.round - 1);

                    newConstructQueue.Enqueue(current.leftChild);
                    newConstructQueue.Enqueue(current.rightChild);
                }

                constructQueue = newConstructQueue;
            }

            List<MatchNode> firstRoundNodes = this.getNodesAtRound(1);
            int index = 0;
            firstRoundNodes.ForEach((match) =>
            {
                Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer> pair = list.ElementAt(index);
                match.p1 = pair.Item1;
                match.p2 = pair.Item2;
                index++;
            });
        }

        public List<MatchNode> getNodesAtRound(int round)
        {
            List<MatchNode> returnValue = new List<MatchNode>();

            Queue<MatchNode> bfs = new Queue<MatchNode>();
            bfs.Enqueue(root);
            while (bfs.Count != 0)
            {
                MatchNode current = bfs.Dequeue();
                if (current.round == round)
                {
                    returnValue.Add(current);
                }
                else
                {
                    if (current.leftChild != null)
                    {
                        bfs.Enqueue(current.leftChild);
                    }
                    if (current.rightChild != null)
                    {
                        bfs.Enqueue(current.rightChild);
                    }
                }
            }

            return returnValue;
        }
    }
}
