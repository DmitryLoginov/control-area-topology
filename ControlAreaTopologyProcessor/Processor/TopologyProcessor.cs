using ControlAreaTopologyProcessor.Exception;
using ControlAreaTopologyProcessor.Model;

namespace ControlAreaTopologyProcessor.Processor
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
                TieFlow startTieFlow = remainingTieFlows.First();

                Traverse(controlArea, startTieFlow, remainingTieFlows, containedEquipments);
            }
        }

        private static void Traverse(ControlArea controlArea, TieFlow startTieFlow,
            HashSet<TieFlow> remainingTieFlows, HashSet<ConductingEquipment> containedEquipments)
        {
            // TODO: map can be optimized
            Dictionary<ConductingEquipment, Terminal> initTerminalsMap = [];
            Stack<ConductingEquipment> equipmentsStack = new();

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

            // DFS обход
            while (equipmentsStack.Count != 0)
            {
                ConductingEquipment nextEquipment = equipmentsStack.Pop();

                if (!containedEquipments.Add(nextEquipment))
                {
                    continue;
                }

                foreach (Terminal terminal in GetOtherTerminals(nextEquipment, initTerminalsMap[nextEquipment]))
                {
                    ProcessNextTerminal(controlArea, terminal, remainingTieFlows, equipmentsStack, initTerminalsMap);
                }
            }
        }

        private static void ProcessNextTerminal(ControlArea controlArea, Terminal terminal, HashSet<TieFlow> remainingTieFlows,
            Stack<ConductingEquipment> equipmentsStack,
            Dictionary<ConductingEquipment, Terminal> initTerminalsMap)
        {
            TieFlow[] tieFlows = terminal.TieFlows;
            var targetTieFlows = terminal.TieFlows.Where(item => item.ControlArea.Equals(controlArea));

            if (targetTieFlows.Count() > 1)
            {
                throw new TieFlowException("Terminal has more than one TieFlow with the same ControlArea");
            }

            if (targetTieFlows.Count() == 1)
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

                return;
            }

            var otherTieFlows = terminal.TieFlows.Where(item => !item.ControlArea.Equals(controlArea));

            if (otherTieFlows.Any())
            {
                throw new TieFlowException("Potential unclosed border: the route trace met border of alien ControlArea");
            }

            ConnectivityNode connectivityNode = terminal.ConnectivityNode;

            if (connectivityNode != null)
            {
                IEnumerable<Terminal> yetAnotherNextTerminals = GetOtherTerminals(connectivityNode, terminal);

                foreach (Terminal yetAnotherTerminal in yetAnotherNextTerminals)
                {
                    if (yetAnotherTerminal.TieFlows.Length != 0)
                    {
                        var targetTieFlows2 = yetAnotherTerminal.TieFlows.Where(item => item.ControlArea.Equals(controlArea));

                        if (targetTieFlows2.Count() > 1)
                        {
                            throw new TieFlowException("Terminal has more than one TieFlow with the same ControlArea");
                        }
                        else if (targetTieFlows2.Count() == 1)
                        {
                            TieFlow targetTieFlow = targetTieFlows2.First();

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
                            var otherTieFlows2 = yetAnotherTerminal.TieFlows.Where(item => !item.ControlArea.Equals(controlArea));

                            if (otherTieFlows2.Any())
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