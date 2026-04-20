using ControlAreaTopologyProcessor.Exception;
using ControlAreaTopologyProcessor.Model;

namespace ControlAreaTopology.Processor.Tests
{
    [TestClass()]
    public class TopologyProcessorTests
    {
        [TestMethod(displayName: "Одна единица проводящего оборудования с двумя полюсами, оба полюса имеют корректные границы.")]
        public void OneConductingEquipmentClosed()
        {
            ConductingEquipment conductingEquipment = new ConductingEquipment();

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(conductingEquipment.Terminals.First(), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(conductingEquipment.Terminals.Last(), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod(displayName: "Две последовательно соединенные единицы оборудования (с двумя полюсами), " +
            "две корректные границы с концов оборудований.")]
        public void TwoSerialConnectedEquipmentsClosed()
        {
            ConductingEquipment first = new ConductingEquipment();
            ConductingEquipment second = new ConductingEquipment();
            ConnectivityNode connectivityNode = new ConnectivityNode(new List<Terminal>()
    {
        first.Terminals.First(item => item.sequenceNumber == 1), second.Terminals.First(item => item.sequenceNumber == 1)
    });

            ControlArea controlArea = new ControlArea();

            TieFlow firstTieFlow = new TieFlow(first.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);
            TieFlow secondTieFlow = new TieFlow(second.Terminals.First(item => item.sequenceNumber == 2), controlArea, true);

            try
            {
                TopologyProcessor.AreBordersClosedV1(controlArea);
            }
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod(displayName: "Три единицы оборудования (с двумя полюсами), соединенные по схеме звезда. " +
            "Три корректные границы с концов оборудований.")]
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
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod(displayName: "Одна единица проводящего оборудования с тремя полюсами, все три полюса имеют корректные границы.")]
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
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod(displayName: "Единица проводящего оборудования с тремя полюсами, к одному из которых присоединена " +
            "другая единица оборудования с двумя полюсами. Три корректные границы.")]
        public void OneThreeTerminalEquipmentSerialConnectedToOtherEquipmentClosed()
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
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        [TestMethod(displayName: "Три единицы оборудования (с двумя полюсами), соединенные по схеме звезда. " +
            "Две корректные границы, одна из единиц оборудования не имеет границы.")]
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
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }



        [TestMethod()]
        public void IncorrectPositiveFlowInOneEquipment()
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
            catch (System.Exception exception)
            {
                Assert.Fail($"ControlArea boundaries check failed: {exception.Message}");
            }
        }

        // ---------------------------------------
        // -----  -----
        // ---------------------------------------

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
    }
}