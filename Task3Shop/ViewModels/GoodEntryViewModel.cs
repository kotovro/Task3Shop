using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;

namespace Task3Shop.ViewModels
{
    public class GoodEntryViewModel : ViewModelBase
    {
        public GoodModel GoodModel { get; set; } = new();

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => this.RaiseAndSetIfChanged(ref _quantity, value);
        }

        public GoodEntryViewModel(String name, int quantity)
        {
            GoodModel = new GoodModel(name);
            Quantity = quantity;
        }
    }
}
