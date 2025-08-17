using Microsoft.UI.Xaml;
using Microsoft.Windows.Globalization;
using MiniPos.Windows;

namespace MiniPos;

public partial class App : Application
{
    private Window? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // remove this line if you want to add other languages and follow the system language
        ApplicationLanguages.PrimaryLanguageOverride = "es";

        _window = new MainWindow();
        _window.Activate();
    }
}
