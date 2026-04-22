using ControlAreaTopologyProcessor.Exception;
using ControlAreaTopologyProcessor.Model;
using ControlAreaTopologyProcessor.Processor;

namespace ControlAreaTopology.Processor.Tests
{
    [TestClass()]
    public class TopologyProcessorTests
    {
        [TestMethod()]
        public void OneEquipmentClosed()
        {
            ConductingEquipment conductingEquipment = new ConductingEquipment();

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(conductingEquipment.Terminals.First(), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(conductingEquipment.Terminals.Last(), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void TwoSerialConnectedEquipmentsClosed()
        {
            ConductingEquipment first = new ConductingEquipment();
            ConductingEquipment second = new ConductingEquipment();

            ConnectivityNode connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                first.Terminals.First(item => item.sequenceNumber == 1),
                second.Terminals.First(item => item.sequenceNumber == 1)
            });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(first.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(second.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void ThreeStarConnectedEquipmentsClosed()
        {
            ConductingEquipment first = new ConductingEquipment();
            ConductingEquipment second = new ConductingEquipment();
            ConductingEquipment third = new ConductingEquipment();

            ConnectivityNode connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                first.Terminals.First(item => item.sequenceNumber == 1),
                second.Terminals.First(item => item.sequenceNumber == 1),
                third.Terminals.First(item => item.sequenceNumber == 1)
            });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(first.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(second.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow thirdTieFlow = new TieFlow(third.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void OneThreeTerminalEquipmentClosed()
        {
            ConductingEquipment conductingEquipment = new ConductingEquipment(3);

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(conductingEquipment.Terminals.First(item => item.sequenceNumber == 1), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(conductingEquipment.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow thirdTieFlow = new TieFlow(conductingEquipment.Terminals.First(item => item.sequenceNumber == 3), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void ThreeTerminalEquipmentConnectedToOtherEquipmentClosed()
        {
            ConductingEquipment threeTerminalEquipment = new ConductingEquipment(3);
            ConductingEquipment twoTerminalEquipment = new ConductingEquipment();

            ConnectivityNode connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                threeTerminalEquipment.Terminals.First(item => item.sequenceNumber == 2),
                twoTerminalEquipment.Terminals.First(item => item.sequenceNumber == 2)
            });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(threeTerminalEquipment.Terminals.First(item => item.sequenceNumber == 1), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(twoTerminalEquipment.Terminals.First(item => item.sequenceNumber == 1), controlArea, true);
            TieFlow thirdTieFlow = new TieFlow(threeTerminalEquipment.Terminals.First(item => item.sequenceNumber == 3), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void ThreeStarConnectedEquipmentsWithTwoBordersClosed()
        {
            ConductingEquipment first = new ConductingEquipment();
            ConductingEquipment second = new ConductingEquipment();
            ConductingEquipment third = new ConductingEquipment();

            ConnectivityNode connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                first.Terminals.First(item => item.sequenceNumber == 1),
                second.Terminals.First(item => item.sequenceNumber == 1),
                third.Terminals.First(item => item.sequenceNumber == 1)
            });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(first.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(second.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void IncorrectPositiveFlowIn()
        {
            ConductingEquipment conductingEquipment = new ConductingEquipment();

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(conductingEquipment.Terminals.First(), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(conductingEquipment.Terminals.Last(), controlArea, false);

            Assert.ThrowsException<TieFlowException>(() => TopologyProcessor.AreBordersClosedV1(controlArea));
        }

        [TestMethod()]
        public void EquipmentLoopInsideControlArea()
        {
            ConductingEquipment equipment_firstInner = new ConductingEquipment();
            ConductingEquipment equipment_secondInner = new ConductingEquipment();
            ConductingEquipment equipment_inter = new ConductingEquipment();
            ConductingEquipment equipment_outer = new ConductingEquipment();

            ConnectivityNode cn_inner = new ConnectivityNode(new List<Terminal>()
            {
                equipment_firstInner.Terminals.First(item => item.sequenceNumber == 1),
                equipment_secondInner.Terminals.First(item => item.sequenceNumber == 1),
                equipment_inter.Terminals.First(item => item.sequenceNumber == 1)
            });

            ConnectivityNode cn_outer = new ConnectivityNode(new List<Terminal>()
            {
                equipment_inter.Terminals.First(item => item.sequenceNumber == 2),
                equipment_outer.Terminals.First(item => item.sequenceNumber == 2)
            });

            ConnectivityNode cn_inter = new ConnectivityNode(new List<Terminal>()
            {
                equipment_outer.Terminals.First(item => item.sequenceNumber == 1),
                equipment_firstInner.Terminals.First(item => item.sequenceNumber == 2)
            });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(equipment_firstInner.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(equipment_secondInner.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod()]
        public void UnclosedBorderWithOtherControlArea()
        {
            ConductingEquipment ca1_first = new ConductingEquipment();
            ConductingEquipment ca1_second = new ConductingEquipment();
            ConductingEquipment ca1_third = new ConductingEquipment();

            ConnectivityNode ca1_connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                ca1_first.Terminals.First(item => item.sequenceNumber == 1),
                ca1_second.Terminals.First(item => item.sequenceNumber == 1),
                ca1_third.Terminals.First(item => item.sequenceNumber == 1)
            });

            ControlArea controlArea_1 = new ControlArea();

            TieFlow ca1_firstTieFlow = new TieFlow(ca1_first.Terminals.First(item => item.sequenceNumber == 2), controlArea_1, true);
            TieFlow ca1_secondTieFlow = new TieFlow(ca1_second.Terminals.First(item => item.sequenceNumber == 2), controlArea_1, true);


            ConductingEquipment ca2_first = new ConductingEquipment();
            ConductingEquipment ca2_second = new ConductingEquipment();
            ConductingEquipment ca2_third = new ConductingEquipment();

            ConnectivityNode ca2_connectivityNode = new ConnectivityNode(new List<Terminal>()
            {
                ca2_first.Terminals.First(item => item.sequenceNumber == 1),
                ca2_second.Terminals.First(item => item.sequenceNumber == 1),
                ca2_third.Terminals.First(item => item.sequenceNumber == 1)
            });

            ControlArea controlArea_2 = new ControlArea();

            TieFlow ca2_firstTieFlow = new TieFlow(ca2_first.Terminals.First(item => item.sequenceNumber == 2), controlArea_2, true);
            TieFlow ca2_secondTieFlow = new TieFlow(ca2_second.Terminals.First(item => item.sequenceNumber == 2), controlArea_2, true);
            // третья граница отсутствует, трассировка выводит на границу смежной ОК

            // граница проходит по оборудованию смежной ОК
            TieFlow ca1_thirdTieFlow = new TieFlow(ca2_third.Terminals.First(item => item.sequenceNumber == 2), controlArea_1, false);

            ConnectivityNode inter = new ConnectivityNode(new List<Terminal>()
            {
                ca1_third.Terminals.First(item => item.sequenceNumber == 2),
                ca2_third.Terminals.First(item => item.sequenceNumber == 2)
            });

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea_1);
            }
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }

            Assert.ThrowsException<TieFlowException>(() => TopologyProcessor.AreBordersClosedV1(controlArea_2));
        }

        [TestMethod()]
        public void EquipmentLoopAndBusbarSection()
        {
            ConductingEquipment line1 = new ConductingEquipment();
            line1.name = "line 1 not included";
            ConductingEquipment line2 = new ConductingEquipment();
            line2.name = "line 2 not included";
            ConductingEquipment line3 = new ConductingEquipment();
            line3.name = "line 3 included";
            ConductingEquipment line4 = new ConductingEquipment();
            line4.name = "line 4 included";
            ConductingEquipment busbar = new ConductingEquipment(1);
            busbar.name = "busbar";
            ConductingEquipment inner1 = new ConductingEquipment();
            inner1.name = "inner 1";
            ConductingEquipment inner2 = new ConductingEquipment();
            inner2.name = "inner 2 (intermediate)";
            ConductingEquipment inner3 = new ConductingEquipment();
            inner3.name = "inner 3";

            ConnectivityNode node1 = new ConnectivityNode(new List<Terminal>()
            {
                line1.Terminals.First(item => item.sequenceNumber == 2),
                line2.Terminals.First(item => item.sequenceNumber == 2),
                line3.Terminals.First(item => item.sequenceNumber == 2),
                line4.Terminals.First(item => item.sequenceNumber == 2),
                busbar.Terminals.First(),
                inner1.Terminals.First(item => item.sequenceNumber == 2),
                inner3.Terminals.First(item => item.sequenceNumber == 2),
            });

            ConnectivityNode node2 = new ConnectivityNode(new List<Terminal>()
            {
                inner1.Terminals.First(item => item.sequenceNumber == 1),
                inner2.Terminals.First(item => item.sequenceNumber == 1),
            });

            ConnectivityNode node3 = new ConnectivityNode(new List<Terminal>()
            {
                inner3.Terminals.First(item => item.sequenceNumber == 1),
                inner2.Terminals.First(item => item.sequenceNumber == 2),
            });

            ControlArea controlArea = new ControlArea();

            TieFlow tieFlowNear1 = new TieFlow(line1.Terminals.First(item => item.sequenceNumber == 2), controlArea, false);
            tieFlowNear1.name = "line 1 tie flow near";
            TieFlow tieFlowNear2 = new TieFlow(line2.Terminals.First(item => item.sequenceNumber == 2), controlArea, false);
            tieFlowNear2.name = "line 2 tie flow near";
            TieFlow tieFlowFar1 = new TieFlow(line3.Terminals.First(item => item.sequenceNumber == 1), controlArea, true);
            tieFlowFar1.name = "line 3 tie flow far";
            TieFlow tieFlowFar2 = new TieFlow(line4.Terminals.First(item => item.sequenceNumber == 1), controlArea, true);
            tieFlowFar2.name = "line 4 tie flow far";

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }
    }
}