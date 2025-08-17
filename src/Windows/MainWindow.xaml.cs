using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace MiniPos.Windows;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        ExtendsContentIntoTitleBar = true;        
        InitializeComponent();
    }

    private void OnWindowActivated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated) return;

        OverlappedPresenter presenter = (OverlappedPresenter)AppWindow.Presenter;
        presenter.Maximize();
        //presenter.IsResizable = false;
        //presenter.IsMaximizable = false;
        //presenter.IsMinimizable = false;
    }
}
