using System.Drawing;

namespace CsGraph
{
    /// <summary>
    /// Rappresenta un nodo all'interno del grafo.
    /// Include la posizione grafica, un'etichetta testuale, un potenziale numerico (es. per algoritmi specifici),
    /// e una lista di archi connessi.
    /// </summary>
    public class Node(Point position, int potential = 0, string label = "")
    {
        public Point Position { get; set; } = position;
        public int Potential { get; set; } = potential;
        public string Label { get; set; } = label;
        public List<Arch> Arches { get; } = [];

        /// <summary>
        /// Aggiunge un arco alla lista se non è già presente.
        /// </summary>
        public void AddArch(Arch arch)
        {
            if (!Arches.Contains(arch))
            {
                Arches.Add(arch);
            }
        }

        /// <summary>
        /// Rimuove un arco dalla lista.
        /// </summary>
        public bool RemoveArch(Arch arch) => Arches.Remove(arch);

        /// <summary>
        /// Restituisce i nodi adiacenti al nodo corrente attraversando gli archi connessi.
        /// La valutazione è lazy, grazie all'utilizzo di 'yield return'.
        /// </summary>
        public IEnumerable<Node> GetAdjacentNodes()
        {
            foreach (Arch arch in Arches)
            {
                /* Restituisce una sequenza enumerabile di tutti i nodi adiacenti al nodo corrente, 
                 * ottenuti attraversando gli archi connessi. L'implementazione utilizza 'yield return'
                 * per garantire un'efficiente valutazione lazy (on-demand).

                 * questa modalità è più efficiente,
                 * i nodi vengono generati uno alla volta quando richiesto,
                 * non viene allocata memoria per l'intera collezione,
                 * se l'iterazione viene boccata, non tutti i nodi vengono elaborati,
                 * elimina la necessità di creare e popolare manualmente una lista
                 */
                yield return arch.GetOppositeNode(this);
            }
        }

        /// <summary>
        /// Restituisce l'etichetta del nodo.
        /// </summary>
        public override string ToString() => Label;
    }
}
