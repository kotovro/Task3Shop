using Avalonia.Controls;
using ToastNotificationAvalonia.Manager;

namespace Task3Shop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ToastManager.Initialize(this.FindControl<Canvas>("ToastContainer"));
    }
}