using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using CC = LandisGyr.AMI.Devices.Capabilities;
using Definitions = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilityCrcComputer
    {
        /// <summary>
        /// This test method is used to validate type of Default Capaility CRC Computer.
        /// If this test fails, this implies CapabilityAbstractFactory's CRC computer is changed.
        /// </summary>
        [TestMethod]
        [Description(@"This test method is used to validate type of Default Capaility CRC Computer. 
                       Test will fail if default capability CRC computer in CapabilityAbstractFactory will be changed")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void ValidateDefaultCrcComputer()
        {
            CapabilityAbstractFactory dummyRegistersCapabilityAbstractFactory = new MockRegistersCapabilityAbstractFactory();

            CapabilityCrcComputer regiterCrcComputer = dummyRegistersCapabilityAbstractFactory.CrcComputer;

            Assert.IsInstanceOfType(regiterCrcComputer, typeof(CapabilityCrcComputer), "Default CRC computer of the capability is not of CapabilityCrcComputer type.");          
        }

        /// <summary>
        /// This test method is used to validate Default CRC Computer is using SHA1 algorithm
        /// This test will fail if the default CRC calulation algorithm is changed in CapabilityCrcComputer class.
        /// </summary>
        [TestMethod]
        [Description(@"This test method is used to validate Default CRC Computer for a capability uses SHA1 algorithm. 
                       Test will fail if default capability CRC computer uses any algorithm other than SHA1")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void ValidateDefaultCrcCalculationAlgo()
        {            
            byte[] dataBytes = new byte[] { 0x00, 0x01, 0x02 , 0x03, 0x04};
            
            CapabilityCrcComputer defaultCrcComputer = new CapabilityCrcComputer();
            Sha1CrcComputer crcComputer = new Sha1CrcComputer();

            PrivateObject obj = new PrivateObject(defaultCrcComputer);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;
            object defaultHash = obj.Invoke("ComputeHash", bindingFlgs, dataBytes);

            string sha1Hash = crcComputer.ComputeHash(dataBytes);

            Assert.IsTrue(defaultHash.ToString() == sha1Hash, "Default CRC computer is not using SHA1 algo for hash computation");
        }
    }
}
