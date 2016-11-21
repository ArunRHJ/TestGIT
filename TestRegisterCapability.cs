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
    public class TestRegisterCapability
    {
        [TestMethod]
        [Description(@"This test method is used to check the defaults of the RegisterSet Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestDefaultValuesOfRegisterSetCapability()
        {
            CapabilityBase registerSetCapability = new RegisterSetCapability();
            Assert.AreEqual(registerSetCapability.CapabilityType, LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.Registers);
            Assert.AreEqual(registerSetCapability.IsConfigurationDependent, true);
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Register's attributes are serialized properly using ASCII encoding")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void ValidateRegisterAttribSerialization()
        {
            Register reg = new Register("1", "INT32");

            ICapabilityElement registerAsElement = (ICapabilityElement)reg;

            Assert.IsTrue(Enumerable.SequenceEqual(registerAsElement.Details, Encoding.ASCII.GetBytes(reg.DataType)), "Register attributes Serialization changed, please check Register.Details getter implementation");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Register's detail is deserialized properly to its corresponding attributes using ASCII encoding")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void ValidateRegisterDetailsDeserialization()
        {
            string dataType = "INT32";

            ICapabilityElement register = new Register("1");
            register.Details = Encoding.ASCII.GetBytes(dataType);

            Assert.IsTrue(((Register)register).DataType == dataType, "Register attributes Deserialization changed, please check Register.Details setter implementation");
        }

        [TestMethod]
        [Description(@"This test method is used to check exception is thrown if sunsupported capabilities are being merged to Register Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        [ExcludeFromCodeCoverageAttribute]
        public void MergeDiffTypeCapabilityIntoRegisterCapability()
        {
            bool capabiltiesNotOfSameType = true;

            Register register1 = new Register("1", "INT");
            Register register2 = new Register("2", "DECIMAL");

            Dictionary<string, ICapabilityElement> registers = new Dictionary<string, ICapabilityElement>();
            registers.Add(register1.Identifier, register1);
            registers.Add(register2.Identifier, register2);

            CapabilityBase registerSetCapability = new RegisterSetCapability();
            ((IElementsBasedCapability)registerSetCapability).Elements = registers;

            MockRegistersCapability mockCapability = new MockRegistersCapability(null);

            try
            {
                registerSetCapability.Merge(new CapabilityBase[] { mockCapability });
                capabiltiesNotOfSameType = false;
            }
            catch (ApplicationException)
            {
                capabiltiesNotOfSameType = true;
            }

            Assert.IsTrue(capabiltiesNotOfSameType, "Expected ApplicationException but because of Capabilities being merged are of same type, so no exception.");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability with common registers being reused")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void MergeTwoRegistersCapabilityWithCommonRegisters()
        {
            Register register1 = new Register("1", "INT");
            Register register2 = new Register("2", "DECIMAL");
            Register register3 = new Register("3", "STRING");

            Dictionary<string, ICapabilityElement> registers = new Dictionary<string, ICapabilityElement>();
            registers.Add(register1.Identifier, register1);
            registers.Add(register2.Identifier, register2);

            IElementsBasedCapability registerSetCapability1 = new RegisterSetCapability();
            registerSetCapability1.Elements = registers;


            Dictionary<string, ICapabilityElement> registers2 = new Dictionary<string, ICapabilityElement>();
            registers2.Add(register2.Identifier, register2);
            registers2.Add(register3.Identifier, register3);

            IElementsBasedCapability registerSetCapability2 = new RegisterSetCapability();
            registerSetCapability2.Elements = registers2;

            RegisterSetCapability mergedRegisterCapability = (RegisterSetCapability)((CapabilityBase)registerSetCapability1).Merge(new CapabilityBase[] { (RegisterSetCapability)registerSetCapability2 });

            Assert.IsTrue(mergedRegisterCapability.Registers.Count == 3, "Merged register capability should contain Register 1, 2, 3 only");
            Assert.IsTrue(mergedRegisterCapability.Registers[register1.Identifier] == ((RegisterSetCapability)registerSetCapability1).Registers[register1.Identifier], "Merged register capability's Register1 should be same as first RegisterCapability");
            Assert.IsTrue(mergedRegisterCapability.Registers[register3.Identifier] == ((RegisterSetCapability)registerSetCapability2).Registers[register3.Identifier], "Merged register capability's Register3 should be same as second RegisterCapability");

            Assert.IsTrue(mergedRegisterCapability.Registers[register2.Identifier] == ((RegisterSetCapability)registerSetCapability1).Registers[register2.Identifier], "Merged register capability Register2 should be same as first RegisterCapability");
            Assert.IsTrue(mergedRegisterCapability.Registers[register2.Identifier] == ((RegisterSetCapability)registerSetCapability2).Registers[register2.Identifier], "Merged register capability Register2 should be same as second RegisterCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to check that merging two or more capabilities gives new capability without any common registers")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void MergeTwoRegistersCapabilityWithoutCommonRegisters()
        {
            Register register1 = new Register("1", "INT");
            Register register2 = new Register("2", "DECIMAL");
            Register register3 = new Register("3", "STRING");
            Register register4 = new Register("4", "STRING");

            Dictionary<string, ICapabilityElement> registers = new Dictionary<string, ICapabilityElement>();
            registers.Add(register1.Identifier, register1);
            registers.Add(register2.Identifier, register2);

            IElementsBasedCapability registerSetCapability1 = new RegisterSetCapability();
            registerSetCapability1.Elements = registers;


            Dictionary<string, ICapabilityElement> registers2 = new Dictionary<string, ICapabilityElement>();
            registers2.Add(register3.Identifier, register3);
            registers2.Add(register4.Identifier, register4);

            IElementsBasedCapability registerSetCapability2 = new RegisterSetCapability();
            registerSetCapability2.Elements = registers2;

            RegisterSetCapability mergedRegisterCapability = (RegisterSetCapability)((CapabilityBase)registerSetCapability1).Merge(new CapabilityBase[] { (RegisterSetCapability)registerSetCapability2 });

            Assert.IsTrue(mergedRegisterCapability.Registers.Count == 4, "Merged register capability should contain Register 1, 2, 3, 4");
            Assert.IsTrue(mergedRegisterCapability.Registers[register1.Identifier] == ((RegisterSetCapability)registerSetCapability1).Registers[register1.Identifier], "Merged register capability's Register1 should be same as first RegisterCapability");
            Assert.IsTrue(mergedRegisterCapability.Registers[register2.Identifier] == ((RegisterSetCapability)registerSetCapability1).Registers[register2.Identifier], "Merged register capability Register2 should be same as first RegisterCapability");

            Assert.IsTrue(mergedRegisterCapability.Registers[register3.Identifier] == ((RegisterSetCapability)registerSetCapability2).Registers[register3.Identifier], "Merged register capability's Register3 should be same as second RegisterCapability");
            Assert.IsTrue(mergedRegisterCapability.Registers[register4.Identifier] == ((RegisterSetCapability)registerSetCapability2).Registers[register4.Identifier], "Merged register capability Register4 should be same as second RegisterCapability");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that unknown attributes are not supported in register capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseCapabilitiesXMLWithExtraAttrib()
        {
            #region Capability XML

            string registersCapabilityXml = @"<Capabilities>
                                               <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.[0-9]' dataType='INT' isGap='True'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' dataType='INT' />
                                                </readingTypes>
                                              </Capabilities>";

            #endregion

            RegisterSetCapabilityBuilder registerCapabilityBuilder = new RegisterSetCapabilityBuilder();
            bool isFailed = false;

            try
            {
                registerCapabilityBuilder.BuildCapability(registersCapabilityXml);
            }
            catch (ApplicationException)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "Register capability XML does not contain element with extra attribute");
        }


        [TestMethod]
        [Description(@"This test method is used to validate that exception is thrown if required attributes are missing")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        [ExcludeFromCodeCoverageAttribute]
        public void ParseCapabilitiesXMLWithMissingRequiredAttrib()
        {
            #region Capability XML

            string registersCapabilityXml = @"<Capabilities>
                                               <readingTypes>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.[0-9]' isGap='True'/>
                                                    <readingType value='1.2.3.4.5.6.7.8.9.10.11' dataType='INT' />
                                                </readingTypes>
                                              </Capabilities>";

            #endregion

            RegisterSetCapabilityBuilder registerCapabilityBuilder = new RegisterSetCapabilityBuilder();
            bool isFailed = false;

            try
            {
                registerCapabilityBuilder.BuildCapability(registersCapabilityXml);
            }
            catch (ApplicationException ex)
            {
                isFailed = true;
            }

            Assert.IsTrue(isFailed, "Register capability XML contains element with required attribute");
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Register Handler loads the specified Register capability properly")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void CreateAndLoadRegisterSetCapability()
        {
            DBHelper.CleanDatabase();
            string capabilityCrc = "123";

            MockRegistersCapability registerSetCapability = new MockRegistersCapability(null);

            RegisterSetCapabilityHandler registerCapabilityHandler = new RegisterSetCapabilityHandler();

            bool capabilityCreated = registerCapabilityHandler.CreateCapabilityIfNotExists(registerSetCapability, Definitions.CapabilitySource.Device, capabilityCrc);

            Assert.IsTrue(capabilityCreated, "New Capability not created because it already exists, clean database and retry");

            CapabilityBase capability = registerCapabilityHandler.LoadCapability(capabilityCrc);

            Assert.IsNotNull(capability as RegisterSetCapability);

            Assert.IsTrue(((RegisterSetCapability)capability).Registers.Count > 0);
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Register capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Model, TargetCapabilityType.Registers)]
        public void ValidateRegisterCapabilitySerialization()
        {
            CapabilityBase registerCap = new RegisterSetCapability();
            Dictionary<string, ICapabilityElement> elements = new Dictionary<string, ICapabilityElement>();
            elements.Add("1.2.3.4.5.6.7.8.9.10.11", new Register("1.2.3.4.5.6.7.8.9.10.11", "Int32"));

            ((IElementsBasedCapability)registerCap).Elements = elements;

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(registerCap.GetType());
            serializer.WriteObject(fs, registerCap);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            //this Assert is to confirm that the main NodeName for the Capability is present i.e. RegisterSetCapability.SortedRegistersForCrcComputer
            Assert.IsTrue(txt.Contains("</Registers>"), "Required Registers node missing");

            //Following Asserts verify that Properties of Register instance are serialized
            Assert.IsTrue(txt.Contains("<Identifier>"), "Required Identifier node of Register instance missing");
            Assert.IsTrue(txt.Contains("<DataType>"), "Required DataType node of Register instance missing");
        }
    }
}
