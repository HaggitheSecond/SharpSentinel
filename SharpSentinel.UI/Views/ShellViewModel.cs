using Caliburn.Micro;

namespace SharpSentinel.UI.Views
{
    public class ShellViewModel : Conductor<Screen>
    {
        public ShellViewModel()
        {
            this.DisplayName = "SharpSentinel Data Viewer";
        }
    }
}