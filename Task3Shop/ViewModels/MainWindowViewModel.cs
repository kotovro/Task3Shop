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

        private readonly Window _mainWindow;
        public ObservableCollection<ShopViewModel> Shops { get; } = new();
        public  async void AddGood()
        {
            var addGoodWindow = new AddGoodWindow();
            var goodFormVM = new AddGoodViewModel(_mainWindow, addGoodWindow);
            addGoodWindow.DataContext = goodFormVM;

            await addGoodWindow.ShowDialog(_mainWindow);
        }
        

        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            //AddShopCommand = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    //var shopFormVM = new ShopViewModel(_allGoods.AllGoods);
            //    //var shopFormView = new ShopView { DataContext = shopFormVM };

            //    //var shopModel = await shopFormView.ShowDialog<ShopModel>(
            //    //    (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow
            //    //);

            //    //if (shopModel != null)
            //    //{
            //    //    //var shopVM = new ShopViewModel(
            //    //    //    initialStock: shopModel.Stock.Select(kv => new GoodEntryViewModel
            //    //    //    (
            //    //    //        kv.Key,
            //    //    //        kv.Value
            //    //    //    )));

            //    //    //Shops.Add(shopVM);
            //    //}
            //});

            //AddGoodCommand = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    var goodFormVM = new AddGoodViewModel();
            //    var goodFormView = new AddGoodWindow
            //    {
            //        DataContext = goodFormVM
            //    };

            //    var result = await goodFormView.ShowDialog<GoodModel>(
            //        (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow
            //    );

            //    if (result != null)
            //    {
            //        _allGoods.AllGoods.Add(result);
            //    }
            //});
        }
        
        public string Greeting { get; } = "Welcome to Avalonia!";
    }
}
