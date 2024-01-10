using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PWRekruter.Enums
{
    public enum TypWynikuEgzaminu
    {
        [Description("Matura OKE")]
        MaturaOKE,
        [Description("Laureat lub finalista olimpiady stopnia centralnego")]
        Olimpiada
    }

    public enum Olimpiada
    {
        Matematyczna,
        Fizyczna,
        Informatyczna,
        [Description("Informacji Technicznych I Wynalazczości")]
        InformacjiTechnicznychIWynalazczosci
    }

    public enum TytulOlimpijczyka
    {
        Finalista,
        Laureat
    }

    public enum PoziomPrzedmiotu
    {
        Podstawowy,
        Rozszerzony
    }

    public enum TypPrzedmiotu
    {
        Matematyka,
        [Description("Fizyka")]
        PrzedmiotDodatkowy_Fizyka,
        [Description("Chemia")]
        PrzedmiotDodatkowy_Chemia,
        [Description("Informatyka")]
        PrzedmiotDodatkowy_Informatyka,
        [Description("Geografia")]
        PrzedmiotDodatkowy_Geografia,
        [Description("Biologia")]
        PrzedmiotDodatkowy_Biologia,
        [Description("Język Polski")]
        JezykPolski,
        [Description("Język Obcy")]
        JezykObcy,
    }
}
