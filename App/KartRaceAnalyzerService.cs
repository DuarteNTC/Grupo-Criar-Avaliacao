using System;
using System.Collections.Generic;
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

            // Calcular o tempo total de cada piloto durante a corrida
            Dictionary<Piloto, TimeSpan> temposTotais = new Dictionary<Piloto, TimeSpan>();
            foreach (Piloto piloto in pilotos)
            {
                TimeSpan tempoTotal = TimeSpan.Zero;
                foreach (Volta volta in piloto.Voltas)
                {
                    tempoTotal += volta.Tempo;
                }
                temposTotais[piloto] = tempoTotal;
            }

            // Ordenar os pilotos com base no tempo total e gerar o ranking da corrida
            var ranking = temposTotais.OrderBy(x => x.Value).Select((x, index) => new { Posicao = index + 1, Piloto = x.Key, TempoTotal = x.Value });

            // Imprimir o ranking da corrida
            Console.WriteLine("Ranking da corrida:");
            foreach (var item in ranking)
            {
                Console.WriteLine($"Posição: {item.Posicao} - Piloto: {item.Piloto.Nome} - Tempo Total: {item.TempoTotal}");
            }

            Console.WriteLine();

            // Encontrar a melhor volta de cada piloto
            Console.WriteLine("Melhor volta de cada piloto:");
            foreach (Piloto piloto in pilotos)
            {
                Volta melhorVolta = piloto.Voltas.OrderBy(x => x.Tempo).FirstOrDefault();
                if (melhorVolta != null)
                    Console.WriteLine($"Piloto: {piloto.Nome} - Volta: {melhorVolta.Numero} - Tempo: {melhorVolta.Tempo}");
            }
            Console.WriteLine();

            // Encontrar a melhor volta da corrida
            Volta melhorVoltaCorrida = pilotos.SelectMany(p => p.Voltas).OrderBy(x => x.Tempo).FirstOrDefault();
            if (melhorVoltaCorrida != null)
                Console.WriteLine($"Melhor volta da corrida: Piloto: {pilotos.First(x => x.Voltas.Contains(melhorVoltaCorrida)).Nome} - Volta: {melhorVoltaCorrida.Numero} - Tempo: {melhorVoltaCorrida.Tempo}");
            else
                Console.WriteLine("Nenhuma volta registrada na corrida");
            Console.WriteLine();

            // Calcular a velocidade média de cada piloto durante a corrida
            Console.WriteLine("Velocidade média de cada piloto durante a corrida:");
            foreach (Piloto piloto in pilotos)
            {
                if (piloto.Voltas.Count > 0)
                {
                    double velocidadeMedia = piloto.Voltas.Select(x => x.VelocidadeMedia).Average();
                    Console.WriteLine($"Piloto: {piloto.Nome} - Velocidade Média: {velocidadeMedia:F3}");
                }
                else
                {
                    Console.WriteLine($"Piloto: {piloto.Nome} - Não completou a corrida");
                }
            }
            Console.WriteLine();

            // Descobrir quanto tempo cada piloto chegou após o vencedor
            Console.WriteLine("Tempo que cada piloto chegou após o vencedor:");
            Piloto vencedor = pilotos.OrderBy(x => x.Voltas.Count).FirstOrDefault();
            if (vencedor != null)
            {
                double tempoVencedor = vencedor.Voltas.Sum(x => x.Tempo.TotalSeconds);

                foreach (Piloto piloto in pilotos)
                {
                    double tempoChegada = piloto.Voltas.Sum(x => x.Tempo.TotalSeconds);
                    double tempoAposVencedor = tempoChegada - tempoVencedor;
                    TimeSpan tempoAposVencedorTimeSpan = TimeSpan.FromSeconds(tempoAposVencedor);
                    Console.WriteLine($"Piloto: {piloto.Nome} - Tempo após o Vencedor: {tempoAposVencedorTimeSpan:mm\\:ss\\.fff}");
                }
            }
            else
            {
                Console.WriteLine("Nenhum piloto completou a corrida");
            }

            Console.WriteLine();
            // Verificar se houve pilotos que não completaram a corrida
            bool algumNaoCompletou = pilotos.Any(x => x.Voltas.Count < 4);

            if (algumNaoCompletou)
            {
                Console.WriteLine("Pilotos que não completaram a corrida:");
                foreach (Piloto piloto in pilotos.Where(x => x.Voltas.Count < 4))
                {
                    Console.WriteLine($"Piloto: {piloto.Nome}");
                }
            }
        }
    }
}
