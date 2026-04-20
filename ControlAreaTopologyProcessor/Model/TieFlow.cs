namespace ControlAreaTopologyProcessor.Model
{
    public class TieFlow
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public string name { get; set; }
        public bool positiveFlowIn { get; set; }
        public Terminal Terminal { get; set; }
        public ControlArea ControlArea { get; set; }

        /// <summary>
        /// Creates a tie flow for given control area and equipment terminal.
        /// </summary>
        /// <param name="terminal">Conducting equipment terminal.</param>
        /// <param name="controlArea">Control area.</param>
        /// <param name="positiveFlowIn">Does equipment belong to control area?</param>
        /// <remarks>
        /// Control area and terminal got their corresponding links updated with this tie flow.
        /// </remarks>
        public TieFlow(Terminal terminal, ControlArea controlArea, bool positiveFlowIn)
        {
            this.Terminal = terminal;
            this.ControlArea = controlArea;
            this.positiveFlowIn = positiveFlowIn;

            terminal.AddToTieFlows(this);
            controlArea.AddToTieFlows(this);
        }

        public override bool Equals(object? obj)
        {
            return obj is TieFlow flow &&
                   Uid.Equals(flow.Uid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid);
        }
    }
}