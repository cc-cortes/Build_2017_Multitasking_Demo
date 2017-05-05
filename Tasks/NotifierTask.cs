using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Enumeration;
using Windows.UI.Notifications;

namespace Tasks
{
    public sealed class NotifierTask : IBackgroundTask
    {
        BackgroundTaskDeferral deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;

            DeviceWatcherTriggerDetails details = (DeviceWatcherTriggerDetails)taskInstance.TriggerDetails;
            var list = details.DeviceWatcherEvents;

            if(list.Count == 0)
            {
                deferral.Complete();
                return;
            }

            DisplayToast("Found new USB Device. Out-Of-Proc Background Task Initiated.");
            deferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //DisplayToast("Background Task Cancelled due to " + reason.ToString());
            deferral.Complete();
        }

        private ToastNotification DisplayToast(String content)
        {
            string xml = $@"<toast activationType='foreground'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                    <text>Build 2017 Multi-tasking</text>
                                                </binding>
                                            </visual>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            var el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el); //Add content to notification

            var toast = new ToastNotification(doc);

            ToastNotificationManager.CreateToastNotifier().Show(toast); //Show the toast

            return toast;
        }
    }
}
