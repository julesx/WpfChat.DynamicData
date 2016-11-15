using WpfChat.DynamicData.Enums;

namespace WpfChat.DynamicData.Interfaces
{
    public interface IAnimalVm
    {
        string Name { get; set; }
        AnimalClass AnimalClass { get; set; }
    }
}
