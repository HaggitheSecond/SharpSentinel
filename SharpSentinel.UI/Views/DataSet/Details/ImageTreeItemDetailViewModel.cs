using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class ImageTreeItemDetailViewModel : TreeItemDetail
    {
        private ImageSource _image;

        public ImageSource Image
        {
            get { return this._image; }
            set { this.Set(ref this._image, value); }
        }

        public ImageTreeItemDetailViewModel() 
            : base(4, PackIconModernKind.Image)
        {
        }

        public void Initialize(string fileLocation)
        {
            this.Image = new BitmapImage(new Uri(fileLocation));
        }

        public override string GetDisplayName()
        {
            return "Image";
        }
    }
}