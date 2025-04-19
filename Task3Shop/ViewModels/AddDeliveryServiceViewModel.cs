using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private int _totalCars;

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(DeliveryServiceName) &&
                       !(mainWindowViewModel?.GlobalDeliveryServiceModels?.Any(deliveryService =>
                           deliveryService.ServiceName.Equals(DeliveryServiceName, StringComparison.OrdinalIgnoreCase)) ?? false);


                return isNameValid;

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

        [Required]
        public int TotalCars
        {
            get => _totalCars;
            set
            {
                this.RaiseAndSetIfChanged(ref _totalCars, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public void Confirm()
        {
            if (CanConfirm)
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                mainWindowViewModel.GlobalDeliveryServiceModels.Add(new DeliveryService(TotalCars, DeliveryServiceName));
                _thisWindow.Close();
            }
        }
    }
}
