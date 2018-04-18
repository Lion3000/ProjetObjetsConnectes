/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 17/04/2018
 */

using RangerTools;
using System;
using System.ComponentModel;
using System.Windows;

namespace RangerControlUser
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] imagedata = new byte[1];
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private Config config;
        private CameraBoundary cameraBoundary;
        private RangerControlRobot rangerControlRobot;
        private LeapBoundary leapBoundary;
        //private Controller controller = new Controller();
        //WriteableBitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();
            this.config = new Config();
            this.leapBoundary = LeapBoundary.Instance;
            this.worker.DoWork += worker_DoWork;
            this.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            this.worker.RunWorkerAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.config.getConfig();
                this.cameraBoundary = new CameraBoundary(this, this.config.IpCamera);
                this.rangerControlRobot = new RangerControlRobot(this.config.RangerControlPortName, this.config.RangerControlBaudRate);
                this.cameraBoundary.start();
                this.rangerControlRobot.connect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                this.Close();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Yveau code ici !!!!!!
            //while(true)
            //this.rangerControlRobot.setCommand()
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
                string xml = @"<iframe width='" + SystemParameters.PrimaryScreenWidth * 1.21 + "' height='" + SystemParameters.PrimaryScreenHeight * 1.21 + "' src='http://" + ip + ":8080/jsfs.html' frameborder='0' allow='autoplay; encrypted-media' allowfullscreen></iframe>";
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

        // public MainWindow()
        // {


        // set greyscale palette for WriteableBitmap object
        // List<Color> grayscale = new List<Color>();
        // for(byte i = 0; i < 0xff; i++) {
        // grayscale.Add( Color.FromArgb( 0xff, i, i, i ) );
        // }
        // BitmapPalette palette = new BitmapPalette( grayscale );
        // bitmap = new WriteableBitmap( 640, 480, 72, 72, PixelFormats.Gray8, palette );
        // displayImages.Source = bitmap;

        // controller.EventContext = SynchronizationContext.Current;
        // controller.FrameReady += newFrameHandler;
        // controller.ImageReady += onImageReady;
        // controller.ImageRequestFailed += onImageRequestFailed;

        // void newFrameHandler( object sender, FrameEventArgs eventArgs )
        // {
        // Leap.Frame frame = eventArgs.frame;
        // The following are Label controls added in design view for the form
        // this.displayID.Content = frame.Id.ToString();
        // this.displayTimestamp.Content = frame.Timestamp.ToString();
        // this.displayFPS.Content = frame.CurrentFramesPerSecond.ToString();
        // this.displayHandCount.Content = frame.Hands.Count.ToString();
        // if (frame.Hands.Count > 0) {
        // this.displayIsFist.Content = frame.Hands[0].GrabStrength;
        // this.displayPalmRotation.Content = frame.Hands[0].Rotation.w;
        // }

        // controller.RequestImages( frame.Id, Leap.Image.ImageType.DEFAULT, imagedata );

        // Hand rightHand = null, leftHand = null;
        // for(int i = 0; i < frame.Hands.Count && (leftHand == null || rightHand == null); i++) {
        // if (frame.Hands[i].IsLeft) {
        // if (leftHand == null)
        // leftHand = frame.Hands[i];
        // } else {
        // if (rightHand == null)
        // rightHand = frame.Hands[i];
        // }
        // }

        // if(leftHand != null) {
        // leapBoundary.LeftActive = true;
        // leapBoundary.setHandAttributes(LeapBoundary.HandId.LEFT, leftHand.Rotation.w, leftHand.GrabStrength);
        // } else {
        // leapBoundary.LeftActive = false;
        // }

        // if(rightHand != null) {
        // leapBoundary.RightActive = true;
        // leapBoundary.setHandAttributes( LeapBoundary.HandId.RIGHT, rightHand.Rotation.w, rightHand.GrabStrength );
        // } else {
        // leapBoundary.RightActive = false;
        // }

        // this.mode.Content = Enum.GetName( typeof( LeapBoundary.Modes ), leapBoundary.Mode );
        // }

        // void onImageRequestFailed( object sender, ImageRequestFailedEventArgs e )
        // {
        // if(e.reason == Leap.Image.RequestFailureReason.Insufficient_Buffer) {
        // imagedata = new byte[e.requiredBufferSize];
        // }
        // debugText.AppendText( "Image request failed: " + e.message + "\n" );
        // }

        // void onImageReady( object sender, ImageEventArgs e )
        // {
        // bitmap.WritePixels( new Int32Rect( 0, 0, 640, 480 ), imagedata, 640, 0 );
        // }
        // }
        }
}
