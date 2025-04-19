using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Task3Shop.Models;
using Task3Shop.Views;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Task3Shop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public List<IOutOfStockClientStrategy> OutOfStockClientStrategies = [
            new DontBuyAnythingStrategy(),
            new MakeNewOrderAnywhereStrategy(),
            new MakeNewOrderHereStrategy()
        ]; 
        private Image _image;
        private double _x = 0;
        public SynchronizedCollection<Good>? GlobalGoods { get; } = new();
        public SynchronizedCollection<Shop>? GlobalShops { get; } = new();
        public SynchronizedCollection<Customer>? GlobalCustomers { get; } = new();
        public SynchronizedCollection<DeliveryService>? GlobalDeliveryServiceModels { get; } = new();

        private readonly Window _mainWindow;
        public  async void AddGood()
        {
            var addGoodWindow = new AddGoodWindow();
            var goodFormVM = new AddGoodViewModel(_mainWindow, addGoodWindow);
            addGoodWindow.DataContext = goodFormVM;

            await addGoodWindow.ShowDialog(_mainWindow);
        }
        public async void AddShop()
        {
            var addShopWindow = new AddShopWindow();
            var shopFormVM = new AddShopViewModel(_mainWindow, addShopWindow);
            addShopWindow.DataContext = shopFormVM;

            await addShopWindow.ShowDialog(_mainWindow);
        }

        public async void AddCustomer()
        {
            var addCustomerWindow = new AddCustomerWindow();
            var customerFormVM = new AddCustomerViewModel(_mainWindow, addCustomerWindow);
            addCustomerWindow.DataContext = customerFormVM;

            await addCustomerWindow.ShowDialog(_mainWindow);
        }

        public async void AddDeliveryService()
        {
            var addDeliveryServiceWindow = new AddDeliveryServiceWindow();
            var deliveryFormVM = new AddDeliveryServiceViewModel(_mainWindow, addDeliveryServiceWindow);
            addDeliveryServiceWindow.DataContext = deliveryFormVM;

            await addDeliveryServiceWindow.ShowDialog(_mainWindow);
        }

        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            
            //_image = new Image
            //{
            //    Source = new Bitmap("avares://YourAppName/Assets/myimage.png"),
            //    Width = 100,
            //    Height = 100
            //};

            //Canvas.SetLeft(_image, _x);
            //Canvas.SetTop(_image, 50);
            //myCanvas.Children.Add(_image); // Assuming x:Name="myCanvas"

            //// Subscribe to frame rendering
            //CompositionTarget.Rendering += OnFrame;

        }

        private void OnFrame(object sender, EventArgs e)
        {
            //_x += 2; // Move to the right
            //Canvas.SetLeft(_image, _x);

            //// Stop after moving off-screen
            //if (_x > Bounds.Width)
            //{
            //    CompositionTarget.Rendering -= OnFrame;
            //}
        }

        public string Greeting { get; } = "Welcome to Avalonia!";
    }


}
