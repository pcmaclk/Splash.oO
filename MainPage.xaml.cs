using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace Splash.oO
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FolderPicker folderPicker;
        private StorageFolder _saveFolder;

        public MainPage()
        {
            this.InitializeComponent();
            MakeSaveFolderAsync();
        }

        private async void MakeSaveFolderAsync()
        {
            if (await KnownFolders.PicturesLibrary.TryGetItemAsync("Splash.oO") == null)
            {
                _saveFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Splash.oO");
            }
            else
            {
                _saveFolder = await KnownFolders.PicturesLibrary.GetFolderAsync("Splash.oO");
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            FolderTextBlock.Text = _saveFolder.Path;
            OptionPage.Visibility = Visibility.Visible;
            this.ShowAnimation.Begin();
        }

        private void CloseOptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideAnimation.Begin();
            this.HideAnimation.Completed += (s, h) => OptionPage.Visibility = Visibility.Collapsed;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            if((Boolean)((ToggleButton)sender).IsChecked)
            {
                InfoPanel.Visibility = Visibility.Visible;
                ShowInfoPanel.Begin();
            }
            else
            {
                HideInfoPanel.Begin();
                HideInfoPanel.Completed += (s, h) => InfoPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void Location_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Location.Opacity = 1;
        }

        private void Location_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Location.Opacity = 0.6;
        }

        private async void FolderButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                FolderTextBlock.Text = folder.Path;
            }
        }
    }
}
