using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Build2017_Multitasking
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region Background Tasks

        private void RegisterTasksButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectButton.Visibility = Visibility.Collapsed;
            DisconnectButton.Visibility = Visibility.Visible;

            //BackgroundTaskRegistrationHelper.RegisterAllTasks();
            BackgroundTaskRegistrationHelper.RegisterMyTasks();
        }

        private void UnregisterTasksButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectButton.Visibility = Visibility.Visible;
            DisconnectButton.Visibility = Visibility.Collapsed;

            //BackgroundTaskRegistrationHelper.UnregisterAllTasks();
            BackgroundTaskRegistrationHelper.UnregisterMyTasks();
        }

        #endregion Background Tasks

        #region Audio Playback

        MediaPlayer player;

        private void PlayAudio_Click(object sender, RoutedEventArgs e)
        {
            player = new MediaPlayer();
            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/12Bar.wav"));
            player.Play();


            PlayButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Visible;
        }

        private void StopAudio_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
            player.Dispose();
            player = null;

            PlayButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
        }

        #endregion Audio Playback

        #region Extended Execution

        ExtendedExecutionSession session;

        private async void StartExtendedExecutionButton_Click(object sender, RoutedEventArgs e)
        {
            if(await ExtendedExecutionHelper.IsLongRunning())
            {
                session = await ExtendedExecutionHelper.StartExtendedExecutionSession();

                if(session != null)
                {
                    NotificationHelper.DisplayToast("Extended Execution Begun");
                    session.Revoked += Session_Revoked;

                    CompileButton.Visibility = Visibility.Collapsed;
                    StopCompileButton.Visibility = Visibility.Visible;

                    DoCompilationWork();
                }
                
            }
            else
            {
                NotificationHelper.DisplayBackgroundActivityDialog();
            } 
        }

        private void Session_Revoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            NotificationHelper.DisplayToast("Extended Processing has been halted due to System Policy");

            CompileButton.Visibility = Visibility.Visible;
            StopCompileButton.Visibility = Visibility.Collapsed;
        }

        private void EndExtendedExecutionButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationHelper.DisplayToast("Extended Execution for Compilation has been stopped");
            ExtendedExecutionHelper.EndExtendedExecutionSession(session);

            CompileButton.Visibility = Visibility.Visible;
            StopCompileButton.Visibility = Visibility.Collapsed;
        }

        private void DoCompilationWork() { }

        #endregion Extended Execution
    }
}
