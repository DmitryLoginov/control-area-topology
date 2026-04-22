namespace ControlAreaTopologyProcessor.Model
{
    public class Terminal
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public int sequenceNumber { get; private set; }
        public ConductingEquipment ConductingEquipment { get; private set; }

        public ConnectivityNode ConnectivityNode { get; set; }
        public TieFlow[] TieFlows { get; private set; } = [];

        /// <summary>
        /// Creates a terminal with given sequence number and parent conducting equipment.
        /// </summary>
        /// <param name="sequenceNumber">Sequence number of this terminal.</param>
        /// <param name="conductingEquipment">Conducting equipment to which terminal belongs.</param>
        /// <remarks>
        /// Conducting equipment DOESN'T get Terminals link updated.
        /// </remarks>
        public Terminal(int sequenceNumber, ConductingEquipment conductingEquipment)
        {
            this.sequenceNumber = sequenceNumber;
            this.ConductingEquipment = conductingEquipment;
        }

        public void AddToTieFlows(TieFlow tieFlow)
        {
            if (!TieFlows.Contains(tieFlow))
            {
                var temp = TieFlows;
                Array.Resize<TieFlow>(ref temp, TieFlows.Length + 1);
                temp[^1] = tieFlow;
                TieFlows = temp;
            }
        }

        public void RemoveFromTieFlows(TieFlow tieFlow)
        {
            if (TieFlows.Contains(tieFlow))
            {
                var temp = TieFlows.ToList<TieFlow>();
                temp.Remove(tieFlow);
                TieFlows = temp.ToArray();
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Terminal terminal &&
                   Uid.Equals(terminal.Uid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid);
        }
    }
}