using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceCapabilityCatalogue
    {
        /// <summary>
        /// This method will validate that all the offered device capabilities are loaded by Device Capability Catalogue.
        /// Along with the verification that offered capabilities count is greater than 0 that confirms that Device capability catalogue is loading assmeblies properly using MEF.
        /// Also verify the existence of one of the dummy capability in the loaded offered capabilities.
        /// </summary>
        /// <remarks>
        /// This test will fail if there are any breaking changes in the DeviceCapabilityCatalogue class.
        /// </remarks>
        [TestMethod]
        [Description(@"This test method verifies that all the offered device capabilities and their corresponding 
                       CapabilityAbstractFactories are loaded by Device Capability Catalogue.
                       If new capability assembly is added in the executing directory then it will be 
                       loaded by Device capability catalogue on it's re-initialization")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestDeviceCapabilityCatalogueForDummyCapability()
        {
            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

            ComposablePartCatalog catalog = new DirectoryCatalog(deviceCataloguePath);

            DeviceCapabilityCatalogue capabilityCatalogue = new DeviceCapabilityCatalogue(catalog);

            Assert.IsTrue(capabilityCatalogue.OfferedDeviceCapabilities.Length > 0);

            Assert.IsNotNull(capabilityCatalogue[CC.CapabilityType.Registers]);

            Assert.IsTrue(capabilityCatalogue[CC.CapabilityType.Registers].GetType() == typeof(MockRegistersCapabilityAbstractFactory));
        }
        
        [TestMethod]
        [Description(@"This test method verifies that Device Capability Catalogue raise an exception when there exists two abstract factories
        for same capability type in the given directory path")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestCatalogueBehaviorForTwoAbstractFactoriesOfSameCapability()
        {
            bool isExceptionRaised = false;

            ComposablePartCatalog catalog = new DirectoryCatalog(Directory.GetCurrentDirectory());

            try
            {
                DeviceCapabilityCatalogue capabilityCatalogue = new DeviceCapabilityCatalogue(catalog);
            }
            catch
            {
                isExceptionRaised = true;
            }

            Assert.IsTrue(isExceptionRaised);
        }  
    }
}
