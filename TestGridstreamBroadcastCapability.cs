using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DD = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestGridstreamBroadcastCapability
    {
        [TestMethod]
        [Description("This Test is to verify default values in GridstreamBroadcast Capability.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.GridstreamBroadcast)]
        public void TestDefaultValuesOfGridstreamBroadcastCapability()
        {
            string expectedParameterVersion = "V1";
            string currentParameterVersion = string.Empty;

            //Create Capability Instance
            GridstreamBroadcastCapability capability = new GridstreamBroadcastCapability();
            capability.ParameterVersion = expectedParameterVersion;

            //Get Current ParameterVersion
            currentParameterVersion = capability.ParameterVersion;

            //Create Abstract Factory Instance
            GridstreamBroadcastCapabilityAbstractFactory capabilityAbstractFactory = new GridstreamBroadcastCapabilityAbstractFactory();
                       
            //Verifying Default Values of GridStreamBroadCastCapability
            Assert.IsTrue(capability.IsConfigurationDependent == false, "Default value of IsConfigurationDependent property is not correct.");
            Assert.IsTrue(currentParameterVersion == expectedParameterVersion, string.Format("Parameter version current value :{0} , expected value:{1} mismatch.", currentParameterVersion, expectedParameterVersion));
            Assert.IsTrue(capability.CapabilityType == DD.CapabilityType.GridstreamBroadcast, "Default value of CapabilityType property is not correct.");

            //Verifying Default Values of GridStreamBroadCastCapabilityAbstractFactory
            Assert.IsTrue(capabilityAbstractFactory.CapabilityHandler.GetType() == typeof(GridstreamBroadcastCapabilityHandler), "Capability handler Type in GridstreamBroadcast Abstract Factory is not correct.");
            Assert.IsTrue(capabilityAbstractFactory.CapabilityBuilder.GetType() == typeof(GridstreamBroadcastCapabilityBuilder), "Capability builder Type in GridstreamBroadcast Abstract Factory is not correct.");
        }

        [TestMethod]
        [Description(@"This test is to verify IElementsBasedCapability to GridstreamBroadcastCapability and GridstreamBroadcastCapability to IElementsBasedCapability Conversion.")]
        [Owner(@"email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.GridstreamBroadcast)]
        public void GridstreamBroadcastCapabilitytoIElementsBasedCapabilityConversion()
        {
            string identifier1 = "1.2.3.4.5";
            string identifier2 = "1.2.3.4.9";

            //Create Commands
            Command expectedCommand1 = new Command(identifier1);
            Command expectedCommand2 = new Command(identifier2);

            string parameterVersion = "V1";

            //Create capability view elements
            Dictionary<string, ICapabilityElement> expectedElements = new Dictionary<string, ICapabilityElement>();
            expectedElements.Add(identifier1, expectedCommand1);
            expectedElements.Add(identifier2, expectedCommand2);

            //Create capability
            GridstreamBroadcastCapability capability = new GridstreamBroadcastCapability();
            capability.ParameterVersion = parameterVersion;

            //Set capability commands from capability view
            ((IElementsBasedCapability)capability).Elements = expectedElements;

            //Get capability commands 
            ReadOnlyDictionary<string, Command> currentCapabilityCommands = capability.Commands;

            //Verify capability commands 
            Assert.IsTrue(currentCapabilityCommands[identifier1].Equals(expectedCommand1), "IElementsBasedCapability to capability conversion failed, command1 mismatch with expected.");
            Assert.IsTrue(currentCapabilityCommands[identifier2].Equals(expectedCommand2), "IElementsBasedCapability to capability conversion failed, command2 mismatch with expected.");

            //Get capability view elemments from capability commands
            Dictionary<string, ICapabilityElement> currentElements = ((IElementsBasedCapability)capability).Elements;

            //Verify capability view elements
            Assert.IsTrue(currentElements[identifier1].Equals(expectedCommand1), "GridstreamBroadcast Capability  to IElementsBasedCapability conversion failed, command1 mismatch with expected.");
            Assert.IsTrue(currentElements[identifier2].Equals(expectedCommand2), "GridstreamBroadcast Capability  to IElementsBasedCapability conversion failed, command2 mismatch with expected.");
        }

        [TestMethod]
        [Description(@"This test is to verify different capability type can not be merged with GridstreamBroadcastCapability.")]
        [Owner(@"email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.GridstreamBroadcast)]
        public void MergeDiffCapabilityTypeWithGridstreamBroadcastCapability()
        {
            bool isMerged = false;
            bool isHandledException = false;
            string errorMsg = string.Empty;

            //Declare Identifiers of commands
            string[] identifierArray = { "1.2.3.4.5", "1.2.3.4.6" };

            //Create  Capability
            GridstreamBroadcastCapability capability = GetGridstreamBroadcastCapability(identifierArray);
            CommandSetCapability commandSetCapability = null;

            try
            {
                //Merge Different capabilities
                commandSetCapability = (CommandSetCapability)capability.Merge(commandSetCapability);
                isMerged = true;
            }
            //To catch handled erros
            catch (ApplicationException ex)
            {
                isHandledException = true;
                errorMsg = ex.Message;
                isMerged = false;
            }
            //To catch unhandled erros
            catch (Exception ex1)
            {
                isHandledException = false;
                errorMsg = ex1.Message;
                isMerged = false;
            }

            Assert.IsTrue(isHandledException, string.Format("Unhandled merging Exception Message is :{0}", errorMsg));
            Assert.IsFalse(isMerged, "Diff. capability type merged with gridstream broadcast capability, check gridstream broadcast capability merging code.");
        }

        [TestMethod]
        [Description(@"This test is to verify the merging of two GridstreamBroadcastCapabilities")]
        [Owner(@"email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.GridstreamBroadcast)]
        public void MergingGridstreamBroadcastCapability()
        {
            bool isMerged = true;
            bool? isHandledException = default(bool?);
            string errorMsg = string.Empty;

            //Define identifiers of commands
            string identifier1 = "1.2.3.4.5";
            string identifier2 = "2.3.4.5.6";
            string identifier3 = "3.4.5.6.7";

            //Create capability commands
            Command expectedCommand1 = new Command(identifier1);
            Command expectedCommand2 = new Command(identifier2);
            Command expectedCommand3 = new Command(identifier3);

            //Create capability View elements
            Dictionary<string, Command> elementsOfCapability1 = new Dictionary<string, Command>();
            elementsOfCapability1.Add(identifier1, expectedCommand1);
            elementsOfCapability1.Add(identifier2, expectedCommand2);

            //Set capability commands from view elements
            GridstreamBroadcastCapability capability1 = new GridstreamBroadcastCapability();
            capability1.Commands = new ReadOnlyDictionary<string, Command>(elementsOfCapability1);

            //Create capability view elements
            Dictionary<string, Command> elementsOfCapability2 = new Dictionary<string, Command>();
            elementsOfCapability2.Add(identifier2, expectedCommand2);
            elementsOfCapability2.Add(identifier3, expectedCommand3);

            //Set capability commands from view elements
            GridstreamBroadcastCapability capability2 = new GridstreamBroadcastCapability();
            capability2.Commands = new ReadOnlyDictionary<string, Command>(elementsOfCapability2);

            GridstreamBroadcastCapability mergedCapability = null;
            ReadOnlyDictionary<string, Command> elementsOfMergedCapability = null;
            try
            {
                //Merge capabilities
                mergedCapability = (GridstreamBroadcastCapability)capability2.Merge(capability1);
                elementsOfMergedCapability = mergedCapability.Commands;
                isMerged = true;
            }
            catch (ApplicationException ex1)
            {
                isHandledException = true;
                isMerged = false;
                errorMsg = ex1.Message;
            }
            catch (Exception ex1)
            {
                isHandledException = false;
                isMerged = false;
                errorMsg = ex1.Message;
            }
            //Get merged capability commands
            Command currentCommand1 = elementsOfMergedCapability[identifier1];
            Command currentCommand2 = elementsOfMergedCapability[identifier2];
            Command currentCommand3 = elementsOfMergedCapability[identifier3];

            Assert.IsFalse(isHandledException == false, string.Format("Unhandled merging Exception Message is :{0}", errorMsg));
            Assert.IsTrue(isMerged, errorMsg);
            Assert.IsTrue(currentCommand1 == expectedCommand1, "Merged GridstreamBroadcast capability Command1 should be same as first GridstreamBroadcast");
            Assert.IsTrue(currentCommand2 == elementsOfCapability1[identifier2], "Merged GridstreamBroadcast capability Command2 should be same as first GridstreamBroadcast");
            Assert.IsTrue(currentCommand2 == elementsOfCapability2[identifier2], "Merged GridstreamBroadcast capability Command1 should be same as second GridstreamBroadcast");
            Assert.IsTrue(currentCommand3 == expectedCommand3, "Merged GridstreamBroadcast capability Command3 should be same as second GridstreamBroadcast");

        }

        [TestMethod]
        [Description("This test is to verify GridstreamBroadcastCapability Build from XML.")]
        [Owner("email:platfrom@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestGridstreamBroadcastCapabilityBuildFromXML()
        {
            bool isCapabilityBuildOk = true;
            bool? isHandledException = default(bool?);
            string expectedParameterVersion = "V1";
            int expectedCommandsCount = 10;
            string errorMsg = string.Empty;

            #region GridstreamBroadcastCapability Builder XML

            string capabilityxml = string.Format(@"<Capabilities>
                                     <GridstreamBroadcast parameterVersion='{0}'>
                                         <SupportedCommands>
                                              <command parameterVersion='v1' lgCommandType='1.2.3.4.[0,1,5-10]' /> 
                                              <command parameterVersion='v1' lgCommandType='1.2.3.4.[3,4]' /> 
                                          </SupportedCommands>
                                     </GridstreamBroadcast>
                                    </Capabilities>
                                    ", expectedParameterVersion);

            #endregion

            GridstreamBroadcastCapability capability = null;
            CapabilityBuilder capabilityBuilder = new GridstreamBroadcastCapabilityBuilder();

            try
            {
                capability = (GridstreamBroadcastCapability)capabilityBuilder.BuildCapability(capabilityxml);
                isCapabilityBuildOk = true;
            }
            catch (ApplicationException ex1)
            {
                isHandledException = true;
                isCapabilityBuildOk = false;
                errorMsg = ex1.Message;            
            }
            catch (Exception ex1)
            {
                isHandledException = true;
                isCapabilityBuildOk = false;
                errorMsg = ex1.Message;  
            }
            Assert.IsFalse(isHandledException == false, string.Format("Unhandled builder Exception Message is :{0}", errorMsg));
            Assert.IsTrue(isCapabilityBuildOk, errorMsg);
            Assert.IsTrue(capability != null, "Capability build wrong by Builder.");

            string currentParameterVersion = capability.ParameterVersion;
            int currentCommandsCount = capability.Commands.Count;

            Assert.IsTrue(isCapabilityBuildOk, "capability build failed in GridStreamBroadCastCapabilityBuilder.");
            Assert.IsTrue(currentCommandsCount == expectedCommandsCount, string.Format("current commands count :{0} and expected :{1} in capability build by builder mismatch.", currentCommandsCount, expectedCommandsCount));
            Assert.IsTrue(currentParameterVersion == expectedParameterVersion, string.Format("ParameterVersion current value:{0} and expected value:{1} in capability build by builder mismatch.", currentParameterVersion, expectedParameterVersion));
        }

        [TestMethod]
        [Description("This test is to verify GridStreamBroadCastCapability Build failed when unexpected XML attributes passed.")]
        [Owner("email:platfrom@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestGridStreamBroadCastCapabilityBuildFromUnexpectedXMLAttrb()
        {
            Boolean isCapabilityBuildOk = true;
            bool? isHandledException = default(bool?);
            string errorMsg = string.Empty;
            #region GridStreamBroadCastCapability Builder XML

            string capabilityxml = @"<Capabilities>
                                     <GridstreamBroadcast parameterVersion='V1' extraAttrb='1'>
                                         <SupportedCommands>
                                               <command parameterVersion='v1' lgCommandType='1.2.3.4.[0,1,5-10]' /> 
                                              <command parameterVersion='v1' lgCommandType='1.2.3.4.[3,7]' /> 
                                          </SupportedCommands>
                                     </GridstreamBroadcast>
                                    </Capabilities>
                                    ";
            #endregion

            GridstreamBroadcastCapability capability = null;
            CapabilityBuilder capabilityBuilder = new GridstreamBroadcastCapabilityBuilder();

            try
            {
                capability = (GridstreamBroadcastCapability)capabilityBuilder.BuildCapability(capabilityxml);
                isCapabilityBuildOk = true;
            }
            catch (ApplicationException ex1)
            {
                isHandledException = true;
                isCapabilityBuildOk = false;
                errorMsg = ex1.Message; 
            }
            catch (Exception ex1)
            {
                isHandledException = false;
                isCapabilityBuildOk = false;
                errorMsg = ex1.Message; 
            }
            Assert.IsFalse(isHandledException == false, string.Format("Unhandled builder Exception Message is :{0}", errorMsg));
            Assert.IsFalse(isCapabilityBuildOk, "GridStreamBroadCastCapabilityBuilder has been built with unexpected Attributes,Check Builder..");
        }

        [TestMethod]
        [Description("This test is to verify GridStreamBroadCastCapabilityHandler Create/Load Capability")]
        [Owner("email:platfrom@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Integration, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestCreateCapabilityandLoadIfAlreadyExist()
        {
            DBHelper.CleanDatabase();

            string identifier1 = "1.2.3.4.5";
            bool isCapabilityCreated = false;

            //Define identifiers
            string[] identifierArray = { identifier1 };

            //Create capability and its view
            GridstreamBroadcastCapability capability1 = GetGridstreamBroadcastCapability(identifierArray);
            ICapabilityElement capabilityElementsOfCapability1 = (ICapabilityElement)capability1.Commands[identifier1];

            //Create handler and CRC
            GridstreamBroadcastCapabilityHandler handler = new GridstreamBroadcastCapabilityHandler();
            CapabilityCrcComputer crcComputer = new CapabilityCrcComputer();
            string capabilityCrc = crcComputer.ComputeCrc(capability1);

            //Save capability in DB
            isCapabilityCreated = handler.CreateCapabilityIfNotExists(capability1, DD.CapabilitySource.Device, capabilityCrc);

            Assert.IsTrue(isCapabilityCreated, "GridStreamBroadCastCapability not created , Clean Database and retry.");

            //Fetch capability
            GridstreamBroadcastCapability capability2 = (GridstreamBroadcastCapability)handler.LoadCapability(capabilityCrc);
            ICapabilityElement capabilityElementsOfCapability2 = (ICapabilityElement)capability2.Commands[identifier1];

            Assert.IsTrue(capability2.IsConfigurationDependent == capability1.IsConfigurationDependent, "Fetched Capability's IsConfigurationDependent property mismatch.");
            Assert.IsTrue(capability2.CapabilityType == capability1.CapabilityType, "Fetched Capability's CapabilityType property mismatch.");
            Assert.IsTrue(capabilityElementsOfCapability1.Identifier == capabilityElementsOfCapability2.Identifier, "Fetched Capability's Command Identifier mismatch.");
           //Validate Details
            Assert.IsTrue(capabilityElementsOfCapability1.Details.Length == capabilityElementsOfCapability2.Details.Length, "Fetched Capability's Command Details mismatch.");
        }

        /// <summary>
        ///  Get Gridstream Broadcast Capability
        /// </summary>
        /// <param name="commandIdentifiers">Identifiers of command</param>
        /// <returns>GridstreamBroadcastCapability</returns>
        private GridstreamBroadcastCapability GetGridstreamBroadcastCapability(string[] commandIdentifiers)
        {
            Dictionary<string, Command> elements = new Dictionary<string, Command>();
            Command command;
            foreach (string identifier in commandIdentifiers)
            {
                command = new Command(identifier, "v1");
                elements.Add(command.Identifier, command);
            }
            GridstreamBroadcastCapability gridStreamBroadCastCapability = new GridstreamBroadcastCapability();
            ((IElementsBasedCapability)gridStreamBroadCastCapability).Elements = elements.ToDictionary(x => x.Key, x => (ICapabilityElement)x.Value);
            return gridStreamBroadCastCapability;
        }

    }
}