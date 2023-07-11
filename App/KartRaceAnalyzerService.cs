using System;
using System.Globalization;
using System.Linq;
using KartRaceAnalyzer.Domain;
using KartRaceAnalyzer.Infrastructure;

namespace KartRaceAnalyzer.Application
{
    public class KartRaceAnalyzerService
    {
        private readonly LogReader _logReader;

        public KartRaceAnalyzerService()
        {
            _logReader = new LogReader();
        }

        public void AnalyzeRace(string filePath)
        {
            var pilotos = _logReader.ReadLogFile(filePath);

            // Restante do código permanece igual

            // Encontrar a melhor volta de cada piloto
            Console.WriteLine("Melhor volta de cada piloto:");
            foreach (Piloto piloto in pilotos)
            {
                Volta melhorVolta = piloto.Voltas.OrderBy(v => v.Tempo).First();
                Console.WriteLine($"Piloto: {piloto.Nome} - Volta: {melhorVolta.Numero} - Tempo: {melhorVolta.Tempo}");
            }
            Console.WriteLine();

            // Encontrar a melhor volta da corrida
            Volta melhorVoltaCorrida = pilotos.SelectMany(p => p.Voltas).OrderBy(v => v.Tempo).First();
            Console.WriteLine($"Melhor volta da corrida: Piloto: {pilotos.First(p => p.Voltas.Contains(melhorVoltaCorrida)).Nome} - Volta: {melhorVoltaCorrida.Numero} - Tempo: {melhorVoltaCorrida.Tempo}");
            Console.WriteLine();

            // Calcular a velocidade média de cada piloto durante a corrida
            Console.WriteLine("Velocidade média de cada piloto durante a corrida:");
            foreach (Piloto piloto in pilotos)
            {
                double velocidadeMedia = piloto.Voltas.Select(v => v.VelocidadeMedia).Average();
                Console.WriteLine($"Piloto: {piloto.Nome} - Velocidade Média: {velocidadeMedia:F3}");
            }
            Console.WriteLine();

            // Descobrir quanto tempo cada piloto chegou após o vencedor
            Console.WriteLine("Tempo que cada piloto chegou após o vencedor:");
            Piloto vencedor = pilotos.OrderBy(p => p.Voltas.Count).First();
            double tempoVencedor = vencedor.Voltas.Sum(v => v.Tempo.TotalSeconds);

            foreach (Piloto piloto in pilotos)
            {
                double tempoChegada = piloto.Voltas.Sum(v => v.Tempo.TotalSeconds);
                double tempoAposVencedor = tempoChegada - tempoVencedor;
                TimeSpan tempoAposVencedorTimeSpan = TimeSpan.FromSeconds(tempoAposVencedor);
                Console.WriteLine($"Piloto: {piloto.Nome} - Tempo após o Vencedor: {tempoAposVencedorTimeSpan:mm\\:ss\\.fff}");
            }
        }
    }
}
