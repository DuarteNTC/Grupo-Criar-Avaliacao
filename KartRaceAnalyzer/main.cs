using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KartRaceAnalyzer
{
    public record Piloto(string Codigo, string Nome, IReadOnlyList<Volta> Voltas);

    public record Volta(int Numero, TimeSpan Tempo, double VelocidadeMedia);

    public class KartRaceAnalyzer
    {
        public static void Main()
        {
            // Ler o log de corrida de um arquivo
            List<string> linhasLog = File.ReadAllLines("log_corrida.txt").Skip(1).ToList();

            // Criar uma lista de pilotos imutável
            List<Piloto> pilotos = linhasLog
                .Select(linha =>
                {
                    string[] dados = linha.Split(' ');

                    string codigo = dados[1];
                    string nome = dados[3];
                    int numeroVolta = int.Parse(dados[4]);
                    TimeSpan tempoVolta = TimeSpan.ParseExact(dados[5], @"m\:ss\.fff", CultureInfo.InvariantCulture);
                    double velocidadeMedia = double.Parse(dados[6], CultureInfo.InvariantCulture);

                    Volta volta = new Volta(numeroVolta, tempoVolta, velocidadeMedia);

                    return (codigo, nome, volta);
                })
                .GroupBy(x => x.codigo)
                .Select(x => new Piloto(x.Key, x.First().nome, x.Select(x => x.volta).ToList()))
                .ToList();

            // Restante do código permanece igual

            // Encontrar a melhor volta de cada piloto
            Console.WriteLine("Melhor volta de cada piloto:");
            foreach (Piloto piloto in pilotos)
            {
                Volta melhorVolta = piloto.Voltas.OrderBy(x => x.Tempo).First();
                Console.WriteLine($"Piloto: {piloto.Nome} - Volta: {melhorVolta.Numero} - Tempo: {melhorVolta.Tempo}");
            }
            Console.WriteLine();

            // Encontrar a melhor volta da corrida
            Volta melhorVoltaCorrida = pilotos.SelectMany(x => x.Voltas).OrderBy(x => x.Tempo).First();
            Console.WriteLine($"Melhor volta da corrida: Piloto: {pilotos.First(x => x.Voltas.Contains(melhorVoltaCorrida)).Nome} - Volta: {melhorVoltaCorrida.Numero} - Tempo: {melhorVoltaCorrida.Tempo}");
            Console.WriteLine();
            
            // Calcular a velocidade média de cada piloto durante a corrida
            Console.WriteLine("Velocidade média de cada piloto durante a corrida:");
            foreach (Piloto piloto in pilotos)
            {
                double velocidadeMedia = piloto.Voltas.Select(v => v.VelocidadeMedia).Average();
                Console.WriteLine($"Piloto: {piloto.Nome} - Velocidade Média: {velocidadeMedia.ToString("F3", CultureInfo.InvariantCulture)}");
            }
            Console.WriteLine();

            // Descobrir quanto tempo cada piloto chegou após o vencedor
            Console.WriteLine("Tempo que cada piloto chegou após o vencedor:");
            Piloto vencedor = pilotos.OrderBy(x => x.Voltas.Count).First();
            TimeSpan tempoVencedor = vencedor.Voltas.Sum(x => x.Tempo);

            foreach (Piloto piloto in pilotos)
            {
                TimeSpan tempoChegada = piloto.Voltas.Sum(x => x.Tempo);
                TimeSpan tempoAposVencedor = tempoChegada - tempoVencedor;
                Console.WriteLine($"Piloto: {piloto.Nome} - Tempo após o Vencedor: {tempoAposVencedor.ToString(@"m\:ss\.fff")}");
            }
        }
    }
}