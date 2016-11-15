using PropertyChanged;
using WpfChat.DynamicData.Enums;

namespace WpfChat.DynamicData.ViewModels
{
    [ImplementPropertyChanged]
    public class CatVm : AnimalVm
    {
        public CatVm(string name, AnimalClass animalClass) : base(name, animalClass)
        {
            
        }
    }
}
