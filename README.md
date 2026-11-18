# Test & Taste – Solar System console app

Console app that reads the *bodies* dataset of the [Solar System OpenData API](https://api.le-systeme-solaire.net/en/)
and writes it to the screen and to a CSV file.

## Requirements
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (`dotnet --version` should print 10.x)
- A free API key: <https://api.le-systeme-solaire.net/generatekey.html>

## Setting the API key (never committed)
The key is read from configuration under the name `SOLAR_SYSTEM_API_KEY`. Choose one:

**Environment variable**
```bash
# macOS / Linux / Git Bash
export SOLAR_SYSTEM_API_KEY="your-key"
# Windows PowerShell
$env:SOLAR_SYSTEM_API_KEY = "your-key"
# Windows cmd
set SOLAR_SYSTEM_API_KEY=your-key
```
**Local file** – copy `Test-Taste-Console-Application/appsettings.example.json` to
`Test-Taste-Console-Application/appsettings.local.json` and fill in the key (the file is git-ignored).

## Run
```bash
cd Test-Taste-Console-Application
dotnet run
```
Output: the three existing lists, then the new list *planets with at least one moon + average moon temperature*
on screen and in `FileOutput/AllPlanetsAndTheirAverageMoonTemperature.csv`.

## What changed and why
| Change | Motivation |
|---|---|
| `MoonDto.AvgTemp` (`avgTemp`, Kelvin) and `Moon.AverageTemperature` (`float?`) | The model had no temperature. The API reports *unknown* as `0`; 0 K is impossible, so `0` becomes `null` and is excluded from averages instead of dragging them down. |
| `Planet.AverageMoonTemperature`, `Planet.MoonsWithTemperatureCount` | The average lives on the domain object (as the exercise hint suggests). Returns `null` when no moon has data, so the output shows `n/a` rather than a fake 0. The output also shows "N of M" moons so the reader knows how solid the average is. |
| `HttpClientService`: `Authorization: Bearer` header + `Get()` | API v2 needs a key on every request. The key comes from config; a 401 raises a clear error instead of silently returning "no data". |
| `PlanetService` caches its result | Every call previously re-did one HTTP request per moon; with several outputs that multiplies. Now it is loaded once. A failing moon request is logged and skipped. |
| `IOutputService.OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole`, new `IFileOutputService` / `FileOutputService` | Screen and file output are separate responsibilities (data access → domain objects → output). Existing interface methods are unchanged, so nothing existing breaks. |
| `Program.cs` | Fixed a compile error (`$""..""`), moved service resolution inside `try`, added progress messages ("Loading data…", "Writing data…"). |
| `net5.0` → `net8.0`, packages updated | net5.0 is end-of-life. |

Temperatures are shown in Kelvin (API unit) and °C.
