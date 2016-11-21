using System;
using System.Diagnostics.CodeAnalysis;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public partial class TestConfiguredCapability
    {
        #region Capability XML
        private String modelCapabilitiesXML = @"<model name = 'TEPCO_6N_200'>
                                   <attributes>
                                        <isBigEndian>True</isBigEndian>    
                                        <targetMarket>Residential</targetMarket> 
                                        <isMultipleUtilityCapable>True</isMultipleUtilityCapable>
                                        <type>ElectricMeter</type>
                                        <isFirmwareReplacable>True</isFirmwareReplacable>
                                        <supportReadingsParameterReprogramming>False</supportReadingsParameterReprogramming>
                                        <addressingMechanism>IP</addressingMechanism>
                                        <isGridstream>True</isGridstream>
                                        <isDSTCapable>True</isDSTCapable>
                                   </attributes>
                                    <capabilities>
                                               <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.[0-9]' datatype='INT'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' datatype='INT' />
                                                </readingTypes>
                                      </capabilities></model>";

        #endregion

        [TestMethod]
        [Description(@"This test method will verify that the Build method of the configured capability builder will always return false")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestConfiguredCapabilityBuilderBuildOperation()
        {
            LoadProfileCapabilityAbstractFactory loadProfileAbstractFactory = new LoadProfileCapabilityAbstractFactory();
            string errorMessage = String.Empty;
            CapabilityBase capabilityDetails;

            CapabilityBuilder builder = loadProfileAbstractFactory.CapabilityBuilder;
            capabilityDetails = builder.BuildCapability(modelCapabilitiesXML);

            Assert.IsNull(capabilityDetails);
        }

        [TestMethod]
        [Description(@"This test method will verify that the IsSupportedByDeviceModel method of a configured capability builder will always return false")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestConfiguredCapabilityBuilderIsSupportedByDeviceModelOperation()
        {
            LoadProfileCapabilityAbstractFactory loadProfileAbstractFactory = new LoadProfileCapabilityAbstractFactory();
            string errorMessage = String.Empty;
            bool isSupported = true; ;

            CapabilityBuilder builder = loadProfileAbstractFactory.CapabilityBuilder;

            isSupported = builder.IsSupportedByDeviceModel(modelCapabilitiesXML);

            Assert.IsFalse(isSupported);
        }
    }
}
