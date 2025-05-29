using CsGraph;
using System.Text.Json;
namespace VisualGraph
{
    public partial class Home : Form
    {
        private Graph? graph;
        private Brush nodeColor = Brushes.LightBlue;
        private Brush textColor = Brushes.Black;
        private Pen archPen = Pens.Black;
        private Pen arrowPen = new(Color.Black, 2) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor };

        public Home()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (graph == null) return;

            //disegno archi
            foreach (Arch arch in graph!.Arches)
            {
                Point source = arch.Source.Position;
                Point target = arch.Target.Position;
                g.DrawLine(arch.IsDirected ? arrowPen : archPen, source, target);

            }

            //disegno nodi
            foreach (Node node in graph.Nodes)
            {
                const int diameter = 30;
                Point point = node.Position;
                Rectangle circle = new(point.X - diameter / 2, point.Y - diameter / 2, diameter, diameter);
                g.FillEllipse(nodeColor, circle);
                g.DrawEllipse(Pens.Black, circle);
                g.DrawString(node.Label, this.Font, textColor, point.X - 6, point.Y - 8);
            }
        }

        private void btnImportGraph_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.Title = "Seleziona un file JSON del grafo";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                try
                {
                    string json = File.ReadAllText(path);

                    // Deserializza in una struttura temporanea che corrisponde al JSON
                    var jsonData = JsonSerializer.Deserialize<JsonElement>(json);
                    Graph loadedGraph = new Graph();

                    // crea tutti i nodi
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

                    // Seconda passa: crea tutti gli archi
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

                    graph = loadedGraph;
                    Invalidate(); // Triggera la chiamata a OnPaint

                    MessageBox.Show($"Grafo caricato con successo!\nNodi: {graph.Nodes.Count}\nArchi: {graph.Arches.Count}", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante il caricamento: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAlgorithmChoice_Click(object sender, EventArgs e)
        {
            if (graph != null)
            {
                SelezioneAlgoritmo algorithmForm = new SelezioneAlgoritmo(graph);

                // Sottoscrivi l'evento
                algorithmForm.AlgorithmExecuted += OnAlgorithmExecuted;

                algorithmForm.Show();
            }
            else
            {
                MessageBox.Show("devi prima caricare un grafo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Metodo che gestisce il risultato dell'algoritmo
        private void OnAlgorithmExecuted(Graph result)
        {
            // Qui puoi gestire il risultato in base al tipo
            MessageBox.Show($"Algoritmo eseguito! Risultato: {result}", "Risultato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            graph = result;

            // Se vuoi aggiornare la visualizzazione del grafo
            Invalidate();
        }
    }
}
