using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;
using Task3Shop.Views;

namespace Task3Shop.ViewModels
{
    public class AddShopViewModel(Window mainWindow, Window thisWindow) : ViewModelBase
    {
        private Window _mainWindow = mainWindow;
        private Window _thisWindow = thisWindow;
        
        private string _shopName;
        private string _shopAddress;

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(ShopName) &&
                       (mainWindowViewModel?.GlobalShops?.Any(shop =>
                           shop.Name.Equals(ShopName, StringComparison.OrdinalIgnoreCase)) == false);
                var isAddressValid = !string.IsNullOrWhiteSpace(ShopAddress) &&
                       (mainWindowViewModel?.GlobalShops?.Any(shop =>
                           shop.Address.Equals(ShopAddress, StringComparison.OrdinalIgnoreCase)) == false);

                var IsAnyProductAdded = mainWindowViewModel.GlobalGoods.Count > 0;
                return isAddressValid && isNameValid && IsAnyProductAdded;
               
            }
        }

        public string ShopName
        {
            get => _shopName;
            set
            {
                this.RaiseAndSetIfChanged(ref _shopName, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public string ShopAddress
        {
            get => _shopAddress;
            set
            {
                this.RaiseAndSetIfChanged(ref _shopAddress, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public void Confirm()
        {
            if (CanConfirm)
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                Shop temp = new Shop(ShopName, ShopAddress, mainWindowViewModel.GlobalDeliveryServices);
                temp.OnOutOfStock += mainWindowViewModel.HandleOutOfStock;
                FillStock(temp);
                mainWindowViewModel.GlobalShops.Add(temp);
                _thisWindow.Close();
                mainWindowViewModel.RedrawCanvas();
                mainWindowViewModel.RaisePropertyChanged(nameof(mainWindowViewModel.IsSimPossible));
            }
        }

        public void FillStock(Shop shop)
        {
            var stock = new Dictionary<Good, int>();

            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
            foreach (var good in mainWindowViewModel.GlobalGoods)
            {
                stock.Add(good, 10);
            }


            shop.Stock = stock;
        }
    }
}
