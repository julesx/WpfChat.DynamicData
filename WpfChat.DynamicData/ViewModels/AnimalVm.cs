using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;
using WpfChat.DynamicData.Annotations;
using WpfChat.DynamicData.Enums;
using WpfChat.DynamicData.Interfaces;

namespace WpfChat.DynamicData.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class AnimalVm : IAnimalVm
    {

        public string Name { get; set; }
        public AnimalClass AnimalClass { get; set; }

        protected AnimalVm(string name, AnimalClass animalClass)
        {
            Name = name;
            AnimalClass = animalClass;
        }




    }
}
