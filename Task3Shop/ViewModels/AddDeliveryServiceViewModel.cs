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
    public class AddDeliveryServiceViewModel(Window mainWindow, Window thisWindow) : ViewModelBase
    {
        private Window _mainWindow = mainWindow;
        private Window _thisWindow = thisWindow;

        private string _deliveyServiceName;
        private string _deliveryServiceAddress;

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(DeliveryServiceName) &&
                       !(mainWindowViewModel?.GlobalDeliveryServiceModels?.Any(deliveryService =>
                           deliveryService.Name.Equals(DeliveryServiceName, StringComparison.OrdinalIgnoreCase)) ?? false);
                var isAddressValid = !string.IsNullOrWhiteSpace(DeliveryServiceAddress) &&
                       !(mainWindowViewModel?.GlobalDeliveryServiceModels?.Any(deliveryService =>
                           deliveryService.Address.Equals(DeliveryServiceAddress, StringComparison.OrdinalIgnoreCase)) ?? false);

                return isAddressValid && isNameValid;

            }
        }

        public string DeliveryServiceName
        {
            get => _deliveyServiceName;
            set
            {
                this.RaiseAndSetIfChanged(ref _deliveyServiceName, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public string DeliveryServiceAddress
        {
            get => _deliveryServiceAddress;
            set
            {
                this.RaiseAndSetIfChanged(ref _deliveryServiceAddress, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public void Confirm()
        {
            if (CanConfirm)
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                mainWindowViewModel.GlobalDeliveryServiceModels.Add(new CustomerModel(DeliveryServiceName, DeliveryServiceAddress));
                _thisWindow.Close();
            }
        }
    }
}
