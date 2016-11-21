using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DD = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestFirmwareDownloadCapability
    {
        [TestMethod]
        [Description(@"This test is to verify FirmwareDownloadParameter values.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadParameterValues()
        {
            string assertMsg = "{0} property's Current value :{1} and Expected value :{2} mismatch.";

            //Expected attribute values of FirmwareDownloadParameter
            FirmwareType expectedFirmwareType = FirmwareType.Meter;
            string expectedFirmwarePlatform = "FP0001";
            string expectedMinVersion = "11.11.111.111";
            bool expectedIsDowngradeable = true;
            bool expectedCanPause = true;
            bool expectedCanAbort = true;
            bool expectedCanChangeActivationDatetime = true;
            bool expectedIsDelayedActivationSupported = false;
            bool expectedCanRollback = true;

            //Create FirmwareDownloadParameter
            FirmwareDownloadParameter firmwareDownloadParameter = GetFirmwareDownloadParameter(expectedFirmwareType, expectedFirmwarePlatform, expectedMinVersion,
                                                                                                expectedIsDowngradeable, expectedCanPause, expectedCanAbort,
                                                                                                expectedCanChangeActivationDatetime, expectedIsDelayedActivationSupported,
                                                                                                expectedCanRollback);
            //Current attribute values of FirmwareDownloadParameter
            FirmwareType currentFirmwareType = firmwareDownloadParameter.FirmwareType;
            string currentFirmwarePlatform = firmwareDownloadParameter.FirmwarePlatform;
            string currentMinVersion = firmwareDownloadParameter.MinVersion;
            bool currentIsDowngradeable = firmwareDownloadParameter.IsDowngradeable;
            bool currentCanPause = firmwareDownloadParameter.CanPause;
            bool currentCanAbort = firmwareDownloadParameter.CanAbort;
            bool currentCanChangeActivationDatetime = firmwareDownloadParameter.CanChangeActivationDatetime;
            bool currentIsDelayedActivationSupported = firmwareDownloadParameter.IsDelayedActivationSupported;
            bool currentCanRollback = firmwareDownloadParameter.CanRollback;

            //Verifying FirmwareDownloadParameter
            Assert.IsTrue(currentFirmwareType == expectedFirmwareType, string.Format(assertMsg, "FirmwareType", currentFirmwareType.ToString(), expectedFirmwareType.ToString()));
            Assert.IsTrue(currentFirmwarePlatform == expectedFirmwarePlatform, string.Format(assertMsg, "FirmwarePlatform", currentFirmwarePlatform, expectedFirmwareType));
            Assert.IsTrue(currentMinVersion == expectedMinVersion, string.Format(assertMsg, "MinVersion", currentMinVersion, expectedMinVersion));
            Assert.IsTrue(currentIsDowngradeable == expectedIsDowngradeable, string.Format(assertMsg, "IsDowngradeable", currentIsDowngradeable, expectedIsDowngradeable));
            Assert.IsTrue(currentCanPause == expectedCanPause, string.Format(assertMsg, "CanPause", currentCanPause, expectedCanPause));
            Assert.IsTrue(currentCanAbort == expectedCanAbort, string.Format(assertMsg, "CanAbort", currentCanAbort, expectedCanAbort));
            Assert.IsTrue(currentCanChangeActivationDatetime == expectedCanChangeActivationDatetime, string.Format(assertMsg, "CanChangeActivationDatetime", currentCanChangeActivationDatetime, expectedCanChangeActivationDatetime));
            Assert.IsTrue(currentIsDelayedActivationSupported == expectedIsDelayedActivationSupported, string.Format(assertMsg, "IsDelayedActivationSupported", currentIsDelayedActivationSupported, expectedIsDelayedActivationSupported));
            Assert.IsTrue(currentCanRollback == expectedCanRollback, string.Format(assertMsg, "CanRollback", currentCanRollback, expectedCanRollback));
        }

        [TestMethod]
        [Description(@"This test is to verify ICapabilityElement to FirmwareDownloadParameter and FirmwareDownloadParameter to ICapabilityElement Conversion.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadParametertoICapabilityElementConversion()
        {
            //Expected attribute values of  FirmwareDownloadParameter
            FirmwareType expectedFirmwareType = FirmwareType.Meter;
            string expectedFirmwarePlatform = "FP0001";
            string expectedMinVersion = "11.11.111.111";
            bool expectedIsDowngradeable = true;
            bool expectedCanPause = true;
            bool expectedCanAbort = true;
            bool expectedCanChangeActivationDatetime = true;
            bool expectedIsDelayedActivationSupported = false;
            bool expectedCanRollback = true;

            string assertMsg = "ICapabilityElement to FirmwareDownloadParameter conversion , {0} property's Current value :{1} and Expected value :{2} mismatch.";
            byte[] currentBytes;
            byte[] expectedBytes;
            string serializationFormat = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";

            //Create FirmwareDownloadParameter  [Here we are only setting FirmwareType property(Identifier)]
            FirmwareDownloadParameter firmwareDownloadParameter = new FirmwareDownloadParameter(expectedFirmwareType);

            //Set FirmwareDownloadParameter other attributes from ICapabilityElement
            string formattedString = string.Format(serializationFormat, expectedFirmwareType, expectedFirmwarePlatform, expectedMinVersion,
                                                   expectedIsDowngradeable, expectedCanPause, expectedCanAbort, expectedCanChangeActivationDatetime,
                                                   expectedIsDelayedActivationSupported, expectedCanRollback);
            currentBytes = Encoding.ASCII.GetBytes(formattedString);
            ((ICapabilityElement)firmwareDownloadParameter).Details = currentBytes;

            //Current attribute values of FirmwareDownloadParameter
            FirmwareType currentFirmwareType = firmwareDownloadParameter.FirmwareType;
            string currentFirmwarePlatform = firmwareDownloadParameter.FirmwarePlatform;
            string currentMinVersion = firmwareDownloadParameter.MinVersion;
            bool currentIsDowngradeable = firmwareDownloadParameter.IsDowngradeable;
            bool currentCanPause = firmwareDownloadParameter.CanPause;
            bool currentCanAbort = firmwareDownloadParameter.CanAbort;
            bool currentCanChangeActivationDatetime = firmwareDownloadParameter.CanChangeActivationDatetime;
            bool currentIsDelayedActivationSupported = firmwareDownloadParameter.IsDelayedActivationSupported;
            bool currentCanRollback = firmwareDownloadParameter.CanRollback;

            //To verify FirmwareDownloadParameter attributes
            Assert.IsTrue(currentFirmwareType == expectedFirmwareType, string.Format(assertMsg, "FirmwareType", currentFirmwareType.ToString(), expectedFirmwareType.ToString()));
            Assert.IsTrue(currentFirmwarePlatform == expectedFirmwarePlatform, string.Format(assertMsg, "FirmwarePlatform", currentFirmwarePlatform, expectedFirmwareType));
            Assert.IsTrue(currentMinVersion == expectedMinVersion, string.Format(assertMsg, "MinVersion", currentMinVersion, expectedMinVersion));
            Assert.IsTrue(currentIsDowngradeable == expectedIsDowngradeable, string.Format(assertMsg, "IsDowngradeable", currentIsDowngradeable, expectedIsDowngradeable));
            Assert.IsTrue(currentCanPause == expectedCanPause, string.Format(assertMsg, "CanPause", currentCanPause, expectedCanPause));
            Assert.IsTrue(currentCanAbort == expectedCanAbort, string.Format(assertMsg, "CanAbort", currentCanAbort, expectedCanAbort));
            Assert.IsTrue(currentCanChangeActivationDatetime == expectedCanChangeActivationDatetime, string.Format(assertMsg, "CanChangeActivationDatetime", currentCanChangeActivationDatetime, expectedCanChangeActivationDatetime));
            Assert.IsTrue(currentIsDelayedActivationSupported == expectedIsDelayedActivationSupported, string.Format(assertMsg, "IsDelayedActivationSupported", currentIsDelayedActivationSupported, expectedIsDelayedActivationSupported));
            Assert.IsTrue(currentCanRollback == expectedCanRollback, string.Format(assertMsg, "CanRollback", currentCanRollback, expectedCanRollback));


            //Get ICapabilityElement Detail attribute from FirmwareDownloadParameter
            expectedBytes = ((ICapabilityElement)firmwareDownloadParameter).Details;

            //Current and Expected ICapabilityElement Details attribute List
            List<byte> expectedBytesList = expectedBytes.ToList<byte>();
            List<byte> currentBytesList = currentBytes.ToList<byte>();

            //To verify ICapabilityElement Details attribute
            bool areBothEqual = expectedBytesList.SequenceEqual(currentBytesList);

            Assert.IsTrue(areBothEqual, "FirmwareDownloadParameter to ICapabilityElement conversion failed.");

        }

        [TestMethod]
        [Description(@"This test is to verify Default values of Firmware Download Capability.")]
        [Owner("email:platformteam@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadCapabilityDefaultValues()
        {
            FirmwareDownloadCapability capability = new FirmwareDownloadCapability();
            //To verify Default values of Firmware Download Capability
            Assert.IsTrue(capability.IsConfigurationDependent == false, "IsConfigurationDependent property of FirmwareDownloadCapability is not correct.");
            Assert.IsTrue(capability.CapabilityType == DD.CapabilityType.FirmwareDownload, "CapabilityType property of FirmwareDownloadCapability is not correct.");

            //To verify Default values of Firmware Download Capability Abstract Factory
            FirmwareDownloadCapabilityAbstractFactory firmwareDownloadAbstractFactory = new FirmwareDownloadCapabilityAbstractFactory();
            Assert.IsTrue(firmwareDownloadAbstractFactory.CapabilityBuilder is FirmwareDownloadCapabilityBuilder, "Builder is not of FirmwareDownloadCapabilityBuilder type.");
            Assert.IsTrue(firmwareDownloadAbstractFactory.CapabilityHandler is FirmwareDownloadCapabilityHandler, "Handler is not of FirmwareDownloadCapabilityHandler type.");
            Assert.IsTrue(firmwareDownloadAbstractFactory.CrcComputer is CapabilityCrcComputer, "CrcComputer is not of CapabilityCrcComputer type.");
        }

        [TestMethod]
        [Description(@"This test is to verify IElementsBasedCapability to FirmwareDownloadCapability and FirmwareDownloadCapability to IElementsBasedCapability Conversion.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadCapabilitytoIElementsBasedCapabilityConversion()
        {
            FirmwareType firmwareType = FirmwareType.Meter;
            string identifier = string.Empty;

            //Create FirmwareDownloadCapability  with one Element[FirmwareDownloadParameter]
            FirmwareDownloadParameter firmwareDownloadParameter = new FirmwareDownloadParameter(firmwareType);
            identifier = firmwareDownloadParameter.FirmwareType.ToString();
            FirmwareDownloadCapability firmwareDownloadCapability = new FirmwareDownloadCapability();

            //Create element attribute of IElementsBasedCapability
            Dictionary<string, ICapabilityElement> elements = new Dictionary<string, ICapabilityElement>();
            elements.Add(identifier, firmwareDownloadParameter);

            //IElementsBasedCapability to FirmwareDownloadCapability [FirmwareDownloadParameters] conversion
            ((IElementsBasedCapability)firmwareDownloadCapability).Elements = elements;

            //Get FirmwareDownloadCapability[FirmwareDownloadParameters] From FirmwareDownloadCapability
            ReadOnlyDictionary<string, FirmwareDownloadParameter> firmwareDownloadParameters = firmwareDownloadCapability.FirmwareDownloadParameters;

            //Verifying FirmwareDownloadCapability [FirmwareDownloadParameters]
            Assert.IsTrue(firmwareDownloadParameters[identifier] == firmwareDownloadParameter, "IElementsBasedCapability to FirmwareDownloadCapability conversion failed.");

            //Get IElementsBasedCapability Element attribute from FirmwareDownloadCapability
            Dictionary<string, ICapabilityElement> elementsGetFromCapability = ((IElementsBasedCapability)firmwareDownloadCapability).Elements;
            Assert.IsTrue(elementsGetFromCapability[identifier] == firmwareDownloadParameter, "FirmwareDownloadCapability to IElementsBasedCapability conversion failed.");
        }

        [TestMethod]
        [Description(@"This test is to verify FirmwareDownloadCapability can not be merged with Diff. Capability Type.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void MergingOfFirmwareDownloadCapabilityWithDiffCapabilityType()
        {
            bool isMergedOk = false;
            bool isHandledException = true;

            string errorMsg = string.Empty;
            FirmwareType firmwareType = FirmwareType.Meter;
            //Create FirmwareDownload capability
            FirmwareDownloadParameter firmwareDownloadParameter = GetFirmwareDownloadParameter(firmwareType);
            FirmwareDownloadParameter[] firmwareDownloadParameters = { firmwareDownloadParameter };

            FirmwareDownloadCapability firmwareDownloadCapability1 = GetFirmwareDownloadCapability(firmwareDownloadParameters);

            //Create Different capability
            CommandSetCapability commandCapability = new CommandSetCapability();
            FirmwareDownloadCapability mergedFirmwareDownloadCapability = null;
            try
            {
                //Merge capability
                mergedFirmwareDownloadCapability = (FirmwareDownloadCapability)firmwareDownloadCapability1.Merge(commandCapability);
                isMergedOk = true;
            }
            catch (ApplicationException ex)
            {
                //Handled exception occur
                errorMsg = ex.Message;
                isMergedOk = false;
                isHandledException = true;
            }
            catch (Exception ex)
            {
                //Unhandled exception occur
                errorMsg = ex.Message;
                isMergedOk = false;
                isHandledException = false;
            }
            Assert.IsTrue(isHandledException, string.Format("Unhandled merging Exception Message is :{0}", errorMsg));
            Assert.IsFalse(isMergedOk, "Diff. capability type merged with firmware download capability, check firmware download capability merging code.");
        }

        [TestMethod]
        [Description(@"This test is to verify  merging of two FirmwareDownloadCapabilities")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void MergingOfFirmwareDownloadCapability()
        {
            bool isMergedOk = true;
            bool? isHandledException = default(bool?);
            string errorMsg = string.Empty;
            string identifier1 = string.Empty;
            string identifier2 = string.Empty;
            string identifier3 = string.Empty;

            //Create Capability1 with two elements 
            FirmwareDownloadParameter element1 = GetFirmwareDownloadParameter(FirmwareType.Meter);
            identifier1 = element1.FirmwareType.ToString();
            FirmwareDownloadParameter element2 = GetFirmwareDownloadParameter(FirmwareType.Module);
            identifier2 = element2.FirmwareType.ToString();

            FirmwareDownloadParameter[] elementArray1 = { element1, element2 };
            FirmwareDownloadCapability capability1 = GetFirmwareDownloadCapability(elementArray1);
            //Get Elements of capability1 
            ReadOnlyDictionary<string, FirmwareDownloadParameter> capability1Elements = capability1.FirmwareDownloadParameters;

            //Create Capability2 with two elements [ 2nd element is common with Capability1]
            FirmwareDownloadParameter element3 = GetFirmwareDownloadParameter(FirmwareType.DCW);
            identifier3 = element3.FirmwareType.ToString();

            FirmwareDownloadParameter[] elementArray2 = { element2, element3 };
            FirmwareDownloadCapability capability2 = GetFirmwareDownloadCapability(elementArray2);
            //Get Elements of capability2
            ReadOnlyDictionary<string, FirmwareDownloadParameter> capability2Elements = capability2.FirmwareDownloadParameters;

            FirmwareDownloadCapability mergedCapability = null;
            ReadOnlyDictionary<string, FirmwareDownloadParameter> mergedCapabilityElements = null;
            try
            {
                mergedCapability = (FirmwareDownloadCapability)capability1.Merge(capability2);
                mergedCapabilityElements = mergedCapability.FirmwareDownloadParameters;
                isMergedOk = true;
            }
            catch (ApplicationException ex1)
            {
                isHandledException = true;
                isMergedOk = false;
                errorMsg = ex1.Message;
            }
            catch (Exception ex1)
            {
                isHandledException = false;
                isMergedOk = false;
                errorMsg = ex1.Message;
            }
            Assert.IsFalse(isHandledException == false, string.Format("Unhandled merging Exception Message is :{0}", errorMsg));
            Assert.IsTrue(isMergedOk, errorMsg);
            Assert.IsTrue(mergedCapabilityElements[identifier1] == capability1Elements[identifier1], "Merged Firmware Download capability's Command1 should be same as first FirmwareDownloadCapability");
            Assert.IsTrue(mergedCapabilityElements[identifier2] == capability1Elements[identifier2], "Merged Firmware Download capability's Command2 should be same as first FirmwareDownloadCapability");
            Assert.IsTrue(mergedCapabilityElements[identifier2] == capability2Elements[identifier2], "Merged Firmware Download capability's Command2 should be same as second FirmwareDownloadCapability");
            Assert.IsTrue(mergedCapabilityElements[identifier3] == capability2Elements[identifier3], "Merged Firmware Download capability's Command3 should be same as second FirmwareDownloadCapability");
        }

        [TestMethod]
        [Description(@"This test is to verify FirmwareDownloadCapability Build from XML.")]
        [Owner("email:platform@landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void BuildFirmwareDownloadCapabilityFromXML()
        {
            string assertMsg = "FirmwareDownloadCapability Build from XML , {0} property's Current value :{1} and Expected value :{2} mismatch.";
            bool isCapabilityBuildOk = true;
            bool? isHandledException = default(bool?);
            string errorMsg = string.Empty;
            //Expected attribute values of FirmwareDownloadParameter
            FirmwareType expectedFirmwareType = FirmwareType.Meter;
            string expectedFirmwarePlatform = "FP0001";
            string expectedMinVersion = "11.11.111.111";
            bool expectedIsDowngradeable = true;
            bool expectedCanPause = true;
            bool expectedCanAbort = true;
            bool expectedCanChangeActivationDatetime = true;
            bool expectedIsDelayedActivationSupported = false;
            bool expectedCanRollback = true;

            string capabilityxml = String.Format(@"<Capabilities>
                                         <FirmwareDownload>
                                             <FirmwareDownloadParameter 
                                     					firmwareType = '{0}'  
                                     					firmwarePlatform = '{1}'
                                     					minVersion = '{2}' 
                                     					isDowngradable = '{3}' 
                                     					canPause = '{4}'
                                     					canAbort = '{5}' 
                                     					canChangeActivationDateTime = '{6}'  
                                     					isDelayedActivationSupported = '{7}'
                                                         canRollback='{8}' />
                                         </FirmwareDownload >
                                     </Capabilities>", expectedFirmwareType, expectedFirmwarePlatform, expectedMinVersion, expectedIsDowngradeable.ToString(),
                                                    expectedCanPause.ToString(), expectedCanAbort.ToString(), expectedCanChangeActivationDatetime.ToString(), expectedIsDelayedActivationSupported.ToString(),
                                                    expectedCanRollback);

            FirmwareDownloadParameter firmwareDownloadParameter = null;
            FirmwareDownloadCapability firmwawreDownloadCapability = null;
            FirmwareDownloadCapabilityBuilder capabilityBuilder = new FirmwareDownloadCapabilityBuilder();
            try
            {
                firmwawreDownloadCapability = (FirmwareDownloadCapability)capabilityBuilder.BuildCapability(capabilityxml);
                firmwareDownloadParameter = firmwawreDownloadCapability.FirmwareDownloadParameters[expectedFirmwareType.ToString()];
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
            Assert.IsTrue(isCapabilityBuildOk, errorMsg);
            Assert.IsTrue(firmwawreDownloadCapability != null, "Capability build wrong by Builder.");

            //Current attribute values of FirmwareDownloadParameter
            FirmwareType currentFirmwareType = firmwareDownloadParameter.FirmwareType;
            string currentFirmwarePlatform = firmwareDownloadParameter.FirmwarePlatform;
            string currentMinVersion = firmwareDownloadParameter.MinVersion;
            bool currentIsDowngradeable = firmwareDownloadParameter.IsDowngradeable;
            bool currentCanPause = firmwareDownloadParameter.CanPause;
            bool currentCanAbort = firmwareDownloadParameter.CanAbort;
            bool currentCanChangeActivationDatetime = firmwareDownloadParameter.CanChangeActivationDatetime;
            bool currentIsDelayedActivationSupported = firmwareDownloadParameter.IsDelayedActivationSupported;
            bool currentCanRollback = firmwareDownloadParameter.CanRollback;

            //Verifying FirmwareDownloadParameter
            Assert.IsTrue(currentFirmwareType == expectedFirmwareType, string.Format(assertMsg, "FirmwareType", currentFirmwareType.ToString(), expectedFirmwareType.ToString()));
            Assert.IsTrue(currentFirmwarePlatform == expectedFirmwarePlatform, string.Format(assertMsg, "FirmwarePlatform", currentFirmwarePlatform, expectedFirmwareType));
            Assert.IsTrue(currentMinVersion == expectedMinVersion, string.Format(assertMsg, "MinVersion", currentMinVersion, expectedMinVersion));
            Assert.IsTrue(currentIsDowngradeable == expectedIsDowngradeable, string.Format(assertMsg, "IsDowngradeable", currentIsDowngradeable, expectedIsDowngradeable));
            Assert.IsTrue(currentCanPause == expectedCanPause, string.Format(assertMsg, "CanPause", currentCanPause, expectedCanPause));
            Assert.IsTrue(currentCanAbort == expectedCanAbort, string.Format(assertMsg, "CanAbort", currentCanAbort, expectedCanAbort));
            Assert.IsTrue(currentCanChangeActivationDatetime == expectedCanChangeActivationDatetime, string.Format(assertMsg, "CanChangeActivationDatetime", currentCanChangeActivationDatetime, expectedCanChangeActivationDatetime));
            Assert.IsTrue(currentIsDelayedActivationSupported == expectedIsDelayedActivationSupported, string.Format(assertMsg, "IsDelayedActivationSupported", currentIsDelayedActivationSupported, expectedIsDelayedActivationSupported));
            Assert.IsTrue(currentCanRollback == expectedCanRollback, string.Format(assertMsg, "CanRollback", currentCanRollback, expectedCanRollback));

        }

        [TestMethod]
        [Description("This test is to verify FirmwareDownloadCapability Build failed when unexpected XML attributes passed.")]
        [Owner("email:platfrom@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestFirmwareDownloadCapabilityBuildFromUnexpectedXMLAttrb()
        {
            Boolean isCapabilityBuildOk = true;
            bool? isHandledException = default(bool?);
            string errorMsg = string.Empty;

            FirmwareType firmwareType = FirmwareType.Meter;
            string firmwarePlatform = "FP0001";
            string minVersion = "11.11.111.111";
            bool isDowngradeable = true;
            bool canPause = true;
            bool canAbort = true;
            bool canChangeActivationDatetime = true;
            bool isDelayedActivationSupported = false;
            bool canRollback = true;

            string capabilityxml = String.Format(@"<Capabilities>
                                         <FirmwareDownload>
                                             <FirmwareDownloadParameter  extraattrib='A'
                                     					firmwareType = '{0}'  
                                     					firmwarePlatform = '{1}'
                                     					minVersion = '{2}' 
                                     					isDowngradable = '{3}' 
                                     					canPause = '{4}'
                                     					canAbort = '{5}' 
                                     					canChangeActivationDateTime = '{6}'  
                                     					activationType = '{7}'
                                                         canRollback='{8}' />
                                         </FirmwareDownload >
                                     </Capabilities>", firmwareType, firmwarePlatform, minVersion, isDowngradeable.ToString(),
                                                    canPause.ToString(), canAbort.ToString(), canChangeActivationDatetime.ToString(), isDelayedActivationSupported,
                                                    canRollback);

            FirmwareDownloadParameter firmwareDownloadParameter1 = null;
            FirmwareDownloadCapability firmwawreDownloadCapability = null;
            FirmwareDownloadCapabilityBuilder capabilityBuilder = new FirmwareDownloadCapabilityBuilder();
            try
            {
                firmwawreDownloadCapability = (FirmwareDownloadCapability)capabilityBuilder.BuildCapability(capabilityxml);
                firmwareDownloadParameter1 = firmwawreDownloadCapability.FirmwareDownloadParameters[firmwareType.ToString()];
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
            Assert.IsFalse(isCapabilityBuildOk, "Firmware Download Capability builder has been built with unexpected Attributes,Check Builder..");
        }

        [TestMethod]
        [Description("This test is to verify FirmwareDownloadCapabilityHandler Create/Load Capability")]
        [Owner("email:platfrom@landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Integration, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.FirmwareDownload)]
        public void TestCreateCapabilityandLoadIfAlreadyExist()
        {
            DBHelper.CleanDatabase();

            string assertMsg = "{0} property's Current value :{1} and Expected value :{2} mismatch.";

            bool isCapabilityCreated = false;
            //Expected attribute values of FirmwareDownloadParameter
            string identifier = string.Empty;
            FirmwareType expectedFirmwareType = FirmwareType.Meter;
            string expectedFirmwarePlatform = "FP0001";
            string expectedMinVersion = "11.11.111.111";
            bool expectedIsDowngradeable = true;
            bool expectedCanPause = true;
            bool expectedCanAbort = true;
            bool expectedCanChangeActivationDatetime = true;
            bool expectedIsDelayedActivationSupported = false;
            bool expectedCanRollback = true;
            FirmwareDownloadCapabilityHandler handler = new FirmwareDownloadCapabilityHandler();

            //Create capability instance
            FirmwareDownloadParameter firmwareDownloadParameter1 = GetFirmwareDownloadParameter(expectedFirmwareType, expectedFirmwarePlatform, expectedMinVersion, expectedIsDowngradeable,
                                                                                                expectedCanPause, expectedCanAbort, expectedCanChangeActivationDatetime, expectedIsDelayedActivationSupported,
                                                                                                expectedCanRollback);
            identifier = firmwareDownloadParameter1.FirmwareType.ToString();
            FirmwareDownloadParameter[] firmwareDownloadParameters = { firmwareDownloadParameter1 };
            FirmwareDownloadCapability firmwareDownloadCapability1 = GetFirmwareDownloadCapability(firmwareDownloadParameters);

            //Create capability in DB
            CapabilityCrcComputer crcComputer = new CapabilityCrcComputer();
            string crcOfFirmwareDownloadCapability = crcComputer.ComputeCrc(firmwareDownloadCapability1);
            isCapabilityCreated = handler.CreateCapabilityIfNotExists(firmwareDownloadCapability1, DD.CapabilitySource.Device, crcOfFirmwareDownloadCapability);
            Assert.IsTrue(isCapabilityCreated, "FirmwareDownloadCapability not created , Clean Database and retry.");

            //Fetch capability
            FirmwareDownloadCapability firmwareDownloadCapability2 = (FirmwareDownloadCapability)handler.LoadCapability(crcOfFirmwareDownloadCapability);
            Assert.IsTrue(firmwareDownloadCapability2.IsConfigurationDependent == false, "Isconfiguration property of FirmwareDownloadCapability is not correct.");
            Assert.IsTrue(firmwareDownloadCapability2.CapabilityType == DD.CapabilityType.FirmwareDownload, "CapabilityType of FirmwareDownloadCapability is not correct.");

            FirmwareDownloadParameter firmwareDownloadParameter2 = firmwareDownloadCapability2.FirmwareDownloadParameters[identifier];
            //Current attribute values of FirmwareDownloadParameter
            FirmwareType currentFirmwareType = firmwareDownloadParameter2.FirmwareType;
            string currentFirmwarePlatform = firmwareDownloadParameter2.FirmwarePlatform;
            string currentMinVersion = firmwareDownloadParameter2.MinVersion;
            bool currentIsDowngradeable = firmwareDownloadParameter2.IsDowngradeable;
            bool currentCanPause = firmwareDownloadParameter2.CanPause;
            bool currentCanAbort = firmwareDownloadParameter2.CanAbort;
            bool currentCanChangeActivationDatetime = firmwareDownloadParameter2.CanChangeActivationDatetime;
            bool currentIsDelayedActivationSupported = firmwareDownloadParameter2.IsDelayedActivationSupported;
            bool currentCanRollback = firmwareDownloadParameter2.CanRollback;

            //Verifying FirmwareDownloadParameter
            Assert.IsTrue(currentFirmwareType == expectedFirmwareType, string.Format(assertMsg, "FirmwareType", currentFirmwareType.ToString(), expectedFirmwareType.ToString()));
            Assert.IsTrue(currentFirmwarePlatform == expectedFirmwarePlatform, string.Format(assertMsg, "FirmwarePlatform", currentFirmwarePlatform, expectedFirmwareType));
            Assert.IsTrue(currentMinVersion == expectedMinVersion, string.Format(assertMsg, "MinVersion", currentMinVersion, expectedMinVersion));
            Assert.IsTrue(currentIsDowngradeable == expectedIsDowngradeable, string.Format(assertMsg, "IsDowngradeable", currentIsDowngradeable, expectedIsDowngradeable));
            Assert.IsTrue(currentCanPause == expectedCanPause, string.Format(assertMsg, "CanPause", currentCanPause, expectedCanPause));
            Assert.IsTrue(currentCanAbort == expectedCanAbort, string.Format(assertMsg, "CanAbort", currentCanAbort, expectedCanAbort));
            Assert.IsTrue(currentCanChangeActivationDatetime == expectedCanChangeActivationDatetime, string.Format(assertMsg, "CanChangeActivationDatetime", currentCanChangeActivationDatetime, expectedCanChangeActivationDatetime));
            Assert.IsTrue(currentIsDelayedActivationSupported == expectedIsDelayedActivationSupported, string.Format(assertMsg, "IsDelayedActivationSupported", currentIsDelayedActivationSupported, expectedIsDelayedActivationSupported));
            Assert.IsTrue(currentCanRollback == expectedCanRollback, string.Format(assertMsg, "CanRollback", currentCanRollback, expectedCanRollback));
        }

        /// <summary>
        /// To get Raw Data Valued Firmware Download Parameter Instance
        /// </summary>
        /// <returns>Firmware Download Parameter instance</returns>
        private FirmwareDownloadParameter GetFirmwareDownloadParameter(FirmwareType firmwareType, string firmwarePlatform, string minVersion,
                                                                       bool isDowngradeable, bool canPause, bool canAbort, bool canChangeActivationDatetime,
                                                                       bool isDelayedActivationSupported, bool canRollback)
        {

            FirmwareDownloadParameter parameter;
            parameter = new FirmwareDownloadParameter(firmwareType, firmwarePlatform, minVersion, isDowngradeable,
                                                          canPause, canAbort, canChangeActivationDatetime, isDelayedActivationSupported, canRollback);
            return parameter;
        }

        /// <summary>
        /// To get Raw Data Valued Firmware Download Parameter Instance
        /// </summary>
        /// <returns>Firmware Download Parameter instance</returns>
        private FirmwareDownloadParameter GetFirmwareDownloadParameter(FirmwareType firmwareType)
        {
            FirmwareDownloadParameter parameter = new FirmwareDownloadParameter(firmwareType);
            return parameter;
        }

        private FirmwareDownloadCapability GetFirmwareDownloadCapability(FirmwareDownloadParameter[] firmwareDownloadParameters)
        {
            FirmwareDownloadCapability capability = new FirmwareDownloadCapability();
            Dictionary<string, ICapabilityElement> firmwareDownloadParameterSet = new Dictionary<string, ICapabilityElement>();
            foreach (FirmwareDownloadParameter firmwareDownloadParameter in firmwareDownloadParameters)
            {
                firmwareDownloadParameterSet.Add(firmwareDownloadParameter.FirmwareType.ToString(), firmwareDownloadParameter);
            }
            ((IElementsBasedCapability)capability).Elements = firmwareDownloadParameterSet;
            return capability;
        }
    }
}