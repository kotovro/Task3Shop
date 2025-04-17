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
        public ObservableCollection<GoodModel>? GlobalGoods { get; } = new();
        public ObservableCollection<ShopModel>? GlobalShops { get; } = new();

        private readonly Window _mainWindow;
        public ObservableCollection<ShopViewModel> Shops { get; } = new();
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


        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }
        
        public string Greeting { get; } = "Welcome to Avalonia!";
    }
}
