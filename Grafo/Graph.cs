namespace CsGraph
{
    public class Graph()
    {
        public List<Node> Nodes { get; } = [];
        public List<Arch> Arches { get; } = [];

        /// <summary>
        /// Costruttore con parametri: inizializza un grafo con nodi e archi specificati
        /// </summary>
        public Graph(List<Node> nodes, List<Arch> arches) : this()
        {
            Nodes = nodes;
            Arches = arches;
        }

        /// <summary>
        /// Aggiunge un nodo al grafo, se non è già presente
        /// </summary>
        public void AddNode(Node node)
        {
            if (!Nodes.Contains(node))
                Nodes.Add(node);
        }

        /// <summary>
        /// Rimuove un nodo dal grafo e tutti gli archi ad esso connessi
        /// </summary>
        public void RemoveNode(Node node)
        {
            if (Nodes.Remove(node))
            {
                // Rimuove gli archi che partono o arrivano al nodo
                IEnumerable<Arch> connectedArches = [.. Arches.Where(e => e.Source == node || e.Target == node)];
                foreach (Arch arch in connectedArches)
                {
                    RemoveArch(arch);
                }
            }
        }

        /// <summary>
        /// Aggiunge un arco tra due nodi, evitando duplicati e conflitti logici
        /// </summary>
        public void AddArch(Node source, Node target, int cost = 1, bool isDirected = false, string label = "")
        {
            Arch arch = new Arch(source, target, cost, isDirected, label);

            foreach (Arch a in Arches)
            {
                if (a.Equals(arch))
                {
                    return; // Arco già presente
                }

                if (a.Target == source && a.Source == target && !a.IsDirected)
                {
                    throw new Exception($"{arch} is not directed and can't be initialised with these nodes");
                }

                if (a.Target == target && a.Source == source)
                {
                    throw new Exception($"{arch} already exists, to edit, remove the existing one first");
                }
            }

            Arches.Add(arch);
        }

        /// <summary>
        /// Rimuove un arco dal grafo e dai nodi a cui è collegato
        /// </summary>
        public void RemoveArch(Arch arch)
        {
            arch.Source.RemoveArch(arch);
            arch.Target.RemoveArch(arch);
            Arches.Remove(arch);
        }

        /// <summary>
        /// Restituisce l'elenco dei nodi adiacenti a un dato nodo
        /// </summary>
        public static IEnumerable<Node> GetAdjacentNodes(Node node)
        {
            return node.GetAdjacentNodes();
        }

        /// <summary>
        /// Restituisce una rappresentazione testuale del grafo (nodi e archi)
        /// </summary>
        public override string ToString()
        {
            string nodesStr = string.Join(", ", Nodes.Select(n => n.Label));
            string edgesStr = string.Join("\n", Arches.Select(e => e.ToString()));
            return $"Nodes: {nodesStr}\nEdges:\n{edgesStr}";
        }

        /// <summary>
        /// Algoritmo di Dijkstra: restituisce un nuovo grafo che rappresenta il percorso minimo
        /// dal nodo di partenza a quello di arrivo.
        /// </summary>
        public Graph Dijkstra(Node startNode, Node endNode)
        {
            if (!Nodes.Contains(startNode) || !Nodes.Contains(endNode))
                throw new ArgumentException("starting or ending node is not on the graph");

            Dictionary<Node, int> distances = [];
            Dictionary<Node, Node> predecessors = [];
            HashSet<Node> visited = [];
            HashSet<Node> unvisited = [];

            // Inizializza le distanze: 0 per il nodo di partenza, infinito per gli altri
            foreach (Node node in Nodes)
            {
                distances[node] = node == startNode ? 0 : int.MaxValue;
                unvisited.Add(node);
            }

            while (unvisited.Count > 0)
            {
                // Trova il nodo non visitato con la distanza minima
                Node? currentNode = null;
                int minDistance = int.MaxValue;

                foreach (Node node in unvisited)
                {
                    if (distances[node] < minDistance)
                    {
                        minDistance = distances[node];
                        currentNode = node;
                    }
                }

                // Se non esiste un nodo raggiungibile, termina
                if (currentNode == null || distances[currentNode] == int.MaxValue)
                    break;

                // Se raggiungiamo il nodo di destinazione, possiamo terminare
                if (currentNode == endNode)
                    break;

                unvisited.Remove(currentNode);
                visited.Add(currentNode);

                // Esamina tutti i vicini
                foreach (Arch arch in currentNode.Arches)
                {
                    Node neighbor = arch.GetOppositeNode(currentNode);

                    // Salta archi direzionali non validi
                    if (arch.IsDirected && arch.Source != currentNode)
                        continue;

                    if (visited.Contains(neighbor))
                        continue;

                    int newDistance = distances[currentNode] + arch.Cost;

                    // Aggiorna distanza se il nuovo percorso è più breve
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        predecessors[neighbor] = currentNode;
                    }
                }
            }

            // Nessun percorso trovato
            if (!predecessors.ContainsKey(endNode) && startNode != endNode)
                return new Graph(); // grafo vuoto

            // Ricostruzione del percorso
            List<Node> path = [];
            Node current = endNode;

            while (current != null)
            {
                path.Add(current);
                predecessors.TryGetValue(current, out current!);
            }

            path.Reverse(); // Dal nodo di partenza a quello di arrivo

            // Costruisce un nuovo grafo contenente solo il percorso minimo
            Graph resultGraph = new();

            foreach (Node node in path)
            {
                resultGraph.AddNode(node);
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                Node fromNode = path[i];
                Node toNode = path[i + 1];

                Arch? originalArch = Arches.FirstOrDefault(arch =>
                    (arch.Source == fromNode && arch.Target == toNode) ||
                    (!arch.IsDirected && arch.Source == toNode && arch.Target == fromNode));

                if (originalArch != null)
                {
                    resultGraph.Arches.Add(new Arch(fromNode, toNode,
                        originalArch.Cost, originalArch.IsDirected, originalArch.Label));
                }
            }

            return resultGraph;
        }
    }
}
