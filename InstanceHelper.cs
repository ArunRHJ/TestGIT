using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandisGyr.AMI.Layers.DataContracts.ControlEvents.Device.Meter;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public static class InstanceHelper
    {
        
        /// <summary>
        /// This method will be used to get the instance of Common To for load profile.
        /// </summary>
        /// <returns></returns>
        public static LoadProfile GetLoadProfileCommonTOInstance()
        {
            LoadProfile loadProfile = new LoadProfile();
            loadProfile.IntervalLength = 15;
            loadProfile.MeterStorageCapacity = 200;

            List<Channel> channels = GetChannels();

            loadProfile.Channels = channels;

            return loadProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Billing GetDemandResetCommonTOInstance()
        {
            Billing demandReset = new Billing();
            demandReset.Frequency = 15;
            demandReset.MeterStorageCapacity = 200;

            List<Channel> channels = GetChannels();

            demandReset.Channels = channels;

            return demandReset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DailySnap GetDailySnapCommonTOInstance()
        {
            DailySnap dailySnap = new DailySnap();
            dailySnap.Frequency = 1440;
            dailySnap.MeterStorageCapacity = 30;

            List<Channel> channels = GetChannels();

            dailySnap.Channels = channels;

            return dailySnap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<Channel> GetChannels()
        {
            List<Channel> channels = new List<Channel>();

            Channel channel = new Channel();
            channel.ReadingType = "1.2.3.4.5.6.7.8.9";
            channels.Add(channel);
            return channels;
        }
    }
}
