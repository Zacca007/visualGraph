using CsGraph;
using System.Text.Json;

namespace VisualGraph
{
    public partial class Home : Form
    {
        private Graph? graph;

        // Definizione colori e penne per il disegno di nodi e archi
        private readonly Brush nodeColor = Brushes.LightBlue;
        private readonly Brush textColor = Brushes.Black;
        private readonly Pen archPen = Pens.Black;
        private readonly Pen arrowPen = new(Color.Black, 2) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };

        public Home()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo di override per il disegno del form.
        /// Disegna archi e nodi del grafo, se presente.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (graph == null) return;

            // Disegno archi: linea semplice o con freccia se diretto
            foreach (Arch arch in graph.Arches)
            {
                Point source = arch.Source.Position;
                Point target = arch.Target.Position;

                g.DrawLine(arch.IsDirected ? arrowPen : archPen, source, target);
            }

            // Disegno nodi come cerchi con etichetta centrata
            foreach (Node node in graph.Nodes)
            {
                const int diameter = 30;
                Point point = node.Position;
                Rectangle circle = new(point.X - diameter / 2, point.Y - diameter / 2, diameter, diameter);

                g.FillEllipse(nodeColor, circle);
                g.DrawEllipse(Pens.Black, circle);

                // Etichetta del nodo disegnata centrata (adattabile)
                g.DrawString(node.Label, this.Font, textColor, point.X - 6, point.Y - 8);
            }
        }

        /// <summary>
        /// Evento click sul bottone di importazione grafo da file JSON.
        /// Esegue parsing e ricostruzione della struttura grafo da JSON.
        /// </summary>
        private void btnImportGraph_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Seleziona un file JSON del grafo"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                try
                {
                    string json = File.ReadAllText(path);

                    // Deserializza JSON in JsonElement per estrarre nodi e archi
                    JsonElement jsonData = JsonSerializer.Deserialize<JsonElement>(json);
                    Graph loadedGraph = new();

                    // Prima passata: crea tutti i nodi e li memorizza in un dizionario per facile lookup
                    Dictionary<string, Node> nodeMap = new Dictionary<string, Node>();
                    if (jsonData.TryGetProperty("nodes", out JsonElement nodesElement))
                    {
                        foreach (JsonElement nodeElement in nodesElement.EnumerateArray())
                        {
                            string label = nodeElement.GetProperty("label").GetString() ?? "";
                            int x = nodeElement.GetProperty("x").GetInt32();
                            int y = nodeElement.GetProperty("y").GetInt32();

                            Node node = new Node(new Point(x, y), label: label);
                            loadedGraph.AddNode(node);
                            nodeMap[label] = node;
                        }
                    }

                    // Seconda passata: crea archi collegando nodi usando il dizionario
                    if (jsonData.TryGetProperty("arches", out JsonElement archesElement))
                    {
                        foreach (JsonElement archElement in archesElement.EnumerateArray())
                        {
                            string sourceLabel = archElement.GetProperty("source").GetString() ?? "";
                            string targetLabel = archElement.GetProperty("target").GetString() ?? "";
                            int cost = archElement.GetProperty("cost").GetInt32();

                            if (nodeMap.TryGetValue(sourceLabel, out Node? sourceNode) &&
                                nodeMap.TryGetValue(targetLabel, out Node? targetNode))
                            {
                                loadedGraph.AddArch(sourceNode, targetNode, cost);
                            }
                        }
                    }

                    // Assegna il grafo caricato e richiede ridisegno
                    graph = loadedGraph;
                    Invalidate();

                    MessageBox.Show($"Graph loaded successfully!\nNodes: {graph.Nodes.Count}\nArches: {graph.Arches.Count}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during the load: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Evento click sul bottone di scelta algoritmo.
        /// Apre la finestra di selezione algoritmo, passandole il grafo attuale.
        /// </summary>
        private void btnAlgorithmChoice_Click(object sender, EventArgs e)
        {
            if (graph != null)
            {
                SelezioneAlgoritmo algorithmForm = new(graph);

                // Sottoscrive l'evento per ottenere il risultato dell'algoritmo
                algorithmForm.AlgorithmExecuted += OnAlgorithmExecuted;

                algorithmForm.Show();
            }
            else
            {
                MessageBox.Show("Must load a graph first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gestisce il risultato restituito dall'esecuzione di un algoritmo.
        /// Aggiorna il grafo visualizzato e mostra messaggio di conferma.
        /// </summary>
        private void OnAlgorithmExecuted(Graph result)
        {
            MessageBox.Show($"Algorithm executed successfully! result:\n{result}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            graph = result;
            Invalidate(); // Ridisegna il grafo aggiornato
        }
    }
}
