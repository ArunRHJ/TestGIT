using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.Service.CapabilityMappers;
using LandisGyr.AMI.Devices.Capabilities.Service.Contracts;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilityMappers
    {
        [TestMethod]
        [Description(@"This test method is to check RegisterSetCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.Registers)]
        public void TestRegisterCapabilityMapper()
        {
            Register register = new Register("123", "INT");
            List<Register> registers = new List<Register>() { register };

            RegisterSetCapability registerCpblty = new RegisterSetCapability();
            registerCpblty.Registers = new ReadOnlyDictionary<string, Register>(registers.ToDictionary<Register, string>(r => r.Identifier));

            ICapabilityMapper mapper = new RegisterSetCapabilityMapper();
            RegisterSetCapabilityTO mappedRegisterCpblty = (RegisterSetCapabilityTO)mapper.GetMappedCapability(registerCpblty);

            Assert.IsNotNull(mappedRegisterCpblty, "Mapped Capability type should be of RegisterSetCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Registers, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedRegisterCpblty.CapabilityType == CapabilityTypeTO.Registers, "Mapped Capability type is not correct");
            Assert.IsNotNull(mappedRegisterCpblty.Registers, "Mapped Register capability should have mapped Registers");
            Assert.IsTrue(mappedRegisterCpblty.Registers.Count == 1, "Mapped Register capability should have 1 mapped Registers");
            Assert.IsTrue(mappedRegisterCpblty.Registers[0].Identifier == register.Identifier, "Mapped Register Identifier should be same as entity");
            Assert.IsTrue(mappedRegisterCpblty.Registers[0].DataType == register.DataType, "Mapped Register Datatype should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check CommandSetCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.Commands)]
        public void TestCommandCapabilityMapper()
        {
            Command command = new Command("123");
            List<Command> commands = new List<Command>() { command };

            CommandSetCapability cmdCpblty = new CommandSetCapability();
            cmdCpblty.Commands = new ReadOnlyDictionary<string, Command>(commands.ToDictionary<Command, string>(c => c.Identifier));

            ICapabilityMapper mapper = new CommandSetCapabilityMapper();
            CommandSetCapabilityTO mappedCmdCpblty = (CommandSetCapabilityTO)mapper.GetMappedCapability(cmdCpblty);

            Assert.IsNotNull(mappedCmdCpblty, "Mapped Capability type should be of CommandSetCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Commands, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedCmdCpblty.CapabilityType == CapabilityTypeTO.Commands, "Mapped Capability type is not correct");
            Assert.IsNotNull(mappedCmdCpblty.Commands, "Mapped Command capability should have mapped Commands");
            Assert.IsTrue(mappedCmdCpblty.Commands.Count == 1, "Mapped Command capability should have 1 mapped Command");
            Assert.IsTrue(mappedCmdCpblty.Commands[0].Identifier == command.Identifier, "Mapped Command Identifier should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check EventSetCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.Events)]
        public void TestEventsCapabilityMapper()
        {
            Event evnt = new Event("123");
            List<Event> events = new List<Event>() { evnt };

            int minHistoricEvents = 10;
            EventGapCollectionAlgo gapCollectionAlgo = EventGapCollectionAlgo.DatePointer;

            EventSetCapability eventCpblty = new EventSetCapability();
            eventCpblty.Events = new ReadOnlyDictionary<string, Event>(events.ToDictionary<Event, string>(e => e.Identifier));
            eventCpblty.MinHistoricEvents = minHistoricEvents;
            eventCpblty.GapCollectionAlgo = gapCollectionAlgo;

            ICapabilityMapper mapper = new EventSetCapabilityMapper();
            EventSetCapabilityTO mappedEventCpblty = (EventSetCapabilityTO)mapper.GetMappedCapability(eventCpblty);

            Assert.IsNotNull(mappedEventCpblty, "Mapped Capability type should be of EventSetCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Events, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedEventCpblty.CapabilityType == CapabilityTypeTO.Events, "Mapped Capability type is not correct");
            Assert.IsNotNull(mappedEventCpblty.Events, "Mapped Event capability should have mapped Events");
            Assert.IsTrue(mappedEventCpblty.Events.Count == 1, "Mapped Event capability should have 1 mapped Event");
            Assert.IsTrue(mappedEventCpblty.Events[0].Identifier == evnt.Identifier, "Mapped Event Identifier should be same as entity");
            Assert.IsTrue(mappedEventCpblty.MinHistoricEvents == minHistoricEvents, "Mapped Event capability should have mapped MinhistoricEvents");
            Assert.IsTrue(mappedEventCpblty.GapCollectionAlgo == (EventGapCollectionAlgoTO)(gapCollectionAlgo), "Mapped Event capability should have mapped GapCollectionAlgo");
        }

        [TestMethod]
        [Description(@"This test method is to check LoadControlSwitchCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.LoadControl)]
        public void TestLoadControlSwitchCapabilityMapper()
        {
            SwitchStatus switchStatus = new SwitchStatus("Off", LoadControlSwitchStatus.Disconnect);
            List<SwitchStatus> switchStatuses = new List<SwitchStatus>() { switchStatus };

            LoadControlSwitchCapability lcsCpblty = new LoadControlSwitchCapability();
            lcsCpblty.SwitchStatuses = new ReadOnlyDictionary<string, SwitchStatus>(switchStatuses.ToDictionary<SwitchStatus, string>(s => s.SwitchStatusCode));

            ICapabilityMapper mapper = new LoadControlSwitchCapabilityMapper();
            LoadControlSwitchCapabilityTO mappedLCSCpblty = (LoadControlSwitchCapabilityTO)mapper.GetMappedCapability(lcsCpblty);

            Assert.IsNotNull(mappedLCSCpblty, "Mapped Capability type should be of LoadControlSwitchCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.LoadControlStatus, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedLCSCpblty.CapabilityType == CapabilityTypeTO.LoadControlStatus, "Mapped Capability type is not correct");
            Assert.IsNotNull(mappedLCSCpblty.SwitchStatuses, "Mapped LoadControlSwitch capability should have mapped SwitchStatuses");
            Assert.IsTrue(mappedLCSCpblty.SwitchStatuses.Count == 1, "Mapped LoadControlSwitch capability should have 1 mapped SwitchStatus");
            Assert.IsTrue(mappedLCSCpblty.SwitchStatuses[0].SwitchStatusCode == switchStatus.SwitchStatusCode, "Mapped LoadControlSwitch SwitchStatusCode should be same as entity");
            Assert.IsTrue(mappedLCSCpblty.SwitchStatuses[0].SwitchStatusValue == (LoadControlSwitchStatusTO)(switchStatus.Value), "Mapped LoadControlSwitch SwitchStatusValue should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check DailySnapCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.DailySnap)]
        public void TestDailySnapCapabilityMapper()
        {
            Register register = new Register("123", "INT");
            Dictionary<string, Register> registers = new Dictionary<string, Register>();
            registers.Add(register.Identifier, register);
            int freq = 1;
            int capacity = 10;

            DailySnapCapability dailySnapCpblty = new DailySnapCapability(freq, capacity, registers);

            ICapabilityMapper mapper = new DailySnapCapabilityMapper();
            DailySnapCapabilityTO mappedDailySnapCpblty = (DailySnapCapabilityTO)mapper.GetMappedCapability(dailySnapCpblty);

            Assert.IsNotNull(mappedDailySnapCpblty, "Mapped Capability type should be of DailySnapCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.DailySnap, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedDailySnapCpblty.CapabilityType == CapabilityTypeTO.DailySnap, "Mapped Capability type is not correct");
            Assert.IsTrue(mappedDailySnapCpblty.Frequency == freq, "Mapped DailySnap capability does not have proper mapping of Frequency");
            Assert.IsTrue(mappedDailySnapCpblty.Capacity == capacity, "Mapped DailySnap capability does not have proper mapping of Capacity");
            Assert.IsNotNull(mappedDailySnapCpblty.Registers, "Mapped DailySnap capability should have mapped Registers");
            Assert.IsTrue(mappedDailySnapCpblty.Registers.Count == 1, "Mapped DailySnap capability should have 1 mapped Registers");
            Assert.IsTrue(mappedDailySnapCpblty.Registers[0].Identifier == register.Identifier, "Mapped DailySnap Identifier should be same as entity");
            Assert.IsTrue(mappedDailySnapCpblty.Registers[0].DataType == register.DataType, "Mapped DailySnap Datatype should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check LoadProfileCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.LoadProfile)]
        public void TestLoadProfileCapabilityMapper()
        {
            Register register = new Register("123", "INT");
            Dictionary<string, Register> registers = new Dictionary<string, Register>();
            registers.Add(register.Identifier, register);
            int freq = 1;
            int capacity = 10;

            LoadProfileCapability lpCpblty = new LoadProfileCapability(freq, capacity, registers);

            ICapabilityMapper mapper = new LoadProfileCapabilityMapper();
            LoadProfileCapabilityTO mappedLPCpblty = (LoadProfileCapabilityTO)mapper.GetMappedCapability(lpCpblty);

            Assert.IsNotNull(mappedLPCpblty, "Mapped Capability type should be of LoadProfileCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.LoadProfile, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedLPCpblty.CapabilityType == CapabilityTypeTO.LoadProfile, "Mapped Capability type is not correct");
            Assert.IsTrue(mappedLPCpblty.Frequency == freq, "Mapped LP capability does not have proper mapping of Frequency");
            Assert.IsTrue(mappedLPCpblty.Capacity == capacity, "Mapped LP capability does not have proper mapping of Capacity");
            Assert.IsNotNull(mappedLPCpblty.Registers, "Mapped LP capability should have mapped Registers");
            Assert.IsTrue(mappedLPCpblty.Registers.Count == 1, "Mapped LP capability should have 1 mapped Registers");
            Assert.IsTrue(mappedLPCpblty.Registers[0].Identifier == register.Identifier, "Mapped LP Identifier should be same as entity");
            Assert.IsTrue(mappedLPCpblty.Registers[0].DataType == register.DataType, "Mapped LP Datatype should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check DemandResetCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.DemandReset)]
        public void TestDemandResetCapabilityMapper()
        {
            Register register = new Register("123", "INT");
            Dictionary<string, Register> registers = new Dictionary<string, Register>();
            registers.Add(register.Identifier, register);
            int freq = 1;
            int capacity = 10;
            bool supportsMultipleBillingDates = true;
            bool supportsRecursiveBillingDate = true;

            DemandResetCapability drCpblty = new DemandResetCapability(freq, capacity, supportsMultipleBillingDates, supportsRecursiveBillingDate, registers);

            ICapabilityMapper mapper = new DemandResetCapabilityMapper();
            DemandResetCapabilityTO mappedDRCpblty = (DemandResetCapabilityTO)mapper.GetMappedCapability(drCpblty);

            Assert.IsNotNull(mappedDRCpblty, "Mapped Capability type should be of DemandResetCapabilityTO");
            Assert.IsTrue(mapper.CapabilityType == LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.DemandReset, "Mapper's Capability type is not correct");
            Assert.IsTrue(mappedDRCpblty.CapabilityType == CapabilityTypeTO.DemandReset, "Mapped Capability type is not correct");
            Assert.IsTrue(mappedDRCpblty.Frequency == freq, "Mapped DailySnap capability does not have proper mapping of Frequency");
            Assert.IsTrue(mappedDRCpblty.Capacity == capacity, "Mapped DailySnap capability does not have proper mapping of Capacity");
            Assert.IsTrue(mappedDRCpblty.SupportsMultipleBillingDates == supportsMultipleBillingDates, "Mapped DailySnap capability does not have proper mapping of SupportsMultipleBillingDates");
            Assert.IsTrue(mappedDRCpblty.SupportsRecursiveBillingDate == supportsRecursiveBillingDate, "Mapped DailySnap capability does not have proper mapping of SupportsRecursiveBillingDate");
            Assert.IsNotNull(mappedDRCpblty.Registers, "Mapped DailySnap capability should have mapped Registers");
            Assert.IsTrue(mappedDRCpblty.Registers.Count == 1, "Mapped DailySnap capability should have 1 mapped Registers");
            Assert.IsTrue(mappedDRCpblty.Registers[0].Identifier == register.Identifier, "Mapped DailySnap Identifier should be same as entity");
            Assert.IsTrue(mappedDRCpblty.Registers[0].DataType == register.DataType, "Mapped DailySnap Datatype should be same as entity");
        }

        [TestMethod]
        [Description(@"This test method is to check FirmwareDownloadCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadCapabilityMapper()
        {
            string assertMsg = "FirmwareDownloadCapability Mapper, {0} property's Current value :{1} and Expected value :{2} mismatch.";
            //Expected attribute values of FirmwareDownloadParameter
            FirmwareType expectedFirmwareType = FirmwareType.Meter;
            string expectedFirmwarePlatform = "FP0001";
            string expectedMinVersion = "11.11.111.111";
            bool expectedIsDowngradeable = true;
            bool expectedCanPause = true;
            bool expectedCanAbort = true;
            bool expectedCanChangeActivationDatetime = true;
            bool expectedIsDelayedActivationSupported = false;
            bool expectedCanRollback = true;
            FirmwareDownloadParameter firmwareDownloadParameter = new FirmwareDownloadParameter(expectedFirmwareType, expectedFirmwarePlatform, expectedMinVersion, expectedIsDowngradeable,
                                                                                                expectedCanPause, expectedCanAbort, expectedCanChangeActivationDatetime, expectedIsDelayedActivationSupported, expectedCanRollback);
            Dictionary<string, FirmwareDownloadParameter> firmwareDownloadParameterSet = new Dictionary<string, FirmwareDownloadParameter>();
            firmwareDownloadParameterSet.Add(firmwareDownloadParameter.FirmwareType.ToString(), firmwareDownloadParameter);

            FirmwareDownloadCapability firmwareDownloadCapability = new FirmwareDownloadCapability();
            firmwareDownloadCapability.FirmwareDownloadParameters = new ReadOnlyDictionary<string, FirmwareDownloadParameter>(firmwareDownloadParameterSet);

            ICapabilityMapper mapper = new FirmwareDownloadCapabilityMapper();

            FirmwareDownloadCapabilityTO firmwareDownloadCapabilityMapper = (FirmwareDownloadCapabilityTO)mapper.GetMappedCapability(firmwareDownloadCapability);
            FirmwareDownloadParameterTO firmwareDownloadParameterTO = firmwareDownloadCapabilityMapper.FirmwareDownloadParameters[0];

            Assert.IsTrue(firmwareDownloadCapabilityMapper.CapabilityType == CapabilityTypeTO.FirmwareDownload, "FirmwareDownloadCapabilityMapper -> Capabilitytype expected value mismatch.");
            //Current attribute values of FirmwareDownloadParameter
            FirmwareType currentFirmwareType =(FirmwareType)Enum.Parse(typeof(FirmwareType), firmwareDownloadParameterTO.FirmwareType.ToString());
            string currentFirmwarePlatform = firmwareDownloadParameterTO.FirmwarePlatform;
            string currentMinVersion = firmwareDownloadParameterTO.MinVersion;
            bool currentIsDowngradeable = firmwareDownloadParameterTO.IsDowngradeable;
            bool currentCanPause = firmwareDownloadParameterTO.CanPause;
            bool currentCanAbort = firmwareDownloadParameterTO.CanAbort;
            bool currentCanChangeActivationDatetime = firmwareDownloadParameterTO.CanChangeActivationDatetime;
            bool currentIsDelayedActivationSupported = firmwareDownloadParameterTO.IsDelayedActivationSupported;
            bool currentCanRollback = firmwareDownloadParameterTO.CanRollback;

            //Verifying FirmwareDownloadParameter
            Assert.IsTrue(currentFirmwareType == expectedFirmwareType, string.Format(assertMsg, "FirmwareType", currentFirmwareType.ToString(), expectedFirmwareType.ToString()));
            Assert.IsTrue(currentFirmwarePlatform == expectedFirmwarePlatform, string.Format(assertMsg, "FirmwarePlatform", currentFirmwarePlatform, expectedFirmwareType));
            Assert.IsTrue(currentMinVersion == expectedMinVersion, string.Format(assertMsg, "MinVersion", currentMinVersion, expectedMinVersion));
            Assert.IsTrue(currentIsDowngradeable == expectedIsDowngradeable, string.Format(assertMsg, "IsDowngradeable", currentIsDowngradeable, expectedIsDowngradeable));
            Assert.IsTrue(currentCanPause == expectedCanPause, string.Format(assertMsg, "CanPause", currentCanPause, expectedCanPause));
            Assert.IsTrue(currentCanAbort == expectedCanAbort, string.Format(assertMsg, "CanAbort", currentCanAbort, expectedCanAbort));
            Assert.IsTrue(currentCanChangeActivationDatetime == expectedCanChangeActivationDatetime, string.Format(assertMsg, "CanChangeActivationDatetime", currentCanChangeActivationDatetime, expectedCanChangeActivationDatetime));
            Assert.IsTrue(currentIsDelayedActivationSupported == expectedIsDelayedActivationSupported, string.Format(assertMsg, "IsDelayedActivationSupported", currentIsDelayedActivationSupported, expectedIsDelayedActivationSupported));
            Assert.IsTrue(currentCanRollback == expectedCanRollback, string.Format(assertMsg, "CanRollback", currentCanRollback, expectedCanRollback));
        }


        [TestMethod]
        [Description(@"This test method is to check GridstreamBroadcastCapability mapping to its corresponding DTO")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Entities, TargetCapabilityCategory.Others, TargetCapabilityType.GridstreamBroadcast)]
        public void TestGridstreamBroadcastMapper()
        {
            string identifier = "1.2.3.4.5";
            string expectedParameterVersion = "V1";

            Command command = new Command(identifier);
            Dictionary<string, Command> commands = new Dictionary<string, Command>();
            commands.Add(identifier, command);

            GridstreamBroadcastCapability capability = new GridstreamBroadcastCapability();
            capability.ParameterVersion = expectedParameterVersion;
            capability.Commands = new ReadOnlyDictionary<string, Command>(commands);

            ICapabilityMapper mapper = new GridstreamBroadcastCapabilityMapper();
            GridstreamBroadcastCapabilityTO capabilityTO = (GridstreamBroadcastCapabilityTO)mapper.GetMappedCapability(capability);

            string currentParameterVersion = capabilityTO.ParameterVersion;

            Assert.IsTrue(currentParameterVersion == expectedParameterVersion, string.Format("GridstreamBroadcastCapabilityTO ,parameterversion current {0} and expected {1} mismatch.", currentParameterVersion, expectedParameterVersion));
            Assert.IsTrue(capabilityTO.Commands.Count == 1, "GridstreamBroadcastCapabilityTO Commands are not correct.");
            Assert.IsTrue(capabilityTO.Commands.Find(x => x.Identifier == identifier).Identifier == identifier, string.Format("GridstreamBroadcastCapabilityTO Commands with Identifier {0} is not correct.", identifier));
        }
    }
}
