using FFMpegCore;
using Microsoft.Win32;
using steamprofile.View;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace steamprofile
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            CustomDialog customDialog = new CustomDialog("Background or Overlay?", "Background", "Overlay", "Select an Option", "Question", () => handleBackground(), () => handleOverlay());
            customDialog.BtnDoneHandler = async () =>
            {
                if (!string.IsNullOrEmpty(tbBackground.Text))
                {
                    string filePath = tbBackground.Text.Replace("Background: ", "");
                    await DisplayImage(filePath, bgPreview);
                }
                if (!string.IsNullOrEmpty(tbOverlay.Text))
                {
                    string filePath = tbOverlay.Text.Replace("Overlay: ", "");
                    await DisplayImage(filePath, ovPreview);
                }
                if (tbOverlay.Text.Length > 0 || tbBackground.Text.Length > 0)
                {
                    btnCrop.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCrop.Visibility = Visibility.Collapsed;
                }
            };
            customDialog.ShowDialog();

        }

        private void handleBackground() {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files | *.png;*.jpg;*.gif";
            fileDialog.Title = "Please select your steam background";
            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                tbBackground.Text = "Background: " + fileDialog.FileName;
            }
        }

        private void handleOverlay()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files | *.png;*.jpg;*.gif";
            fileDialog.Title = "Please select your desired overlay";
            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                tbOverlay.Text = "Overlay: " + fileDialog.FileName;
            }
        }

        public async Task DisplayImage(string filePath, Image targetImage)
        {
            try
            {
                tbStatus.Visibility = Visibility.Visible;

                BitmapImage bitmap = null;
                await Task.Run(() =>
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                });

                ImageBehavior.SetAnimatedSource(targetImage, bitmap);
                tbStatus.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                tbStatus.Visibility = Visibility.Visible;
                MessageBox.Show($"Failed to load image: {ex.Message}");
            }
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCloseW_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaxW_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnMinW_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void btnCrop_Click(object sender, RoutedEventArgs e)
        {
            string background = tbBackground.Text.Replace("Background: ", "");
            string overlay = tbOverlay.Text.Replace("Overlay: ", "");
            OpenFolderDialog folderDialog = new OpenFolderDialog();
            folderDialog.Title = "Select desired output location";
            bool? success = folderDialog.ShowDialog();
            string outputFolder = folderDialog.FolderName;

            if (!File.Exists(background))
            {
                MessageBox.Show("Invalid input image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string bgExt = Path.GetExtension(background).ToLower();
            bool isGif = bgExt == ".gif";
            bool hasOverlay = File.Exists(overlay);

            string composited = isGif
                ? Path.Combine(outputFolder, "composited.mkv")
                : Path.Combine(outputFolder, "composited" + bgExt);

            string output1 = Path.Combine(outputFolder, "crop1" + bgExt);
            string output2 = Path.Combine(outputFolder, "crop2" + bgExt);

            string palette1 = Path.Combine(outputFolder, "palette1.png");
            string palette2 = Path.Combine(outputFolder, "palette2.png");

            try
            {
                tbStatus.Visibility = Visibility.Visible;

                await Task.Run(() =>
                {
                    
                    string overlayFilter;

                    if (hasOverlay)
                    {
                        overlayFilter =
                            "[0:v]scale=1920:1080[bg];" +
                            "[1:v]format=rgba," +
                            "pad=max(iw\\,779):max(ih\\,779):(ow-iw)/2:(oh-ih)/2:color=0x00000000[ov];" +
                            "[bg][ov]overlay=493:256";
                    }
                    else
                    {
                        overlayFilter = "[0:v]scale=1920:1080";
                    }

                    var overlayArgs = FFMpegArguments.FromFileInput(background);

                    if (hasOverlay)
                        overlayArgs = overlayArgs.AddFileInput(overlay);

                    if (isGif)
                    {
                        overlayArgs
                            .OutputToFile(composited, true, opt =>
                                opt.WithCustomArgument(
                                    $"-filter_complex \"{overlayFilter}\" -c:v ffv1"))
                            .ProcessSynchronously();
                    }
                    else
                    {
                        overlayArgs
                            .OutputToFile(composited, true, opt =>
                                opt.WithCustomArgument(
                                    $"-filter_complex \"{overlayFilter}\""))
                            .ProcessSynchronously();
                    }

                    if (isGif)
                    {
                        FFMpegArguments
                            .FromFileInput(composited)
                            .OutputToFile(palette1, true, opt =>
                                opt.WithCustomArgument(
                                    "-vf \"crop=506:779:493:256,palettegen=stats_mode=diff\""))
                            .ProcessSynchronously();

                        FFMpegArguments
                            .FromFileInput(composited)
                            .AddFileInput(palette1)
                            .OutputToFile(output1, true, opt =>
                                opt.WithCustomArgument(
                                    "-filter_complex \"crop=506:779:493:256[x];[x][1:v]paletteuse=dither=sierra2_4a\""))
                            .ProcessSynchronously();
                    }
                    else
                    {
                        FFMpegArguments
                            .FromFileInput(composited)
                            .OutputToFile(output1, true, opt =>
                                opt.WithCustomArgument(
                                    "-vf crop=506:779:493:256"))
                            .ProcessSynchronously();
                    }

                    if (isGif)
                    {
                        FFMpegArguments
                            .FromFileInput(composited)
                            .OutputToFile(palette2, true, opt =>
                                opt.WithCustomArgument(
                                    "-vf \"crop=100:779:1007:256,palettegen=stats_mode=diff\""))
                            .ProcessSynchronously();

                        FFMpegArguments
                            .FromFileInput(composited)
                            .AddFileInput(palette2)
                            .OutputToFile(output2, true, opt =>
                                opt.WithCustomArgument(
                                    "-filter_complex \"crop=100:779:1007:256[x];[x][1:v]paletteuse=dither=sierra2_4a\""))
                            .ProcessSynchronously();
                    }
                    else
                    {
                        FFMpegArguments
                            .FromFileInput(composited)
                            .OutputToFile(output2, true, opt =>
                                opt.WithCustomArgument(
                                    "-vf crop=100:779:1007:256"))
                            .ProcessSynchronously();
                    }

                    if (File.Exists(composited))
                        File.Delete(composited);

                    if (File.Exists(palette1))
                        File.Delete(palette1);

                    if (File.Exists(palette2))
                        File.Delete(palette2);
                });


                await DisplayImage(output1, cropOnePreview);
                await DisplayImage(output2, cropTwoPreview);

                tbStatus.Visibility = Visibility.Collapsed;
                btnCrop.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                tbStatus.Visibility = Visibility.Collapsed;
                MessageBox.Show(ex.Message, "FFmpeg error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}