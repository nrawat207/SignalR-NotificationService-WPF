using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace JobNotificationsClient.Infrastructure
{
    public static class DialogService
    {
        public static async Task<MessageDialogResult> ShowMessage(string message, MessageDialogStyle dialogStyle, string dialogTitle = null, string affirmativeButtonText = null)
        {
            var window = (Application.Current.MainWindow as MetroWindow);
            window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            window.MetroDialogOptions.AffirmativeButtonText = !string.IsNullOrEmpty(affirmativeButtonText) ? affirmativeButtonText : "OK";
            return await window.ShowMessageAsync(dialogTitle, message, dialogStyle, window.MetroDialogOptions);
        }
        public static void ShowDialog(MetroWindow window, string message, MessageDialogStyle dialogStyle, string dialogTitle = null, string affirmativeButtonText = null)
        {
            window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            window.MetroDialogOptions.AffirmativeButtonText = !string.IsNullOrEmpty(affirmativeButtonText) ? affirmativeButtonText : "OK";
            window.ShowDialog();
        }

    }
}
