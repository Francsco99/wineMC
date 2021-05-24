using System;
using CoreGraphics;
using SommeliAr.iOS;
using SommeliAr.Menu;
using SommeliAr.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyPageRenderer))]
namespace SommeliAr.iOS
{
    class MyPageRenderer : TabbedRenderer
    {
        private IPageController PageController => Element as IPageController;


        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                TabBar.Translucent = true;
                TabBar.Opaque = true;
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            var frame = View.Frame;
            PageController.ContainerArea = new Rectangle(0, 0, frame.Width, frame.Height);
            //TabBar.UnselectedItemTintColor = UIColor.Black;
            TabBar.TintColor = UIColor.FromRGB(139,82,255);
            for(int i=0; i<3; i++)
            {
                var item = TabBar.Items[i];
                item.ImageInsets = new UIEdgeInsets(15, 0, -15, 0);
            }
            
        }
    }
}