using System;
using Windows.Data.Xml.Dom;
using Windows.Devices.Power;
using Windows.System.Power;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Build2017_Multitasking
{
    static class NotificationHelper
    {
        public static void DisplayBackgroundActivityDialog()
        {
            if(Battery.AggregateBattery.GetReport().Status == BatteryStatus.NotPresent)
            {
                //No Battery means no Battery Usage Page
                DisplayPrivacyDialog();
            }
            else
            {
                DisplayEnergyPolicyDialog();
            }

            
        }

        private static async void DisplayPrivacyDialog()
        {
            string appName = "Build 2017 Multi-tasking";

            string description = "Extended background activity is required to complete your request. " +
                "With the current Privacy user settings this is not possible. " +
                "Please go to the Background Apps Privacy settings page and set this app to be Allowed in the background to complete this action.";

            var messageDialog = new MessageDialog(description, appName);
            messageDialog.Commands.Add(new UICommand("Go to Background Apps", new UICommandInvokedHandler(PrivacyDialogButtonHandler), 1));
            messageDialog.Commands.Add(new UICommand("OK", new UICommandInvokedHandler(PrivacyDialogButtonHandler), 0));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private static async void DisplayEnergyPolicyDialog()
        {
            string appName = "Build 2017 Multi-tasking";

            string description = "Extended background activity is required to complete your request. " +
                "With the current Battery Usage user settings this is not possible. " +
                "Please go to the Battery Usage settings page and set this app to be Always Allowed in the background or Managed By User to complete this action.";

            var messageDialog = new MessageDialog(description, appName);
            messageDialog.Commands.Add(new UICommand("Go to Battery Usage", new UICommandInvokedHandler(EnergyPolicyDialogButtonHandler), 1));
            messageDialog.Commands.Add(new UICommand("OK", new UICommandInvokedHandler(EnergyPolicyDialogButtonHandler), 0));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private static void PrivacyDialogButtonHandler(IUICommand command)
        {
            int commandId = (int)command.Id;
            if (commandId == 1)
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-backgroundapps"));
            }
        }

        private static void EnergyPolicyDialogButtonHandler(IUICommand command)
        {
            int commandId = (int)command.Id;
            if (commandId == 1)
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver-usagedetails"));
            }
        }

        public static ToastNotification DisplayToast(String content)
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
