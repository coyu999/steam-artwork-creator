using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace steamprofile.View
{
    public static class MBIcons
    {
        public static BitmapSource Question =>
        Imaging.CreateBitmapSourceFromHIcon(
            SystemIcons.Question.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

        public static BitmapSource Warning =>
            Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Warning.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

        public static BitmapSource Error =>
            Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Error.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

        public static BitmapSource Information =>
            Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Information.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
    }
}
