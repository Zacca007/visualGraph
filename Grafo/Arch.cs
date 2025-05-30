namespace CsGraph
{
    public class Arch
    {
        public Node Source { get; }
        public Node Target { get; }
        public int Cost { get; set; }
        public bool IsDirected { get; set; }
        public string Label { get; set; }

        /// <summary>
        /// Costruttore dell'arco che collega due nodi con un costo, 
        /// una direzionalità e un'etichetta opzionali.
        /// Aggiunge automaticamente l'arco alla lista degli archi dei due nodi.
        /// </summary>
        public Arch(Node source, Node target, int cost = 1, bool isDirected = false, string label = "")
        {
            Source = source;
            Target = target;
            Cost = cost;
            IsDirected = isDirected;
            Label = label;

            // Connessione bidirezionale automatica (aggiunge questo arco a entrambi i nodi)
            Source.AddArch(this);
            Target.AddArch(this);
        }

        /// <summary>
        /// Restituisce il nodo opposto a quello passato come parametro, 
        /// se appartiene all'arco. Solleva eccezione altrimenti.
        /// </summary>
        public Node GetOppositeNode(Node node)
        {
            if (node == Source) return Target;
            if (node == Target) return Source;
            throw new ArgumentException("Il nodo specificato non appartiene a questo arco.");
        }

        /// <summary>
        /// Rappresentazione testuale dell'arco, con indicazione della direzione e del costo.
        /// </summary>
        public override string ToString()
        {
            string arrow = IsDirected ? " -> " : " -- ";
            return $"{Source.Label}{arrow}{Target.Label} ({Cost})";
        }

        /// <summary>
        /// Confronta due archi per uguaglianza basandosi su nodi, costo, direzionalità ed etichetta.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is Arch arch &&
                   EqualityComparer<Node>.Default.Equals(Source, arch.Source) &&
                   EqualityComparer<Node>.Default.Equals(Target, arch.Target) &&
                   Cost == arch.Cost &&
                   IsDirected == arch.IsDirected &&
                   Label == arch.Label;
        }
    }
}
