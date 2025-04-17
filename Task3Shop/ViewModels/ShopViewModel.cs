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

        public ObservableCollection<GoodEntryViewModel> Stock { get; }

        public ReactiveCommand<Unit, ShopModel> ConfirmCommand { get; }

        public ShopViewModel(
            IEnumerable<GoodEntryViewModel> initialStock)
        {
            Stock = new ObservableCollection<GoodEntryViewModel>(initialStock);

            ConfirmCommand = ReactiveCommand.Create(() =>
            {
                var model = new ShopModel
                (
                    Stock.ToDictionary(x => x.GoodModel, x => x.Quantity),
                    Name,
                    Address
                );
                return model;
            });
        }
    }
}
