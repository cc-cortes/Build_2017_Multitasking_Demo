using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Enumeration;
using Windows.UI.Notifications;

namespace Build2017_Multitasking
{
    class NotifierTask : IBackgroundTask
    {
        BackgroundTaskDeferral deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;

            DeviceWatcherTriggerDetails details = (DeviceWatcherTriggerDetails)taskInstance.TriggerDetails;
            var list = details.DeviceWatcherEvents;

            if (list.Count == 0)
            {
                deferral.Complete();
                return;
            }

            NotificationHelper.DisplayToast("Found new USB Device. In-Proc Background Task Initiated.");
            deferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //NotificationHelper.DisplayToast("Background Task Cancelled due to " + reason.ToString());
            deferral.Complete();
        }
    }
}
