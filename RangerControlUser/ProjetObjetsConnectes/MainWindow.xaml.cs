using Leap;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RangerControlUser
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] imagedata = new byte[1];
        private Controller controller = new Controller();
        WriteableBitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();

            //set greyscale palette for WriteableBitmap object
            List<Color> grayscale = new List<Color>();
            for(byte i = 0; i < 0xff; i++) {
                grayscale.Add( Color.FromArgb( 0xff, i, i, i ) );
            }
            BitmapPalette palette = new BitmapPalette( grayscale );
            bitmap = new WriteableBitmap( 640, 480, 72, 72, PixelFormats.Gray8, palette );
            displayImages.Source = bitmap;

            controller.EventContext = SynchronizationContext.Current;
            controller.FrameReady += newFrameHandler;
            controller.ImageReady += onImageReady;
            controller.ImageRequestFailed += onImageRequestFailed;

            void newFrameHandler( object sender, FrameEventArgs eventArgs )
            {
                Leap.Frame frame = eventArgs.frame;
                //The following are Label controls added in design view for the form
                this.displayID.Content = frame.Id.ToString();
                this.displayTimestamp.Content = frame.Timestamp.ToString();
                this.displayFPS.Content = frame.CurrentFramesPerSecond.ToString();
                this.displayHandCount.Content = frame.Hands.Count.ToString();
                if (frame.Hands.Count > 0) {
                    this.displayIsFist.Content = frame.Hands[0].PinchStrength;
                    this.displayPalmRotation.Content = frame.Hands[0].Rotation;
                }

                controller.RequestImages( frame.Id, Leap.Image.ImageType.DEFAULT, imagedata );
            }

            void onImageRequestFailed( object sender, ImageRequestFailedEventArgs e )
            {
                if(e.reason == Leap.Image.RequestFailureReason.Insufficient_Buffer) {
                    imagedata = new byte[e.requiredBufferSize];
                }
                debugText.AppendText( "Image request failed: " + e.message + "\n" );
            }

            void onImageReady( object sender, ImageEventArgs e )
            {
                bitmap.WritePixels( new Int32Rect( 0, 0, 640, 480 ), imagedata, 640, 0 );
            }
        }
    }
}
