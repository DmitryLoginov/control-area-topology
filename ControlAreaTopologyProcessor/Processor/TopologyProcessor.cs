using ControlAreaTopologyProcessor.Exception;
using ControlAreaTopologyProcessor.Model;

namespace ControlAreaTopology.Processor
{
    public static class TopologyProcessor
    {
        public static void AreBordersClosedV1(ControlArea controlArea)
        {
            if (controlArea.TieFlows.Length == 0)
            {
                throw new TieFlowException("ControlArea has no borders");
            }

            HashSet<ConductingEquipment> containedEquipments = [];
            HashSet<TieFlow> remainingTieFlows = [.. controlArea.TieFlows];

            while (remainingTieFlows.Count > 0)
            {
                // TODO: map can be optimized
                Dictionary<ConductingEquipment, Terminal> initTerminalsMap = [];
                Stack<ConductingEquipment> equipmentsStack = new();

                TieFlow startTieFlow = remainingTieFlows.First();

                if (startTieFlow.positiveFlowIn)
                {
                    Terminal initTerminal = startTieFlow.Terminal;
                    ConductingEquipment initEquipment = initTerminal.ConductingEquipment;

                    initTerminalsMap.Add(initEquipment, initTerminal);
                    equipmentsStack.Push(initEquipment);

                    remainingTieFlows.Remove(startTieFlow);
                }
                else
                {
                    Terminal initTerminal = startTieFlow.Terminal;
                    ConnectivityNode initConnectivityNode = startTieFlow.Terminal.ConnectivityNode;
                    IEnumerable<Terminal> nextTerminals = GetOtherTerminals(initConnectivityNode, initTerminal);

                    foreach (Terminal terminal in nextTerminals)
                    {
                        ConductingEquipment equipment = terminal.ConductingEquipment;

                        initTerminalsMap.Add(equipment, terminal);
                        equipmentsStack.Push(equipment);
                    }

                    remainingTieFlows.Remove(startTieFlow);
                }

                while (equipmentsStack.TryPeek(out _))
                {
                    ConductingEquipment nextEquipment = equipmentsStack.Pop();

                    if (containedEquipments.Contains(nextEquipment))
                    {
                        continue;
                    }

                    containedEquipments.Add(nextEquipment);

                    IEnumerable<Terminal> nextTerminals = GetOtherTerminals(nextEquipment, initTerminalsMap[nextEquipment]);

                    if (!nextTerminals.Any())
                    {
                        continue;
                    }

                    foreach (Terminal terminal in nextTerminals)
                    {
                        TieFlow[] tieFlows = terminal.TieFlows;

                        if (tieFlows.Length != 0)
                        {
                            var targetTieFlows = terminal.TieFlows.Where(item => item.ControlArea.Equals(controlArea));

                            if (targetTieFlows.Count() > 1)
                            {
                                throw new TieFlowException("Terminal has more than one TieFlow with the same ControlArea");
                            }
                            else if (targetTieFlows.Count() == 1)
                            {
                                TieFlow targetTieFlow = targetTieFlows.First();

                                if (remainingTieFlows.Contains(targetTieFlow))
                                {
                                    if (!targetTieFlow.positiveFlowIn)
                                    {
                                        throw new TieFlowException("Incorrect border: Terminal should has positiveFlowIn equals to true");
                                    }
                                    else
                                    {
                                        remainingTieFlows.Remove(targetTieFlow);
                                    }
                                }
                            }
                            else
                            {
                                var otherTieFlows = terminal.TieFlows.Where(item => !item.ControlArea.Equals(controlArea));

                                if (otherTieFlows.Any())
                                {
                                    throw new TieFlowException("Potential unclosed border: the route trace met border of alien ControlArea");
                                }
                            }
                        }
                        else
                        {
                            ConnectivityNode connectivityNode = terminal.ConnectivityNode;

                            if (connectivityNode != null)
                            {
                                IEnumerable<Terminal> yetAnotherNextTerminals = GetOtherTerminals(connectivityNode, terminal);

                                foreach (Terminal yetAnotherTerminal in yetAnotherNextTerminals)
                                {
                                    if (yetAnotherTerminal.TieFlows.Length != 0)
                                    {
                                        var targetTieFlows = yetAnotherTerminal.TieFlows.Where(item => item.ControlArea.Equals(controlArea));

                                        if (targetTieFlows.Count() > 1)
                                        {
                                            throw new TieFlowException("Terminal has more than one TieFlow with the same ControlArea");
                                        }
                                        else if (targetTieFlows.Count() == 1)
                                        {
                                            TieFlow targetTieFlow = targetTieFlows.First();

                                            if (remainingTieFlows.Contains(targetTieFlow))
                                            {
                                                if (targetTieFlow.positiveFlowIn)
                                                {
                                                    throw new TieFlowException("Incorrect border: Terminal should has positiveFlowIn equals to false");
                                                }
                                                else
                                                {
                                                    remainingTieFlows.Remove(targetTieFlow);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var otherTieFlows = yetAnotherTerminal.TieFlows.Where(item => !item.ControlArea.Equals(controlArea));

                                            if (otherTieFlows.Any())
                                            {
                                                throw new TieFlowException("Potential unclosed border: the route trace met border of alien ControlArea");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ConductingEquipment equipment = yetAnotherTerminal.ConductingEquipment;

                                        if (!initTerminalsMap.TryGetValue(equipment, out _))
                                        {
                                            // TODO: different terminals
                                            initTerminalsMap.Add(equipment, yetAnotherTerminal);
                                            equipmentsStack.Push(equipment);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<Terminal> GetOtherTerminals(ConductingEquipment conductingEquipment, Terminal except)
        {
            return conductingEquipment.Terminals.Where(item => !item.Equals(except));
        }

        private static IEnumerable<Terminal> GetOtherTerminals(ConnectivityNode connectivityNode, Terminal except)
        {
            return connectivityNode.Terminals.Where(item => !item.Equals(except));
        }
    }
}