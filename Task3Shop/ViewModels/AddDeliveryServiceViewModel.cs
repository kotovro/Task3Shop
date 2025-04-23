using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string _totalCars;

        
        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(DeliveryServiceName) &&
                       !(mainWindowViewModel?.GlobalDeliveryServices?.Any(deliveryService =>
                           deliveryService.ServiceName.Equals(DeliveryServiceName, StringComparison.OrdinalIgnoreCase)) ?? false);
                var regexPattern = @"^[1-9]\d{0,4}$";

                var isCarAmountValid = Regex.IsMatch(TotalCars, regexPattern);

                return isNameValid && isCarAmountValid;

            }
        }

        [Required]
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
        public string TotalCars
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
                if (Int32.TryParse(TotalCars, out int count))
                    mainWindowViewModel.GlobalDeliveryServices.Add(new DeliveryService(count, DeliveryServiceName, 5));
                    _thisWindow.Close();
                    mainWindowViewModel.RedrawCanvas();
                    mainWindowViewModel.RaisePropertyChanged(nameof(mainWindowViewModel.IsSimPossible));
            }
        }
    }
}
