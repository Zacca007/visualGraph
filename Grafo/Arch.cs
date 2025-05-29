namespace CsGraph
{
    public class Arch
    {
        public Node Source { get; }
        public Node Target { get; }
        public int Cost { get; set; }
        public bool IsDirected { get; set; }
        public string Label { get; set; }

        public Arch(Node source, Node target, int cost = 1, bool isDirected = false, string label = "")
        {
            Source = source;
            Target = target;
            Cost = cost;
            IsDirected = isDirected;
            Label = label;

            // Connessione bidirezionale automatica
            Source.AddArch(this);
            Target.AddArch(this);
        }

        public Node GetOppositeNode(Node node)
        {
            if (node == Source) return Target;
            if (node == Target) return Source;
            throw new ArgumentException("Il nodo specificato non appartiene a questo arco.");
        }

        public override string ToString()
        {
            string arrow = IsDirected ? " -> " : " -- ";
            return $"{Source.Label}{arrow}{Target.Label} ({Cost})";
        }

        public override bool Equals(object? obj)
        {
            return obj is Arch arch &&
                   EqualityComparer<Node>.Default.Equals(Source, arch.Source) &&
                   EqualityComparer<Node>.Default.Equals(Target, arch.Target) &&
                   Cost == arch.Cost &&
                   IsDirected == arch.IsDirected &&
                   Label == arch.Label;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Target, Cost, IsDirected, Label);
        }
    }
}
