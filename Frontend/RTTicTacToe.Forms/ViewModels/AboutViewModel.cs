using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace RTTicTacToe.Forms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/zleao/RTTicTacToe")));
        }

        public ICommand OpenWebCommand { get; }
    }
}