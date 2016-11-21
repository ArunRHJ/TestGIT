using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LandisGyr.AMI.Devices.Capabilities.CapabilityStoreImplementation;
using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceModelCapabilityProcessor
    {
        private string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

        #region Input Capability XML

        #region Valid XML TEPCO_6N_200
        String validInputXML_Tepco_6n_200 = @"<model name = 'TEPCO_6N_200'>
                                   <attributes>
                                        <isBigEndian>True</isBigEndian>    
                                        <targetMarket>Residential</targetMarket> 
                                        <isMultipleUtilityCapable>True</isMultipleUtilityCapable>
                                        <type>ElectricMeter</type>
                                        <isFirmwareReplacable>True</isFirmwareReplacable>
                                        <supportReadingsParameterReprogramming>True</supportReadingsParameterReprogramming>
                                        <addressingMechanism>IP</addressingMechanism>
                                        <isGridstream>True</isGridstream>
                                        <isDSTCapable>True</isDSTCapable>
                                   </attributes>
                                    <capabilities>
                                        <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.12' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                        </readingTypes>
                                        <commands>
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.5' />
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.6' />
                                        </commands>
                                      </capabilities></model>";

        String validInputXML_Tepco_6n_200_modifiedRegs = @"<model name = 'TEPCO_6N_200'>
                                   <attributes>
                                        <isBigEndian>True</isBigEndian>    
                                        <targetMarket>Residential</targetMarket> 
                                        <isMultipleUtilityCapable>True</isMultipleUtilityCapable>
                                        <type>ElectricMeter</type>
                                        <isFirmwareReplacable>True</isFirmwareReplacable>
                                        <supportReadingsParameterReprogramming>True</supportReadingsParameterReprogramming>
                                        <addressingMechanism>IP</addressingMechanism>
                                        <isGridstream>True</isGridstream>
                                        <isDSTCapable>True</isDSTCapable>
                                   </attributes>
                                    <capabilities>
                                        <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.13' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                        </readingTypes>
                                        <commands>
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.5' />
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.6' />
                                        </commands>
                                      </capabilities></model>";
        #endregion

        #region Valid XML PDU_1.2
        String validInputXML_PDU_1_2 = @"<model name = 'PDU_1.2'>
                                   <attributes>
                                        <type>ElectricMeter</type>
                                   </attributes>
                                    <capabilities>
                                        <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.12' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                        </readingTypes>
                                        <commands>
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.5' />
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.6' />
                                        </commands>
                                      </capabilities></model>";
        #endregion

        #region Valid XML CommsTech_SBS
        String validInputXML_CommsTech_SBS = @"<model name = 'CommsTech_SBS'>
                                   <attributes>
                                        <type>ElectricMeter</type>
                                   </attributes>
                                    <capabilities>
                                         <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.12' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                        </readingTypes>
                                        <commands>
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.5' />
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.6' />
                                        </commands>
                                      </capabilities></model>";
        #endregion

        #region XML with Unsupported ModelAttributes
        String inputXmlWithInvalidModelAttribute = @"<model name = 'TEPCO_6N_200'>
                                   <attributes>
                                        <isBigEndian>True</isBigEndian>    
                                        <targetMarket>Residential</targetMarket> 
                                        <Version>True</Version> 
                                   </attributes>
                                    <capabilities>
                                         <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.12' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                        </readingTypes>
                                        <commands>
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.5' />
                                                    <command parameterVersion='v1' lgCommandType='1.2.3.4.6' />
                                        </commands>
                                      </capabilities></model>";
        #endregion


        #endregion

        #region Test Methods
        /// <summary>
        /// This test method verifies the process of extracting the model attributes from the capability XML file. 
        /// This method will validate the values of model attributes after processing are same as the input capability XML file
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies the process of extracting the model attributes from the capability XML file. 
                       This method will validate the values of model attributes after processing are same as the input capability XML file.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestModelCapabilityProcessorModelAttributesExtraction()
        {
            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(null, null);

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;
            object defaultHash = obj.Invoke("ExtractModelAttributesFromXml", bindingFlgs, validInputXML_Tepco_6n_200);

            Dictionary<ModelAttribute, string> modelAttribs = defaultHash as Dictionary<ModelAttribute, string>;

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.TargetMarket));
            Assert.AreEqual(TargetMarket.Residential.ToString(), modelAttribs[ModelAttribute.TargetMarket]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.IsBigEndian));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.IsBigEndian]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.IsFirmwareReplacable));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.IsFirmwareReplacable]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.IsMultipleUtilityCapable));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.IsMultipleUtilityCapable]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.SupportReadingsParameterReprogramming));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.SupportReadingsParameterReprogramming]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.Type));
            Assert.AreEqual(DeviceType.ElectricMeter.ToString(), modelAttribs[ModelAttribute.Type]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.AddressingMechanism));
            Assert.AreEqual(AddressingMechanism.IP.ToString(), modelAttribs[ModelAttribute.AddressingMechanism]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.IsGridstream));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.IsGridstream]);

            Assert.IsTrue(modelAttribs.ContainsKey(ModelAttribute.IsDSTCapable));
            Assert.AreEqual("True", modelAttribs[ModelAttribute.IsDSTCapable]);

        }

        /// <summary>
        /// This test method verifies that the dafault algorithm for computing the hash of capability group is SHA1.
        /// This test will fail if there will be change in the algorithm of calculating the hash for a capability group
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies that the dafault algorithm for computing the hash of capability group is SHA1.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestDefaultCapabilityGroupCRCCalculationAlgorithm()
        {
            byte[] capabiltiesCrcArray = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 };

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(null, null);
            Sha1CrcComputer crcComputer = new Sha1CrcComputer();

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;
            object defaultHash = obj.Invoke("ComputeSHA1HashCode", bindingFlgs, capabiltiesCrcArray);

            string sha1Hash = crcComputer.ComputeHash(capabiltiesCrcArray);

            Assert.IsTrue(defaultHash.ToString() == sha1Hash, @"Default CRC computation method for calculating the CRC of group 
                                                                capabilities is not using SHA1 algo for hash computation");
        }

        /// <summary>
        /// This test method verifies the process of extracting the details of capabilities of a device model from a XML file which is written in the 
        /// Capability Description language, the output of the process will be the capabilities and attributes of a device model in processed format
        /// This test will fail if the input XML doesn't abide by the rules of Capability Description language.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies the process of extracting the details of capabilities of a device model from a XML file which is written in the 
                       Capability Description language, the output of the process will be the capabilities and attributes of a model in processed format")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestDeviceModelCapabilitiesXMLProcessing()
        {
            IDeviceModelCapabilityStore modelCapabilityStore = new MockDeviceModelCapabilityStore();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(modelCapabilityStore, deviceCapabilityCatalogue);

            DeviceModel newDeviceModel = capabilitiesProcessor.Process(validInputXML_Tepco_6n_200, DD.CapabilitySource.Device);

            //verify model name
            Assert.AreEqual("TEPCO_6N_200", newDeviceModel.Name);

            //verify attributes
            Assert.IsTrue(newDeviceModel.IsBigEndian);
            Assert.IsTrue(newDeviceModel.IsMultiUtilityCapable);
            Assert.IsTrue(newDeviceModel.IsFirmwareReplaceable);
            Assert.IsTrue(newDeviceModel.SupportsReadingsParametersReprogramming);
            Assert.AreEqual(TargetMarket.Residential, newDeviceModel.TargetMarket);
            Assert.AreEqual(DeviceType.ElectricMeter, newDeviceModel.DeviceType);

            //verify Capabilities
            Assert.IsNotNull(newDeviceModel[DD.CapabilityType.Registers]);
            Assert.IsNotNull(newDeviceModel[DD.CapabilityType.Commands]);
        }

        /// <summary>
        /// This test method verifies the process of extracting the details of capabilities of a PDU model from a XML file which is written in the 
        /// Capability Description language, the output of the process will be the capabilities and attributes of a PDU model in processed format
        /// This test will fail if the input XML doesn't abide by the rules of Capability Description language.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies the process of extracting the details of capabilities of a PDU model from a XML file which is written in the 
                       Capability Description language, the output of the process will be the capabilities and attributes of a model in processed format")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestPDUModelCapabilitiesXMLProcessing()
        {
            IDeviceModelCapabilityStore modelCapabilityStore = new MockDeviceModelCapabilityStore();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(modelCapabilityStore, deviceCapabilityCatalogue);

            DeviceModel newPDUModel = capabilitiesProcessor.Process(validInputXML_PDU_1_2, DD.CapabilitySource.PDU);

            //verify model name
            Assert.AreEqual("PDU_1.2", newPDUModel.Name);

            //verify attributes
            Assert.AreEqual(DeviceType.ElectricMeter, newPDUModel.DeviceType);

            //verify Capabilities
            Assert.IsNotNull(newPDUModel[DD.CapabilityType.Registers]);
        }

        /// <summary>
        /// This test method verifies the process of extracting the details of capabilities of a Comms Tech model from a XML file which is written in the 
        /// Capability Description language, the output of the process will be the capabilities and attributes of a Comms Tech model in processed format
        /// This test will fail if the input XML doesn't abide by the rules of Capability Description language.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies the process of extracting the details of capabilities of a Comms Tech model from a XML file which is written in the 
                       Capability Description language, the output of the process will be the capabilities and attributes of a model in processed format")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCommsTechModelCapabilitiesXMLProcessing()
        {
            IDeviceModelCapabilityStore modelCapabilityStore = new MockDeviceModelCapabilityStore();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(modelCapabilityStore, deviceCapabilityCatalogue);

            DeviceModel newCommsTechModel = capabilitiesProcessor.Process(validInputXML_CommsTech_SBS, DD.CapabilitySource.CommunicationTechnology);

            //verify model name
            Assert.AreEqual("CommsTech_SBS", newCommsTechModel.Name);

            //verify attributes
            Assert.AreEqual(DeviceType.ElectricMeter, newCommsTechModel.DeviceType);

            //verify Capabilities
            Assert.IsNotNull(newCommsTechModel[DD.CapabilityType.Registers]);
        }

        /// <summary>
        /// This test method verifies that we are not supressing the exception which occurs in case of processing the capability XML with Unsupported Model attributes.
        /// This test will fail if the exception is supressed in the code.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies that we are not supressing the exception which occurs in case of processing the capability XML with Unsupported Model attributes")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestXMLWithUnsupportedModelAttributeProcessing()
        {
            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(null, null);

            try
            {
                DeviceModel newDeviceModel = capabilitiesProcessor.Process(inputXmlWithInvalidModelAttribute, DD.CapabilitySource.Device);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(NotSupportedException), ex.GetType());
            }
        }

        /// <summary>
        /// This test method verifies that when we try to process the capability XML file of same model twice, 
        /// the capabilities are processed and persisted in data store only once.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies that when we try to process the capability XML file of same model twice, 
        the capabilities are processed and persisted in data store only once.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestModelCapabilitiesXMLProcessingWhenSameModelAlreadyExists()
        {
            DBHelper.CleanDatabase();

            IDeviceModelCapabilityStore modelCapabilityStore = new DeviceModelCapabilityStore();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(modelCapabilityStore, deviceCapabilityCatalogue);

            DeviceModel firstInstanceOfModel = capabilitiesProcessor.Process(validInputXML_PDU_1_2, DD.CapabilitySource.PDU);

            DeviceModel secondInstanceOfModel = capabilitiesProcessor.Process(validInputXML_PDU_1_2, DD.CapabilitySource.PDU);

            Assert.IsTrue(modelCapabilityStore.ModelExists("PDU_1.2"));
        }

        
        /// <summary>
        /// This test method verifies the process of extracting the details of capabilities of a model from a XML file 
        /// and getting an exception as there already exists a model with same name but different 
        /// capabilities or attributes in the data store.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies the process of extracting the details of capabilities of a model from a XML file and getting an exception
        as there already exists a model with same name but different capabilities or attributes in the data store.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        [ExcludeFromCodeCoverage]
        public void TestProcessingOfCpbltyXMLOfTwoModelsWithSameNameAndDifferentCapabilities()
        {
            bool isExceptionRaised = false;

            DBHelper.CleanDatabase();

            IDeviceModelCapabilityStore modelCapabilityStore = new DeviceModelCapabilityStore();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            DeviceModelCapabilitiesProcessor capabilitiesProcessor = new DeviceModelCapabilitiesProcessor(modelCapabilityStore, deviceCapabilityCatalogue);

            DeviceModel firstInstanceOfModel = capabilitiesProcessor.Process(validInputXML_Tepco_6n_200, DD.CapabilitySource.Device);

            try
            {
                // Process the capability XML of a model same as first other than the modeified register capability
                DeviceModel secondInstanceOfModel = capabilitiesProcessor.Process(validInputXML_Tepco_6n_200_modifiedRegs, DD.CapabilitySource.Device);
            }
            catch
            {
                isExceptionRaised = true;
            }

            Assert.IsTrue(isExceptionRaised);
        }
        #endregion

       
    }
}



