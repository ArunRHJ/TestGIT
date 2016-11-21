using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using LandisGyr.AMI.Layers.DataContracts.ControlEvents.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceNetworkCapabilitiesProcessor
    {
        [TestMethod]
        [Description(@"This test method verifies that communication technology version and name are properly extracted from Registration information.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured, TargetCapabilityType.None)]
        public void TestExtractUpdateModelNameAndVersion()
        {
            ModelCapabilitiesInformation modelCpbltiesInfo = new ModelCapabilitiesInformation();
            modelCpbltiesInfo.DeviceIdentifier = "12";
            EndPointRegistrationInformation epRegistrationInfo = new EndPointRegistrationInformation();
            epRegistrationInfo.CommsTechName = "SBS";
            epRegistrationInfo.CommsTechVersion = "1";

            DeviceNetworkCapabilitiesProcessor deviceNetworkCpbltiesProcessor = new DeviceNetworkCapabilitiesProcessor(null, null);
            PrivateObject obj = new PrivateObject(deviceNetworkCpbltiesProcessor);
            obj.Invoke("ExtractUpdateModelNameAndVersion", modelCpbltiesInfo, epRegistrationInfo);

            Assert.IsTrue(modelCpbltiesInfo.CommsTechModelName == epRegistrationInfo.CommsTechName, "Communication Technology Model name not extracted correctly");
            Assert.IsTrue(modelCpbltiesInfo.CommsTechModelVersion == epRegistrationInfo.CommsTechVersion, "Communication Technology Model version  not extracted correctly");
        }
    }
}
