namespace Test_Taste_Console_Application.Domain.Services.Interfaces
{
    public interface IFileOutputService
    {
        /// <summary>Writes planets with at least one moon and their average moon temperature to a CSV file.</summary>
        /// <returns>The full path of the created file.</returns>
        string WriteAllPlanetsAndTheirAverageMoonTemperatureToFile();
    }
}
