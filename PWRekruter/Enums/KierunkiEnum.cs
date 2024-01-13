using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Enums
{
    public enum StopienStudiow
    {
        [Description("I stopień")]
        Istopien,
        [Description("II stopień")]
        IIstopien
    }
    public enum TrybStudiow
    {
        Dzienne,
        Zaoczne
    }
    public enum FormaStudiow
    {
        [Description("stacjonarne")]
        Stacjonarne,
        [Description("niestacjonarne")]
        Niestacjonarne
    }
    public enum ProfilKierunku
    {
        Ogolnoakademicki,
        Praktyczny
    }
    public enum Jezyk
    {
        Polski,
        Angielski
    }
}
