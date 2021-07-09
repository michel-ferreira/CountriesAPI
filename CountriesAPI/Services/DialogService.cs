

namespace CountriesAPI.Services
{
    using System.Windows;
    class DialogService
    {
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
