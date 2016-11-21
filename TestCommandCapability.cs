using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCommandCapability
    {
        [TestMethod]
        [Description(@"This test method is used to check the defaults of the CommandSet Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestDefaultValuesOfCommandSetCapability()
        {
            CapabilityBase commandSetCapability = new CommandSetCapability();
            Assert.AreEqual(commandSetCapability.CapabilityType, LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Commands);
            Assert.AreEqual(commandSetCapability.IsConfigurationDependent, true);
        }

        [TestMethod]
        [Description(@"This test method is used to check exception is thrown if unsupported capabilities are being merged to Command Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        [ExcludeFromCodeCoverageAttribute]
        public void MergeDiffTypeCapabilityIntoCommandCapability()
        {
            bool capabiltiesNotOfSameType = true;

            CapabilityBase commandSetCapability = new CommandSetCapability();
            ((IElementsBasedCapability)commandSetCapability).Elements = new Dictionary<string,ICapabilityElement>();

            MockRegistersCapability mockCapability = new MockRegistersCapability(null);

            try
            {
                commandSetCapability.Merge(new CapabilityBase[] { mockCapability });
                capabiltiesNotOfSameType = false;
            }
            catch (ApplicationException)
            {
                capabiltiesNotOfSameType = true;
            }

            Assert.IsTrue(capabiltiesNotOfSameType, "Expected ApplicationException but because of Capabilities being merged are of same type, so no exception.");
        }
        
        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability with common commands being reused")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        public void MergeTwoCommandsCapabilityWithCommonCommands()
        {
            Command command1 = new Command("1");
            Command command2 = new Command("2");
            Command command3 = new Command("3");

            Dictionary<string, ICapabilityElement> commands = new Dictionary<string, ICapabilityElement>();
            commands.Add(command1.Identifier, command1);
            commands.Add(command2.Identifier, command2);

            IElementsBasedCapability commandSetCapability1 = new CommandSetCapability();
            commandSetCapability1.Elements = commands;


            Dictionary<string, ICapabilityElement> commands2 = new Dictionary<string, ICapabilityElement>();
            commands2.Add(command2.Identifier, command2);
            commands2.Add(command3.Identifier, command3);

            IElementsBasedCapability commandSetCapability2 = new CommandSetCapability();
            commandSetCapability2.Elements = commands2;

            CommandSetCapability mergedCommandCapability = (CommandSetCapability)((CapabilityBase)commandSetCapability1).Merge(new CapabilityBase[] { (CommandSetCapability)commandSetCapability2 });

            Assert.IsTrue(mergedCommandCapability.Commands.Count == 3, "Merged Command capability should contain Command 1, 2, 3 only");
            Assert.IsTrue(mergedCommandCapability.Commands[command1.Identifier] == ((CommandSetCapability)commandSetCapability1).Commands[command1.Identifier], "Merged Command capability's Command1 should be same as first CommandCapability");
            Assert.IsTrue(mergedCommandCapability.Commands[command3.Identifier] == ((CommandSetCapability)commandSetCapability2).Commands[command3.Identifier], "Merged Command capability's Command3 should be same as second CommandCapability");

            Assert.IsTrue(mergedCommandCapability.Commands[command2.Identifier] == ((CommandSetCapability)commandSetCapability1).Commands[command2.Identifier], "Merged Command capability Command2 should be same as first CommandCapability");
            Assert.IsTrue(mergedCommandCapability.Commands[command2.Identifier] == ((CommandSetCapability)commandSetCapability2).Commands[command2.Identifier], "Merged Command capability Command2 should be same as second CommandCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability without any common commands")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        public void MergeTwoCommandsCapabilityWithoutCommonCommands()
        {
            Command command1 = new Command("1");
            Command command2 = new Command("2");
            Command command3 = new Command("3");
            Command command4 = new Command("4");

            Dictionary<string, ICapabilityElement> commands = new Dictionary<string, ICapabilityElement>();
            commands.Add(command1.Identifier, command1);
            commands.Add(command2.Identifier, command2);

            IElementsBasedCapability commandSetCapability1 = new CommandSetCapability();
            commandSetCapability1.Elements = commands;


            Dictionary<string, ICapabilityElement> commands2 = new Dictionary<string, ICapabilityElement>();
            commands2.Add(command3.Identifier, command3);
            commands2.Add(command4.Identifier, command4);

            IElementsBasedCapability commandSetCapability2 = new CommandSetCapability();
            commandSetCapability2.Elements = commands2;

            CommandSetCapability mergedCommandCapability = (CommandSetCapability)((CapabilityBase)commandSetCapability1).Merge(new CapabilityBase[] { (CommandSetCapability)commandSetCapability2 });

            Assert.IsTrue(mergedCommandCapability.Commands.Count == 4, "Merged Command capability should contain Command 1, 2, 3, 4");
            Assert.IsTrue(mergedCommandCapability.Commands[command1.Identifier] == ((CommandSetCapability)commandSetCapability1).Commands[command1.Identifier], "Merged Command capability's Command1 should be same as first CommandCapability");
            Assert.IsTrue(mergedCommandCapability.Commands[command2.Identifier] == ((CommandSetCapability)commandSetCapability1).Commands[command2.Identifier], "Merged Command capability Command2 should be same as first CommandCapability");

            Assert.IsTrue(mergedCommandCapability.Commands[command3.Identifier] == ((CommandSetCapability)commandSetCapability2).Commands[command3.Identifier], "Merged Command capability's Command3 should be same as second CommandCapability");
            Assert.IsTrue(mergedCommandCapability.Commands[command4.Identifier] == ((CommandSetCapability)commandSetCapability2).Commands[command4.Identifier], "Merged Command capability Command4 should be same as second CommandCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to validate unsupported attributes fails command parsing")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseCommandCapabilitiesXMLWithExtraAttrib()
        {
            #region Capability XML

            string commandsCapabilityXml = @"<Capabilities>
                                               <commands>
                                                    <command parameterVersion='V1' lgCommandType='1.2.3.4.5.6.7.8.9.10.[0-9]' isGap='True'/>
                                                    <command parameterVersion='V2' lgCommandType='1.2.3.4.5.6.7.8.9.10.11' />
                                                </commands>
                                              </Capabilities>";

            #endregion

            CommandSetCapabilityBuilder registerCapabilityBuilder = new CommandSetCapabilityBuilder();
            bool isFailed = false;

            try
            {
                registerCapabilityBuilder.BuildCapability(commandsCapabilityXml);
            }
            catch (ApplicationException)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "Command capability XML does not contain element with extra attribute");
        }
        
        [TestMethod]
        [Description(@"This test method is used to validate that Command Handler loads the specified Commands capability properly")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        public void CreateAndLoadCommandSetCapability()
        {
            DBHelper.CleanDatabase();
            string capabilityCrc = "456";

            CapabilityBase commandSetCapability = new CommandSetCapability();
            Dictionary<string, ICapabilityElement> commands = new Dictionary<string, ICapabilityElement>();
            commands.Add("1", new Command("1", "v1"));
            ((IElementsBasedCapability)commandSetCapability).Elements = commands;

            CommandSetCapabilityHandler commandCapabilityHandler = new CommandSetCapabilityHandler();

            bool capabilityCreated = commandCapabilityHandler.CreateCapabilityIfNotExists(commandSetCapability, Definitions.CapabilitySource.Device, capabilityCrc);

            Assert.IsTrue(capabilityCreated, "New Capability not created because it already exists, clean database and retry");

            CapabilityBase capability = commandCapabilityHandler.LoadCapability(capabilityCrc);

            Assert.IsNotNull(capability as CommandSetCapability);

            Assert.IsTrue(((CommandSetCapability)capability).Commands.Count > 0);
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Command capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Commands)]
        public void ValidateCommandCapabilitySerialization()
        {
            CapabilityBase commandCap = new CommandSetCapability();
            Dictionary<string, ICapabilityElement> elements = new Dictionary<string, ICapabilityElement>();
            elements.Add("1.2.3.4.5.6.7.8.9.10.11", new Command("1.2.3.4.5.6.7.8.9.10.11"));

            ((IElementsBasedCapability)commandCap).Elements = elements;
            
            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(commandCap.GetType());
            serializer.WriteObject(fs, commandCap);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            //this Assert is to confirm that the main NodeName for the Capability is present i.e. CommandSetCapability.SortedCommandsForCrcComputer
            Assert.IsTrue(txt.Contains("</Commands>"), "Required Commands node missing");

            //following Assert verify that Property of Command instance is serialized
            Assert.IsTrue(txt.Contains("<Identifier>"), "Required Identifier node of Command instance missing");
        }
    }
}
