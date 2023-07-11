namespace KartRaceAnalyzer.Domain
{
    public class Volta
    {
        public int Numero { get; }
        public TimeSpan Tempo { get; }
        public double VelocidadeMedia { get; }

        public Volta(int numero, TimeSpan tempo, double velocidadeMedia)
        {
            Numero = numero;
            Tempo = tempo;
            VelocidadeMedia = velocidadeMedia;
        }
    }
}
