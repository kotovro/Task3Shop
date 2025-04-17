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
                       (mainWindowViewModel?.GlobalShopsModels?.Any(shop =>
                           shop.Name.Equals(ShopName, StringComparison.OrdinalIgnoreCase)) == false);
                var isAddressValid = !string.IsNullOrWhiteSpace(ShopAddress) &&
                       (mainWindowViewModel?.GlobalShopsModels?.Any(shop =>
                           shop.Address.Equals(ShopAddress, StringComparison.OrdinalIgnoreCase)) == false);
                
                return isAddressValid && isNameValid;
               
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
                mainWindowViewModel.GlobalShopsModels.Add(new ShopModel(ShopName, ShopAddress));
                _thisWindow.Close();
            }
        }
    }
}
