using System.IO;
using KartRaceAnalyzer.Application;

namespace KartRaceAnalyzer
{
    public class Program
    {
        public static void Main()
        {
            string directoryPath = @"D:\Projetos\Grupo-Criar-Avaliacao\Shared";
            string fileName = "log_corrida.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            KartRaceAnalyzerService analyzerService = new KartRaceAnalyzerService();
            analyzerService.AnalyzeRace(filePath);
        }
    }
}