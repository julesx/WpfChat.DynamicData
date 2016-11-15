using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace WpfChat.DynamicData.ViewModels
{
    [ImplementPropertyChanged]
    public class EnumSelectorVm
    {
        public bool IsSelected { get; set; }
        public string Header { get; set; }

        public EnumSelectorVm(Enum @enum)
        {
            Header = @enum.ToString();
        }

        private void OnIsSelectedChanged()
        {
            //MainWindowVm.Instance.FilterAnimals();
        }
    }
}
