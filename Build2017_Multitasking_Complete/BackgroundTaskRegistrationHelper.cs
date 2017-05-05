using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;

namespace Build2017_Multitasking
{
    static class BackgroundTaskRegistrationHelper
    {
        #region All Tasks

        static string TaskName = "Device Watcher Notifier Task";

        public static async  void RegisterAllTasks()
        {
            await BackgroundExecutionManager.RequestAccessAsync();

            DeviceWatcherEventKind[] triggerEventKinds = { DeviceWatcherEventKind.Add };
            DeviceWatcher dw = DeviceInformation.CreateWatcher(DeviceClass.PortableStorageDevice);
            var trigger = dw.GetBackgroundTrigger(triggerEventKinds);

            var builder = new BackgroundTaskBuilder();
            builder.Name = TaskName;
            builder.IsNetworkRequested = true;
            //builder.TaskEntryPoint = "Tasks.NotifierTask"; //Remove for In-Process
            builder.SetTrigger(trigger);
            builder.Register();
        }

        public static void UnregisterAllTasks()
        {
            foreach (var regPair in BackgroundTaskRegistration.AllTasks)
            {
                regPair.Value.Unregister(true);
            }
        }

        public static void CheckTasks()
        {
            bool isRegistered = false;

            foreach (var regPair in BackgroundTaskRegistration.AllTasks)
            {
                if(regPair.Value.Name == TaskName)
                {
                    isRegistered = true;
                }
            }

            if(isRegistered == false)
            {
                RegisterAllTasks();
            }
        }

        #endregion All Tasks

        #region Grouped Tasks

        private const string MyTaskGroupId = "3F2504E0-4F89-41D3-9A0C-0305E82C3333";
        private const string MyTaskGroupDebugName = "My Task Group";

        public static void RegisterMyTasks()
        {
            DeviceWatcherEventKind[] triggerEventKinds = { DeviceWatcherEventKind.Add };
            DeviceWatcher dw = DeviceInformation.CreateWatcher(DeviceClass.PortableStorageDevice);
            var trigger = dw.GetBackgroundTrigger(triggerEventKinds);

            var builder = new BackgroundTaskBuilder();
            builder.Name = TaskName;
            builder.IsNetworkRequested = true;
            builder.SetTrigger(trigger);
            builder.TaskGroup = GetMyGroup(); //Add to enable grouping, along with event registration in constructor
            builder.Register();
        }

        public static void UnregisterMyTasks()
        {
            var group = GetMyGroup();

            foreach (var regPair in group.AllTasks)
            {
                regPair.Value.Unregister(true);
            }

        }

        public static BackgroundTaskRegistrationGroup GetMyGroup()
        {
            var group  = BackgroundTaskRegistration.GetTaskGroup(MyTaskGroupId);
            
            if(group == null)
            {
                group = new BackgroundTaskRegistrationGroup(MyTaskGroupId, MyTaskGroupDebugName);
            }

            return group;
        }

        #endregion Grouped Tasks
    }
}
