using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WpfChat.DynamicData.Annotations;
using WpfChat.DynamicData.Enums;

namespace WpfChat.DynamicData.ViewModels
{
    [ImplementPropertyChanged]
    public class EnumSelectorVm: INotifyPropertyChanged
    {

        //implement INotifyPropertyChanged so dynamic data can responsd to property changes
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected { get; set; }
        public string Header { get; private set; }

        public AnimalClass AnimalClass { get; private set; }

        public EnumSelectorVm(AnimalClass animalClass)
        {
            AnimalClass = animalClass;
            Header = animalClass.ToString();
        }

        private void OnIsSelectedChanged()
        {
            //MainWindowVm.Instance.FilterAnimals();
        }


    }
}
