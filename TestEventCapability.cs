using LandisGyr.AMI.Devices.Capabilities.Definitions;
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
    public class TestEventCapability
    {
        [TestMethod]
        [Description(@"This test method is used to check the defaults of the EventSet Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestDefaultValuesOfEventSetCapability()
        {
            CapabilityBase eventSetCapability = new EventSetCapability();
            Assert.AreEqual(eventSetCapability.CapabilityType, LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Events);
            Assert.AreEqual(eventSetCapability.IsConfigurationDependent, false);
        }

        [TestMethod]
        [Description(@"This test method is used to check exception is thrown if capabilities other than Event Capability are being merged to Event Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        [ExcludeFromCodeCoverageAttribute]
        public void MergeDiffTypeCapabilityIntoEventCapability()
        {
            bool capabiltiesNotOfSameType = true;

            CapabilityBase eventSetCapability = new EventSetCapability();
            ((IElementsBasedCapability)eventSetCapability).Elements = new Dictionary<string, ICapabilityElement>();

            MockRegistersCapability mockCapability = new MockRegistersCapability(null);

            try
            {
                eventSetCapability.Merge(new CapabilityBase[] { mockCapability });
                capabiltiesNotOfSameType = false;
            }
            catch (ApplicationException)
            {
                capabiltiesNotOfSameType = true;
            }

            Assert.IsTrue(capabiltiesNotOfSameType, "Expected ApplicationException but because of Capabilities being merged are of same type, so no exception.");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability with common events being reused")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        public void MergeTwoEventsCapabilityWithCommonEvents()
        {
            Event event1 = new Event("1.2.3.4.5.6.7.8.9.10.11.12.13");
            Event event2 = new Event("2.2.3.4.5.6.7.8.9.10.11.12.13");
            Event event3 = new Event("3.2.3.4.5.6.7.8.9.10.11.12.13");

            Dictionary<string, ICapabilityElement> events = new Dictionary<string, ICapabilityElement>();
            events.Add(event1.Identifier, event1);
            events.Add(event2.Identifier, event2);

            IElementsBasedCapability eventSetCapability1 = new EventSetCapability();
            eventSetCapability1.Elements = events;


            Dictionary<string, ICapabilityElement> events2 = new Dictionary<string, ICapabilityElement>();
            events2.Add(event2.Identifier, event2);
            events2.Add(event3.Identifier, event3);

            IElementsBasedCapability eventSetCapability2 = new EventSetCapability();
            eventSetCapability2.Elements = events2;

            EventSetCapability mergedEventCapability = (EventSetCapability)((CapabilityBase)eventSetCapability1).Merge(new CapabilityBase[] { (EventSetCapability)eventSetCapability2 });

            Assert.IsTrue(mergedEventCapability.Events.Count == 3, "Merged Event capability should contain Event 1, 2, 3 only");
            Assert.IsTrue(mergedEventCapability.Events[event1.Identifier] == ((EventSetCapability)eventSetCapability1).Events[event1.Identifier], "Merged Event capability's Event1 should be same as first EventCapability");
            Assert.IsTrue(mergedEventCapability.Events[event3.Identifier] == ((EventSetCapability)eventSetCapability2).Events[event3.Identifier], "Merged Event capability's Event3 should be same as second EventCapability");

            Assert.IsTrue(mergedEventCapability.Events[event2.Identifier] == ((EventSetCapability)eventSetCapability1).Events[event2.Identifier], "Merged Event capability Event2 should be same as first EventCapability");
            Assert.IsTrue(mergedEventCapability.Events[event2.Identifier] == ((EventSetCapability)eventSetCapability2).Events[event2.Identifier], "Merged Event capability Event2 should be same as second EventCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability without any common events")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        public void MergeTwoEventsCapabilityWithoutCommonEvents()
        {
            Event event1 = new Event("1.2.3.4.5.6.7.8.9.10.11.12.13");
            Event event2 = new Event("2.2.3.4.5.6.7.8.9.10.11.12.13");
            Event event3 = new Event("3.2.3.4.5.6.7.8.9.10.11.12.13");
            Event event4 = new Event("4.2.3.4.5.6.7.8.9.10.11.12.13");

            Dictionary<string, ICapabilityElement> events = new Dictionary<string, ICapabilityElement>();
            events.Add(event1.Identifier, event1);
            events.Add(event2.Identifier, event2);

            IElementsBasedCapability eventSetCapability1 = new EventSetCapability();
            eventSetCapability1.Elements = events;


            Dictionary<string, ICapabilityElement> events2 = new Dictionary<string, ICapabilityElement>();
            events2.Add(event3.Identifier, event3);
            events2.Add(event4.Identifier, event4);

            IElementsBasedCapability eventSetCapability2 = new EventSetCapability();
            eventSetCapability2.Elements = events2;

            EventSetCapability mergedEventCapability = (EventSetCapability)((CapabilityBase)eventSetCapability1).Merge(new CapabilityBase[] { (EventSetCapability)eventSetCapability2 });

            Assert.IsTrue(mergedEventCapability.Events.Count == 4, "Merged event capability should contain Event 1, 2, 3, 4");
            Assert.IsTrue(mergedEventCapability.Events[event1.Identifier] == ((EventSetCapability)eventSetCapability1).Events[event1.Identifier], "Merged Event capability's Event1 should be same as first EventCapability");
            Assert.IsTrue(mergedEventCapability.Events[event2.Identifier] == ((EventSetCapability)eventSetCapability1).Events[event2.Identifier], "Merged Event capability Event2 should be same as first EventCapability");

            Assert.IsTrue(mergedEventCapability.Events[event3.Identifier] == ((EventSetCapability)eventSetCapability2).Events[event3.Identifier], "Merged Event capability's Event3 should be same as second EventCapability");
            Assert.IsTrue(mergedEventCapability.Events[event4.Identifier] == ((EventSetCapability)eventSetCapability2).Events[event4.Identifier], "Merged Event capability Event4 should be same as second EventCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to check exception is thrown if unsupported capabilities are being merged to Event Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        public void ParseEventSetCapabilityWithAttribs()
        {
            #region Capability XML

            string eventsCapabilityXml = @"<Capabilities>
                                               <events isConfigurationDependent='false' minHistoricEvents='40' gapCollectionAlgo='DatePointer'>
                                                    <event id='1.2.3.4.5.6.7.8.9.10.[0-9].12.13'/>
                                                    <event id='1.2.3.4.5.6.7.8.9.10.11.12.13' />
                                                </events>
                                              </Capabilities>";

            #endregion

            CapabilityBase eventsCapability = null;
            EventSetCapabilityBuilder eventCapabilityBuilder = new EventSetCapabilityBuilder();
            bool isEventCapabilityParsed = true;

            eventsCapability = eventCapabilityBuilder.BuildCapability(eventsCapabilityXml);

            Assert.IsTrue(isEventCapabilityParsed, "Event capability XML parsing failed due to invalid XML");
            Assert.IsTrue(eventsCapability != null);
            Assert.IsTrue(eventsCapability is EventSetCapability);
            Assert.IsTrue(((EventSetCapability)eventsCapability).MinHistoricEvents == 40);
            Assert.IsTrue(((EventSetCapability)eventsCapability).GapCollectionAlgo == EventGapCollectionAlgo.DatePointer);
            Assert.IsTrue(((EventSetCapability)eventsCapability).Events.Count == 11);
        }


        [TestMethod]
        [Description(@"This test method is used to validate unsupported attributes fails event parsing")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseEventCapabilitiesXMLWithExtraAttrib()
        {
            #region Capability XML

            string eventsCapabilityXml = @"<Capabilities>
                                               <events isConfigurationDependent='false' minHistoricEvents='40' gapCollectionAlgo='DatePointer'>
                                                    <event id='1.2.3.4.5.6.7.8.9.10.[0-9].12.13' isGap='True'/>
                                                    <event id='1.2.3.4.5.6.7.8.9.10.11.12.13' />
                                                </events>
                                              </Capabilities>";

            #endregion

            EventSetCapabilityBuilder eventCapabilityBuilder = new EventSetCapabilityBuilder();
            bool isFailed = false;

            try
            {
                eventCapabilityBuilder.BuildCapability(eventsCapabilityXml);
            }
            catch (ApplicationException)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "Event capability XML does not contain element with extra attribute");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Event Handler loads the specified Event capability properly")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        public void CreateAndLoadEventSetCapability()
        {
            DBHelper.CleanDatabase();
            string capabilityCrc = "E567";
            int minHistoricEventCnt = 40;
            EventGapCollectionAlgo eventGapAlgo = EventGapCollectionAlgo.CountPointer;

            CapabilityBase eventSetCapability = new EventSetCapability();
            Dictionary<string, ICapabilityElement> events = new Dictionary<string, ICapabilityElement>();
            events.Add("1.2.3.4.5.6.7.8.9.10.11.12.13", new Event("1.2.3.4.5.6.7.8.9.10.11.12.13"));

            ((IElementsBasedCapability)eventSetCapability).Elements = events;

            ((EventSetCapability)eventSetCapability).MinHistoricEvents = minHistoricEventCnt;
            ((EventSetCapability)eventSetCapability).GapCollectionAlgo = eventGapAlgo;

            EventSetCapabilityHandler eventCapabilityHandler = new EventSetCapabilityHandler();

            bool capabilityCreated = eventCapabilityHandler.CreateCapabilityIfNotExists(eventSetCapability, Definitions.CapabilitySource.Device, capabilityCrc);

            Assert.IsTrue(capabilityCreated, "New Capability not created because it already exists, clean database and retry");

            CapabilityBase capability = eventCapabilityHandler.LoadCapability(capabilityCrc);

            Assert.IsNotNull(capability as EventSetCapability);

            Assert.IsTrue(((EventSetCapability)capability).Events.Count > 0);
            Assert.IsTrue(((EventSetCapability)capability).MinHistoricEvents == minHistoricEventCnt);
            Assert.IsTrue(((EventSetCapability)capability).GapCollectionAlgo == eventGapAlgo);
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Event capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Events)]
        public void ValidateEventCapabilitySerialization()
        {
            CapabilityBase eventCap = new EventSetCapability();
            Dictionary<string, ICapabilityElement> elements = new Dictionary<string, ICapabilityElement>();
            elements.Add("1.2.3.4.5", new Event("1.2.3.4.5"));

            ((IElementsBasedCapability)eventCap).Elements = elements;
            ((EventSetCapability)eventCap).MinHistoricEvents = 40;
            ((EventSetCapability)eventCap).GapCollectionAlgo = EventGapCollectionAlgo.CountPointer;

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(eventCap.GetType());
            serializer.WriteObject(fs, eventCap);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            //this Assert is to confirm that EventSetCapability properties are serialized i.e. SortedEventsForCrcComputer, MinHistoricEvents,GapCollectionMechanism
            Assert.IsTrue(txt.Contains("</Events>"));
            Assert.IsTrue(txt.Contains("<MinHistoricEvents>"));
            Assert.IsTrue(txt.Contains("<GapCollectionAlgo>"));

            //This Assert to verify the Custom Data Type of GapCollectionMechanism is serialized
            Assert.IsTrue(txt.Contains(string.Format("<GapCollectionAlgo>{0}</GapCollectionAlgo>", EventGapCollectionAlgo.CountPointer.ToString())));

            //Following Asserts verify that Property of Event instance is serialized
            Assert.IsTrue(txt.Contains("<Identifier>"));
        }
    }
}
