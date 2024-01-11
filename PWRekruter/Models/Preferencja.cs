namespace PWRekruter.Models
{
    public enum WynikAplikacji
    {
        Zakwalfikowano,
        Odrzucono
    }
    public class Preferencja
    {
        public long Id { get; set; }
        public int Priorytet { get; set; }
        public double WartoscWskaznika { get; set; }
        public WynikAplikacji? Wynik {  get; set; }
        public long IdKierunku { get; set; }
        public Kierunek Kierunek { get; set; }
        public long IdAplikacji {  get; set; }
        public Aplikacja Aplikacja { get; set; }
    }
}
