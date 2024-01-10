namespace PWRekruter.Models
{
    public class ProgPunktowy
    {
        public long Id { get; set; }
        public int Rok {  get; set; }
        public double Wartosc { get; set; }
        public long IdKierunku { get; set; }
        public Kierunek Kierunek { get; set; }


    }
}
