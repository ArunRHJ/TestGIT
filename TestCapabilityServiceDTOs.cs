using LandisGyr.AMI.Devices.Capabilities.Service.Contracts;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilityServiceDTOs
    {        
        [TestMethod]
        [Description(@"This test method is used to CommandCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestCommandCapabilitySer_DeSer()
        {
            CommandSetCapabilityTO commandCapability = new CommandSetCapabilityTO();
            commandCapability.Commands = new List<CommandTO>();
            commandCapability.Commands.Add(new CommandTO() { Identifier = "C1" });
            commandCapability.Commands.Add(new CommandTO() { Identifier = "C2" });

            DataContractSerializer dcsSer = new DataContractSerializer(commandCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, commandCapability);
            mmrStr.Close();

            string serializedCommandCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            CommandSetCapabilityTO deserCommandCapability = (CommandSetCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserCommandCapability, "Problem in Command Capability Serialization/Deserialization");
            Assert.IsTrue(commandCapability.Commands.Count == deserCommandCapability.Commands.Count, "Problem in serializing/deserializing Commands");

            foreach (CommandTO command in commandCapability.Commands)
            {
                Assert.IsTrue(deserCommandCapability.Commands.Exists(rg => rg.Identifier == command.Identifier), "Problem in CommandTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to RegsiterCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestRegisterCapabilitySer_DeSer()
        {
            RegisterSetCapabilityTO registerCapability = new RegisterSetCapabilityTO();
            registerCapability.Registers = new List<RegisterTO>();
            registerCapability.Registers.Add(new RegisterTO() { Identifier = "C1", DataType = "INT32" });
            registerCapability.Registers.Add(new RegisterTO() { Identifier = "C2", DataType = "INT64" });

            DataContractSerializer dcsSer = new DataContractSerializer(registerCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, registerCapability);
            mmrStr.Close();

            string serializedRegisterCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            RegisterSetCapabilityTO deserRegisterCapability = (RegisterSetCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserRegisterCapability, "Problem in Register Capability Serialization/Deserialization");
            Assert.IsTrue(registerCapability.Registers.Count == deserRegisterCapability.Registers.Count, "Problem in serializing/deserializing Commands");
            foreach (RegisterTO register in registerCapability.Registers)
            {
                Assert.IsTrue(deserRegisterCapability.Registers.Exists(rg => rg.Identifier == register.Identifier && rg.DataType == register.DataType), "Problem in RegisterTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to EventCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestEventCapabilitySer_Deser()
        {
            EventSetCapabilityTO eventCapability = new EventSetCapabilityTO();
            eventCapability.GapCollectionAlgo = EventGapCollectionAlgoTO.DatePointer;
            eventCapability.MinHistoricEvents = 40;
            eventCapability.Events = new List<EventTO>();
            eventCapability.Events.Add(new EventTO() { Identifier = "E1" });
            eventCapability.Events.Add(new EventTO() { Identifier = "E2" });

            DataContractSerializer dcsSer = new DataContractSerializer(eventCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, eventCapability);
            mmrStr.Close();

            string serializedEventCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            EventSetCapabilityTO deserEventCapability = (EventSetCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));
           
            Assert.IsNotNull(deserEventCapability, "Problem in Event Capability Serialization/Deserialization");
            Assert.IsTrue(eventCapability.Events.Count == deserEventCapability.Events.Count, "Problem in serializing/deserializing Events");
            Assert.IsTrue(eventCapability.GapCollectionAlgo == deserEventCapability.GapCollectionAlgo, "Problem in EventGapCollectionAlgo Serialization/Deserialization");
            Assert.IsTrue(eventCapability.MinHistoricEvents == deserEventCapability.MinHistoricEvents, "Problem in MinHistoricEvents Serialization/Deserialization");

            foreach (EventTO eventC in eventCapability.Events)
            {
                Assert.IsTrue(deserEventCapability.Events.Exists(rg => rg.Identifier == eventC.Identifier), "Problem in EventTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to LoadControlSwitchCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestLoadControlSwitchCapabilitySer_Deser()
        {
            LoadControlSwitchCapabilityTO lcsCapability = new LoadControlSwitchCapabilityTO();
            lcsCapability.SwitchStatuses = new List<SwitchStatusTO>();
            lcsCapability.SwitchStatuses.Add(new SwitchStatusTO() { SwitchStatusCode = "Off", SwitchStatusValue = LoadControlSwitchStatusTO.Disconnect });
            lcsCapability.SwitchStatuses.Add(new SwitchStatusTO() { SwitchStatusCode = "On", SwitchStatusValue = LoadControlSwitchStatusTO.Connect });

            DataContractSerializer dcsSer = new DataContractSerializer(lcsCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, lcsCapability);
            mmrStr.Close();

            string serializedLCSCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            LoadControlSwitchCapabilityTO deserLCSCapability = (LoadControlSwitchCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserLCSCapability, "Problem in LoadControlSwitch Capability Serialization/Deserialization");
            Assert.IsTrue(lcsCapability.SwitchStatuses.Count == deserLCSCapability.SwitchStatuses.Count, "Problem in serializing/deserializing SwitchStatus");

            foreach (SwitchStatusTO switchStatus in lcsCapability.SwitchStatuses)
            {
                Assert.IsTrue(deserLCSCapability.SwitchStatuses.Exists(rg => rg.SwitchStatusCode == switchStatus.SwitchStatusCode && rg.SwitchStatusValue == switchStatus.SwitchStatusValue), "Problem in SwitchStatusTO Serialization/Deserialization");
            }
        }


        [TestMethod]
        [Description(@"This test method is used to DailySnapCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestDailySnapCapabilitySer_Deser()
        {
            DailySnapCapabilityTO dailySnapCapability = new DailySnapCapabilityTO();
            dailySnapCapability.Frequency = 10;
            dailySnapCapability.Capacity = 20;
            dailySnapCapability.Registers = new List<RegisterTO>();
            dailySnapCapability.Registers.Add(new RegisterTO() { Identifier = "R1", DataType = "Int64" });
            dailySnapCapability.Registers.Add(new RegisterTO() { Identifier = "R2", DataType = "Int32" });

            DataContractSerializer dcsSer = new DataContractSerializer(dailySnapCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, dailySnapCapability);
            mmrStr.Close();

            string serializedDSCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            DailySnapCapabilityTO deserDailySnapCapability = (DailySnapCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserDailySnapCapability, "Problem in DailySnap Capability Serialization/Deserialization");
            Assert.IsTrue(dailySnapCapability.Registers.Count == deserDailySnapCapability.Registers.Count, "Problem in serializing/deserializing Registers");
            Assert.IsTrue(dailySnapCapability.Frequency == deserDailySnapCapability.Frequency, "Problem in Frequency Serialization/Deserialization");
            Assert.IsTrue(dailySnapCapability.Capacity == deserDailySnapCapability.Capacity, "Problem in Capacity Serialization/Deserialization");

            foreach (RegisterTO register in dailySnapCapability.Registers)
            {
                Assert.IsTrue(deserDailySnapCapability.Registers.Exists(rg => rg.Identifier == register.Identifier && rg.DataType == register.DataType), "Problem in RegisterTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to LoadProfileCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestLoadProfileCapabilitySer_Deser()
        {
            LoadProfileCapabilityTO lpCapability = new LoadProfileCapabilityTO();
            lpCapability.Frequency = 10;
            lpCapability.Capacity = 20;
            lpCapability.IsFullRegisterRead = true;
            lpCapability.Registers = new List<RegisterTO>();
            lpCapability.Registers.Add(new RegisterTO() { Identifier = "R1", DataType = "Int64" });
            lpCapability.Registers.Add(new RegisterTO() { Identifier = "R2", DataType = "Int32" });

            DataContractSerializer dcsSer = new DataContractSerializer(lpCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, lpCapability);
            mmrStr.Close();

            string serializedDSCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            LoadProfileCapabilityTO deserLPCapability = (LoadProfileCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserLPCapability, "Problem in LoadProfile Capability Serialization/Deserialization");
            Assert.IsTrue(lpCapability.Registers.Count == deserLPCapability.Registers.Count, "Problem in serializing/deserializing Registers");
            Assert.IsTrue(lpCapability.Frequency == deserLPCapability.Frequency, "Problem in Frequency Serialization/Deserialization");
            Assert.IsTrue(lpCapability.Capacity == deserLPCapability.Capacity, "Problem in Capacity Serialization/Deserialization");
            Assert.IsTrue(lpCapability.IsFullRegisterRead == deserLPCapability.IsFullRegisterRead, "Problem in IsFullRegisterRead Serialization/Deserialization");

            foreach (RegisterTO register in lpCapability.Registers)
            {
                Assert.IsTrue(deserLPCapability.Registers.Exists(rg => rg.Identifier == register.Identifier && rg.DataType == register.DataType), "Problem in RegisterTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to DemandResetCapabilityTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestDemandResetCapabilitySer_Deser()
        {
            DemandResetCapabilityTO drCapability = new DemandResetCapabilityTO();
            drCapability.Frequency = 10;
            drCapability.Capacity = 20;
            drCapability.SupportsMultipleBillingDates = true;
            drCapability.SupportsRecursiveBillingDate = true;
            drCapability.Registers = new List<RegisterTO>();
            drCapability.Registers.Add(new RegisterTO() { Identifier = "R1", DataType = "Int64" });
            drCapability.Registers.Add(new RegisterTO() { Identifier = "R2", DataType = "Int32" });

            DataContractSerializer dcsSer = new DataContractSerializer(drCapability.GetType());
            MemoryStream mmrStr = new MemoryStream();
            dcsSer.WriteObject(mmrStr, drCapability);
            mmrStr.Close();

            string serializedDRCapability = Encoding.ASCII.GetString(mmrStr.ToArray());

            DemandResetCapabilityTO deserDRCapability = (DemandResetCapabilityTO)dcsSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.IsNotNull(deserDRCapability, "Problem in LoadProfile Capability Serialization/Deserialization");
            Assert.IsTrue(drCapability.Registers.Count == deserDRCapability.Registers.Count, "Problem in serializing/deserializing Registers");
            Assert.IsTrue(drCapability.Frequency == deserDRCapability.Frequency, "Problem in Frequency Serialization/Deserialization");
            Assert.IsTrue(drCapability.Capacity == deserDRCapability.Capacity, "Problem in Capacity Serialization/Deserialization");
            Assert.IsTrue(drCapability.SupportsMultipleBillingDates == deserDRCapability.SupportsMultipleBillingDates, "Problem in SupportsMultipleBillingDates Serialization/Deserialization");
            Assert.IsTrue(drCapability.SupportsRecursiveBillingDate == deserDRCapability.SupportsRecursiveBillingDate, "Problem in SupportsRecursiveBillingDate Serialization/Deserialization");

            foreach (RegisterTO register in drCapability.Registers)
            {
                Assert.IsTrue(deserDRCapability.Registers.Exists(rg => rg.Identifier == register.Identifier && rg.DataType == register.DataType), "Problem in RegisterTO Serialization/Deserialization");
            }
        }

        [TestMethod]
        [Description(@"This test method is used to DeviceTO and its properties are properly serialized and deserialized")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestDeviceSer_Deser()
        {
            DeviceTO device = new DeviceTO();
            device.DeviceIdentifier = "D1";

            AddressingMechanismTO expectedAddressingMechanism = AddressingMechanismTO.WAN;
            bool expectedIsGridStream = true;
            device.CommunicationTechnology = new CommunicationTechnologyTO() { AddressingMechanism = expectedAddressingMechanism };
            device.MessagingProtocol = new MessagingProtocolTO() { IsGridStream = expectedIsGridStream };

            CommandSetCapabilityTO commandCapability = new CommandSetCapabilityTO();
            commandCapability.Commands = new List<CommandTO>();
            commandCapability.Commands.Add(new CommandTO() { Identifier = "C1" });

            LoadProfileCapabilityTO lpCapability = new LoadProfileCapabilityTO();
            lpCapability.Frequency = 20;
            lpCapability.Capacity = 30;
            lpCapability.IsFullRegisterRead = true;
            lpCapability.Registers = new List<RegisterTO>();
            lpCapability.Registers.Add(new RegisterTO() { Identifier = "R3", DataType = "Int32" });

            device.Capabilities = new List<BaseCapabilityTO>();
            device.Capabilities.Add(commandCapability);
            device.Capabilities.Add(lpCapability);

            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(commandCapability.GetType());
            knownTypes.Add(lpCapability.GetType());

            DataContractSerializer dcSer = new DataContractSerializer(device.GetType(), knownTypes);
            MemoryStream mmrStr = new MemoryStream();
            dcSer.WriteObject(mmrStr, device);
            mmrStr.Close();

            string serializedDeviceObj = Encoding.ASCII.GetString(mmrStr.ToArray());

            DeviceTO deSerDevice = null;

            deSerDevice = (DeviceTO)dcSer.ReadObject(new MemoryStream(mmrStr.ToArray()));

            Assert.AreEqual(device.Capabilities.Count, deSerDevice.Capabilities.Count, "Problem in Serialization/Deserialization");

            CommandSetCapabilityTO deserCommandCapability = (CommandSetCapabilityTO)deSerDevice.Capabilities.Find(cr => cr.CapabilityType == CapabilityTypeTO.Commands);            
            LoadProfileCapabilityTO deserLPCapability = (LoadProfileCapabilityTO)deSerDevice.Capabilities.Find(cr => cr.CapabilityType == CapabilityTypeTO.LoadProfile);

            Assert.IsNotNull(deSerDevice.CommunicationTechnology, "Communication Technology Serialization/Deserialization Failed");
            Assert.IsTrue(deSerDevice.CommunicationTechnology.AddressingMechanism == expectedAddressingMechanism, "Communication Technology Addressing Mechanism not set as expected value");

            Assert.IsNotNull(deSerDevice.MessagingProtocol, "Messaging Protocol Serialization/Deserialization Failed");
            Assert.IsTrue(deSerDevice.MessagingProtocol.IsGridStream == expectedIsGridStream, "Messaging Protocol IsGridStream not set as expected value");
            
            Assert.IsNotNull(deserCommandCapability, "Deserialized Capability must have Command Capability");
            Assert.IsNotNull(deserLPCapability, "Deserialized Capability must have LoadpProfile Capability");          
        }
    }
}

