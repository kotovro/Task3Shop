using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Task3Shop.Models;

namespace Task3Shop.ViewModels
{
    public class ShopViewModel : ReactiveObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ObservableCollection<StockItem> Stock { get; set; }

        public ReactiveCommand<Unit, ShopModel> ConfirmCommand { get; }

        public ShopViewModel(ShopModel shopModel)
        {
            Name = shopModel.Name;
            Address = shopModel.Address;
            Stock = new ObservableCollection<StockItem>(shopModel.Stock);
        }
    }

   
}
