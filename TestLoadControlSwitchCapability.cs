using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestLoadControlSwitchCapability
    {        
        [TestMethod]
        [Description(@"This test method is used to validate that SwitchStatus's attributes are serialized properly using ASCII encoding")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        public void ValidateSwitchStatusAttribSerialization()
        {
            SwitchStatus switchStatus = new SwitchStatus("On By Load", LoadControlSwitchStatus.Connect);

            ICapabilityElement switchStatusAsElement = (ICapabilityElement)switchStatus;

            Assert.IsTrue(Enumerable.SequenceEqual(switchStatusAsElement.Details, Encoding.ASCII.GetBytes(switchStatus.Value.ToString())), "SwitchStatus attributes Serialization changed, please check SwitchStatus.Details getter implementation");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that SwitchStatus's detail is deserialized properly to its corresponding attributes using ASCII encoding")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        [ExcludeFromCodeCoverageAttribute]
        public void ValidateSwitchStatusDetailsDeserialization()
        {
            LoadControlSwitchStatus switchStatusValue = LoadControlSwitchStatus.Disconnect;

            ICapabilityElement switchStatus = new SwitchStatus("Off by Load");
            switchStatus.Details = Encoding.ASCII.GetBytes(switchStatusValue.ToString());

            Assert.IsTrue(((SwitchStatus)switchStatus).Value == switchStatusValue, "SwitchStatus attributes Deserialization changed, please check SwitchSstatu.Details setter implementation");
        }

        [TestMethod]
        [Description(@"This test method is used to check exception is thrown if unsupported capabilities are being merged to LoadControlSwitch Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        [ExcludeFromCodeCoverageAttribute]
        public void MergeDiffTypeCapabilityIntoLoadControlSwitchCapability()
        {
            bool capabiltiesNotOfSameType = true;

            SwitchStatus switchStatus = new SwitchStatus("ON", LoadControlSwitchStatus.Connect );

            Dictionary<string, ICapabilityElement> switchStatuses = new Dictionary<string, ICapabilityElement>();
            switchStatuses.Add(switchStatus.SwitchStatusCode, switchStatus);

            CapabilityBase lcsCapability = new LoadControlSwitchCapability();
            ((IElementsBasedCapability)lcsCapability).Elements = switchStatuses;

            MockRegistersCapability mockCapability = new MockRegistersCapability(null);

            try
            {
                lcsCapability.Merge(new CapabilityBase[] { mockCapability });
                capabiltiesNotOfSameType = false;
            }
            catch(ApplicationException)
            {
                capabiltiesNotOfSameType = true;
            }

            Assert.IsTrue(capabiltiesNotOfSameType, "Expected ApplicationException but because of Capabilities being merged are of same type, so no exception.");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability with common SwitchStatus being reused")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        public void MergeTwoLoadControlSwitchCapabilityWithCommonSwitchStatuses()
        {
            SwitchStatus switchStatus1 = new SwitchStatus("ON", LoadControlSwitchStatus.Connect);
            SwitchStatus switchStatus2 = new SwitchStatus("OFF", LoadControlSwitchStatus.Disconnect);
            SwitchStatus switchStatus3 = new SwitchStatus("ON By Load", LoadControlSwitchStatus.Connect);

            Dictionary<string, ICapabilityElement> switchStatuses = new Dictionary<string, ICapabilityElement>();
            switchStatuses.Add(switchStatus1.SwitchStatusCode, switchStatus1);
            switchStatuses.Add(switchStatus2.SwitchStatusCode, switchStatus2);

            IElementsBasedCapability lcsCapability1 = new LoadControlSwitchCapability();
            lcsCapability1.Elements = switchStatuses;


            Dictionary<string, ICapabilityElement> switchStatuses2 = new Dictionary<string, ICapabilityElement>();
            switchStatuses2.Add(switchStatus2.SwitchStatusCode, switchStatus2);
            switchStatuses2.Add(switchStatus3.SwitchStatusCode, switchStatus3);

            IElementsBasedCapability lcsCapability2 = new LoadControlSwitchCapability();
            lcsCapability2.Elements = switchStatuses2;

            LoadControlSwitchCapability mergedLCSCapability = (LoadControlSwitchCapability)((CapabilityBase)lcsCapability1).Merge(new CapabilityBase[] { (LoadControlSwitchCapability)lcsCapability2 });

            Assert.IsTrue(mergedLCSCapability.SwitchStatuses.Count == 3, "Merged LoadcontrolSwitch capability should contain SwitchStatus 1, 2, 3 only");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus1.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability1).SwitchStatuses[switchStatus1.SwitchStatusCode], "Merged LoadControlSwitch capability's Switchstatus1 should be same as first LoadControlSwitch Capability");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus3.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability2).SwitchStatuses[switchStatus3.SwitchStatusCode], "Merged LoadControlSwitch capability's SwitchStusat3 should be same as second LoadControlSwitch Capability");

            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus2.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability1).SwitchStatuses[switchStatus2.SwitchStatusCode], "Merged LoadControlSwitch capability SwitchStatus2 should be same as first LoadControlSwitch Capability");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus2.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability2).SwitchStatuses[switchStatus2.SwitchStatusCode], "Merged LoadControlSwitch capability Switchstatus2 should be same as second LoadControlSwitch Capability");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability without any common SwitchStatus")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        public void MergeTwoLoadControlSwitchCapabilityWithoutCommonSwitchStatuses()
        {
            SwitchStatus switchStatus1 = new SwitchStatus("ON By Curent", LoadControlSwitchStatus.Connect);
            SwitchStatus switchStatus2 = new SwitchStatus("ARM", LoadControlSwitchStatus.Arm);
            SwitchStatus switchStatus3 = new SwitchStatus("OFF", LoadControlSwitchStatus.Disconnect);
            SwitchStatus switchStatus4 = new SwitchStatus("Off by Load", LoadControlSwitchStatus.Disconnect);

            Dictionary<string, ICapabilityElement> switchStatuses = new Dictionary<string, ICapabilityElement>();
            switchStatuses.Add(switchStatus1.SwitchStatusCode, switchStatus1);
            switchStatuses.Add(switchStatus2.SwitchStatusCode, switchStatus2);

            IElementsBasedCapability lcsCapability1 = new LoadControlSwitchCapability();
            lcsCapability1.Elements = switchStatuses;


            Dictionary<string, ICapabilityElement> switchStatuses2 = new Dictionary<string, ICapabilityElement>();
            switchStatuses2.Add(switchStatus3.SwitchStatusCode, switchStatus3);
            switchStatuses2.Add(switchStatus4.SwitchStatusCode, switchStatus4);

            IElementsBasedCapability lcsCapability2 = new LoadControlSwitchCapability();
            lcsCapability2.Elements = switchStatuses2;

            LoadControlSwitchCapability mergedLCSCapability = (LoadControlSwitchCapability)((CapabilityBase)lcsCapability1).Merge(new CapabilityBase[] { (LoadControlSwitchCapability)lcsCapability2 });

            Assert.IsTrue(mergedLCSCapability.SwitchStatuses.Count == 4, "Merged LoadControlSwitch capability should contain switchStatus 1, 2, 3, 4");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus1.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability1).SwitchStatuses[switchStatus1.SwitchStatusCode], "Merged LoadcontrolSwitch capability's SwitchStatus1 should be same as first LoadcontrolSwitch Capability");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus2.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability1).SwitchStatuses[switchStatus2.SwitchStatusCode], "Merged LoadcontrolSwitch capability SwitchStatus2 should be same as first LoadcontrolSwitch Capability");

            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus3.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability2).SwitchStatuses[switchStatus3.SwitchStatusCode], "Merged LoadcontrolSwitch capability's SwitchStatus3 should be same as second LoadcontrolSwitch Capability");
            Assert.IsTrue(mergedLCSCapability.SwitchStatuses[switchStatus4.SwitchStatusCode] == ((LoadControlSwitchCapability)lcsCapability2).SwitchStatuses[switchStatus4.SwitchStatusCode], "Merged LoadcontrolSwitch capability SwitchStatus4 should be same as second LoadcontrolSwitch Capability");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that unknown attributes are not supported in LoadControlSwitch capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseLoadControlSwitchCapabilitiesXMLWithExtraAttrib()
        {
            #region Capability XML

            string lcsCapabilityXml = @"<Capabilities>
                                               <loadControlSwitch>
                                                   <switchStatus code='On' value='Connect' bitValue='001'/>
                                                    <switchStatus code='Off' value='Disconnect' />
                                                </loadControlSwitch>
                                              </Capabilities>";

            #endregion

            LoadControlSwitchCapabilityBuilder lcsCapabilityBuilder = new LoadControlSwitchCapabilityBuilder();
            bool isFailed = false;

            try
            {
                lcsCapabilityBuilder.BuildCapability(lcsCapabilityXml);
            }
            catch (ApplicationException)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "LoadControlSwitch capability XML does not contain element with extra attribute");
        }


        [TestMethod]
        [Description(@"This test method is used to validate that exception is thrown if required attributes are missing")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseLoadControlSwitchCapabilitiesXMLWithMissingRequiredAttrib()
        {
            #region Capability XML

            string lcsCapabilityXml = @"<Capabilities>
                                               <loadControlSwitch>
                                                   <switchStatus code='On' bitValue='001'/>
                                                    <switchStatus code='Off' dataType='Disconnect' />
                                                </loadControlSwitch>
                                              </Capabilities>";

            #endregion

            LoadControlSwitchCapabilityBuilder lcsCapabilityBuilder = new LoadControlSwitchCapabilityBuilder();
            bool isFailed = false;

            try
            {
                lcsCapabilityBuilder.BuildCapability(lcsCapabilityXml);
            }
            catch (ApplicationException)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "LoadControl capability XML contains element with required attribute");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that LoadControlSwitch Handler loads the specified LoadControl capability properly")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        public void CreateAndLoadLoadControlSwitchCapability()
        {
            DBHelper.CleanDatabase();
            string capabilityCrc = "LCS123";

            LoadControlSwitchCapability lcsCapability = new LoadControlSwitchCapability();
            SwitchStatus switchStatus1 = new SwitchStatus("On", LoadControlSwitchStatus.Connect);
            SwitchStatus switchStatus2 = new SwitchStatus("Off", LoadControlSwitchStatus.Disconnect);

            Dictionary<string, ICapabilityElement> switchStatuses = new Dictionary<string,ICapabilityElement>();
            switchStatuses.Add(switchStatus1.SwitchStatusCode, switchStatus1);
            switchStatuses.Add(switchStatus2.SwitchStatusCode, switchStatus2);

            ((IElementsBasedCapability)lcsCapability).Elements = switchStatuses;

            LoadControlSwitchCapabilityHandler lcsCapabilityHandler = new LoadControlSwitchCapabilityHandler();

            bool capabilityCreated = lcsCapabilityHandler.CreateCapabilityIfNotExists(lcsCapability, Definitions.CapabilitySource.Device, capabilityCrc);

            Assert.IsTrue(capabilityCreated, "New Capability not created because it already exists, clean database and retry");

            CapabilityBase capability = lcsCapabilityHandler.LoadCapability(capabilityCrc);

            Assert.IsNotNull(capability as LoadControlSwitchCapability);

            Assert.IsTrue(((LoadControlSwitchCapability)capability).SwitchStatuses.Count > 0);
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Loadcontrol capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.LoadControl)]
        public void ValidateLoadControlSwitchCapabilitySerialization()
        {
            CapabilityBase lcsCap = new LoadControlSwitchCapability();
            Dictionary<string, ICapabilityElement> elements = new Dictionary<string, ICapabilityElement>();
            elements.Add("Off by load", new SwitchStatus("Off by load", LoadControlSwitchStatus.Disconnect));

            ((IElementsBasedCapability)lcsCap).Elements = elements;

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(lcsCap.GetType());
            serializer.WriteObject(fs, lcsCap);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            //this Assert is to confirm that the main NodeName for the Capability is present i.e. LoadControlSwitchCapability.SortedSwitchStatusesForCrcComputer
            Assert.IsTrue(txt.Contains("</SwitchStatuses>"));

            //Following Asserts verify that Properties of SwitchStatus instance are serialized
            Assert.IsTrue(txt.Contains("<SwitchStatusCode>"));
            Assert.IsTrue(txt.Contains("<Value>"));
        }
    }
}
