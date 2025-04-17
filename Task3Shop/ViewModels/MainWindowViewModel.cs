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

namespace Task3Shop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<GoodModel>? GlobalGoodsModels { get; } = new();
        public ObservableCollection<ShopModel>? GlobalShopsModels { get; } = new();
        public ObservableCollection<CustomerModel>? GlobalCustomerModels { get; } = new();
        public ObservableCollection<CustomerModel>? GlobalDeliveryServiceModels { get; } = new();

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
        }
        
        public string Greeting { get; } = "Welcome to Avalonia!";
    }
}
