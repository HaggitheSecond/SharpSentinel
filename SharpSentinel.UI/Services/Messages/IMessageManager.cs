using System.Windows;

namespace SharpSentinel.UI.Services.Messages
{
    public interface IMessageManager : IService
    {
        (bool? result, string selectedPath) ShowSelectFolderDialog();

        MessageBoxResult ShowMessageBox(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon);
    }
}