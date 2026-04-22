namespace ControlAreaTopologyProcessor.Model
{
    public class ConnectivityNode
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public Terminal[] Terminals { get; private set; } = [];

        /// <summary>
        /// Creates an empty connectivity node (with no terminals connected).
        /// </summary>
        public ConnectivityNode()
        {
        }

        /// <summary>
        /// Creates a connectivity node with given terminals connected.
        /// </summary>
        /// <param name="terminals">Terminals that should be connected to this connectivity node.</param>
        /// <remarks>
        /// All terminals got ConnectivityNode link set to this connectivity node.
        /// </remarks>
        public ConnectivityNode(IEnumerable<Terminal> terminals)
        {
            Terminals = terminals.ToArray();

            foreach (Terminal terminal in terminals)
            {
                terminal.ConnectivityNode = this;
            }
        }

        public void AddToTerminals(Terminal terminal)
        {
            if (!Terminals.Contains(terminal))
            {
                var temp = Terminals;
                Array.Resize<Terminal>(ref temp, Terminals.Length + 1);
                temp[^1] = terminal;
                Terminals = temp;
            }
        }

        public void RemoveFromTerminals(Terminal terminal)
        {
            if (Terminals.Contains(terminal))
            {
                var temp = Terminals.ToList<Terminal>();
                temp.Remove(terminal);
                Terminals = temp.ToArray();
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is ConnectivityNode node &&
                   Uid.Equals(node.Uid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid);
        }
    }
}