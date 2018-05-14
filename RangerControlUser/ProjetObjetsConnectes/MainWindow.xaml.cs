/* ======================================================
 * Auteurs : Alex Zarzitski, Ivaylo Dimitrov
 * Date : 13/05/2018
 */

using RangerTools;
using Leap;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading;

namespace RangerControlUser
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private RangerTools.Config config;
        private CameraBoundary cameraBoundary;
        private RangerControlRobot rangerControlRobot;
        private LeapBoundary leapBoundary;
        private Controller controller = new Controller();
        ControlStates state = ControlStates.ALL_STOP, lastState;

        public MainWindow()
        {
            InitializeComponent();
            this.config = new RangerTools.Config();
            this.leapBoundary = LeapBoundary.Instance;

            controller.EventContext = SynchronizationContext.Current;
            controller.FrameReady += newFrameHandler;

            void newFrameHandler(object sender, FrameEventArgs eventArgs) {
                Frame frame = eventArgs.frame;

                Hand rightHand = null, leftHand = null;
                for (int i = 0; i < frame.Hands.Count && (leftHand == null || rightHand == null); i++) {
                    if (frame.Hands[i].IsLeft) {
                        if (leftHand == null)
                            leftHand = frame.Hands[i];
                    }
                    else {
                        if (rightHand == null)
                            rightHand = frame.Hands[i];
                    }
                }

                if (leftHand != null) {
                    leapBoundary.LeftActive = true;
                    leapBoundary.setHandAttributes(LeapBoundary.HandId.LEFT, leftHand.Rotation.w, leftHand.GrabStrength);
                }
                else {
                    leapBoundary.LeftActive = false;
                }

                if (rightHand != null) {
                    leapBoundary.RightActive = true;
                    leapBoundary.setHandAttributes(LeapBoundary.HandId.RIGHT, rightHand.Rotation.w, rightHand.GrabStrength);
                }
                else {
                    leapBoundary.RightActive = false;
                }

                this.mode.Text = Enum.GetName(typeof(ControlStates), leapBoundary.toControlState());
                state = leapBoundary.toControlState();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.config.getConfig();
                this.cameraBoundary = new CameraBoundary(this, this.config.IpCamera);
                this.rangerControlRobot = new RangerControlRobot(this.config.RangerControlPortName);
                this.cameraBoundary.start();
                this.rangerControlRobot.connect();
                this.worker.DoWork += worker_DoWork;
                this.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                this.worker.RunWorkerAsync();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                this.Close();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (lastState != state) {
                    this.rangerControlRobot.setCommand(ControlStates.ALL_STOP);
                    this.rangerControlRobot.setCommand(state);
                }
                lastState = state;
                Thread.Sleep(100);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
        }

        public void showVideo(string ip)
        {
            Dispatcher.Invoke((Action)delegate
            {
                // faire qqch avec ton TextBlock
                string xml = @"<iframe  width='" + SystemParameters.PrimaryScreenWidth * 1 + "' height='" + SystemParameters.PrimaryScreenHeight * 1 + "' src='http://" + ip + ":8080/jsfs.html' frameborder='0' allow='autoplay; encrypted-media' allowfullscreen></iframe>";
                myWebView.NavigateToString(xml);
            });
        }

        public void hideVideo()
        {
            Dispatcher.Invoke((Action)delegate
            {
                string xml = @" ";
                myWebView.NavigateToString(xml);
            });
        }
    }
}
