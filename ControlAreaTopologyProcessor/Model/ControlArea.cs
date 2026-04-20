namespace ControlAreaTopologyProcessor.Model
{
    public class ControlArea
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public string name { get; set; }
        public TieFlow[] TieFlows { get; private set; } = [];

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
            return obj is ControlArea area &&
                   Uid.Equals(area.Uid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid);
        }
    }
}