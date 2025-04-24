using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;

namespace Task3Shop.ViewModels
{
    public class AddGoodViewModel(Window mainWindow, Window thisWindow) : ReactiveObject
    {
        private Window _mainWindow = mainWindow;
        private Window _thisWindow = thisWindow;


         private string _goodName;

        public bool CanConfirm
        {
            get
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                return !string.IsNullOrWhiteSpace(GoodName) &&
                       !(mainWindowViewModel?.GlobalGoods?.Any(good =>
                           good.Name.Equals(GoodName, StringComparison.OrdinalIgnoreCase)) ?? false);
            }
        }

        [Required]
        public string GoodName
        {
            get => _goodName;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _goodName, value);
                this.RaisePropertyChanged(nameof(CanConfirm));    
            }
        }

        public void Confirm() 
        {
            if (CanConfirm) // Or canConfirm() if it's a method
            {
                var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
                var good = new Good(GoodName);
                mainWindowViewModel.GlobalGoods.Add(good);
                mainWindowViewModel.IsAnyGoodAdded = true;

                mainWindowViewModel.LogText = $"{DateTime.Now} New good {good.Name} added\n------\n" + mainWindowViewModel.LogText;
                mainWindowViewModel.RaisePropertyChanged(nameof(mainWindowViewModel.LogText));

                _thisWindow.Close();
            }
        }
    }
}
