using LandisGyr.AMI.Devices.Capabilities.Service.CapabilityMappers;
using LandisGyr.AMI.Devices.Capabilities.Service.Contracts;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public class MockCapabilityMapper : ICapabilityMapper
    {
        /// <summary>
        /// get Mapped CapabilityTO from corresponding MockCapability
        /// </summary>
        /// <param name="cpblty">Capability to be mapped</param>
        /// <returns>CapabilityTO</returns>
        [ExcludeFromCodeCoverageAttribute]
        public BaseCapabilityTO GetMappedCapability(CapabilityBase cpblty)
        {
            BaseCapabilityTO mappedCpblty = null;

            switch (cpblty.CapabilityType)
            {
                case Definitions.CapabilityType.Registers:
                    mappedCpblty = GetMappedRegisterCapability(cpblty);

                    break;
                default:
                    break;
            }

            return mappedCpblty;

        }


        private RegisterSetCapabilityTO GetMappedRegisterCapability(CapabilityBase cpblty)
        {
            RegisterSetCapabilityTO mappedRegisterCpblty = new RegisterSetCapabilityTO();
            mappedRegisterCpblty.Registers = GetMappedRegisters(((MockRegistersCapability)cpblty).Registers);

            return mappedRegisterCpblty;
        }

        private List<RegisterTO> GetMappedRegisters(ReadOnlyDictionary<string, TestLibrary.Register> registers)
        {
            List<RegisterTO> mappedRegisters = registers.Values.ToList().ConvertAll<RegisterTO>(reg => new RegisterTO() { Identifier = reg.Identifier, DataType = reg.DataType });

            return mappedRegisters;
        }

        [ExcludeFromCodeCoverageAttribute]
        public EventGapCollectionAlgoTO GetMappedGapCollectionAlgo(Definitions.EventGapCollectionAlgo eventGapCollectionAlgo)
        {
            throw new System.NotImplementedException();
        }

        [ExcludeFromCodeCoverageAttribute]
        public LoadControlSwitchStatusTO GetMappedLoadControlSwitchStatus(Definitions.LoadControlSwitchStatus? switchStatusValue)
        {
            throw new System.NotImplementedException();
        }

        [ExcludeFromCodeCoverageAttribute]
        public Definitions.CapabilityType GetCapabilityType(CapabilityTypeTO mappedCapabilityType)
        {
            Definitions.CapabilityType capabilityType = default(Definitions.CapabilityType);

            switch (mappedCapabilityType)
            {
                case CapabilityTypeTO.Registers:
                    capabilityType = Definitions.CapabilityType.Registers;
                    break;
                case CapabilityTypeTO.Events:
                    capabilityType = Definitions.CapabilityType.Events;
                    break;
                case CapabilityTypeTO.Commands:
                    capabilityType = Definitions.CapabilityType.Commands;
                    break;
                case CapabilityTypeTO.LoadControlStatus:
                    capabilityType = Definitions.CapabilityType.LoadControlStatus;
                    break;
                case CapabilityTypeTO.LoadProfile:
                    capabilityType = Definitions.CapabilityType.LoadProfile;
                    break;
                case CapabilityTypeTO.DailySnap:
                    capabilityType = Definitions.CapabilityType.DailySnap;
                    break;
                case CapabilityTypeTO.DemandReset:
                    capabilityType = Definitions.CapabilityType.DemandReset;
                    break;
                default:
                    throw new Exception(string.Format("Invalid mappedCapabilityType {0}", mappedCapabilityType));
            }

            return capabilityType;
        }


        public Definitions.CapabilityType CapabilityType
        {
            get { return Definitions.CapabilityType.Registers; }
        }
    }
}
