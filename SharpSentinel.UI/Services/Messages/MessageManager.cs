using System.Windows;
using Ookii.Dialogs.Wpf;

namespace SharpSentinel.UI.Services.Messages
{
    public class MessageManager : IMessageManager
    {
        public (bool? result, string selectedPath) ShowSelectFolderDialog()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                ShowNewFolderButton = false
            };
            var result = dialog.ShowDialog();

            return (result, dialog.SelectedPath);
        }

        public MessageBoxResult ShowMessageBox(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }
    }
}