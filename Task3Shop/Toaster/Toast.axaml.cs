using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Task3Shop.ViewModels;
using ToastNotificationAvalonia.UserControls;

namespace Task3Shop.Toaster;

public partial class Toast : ToastControl
{    
    public Toast(string text, IBrush color)
    {
        InitializeComponent();
        this.FindControl<Border>("ToastBody").Background = color;
        this.FindControl<TextBlock>("ToastText").Text = text;
    }
}