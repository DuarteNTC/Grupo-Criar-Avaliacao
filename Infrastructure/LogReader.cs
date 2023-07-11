using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using KartRaceAnalyzer.Domain;

namespace KartRaceAnalyzer.Infrastructure
{
    public class LogReader
    {
        public List<Piloto> ReadLogFile(string filePath)
        {
            List<string> linhasLog = File.ReadAllLines(filePath).Skip(1).ToList();

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

                    return new Piloto(codigo, nome, new List<Volta> { volta });
                })
                .GroupBy(x => x.Codigo)
                .Select(x => new Piloto(x.Key, x.First().Nome, x.SelectMany(x => x.Voltas).ToList()))
                .ToList();

            return pilotos;
        }
    }
}
