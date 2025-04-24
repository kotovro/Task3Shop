using Avalonia.Controls;
using ReactiveUI;
using System.Threading.Tasks;
using Task3Shop.Models;
using Task3Shop.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Avalonia.Media;
using Task3Shop.Toaster;
using ToastNotificationAvalonia.Manager;
using System.Threading;
using Avalonia.Threading;

namespace Task3Shop.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        private readonly Window _mainWindow;
        private Simulation _simulation;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isSimPausePossible;
        private int _currentStep = 0;
        private Tools.Animation _animation;
        public bool IsLogEnabled { get; set; } = true;
        public string LogText { get; set; } = string.Empty;
        public bool IsSimPossible
        {
            get => GlobalCustomers.Count > 0 && GlobalShops.Count > 0 && GlobalDeliveryServices.Count > 0 && !_isSimPausePossible;
        }

        public bool IsSimPausePossible
        {
            get => _isSimPausePossible;
        }
        public bool IsAnyGoodAdded { get; set; } = false;

        public ConcurrentBag<Good> GlobalGoods { get; } = new();
        public ConcurrentBag<Shop> GlobalShops { get; } = new();
        public ConcurrentBag<Customer> GlobalCustomers { get; } = new();
        public ConcurrentBag<DeliveryService> GlobalDeliveryServices { get; } = new();

        public List<IOutOfStockClientStrategy> OutOfStockClientStrategies = [
            new DontBuyAnythingStrategy(),
            new MakeNewOrderAnywhereStrategy(),
            new MakeNewOrderHereStrategy()
        ];
        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _mainWindow.Resized += (sender, args) => RedrawCanvas();
            
            _simulation = new Simulation();
            _simulation.SimulationStepComplete += SimulationStepCompleteHandler;
            _simulation.SimulationFinished += SimulationFinishedHandler;

            _animation = new Tools.Animation(GlobalShops, GlobalCustomers, GlobalDeliveryServices);
        }
        public void ShowLog()
        {
            var simulationLogWindow = new SimulationLogWindow();
            simulationLogWindow.Closed += (sender, args) => { IsLogEnabled = true; this.RaisePropertyChanged(nameof(IsLogEnabled)); };
            simulationLogWindow.DataContext = _mainWindow.DataContext;

            IsLogEnabled = false;
            this.RaisePropertyChanged(nameof(IsLogEnabled));
            simulationLogWindow.Show(_mainWindow);
        }
        public void RedrawCanvas()
        {
            var scaleFactor = 0.05;
            Canvas canvas = _mainWindow.FindControl<Canvas>("Canvas");
            canvas.Children.Clear();
            if (GlobalShops.Count > 0)
            {
                _animation.DrawAllShops(canvas, scaleFactor);
            }
            if (GlobalCustomers.Count > 0)
            {
                _animation.DrawAllCustomers(canvas, scaleFactor);
            }
            if (GlobalDeliveryServices.Count > 0)
            {
                _animation.DrawAllDeliveryServices(canvas, scaleFactor);
                _animation.DrawAllVehicles(canvas, scaleFactor);
            }
        }
        public void RunSimulation()
        {
            _isSimPausePossible = true;
            this.RaisePropertyChanged(nameof(IsSimPausePossible));
            this.RaisePropertyChanged(nameof(IsSimPossible));

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
           
            _mainWindow.FindControl<Button>("RunButton").Content = "Resume";

            Task.Run(() =>
            {
               _simulation.DoSimulation(GlobalShops, GlobalCustomers, GlobalDeliveryServices, GlobalGoods, cancellationToken);
            }, cancellationToken);
        }

        public async void StopSimulation()
        {
            _isSimPausePossible = false;
            this.RaisePropertyChanged(nameof(IsSimPausePossible));
            this.RaisePropertyChanged(nameof(IsSimPossible));
            _cancellationTokenSource.Cancel();
        }

        #region Add EEntities popupa
        public async void AddGood()
        {
            var addGoodWindow = new AddGoodWindow();
            var goodFormVM = new AddGoodViewModel(_mainWindow, addGoodWindow);
            addGoodWindow.DataContext = goodFormVM;

            await addGoodWindow.ShowDialog(_mainWindow);
            this.RaisePropertyChanged(nameof(IsAnyGoodAdded));
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
        #endregion

        #region Event handlers
        private void OnFrame(object sender, EventArgs e)
        {
        }

        public async void HandleDeliveyStarted(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} Start delivery {((DeliveryService)sender).ServiceName}\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"{((DeliveryService)sender).ServiceName} start delivery\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Blue)));
        }

        public async void HandleDeliveryFinished(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} Finish delivery {((DeliveryService)sender).ServiceName}\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"{((DeliveryService)sender).ServiceName} finish delivery\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Blue)));
        }

        public async void HandleDeliveryScheduled(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} Schedule delivery {((DeliveryService)sender).ServiceName}\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"{((DeliveryService)sender).ServiceName} schedule delivery\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Blue)));
        }

        public async void HandleOrderTaken(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} Delivery in progress {((DeliveryService)sender).ServiceName}\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"{((DeliveryService)sender).ServiceName} delivery in progress\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Blue)));
        }

        public async void HandleOutOfStock(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} Out of stock\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"Out of stock\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Red)));
        }

        public async void HandleMakeOrder(object? sender, Order o)
        {
            LogText = $"{DateTime.Now} New order info:\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"New order info:\nCustomer: {o.Customer.Name}\nGood: {o.Good.Name}\nShop: {o.Shop.Name} ", Brushes.Green)));
        }
        public async void SimulationStepCompleteHandler(object? sender, int step)
        {
            _currentStep++;
            if (step % 10 == 0)
            {
                LogText = $"<--- {DateTime.Now} Simulation step {_currentStep} complete --->\n------\n" + LogText;
                this.RaisePropertyChanged(nameof(LogText));

                Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"Step {_currentStep} complete", Brushes.DarkGray)));
            }
            Dispatcher.UIThread.Post(() => RedrawCanvas());
        }

        public async void SimulationFinishedHandler(object? sender, bool isCancelled)
        {
            LogText = $"{DateTime.Now} Simulation {(isCancelled ? "paused" : "finished")}\n------\n" + LogText;
            this.RaisePropertyChanged(nameof(LogText));
            _isSimPausePossible = false;
            this.RaisePropertyChanged(nameof(IsSimPausePossible));
            this.RaisePropertyChanged(nameof(IsSimPossible));

            _cancellationTokenSource.Dispose();

            Dispatcher.UIThread.Post(() => ToastManager.ShowToastAsync(new Toast($"Simulation {(isCancelled ? "paused" : "finished")}", Brushes.DarkGray)));
        }
        #endregion
    }
}
