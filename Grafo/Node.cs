using System.Drawing;

namespace CsGraph
{
    public class Node
    {
        public Point Position { get; set; }
        public int Potential { get; set; }
        public string Label { get; set; }
        public List<Arch> Arches { get; } = new();

        public Node(Point position, int potential = 0, string label = "")
        {
            Position = position;
            Potential = potential;
            Label = label;
        }

        public void AddArch(Arch arch)
        {
            if (!Arches.Contains(arch))
            {
                Arches.Add(arch);
            }
        }

        public bool RemoveArch(Arch arch) => Arches.Remove(arch);

        public IEnumerable<Node> GetAdjacentNodes()
        {
            foreach (var arch in Arches)
            {
                yield return arch.GetOppositeNode(this);
            }
        }

        public override string ToString() => Label;
    }
}
