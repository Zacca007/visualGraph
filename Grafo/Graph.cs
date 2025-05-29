namespace CsGraph
{
    public class Graph()
    {
        public List<Node> Nodes { get; } = [];
        public List<Arch> Arches { get; } = [];

        public Graph(List<Node> nodes, List<Arch> arches) : this()
        {
            Nodes = nodes;
            Arches = arches;
        }

        public void AddNode(Node node)
        {
            if (!Nodes.Contains(node))
                Nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            if (Nodes.Remove(node))
            {
                // Rimuove tutti gli archi connessi al nodo
                IEnumerable<Arch> connectedArches = Arches.Where(e => e.Source == node || e.Target == node).ToList();
                foreach (Arch arch in connectedArches)
                {
                    RemoveArch(arch);
                }
            }
        }

        public void AddArch(Node source, Node target, int cost = 1, bool isDirected = false, string label = "")
        {
            var arch = new Arch(source, target, cost, isDirected, label);
            foreach (Arch a in Arches)
            {
                if (a.Equals(arch))
                {
                    return;
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

        public void RemoveArch(Arch arch)
        {
            arch.Source.RemoveArch(arch);
            arch.Target.RemoveArch(arch);
            Arches.Remove(arch);
        }

        public IEnumerable<Node> GetAdjacentNodes(Node node)
        {
            return node.GetAdjacentNodes();
        }

        public override string ToString()
        {
            var nodesStr = string.Join(", ", Nodes.Select(n => n.Label));
            var edgesStr = string.Join("\n", Arches.Select(e => e.ToString()));
            return $"Nodes: {nodesStr}\nEdges:\n{edgesStr}";
        }

        public Graph Dijkstra(Node startNode, Node endNode)
        {
            // Verifica che i nodi esistano nel grafo
            if (!Nodes.Contains(startNode) || !Nodes.Contains(endNode))
                throw new ArgumentException("I nodi di partenza o destinazione non esistono nel grafo");

            // Dizionari per tenere traccia delle distanze e dei predecessori
            var distances = new Dictionary<Node, int>();
            var predecessors = new Dictionary<Node, Node>();
            var visited = new HashSet<Node>();
            var unvisited = new HashSet<Node>();

            // Inizializzazione: distanza infinita per tutti i nodi tranne il nodo di partenza
            foreach (var node in Nodes)
            {
                distances[node] = node == startNode ? 0 : int.MaxValue;
                unvisited.Add(node);
            }

            while (unvisited.Count > 0)
            {
                // Trova il nodo non visitato con la distanza minima
                Node currentNode = null;
                int minDistance = int.MaxValue;

                foreach (var node in unvisited)
                {
                    if (distances[node] < minDistance)
                    {
                        minDistance = distances[node];
                        currentNode = node;
                    }
                }

                // Se non riusciamo a trovare un nodo raggiungibile, non esiste un percorso
                if (currentNode == null || distances[currentNode] == int.MaxValue)
                    break;

                // Se abbiamo raggiunto il nodo di destinazione, possiamo fermarci
                if (currentNode == endNode)
                    break;

                // Rimuovi il nodo corrente dai non visitati
                unvisited.Remove(currentNode);
                visited.Add(currentNode);

                // Esamina tutti i vicini del nodo corrente
                foreach (var arch in currentNode.Arches)
                {
                    Node neighbor = arch.GetOppositeNode(currentNode);

                    // Se è un arco diretto, verifica la direzione
                    if (arch.IsDirected && arch.Source != currentNode)
                        continue;

                    // Se il vicino è già stato visitato, saltalo
                    if (visited.Contains(neighbor))
                        continue;

                    // Calcola la nuova distanza attraverso il nodo corrente
                    int newDistance = distances[currentNode] + arch.Cost;

                    // Se abbiamo trovato un percorso più breve, aggiorna
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        predecessors[neighbor] = currentNode;
                    }
                }
            }

            // Se il nodo di destinazione non è raggiungibile
            if (!predecessors.ContainsKey(endNode) && startNode != endNode)
                return new Graph(); // Ritorna un grafo vuoto

            // Ricostruisci il percorso dal nodo di destinazione al nodo di partenza
            var path = new List<Node>();
            Node current = endNode;

            while (current != null)
            {
                path.Add(current);
                predecessors.TryGetValue(current, out current);
            }

            path.Reverse(); // Inverti per avere il percorso dal start all'end

            // Crea il grafo risultante con il percorso più breve
            var resultGraph = new Graph();

            // Aggiungi tutti i nodi del percorso
            foreach (var node in path)
            {
                resultGraph.AddNode(node);
            }

            // Aggiungi gli archi che collegano i nodi consecutivi nel percorso
            for (int i = 0; i < path.Count - 1; i++)
            {
                Node fromNode = path[i];
                Node toNode = path[i + 1];

                // Trova l'arco originale tra questi due nodi
                var originalArch = Arches.FirstOrDefault(arch =>
                    (arch.Source == fromNode && arch.Target == toNode) ||
                    (!arch.IsDirected && arch.Source == toNode && arch.Target == fromNode));

                if (originalArch != null)
                {
                    // Aggiungi l'arco al grafo risultante mantenendo le proprietà originali
                    resultGraph.Arches.Add(new Arch(fromNode, toNode,
                        originalArch.Cost, originalArch.IsDirected, originalArch.Label));
                }
            }

            return resultGraph;
        }
    }
}