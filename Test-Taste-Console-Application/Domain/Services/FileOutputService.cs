using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services.Interfaces;

namespace Test_Taste_Console_Application.Domain.Services
{
    /// <inheritdoc />
    public class FileOutputService : IFileOutputService
    {
        private readonly IPlanetService _planetService;

        public FileOutputService(IPlanetService planetService)
        {
            _planetService = planetService;
        }

        public string WriteAllPlanetsAndTheirAverageMoonTemperatureToFile()
        {
            Console.WriteLine(OutputString.LoadingData);
            var planets = _planetService.GetAllPlanets().Where(planet => planet.HasMoons()).ToArray();

            Console.WriteLine(OutputString.WritingFile);
            Directory.CreateDirectory(PathName.PathToOutputFolder);
            var path = Path.Combine(PathName.PathToOutputFolder, PathName.AllPlanetsAndTheirAverageMoonTemperatureFile);

            //Invariant culture keeps the decimal separator a dot, so the CSV is the same on every machine.
            var csv = new StringBuilder();
            csv.AppendLine("PlanetId,TotalMoons,MoonsWithTemperature,AverageMoonTemperatureKelvin,AverageMoonTemperatureCelsius");
            foreach (var planet in planets)
            {
                var kelvin = planet.AverageMoonTemperature;
                csv.AppendLine(string.Join(",",
                    planet.Id,
                    planet.Moons.Count,
                    planet.MoonsWithTemperatureCount,
                    kelvin.HasValue ? kelvin.Value.ToString("F2", CultureInfo.InvariantCulture) : string.Empty,
                    kelvin.HasValue ? (kelvin.Value - 273.15f).ToString("F2", CultureInfo.InvariantCulture) : string.Empty));
            }

            File.WriteAllText(path, csv.ToString(), new UTF8Encoding(false));
            Console.WriteLine($"{OutputString.FileCreated}{path}");
            return path;
        }
    }
}
