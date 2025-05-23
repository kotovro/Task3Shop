﻿using Avalonia.Controls;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;

namespace Task3Shop.ViewModels
{
    public class AddCustomerViewModel(Window mainWindow, Window thisWindow) : ReactiveObject
    {
        private Window _mainWindow = mainWindow;
        private Window _thisWindow = thisWindow;

        private string _customerName;
       

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var isNameValid = !string.IsNullOrWhiteSpace(CustomerName) &&
                       !(mainWindowViewModel?.GlobalCustomers?.Any(customer =>
                           customer.Name.Equals(CustomerName, StringComparison.OrdinalIgnoreCase)) ?? false);

                return isNameValid;
            }
        }

        [Required]
        public string CustomerName
        {
            get => _customerName;
            set
            {

                this.RaiseAndSetIfChanged(ref _customerName, value);
                this.RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public void Confirm()
        {
            if (CanConfirm)
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var random = new Random();
                var strategy = mainWindowViewModel.OutOfStockClientStrategies.ElementAt(random.Next(mainWindowViewModel.OutOfStockClientStrategies.Count));
                var customer = new Customer(CustomerName, strategy, mainWindowViewModel.GlobalShops);

                mainWindowViewModel.GlobalCustomers.Add(customer);
                mainWindowViewModel.RaisePropertyChanged(nameof(mainWindowViewModel.IsSimPossible));
                mainWindowViewModel.RedrawCanvas();
                mainWindowViewModel.LogText = $"{DateTime.Now} New customer {customer.Name} added\n------\n" + mainWindowViewModel.LogText;
                mainWindowViewModel.RaisePropertyChanged(nameof(mainWindowViewModel.LogText));

                customer.OnMakeOrder += mainWindowViewModel.HandleMakeOrder;
                _thisWindow.Close();

            }
        }
    }
}
