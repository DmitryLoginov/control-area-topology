namespace ControlAreaTopologyProcessor.Model
{
    public class ConductingEquipment
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public string name { get; set; }
        public Terminal[] Terminals { get; private set; }

        /// <summary>
        /// Creates a conducting equipment with two terminals.
        /// </summary>
        /// <remarks>
        /// Terminals got ConductingEquipment link set to this conducting equipment.
        /// </remarks>
        public ConductingEquipment()
        {
            Terminal[] terminals = [new Terminal(1, this), new Terminal(2, this)];
            this.Terminals = terminals;
        }

        /// <summary>
        /// Creates conducting equipment with given amount of terminals.
        /// </summary>
        /// <param name="terminalsCount">Count of equipment terminals.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when terminalsCount less or equals to zero.</exception>
        /// <remarks>
        /// Terminals got ConductingEquipment link set to this conducting equipment.
        /// </remarks>
        public ConductingEquipment(int terminalsCount)
        {
            if (terminalsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(terminalsCount), "Terminal count should be greater than zero");
            }
            else
            {
                Terminal[] terminals = new Terminal[terminalsCount];

                for (int i = 0; i < terminalsCount; i++)
                {
                    terminals[i] = new Terminal(i + 1, this);
                }

                this.Terminals = terminals;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is ConductingEquipment equipment &&
                   Uid.Equals(equipment.Uid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid);
        }
    }
}