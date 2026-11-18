using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Test_Taste_Console_Application.Domain.DataTransferObjects;

namespace Test_Taste_Console_Application.Domain.Objects
{
    public class Planet
    {
        public string Id { get; set; }
        public float SemiMajorAxis { get; set; }
        public ICollection<Moon> Moons { get; set; }
        public float AverageMoonGravity
        {
            get => 0.0f;
        }

        /// <summary>
        /// Average of the known moon temperatures in Kelvin; null when the planet has no moon with temperature data.
        /// </summary>
        public float? AverageMoonTemperature
        {
            get
            {
                var temperatures = Moons?
                    .Where(moon => moon.AverageTemperature.HasValue)
                    .Select(moon => moon.AverageTemperature.Value)
                    .ToList();

                return temperatures != null && temperatures.Count > 0 ? temperatures.Average() : (float?)null;
            }
        }

        /// <summary>Number of moons for which the API knows a temperature.</summary>
        public int MoonsWithTemperatureCount => Moons?.Count(moon => moon.AverageTemperature.HasValue) ?? 0;

        public Planet(PlanetDto planetDto)
        {
            Id = planetDto.Id;
            SemiMajorAxis = planetDto.SemiMajorAxis;
            Moons = new Collection<Moon>();
            if(planetDto.Moons != null)
            {
                foreach (MoonDto moonDto in planetDto.Moons)
                {
                    Moons.Add(new Moon(moonDto));
                }
            }
        }

        public Boolean HasMoons()
        {
            return (Moons != null && Moons.Count > 0);
        }
    }
}
