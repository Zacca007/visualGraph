using CsGraph;
namespace VisualGraph
{
    public enum NodeSelectionType
    {
        None,
        Single,
        Double
    }

    public partial class SelezioneAlgoritmo : Form
    {
        private string algorithm = "";
        private readonly Graph graph;
        private Graph result = new();
        private Node? startNode;
        private Node? endNode;
        private static readonly Dictionary<string, NodeSelectionType> algorithmSelectionMap = new()
        {
            { "kruskal", NodeSelectionType.None },
            { "prim", NodeSelectionType.None },
            { "BFS", NodeSelectionType.Single },
            { "DFS", NodeSelectionType.Single },
            { "dijkstra", NodeSelectionType.Double }
        };

        public event Action<Graph>? AlgorithmExecuted;


        public SelezioneAlgoritmo(Graph graph)
        {
            InitializeComponent();
            this.graph = graph;

            cbStartNode.DataSource = graph.Nodes.ToList();
            cbStartNode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEndNode.DataSource = graph.Nodes.ToList();
            cbEndNode.DropDownStyle = ComboBoxStyle.DropDownList;


            lbStartNode.Visible = false;
            lbEndNode.Visible = false;
            cbStartNode.Visible = false;
            cbEndNode.Visible = false;
        }

        public string Algorithm => algorithm;
        public Graph Graph => graph;

        private string? GetSelectedAlgorithm()
        {
            return cbAlgorithm.SelectedItem!.ToString();
        }

        private void cbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithm = GetSelectedAlgorithm()!;
            NodeSelectionType type = algorithmSelectionMap[algorithm];
            switch (type)
            {
                case NodeSelectionType.Double: DisplayDoubleNodeChoice(); break;
                case NodeSelectionType.Single: DisplaySingleNodeChoice(); break;
                case NodeSelectionType.None: break;
                default: break;
            }
            Invalidate();
        }

        private void DisplaySingleNodeChoice()
        {
            lbStartNode.Visible = true;
            cbStartNode.Visible = true;
        }

        private void DisplayDoubleNodeChoice()
        {
            DisplaySingleNodeChoice();
            lbEndNode.Visible = true;
            cbEndNode.Visible = true;
        }

        private void cbStartNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            startNode = graph.Nodes[cbStartNode.SelectedIndex];
        }

        private void cbEndNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            endNode = graph.Nodes[cbEndNode.SelectedIndex];
        }

        private void cbExecution_Click(object sender, EventArgs e)
        {
            switch (algorithm)
            {
                case "dijkstra": result = graph.Dijkstra(startNode!, endNode!); break;
                default: break;
            }
            AlgorithmExecuted?.Invoke(result!);
        }
    }
}
