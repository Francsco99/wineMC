using SommeliAr.iOS;
using SommeliAr.Views.Menu;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
/*
[assembly: ExportRenderer(typeof(HomePage), typeof(MyPageRenderer))]
namespace SommeliAr.iOS
{
    class MyPageRenderer : PageRenderer
    {

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            //move up Tab
            var tab = this.TabBarController.TabBar;
            var frame = tab.Frame;
            if (this.NavigationController != null)
            {
                frame.Y = 64;
            }
            else
            {
                frame.Y = 0;
            }

            tab.Frame = frame;


            //move down this.View
            var viewFrame = View.Frame;
            viewFrame.Y = tab.Frame.Height;
            View.Frame = viewFrame;

            //color
            tab.BarTintColor = UIColor.Purple;
        }
    }
}
*/