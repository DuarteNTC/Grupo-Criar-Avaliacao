namespace KartRaceAnalyzer.Domain
{
    public class Piloto
    {
        public string Codigo { get; }
        public string Nome { get; }
        public List<Volta> Voltas { get; }

        public Piloto(string codigo, string nome, List<Volta> voltas)
        {
            Codigo = codigo;
            Nome = nome;
            Voltas = voltas;
        }
    }
}
