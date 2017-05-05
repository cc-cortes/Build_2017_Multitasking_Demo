using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Devices.Power;
using Windows.System.Power;

namespace Build2017_Multitasking
{
    static class ExtendedExecutionHelper
    {
        public static async Task<bool> IsLongRunning()
        {
            var batteryStatus = Battery.AggregateBattery.GetReport().Status;
            var backgroundStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if ((batteryStatus == BatteryStatus.NotPresent || batteryStatus == BatteryStatus.Charging || batteryStatus == BatteryStatus.Idle) && backgroundStatus != BackgroundAccessStatus.DeniedByUser)
            {
                return true;
            }
            else if (batteryStatus == BatteryStatus.Discharging && backgroundStatus == BackgroundAccessStatus.AlwaysAllowed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<ExtendedExecutionSession> StartExtendedExecutionSession()
        {
            ExtendedExecutionSession session = new ExtendedExecutionSession();
            session.Description = "Running while minimized";
            session.Reason = ExtendedExecutionReason.Unspecified;
            
            switch (await session.RequestExtensionAsync())
            {
                case ExtendedExecutionResult.Allowed:
                    return session;
                case ExtendedExecutionResult.Denied:
                    return null;
                default:
                    return null;
            }
        }

        public static void EndExtendedExecutionSession(ExtendedExecutionSession session)
        {
            if(session == null)
            {
                return;
            }

            session.Dispose();
        }

    }
}
