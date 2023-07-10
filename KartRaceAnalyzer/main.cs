using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KartRaceAnalyzer
{
    public class Piloto
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public List<Volta> Voltas { get; set; }

        public Piloto()
        {
            Voltas = new List<Volta>();
        }
    }

    public class Volta
    {
        public int Numero { get; set; }
        public TimeSpan Tempo { get; set; }
        public double VelocidadeMedia { get; set; }
    }

    public class KartRaceAnalyzer
    {
        public static void Main()
        {
            // Ler o log de corrida de um arquivo
            List<string> linhasLog = File.ReadAllLines("log_corrida.txt").Skip(1).ToList();

            // Criar uma lista de pilotos
            List<Piloto> pilotos = new List<Piloto>();

            foreach (string linha in linhasLog)
            {
                string[] dados = linha.Split(' ');

                string hora = dados[0];
                string codigo = dados[1];
                string nome = dados[3];
                int numeroVolta = int.Parse(dados[4]);

                // Rever o CultureInfo.InvariantCulture
                TimeSpan tempoVolta = TimeSpan.ParseExact(dados[5], @"m\:ss\.fff", CultureInfo.InvariantCulture);

                double velocidadeMedia = double.Parse(dados[6], CultureInfo.InvariantCulture);

                // Verificar se o piloto já existe na lista, caso contrário, adicioná-lo
                Piloto piloto = pilotos.FirstOrDefault(x => x.Codigo == codigo);

                if (piloto == null)
                {
                    piloto = new Piloto { Codigo = codigo, Nome = nome };
                    pilotos.Add(piloto);
                }

                // Adicionar a volta ao piloto
                Volta volta = new Volta { Numero = numeroVolta, Tempo = tempoVolta, VelocidadeMedia = velocidadeMedia };
                piloto.Voltas.Add(volta);
            }
        }
    }
}