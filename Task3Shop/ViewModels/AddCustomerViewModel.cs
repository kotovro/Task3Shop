using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;

namespace Task3Shop.ViewModels
{
    public class AddCustomerViewModel(Window mainWindow, Window thisWindow) : ViewModelBase
    {
        private Window _mainWindow = mainWindow;
        private Window _thisWindow = thisWindow;

        private string _customerName;
        private string _customerAddress;

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(CustomerName) &&
                       !(mainWindowViewModel?.GlobalCustomerModels?.Any(customer =>
                           customer.Name.Equals(CustomerName, StringComparison.OrdinalIgnoreCase)) ?? false);
                var isAddressValid = !string.IsNullOrWhiteSpace(CustomerAddress) &&
                       !(mainWindowViewModel?.GlobalCustomerModels?.Any(customer =>
                           customer.Address.Equals(CustomerAddress, StringComparison.OrdinalIgnoreCase)) ?? false);

                return isAddressValid && isNameValid;

            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                this.RaiseAndSetIfChanged(ref _customerName, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public string CustomerAddress
        {
            get => _customerAddress;
            set
            {
                this.RaiseAndSetIfChanged(ref _customerAddress, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public void Confirm()
        {
            if (CanConfirm)
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                mainWindowViewModel.GlobalCustomerModels.Add(new CustomerModel(CustomerName, CustomerAddress));
                _thisWindow.Close();
            }
        }
    }
}
