using System.Text.Json;

using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ManyEntitiesSender.BLL.Services.Implementations
{
    /// <summary>
    /// Класс заполняет базу основываясь на namesObjectsGenerator.json в папке internalFiles
    /// </summary>
    public class RandomObjectGenerator: AbsObjectGenerator
    {
        NamesObjectsGenerator _properties;
        Random _random = new Random(123);
        public RandomObjectGenerator(IPackageContext context, IOptions<PackageSettings> options, ILogger<AbsObjectGenerator> logger) : base(context, options, logger)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "internalFiles", "namesObjectsGenerator.json");
            if (!Path.Exists(path))
                throw new FileNotFoundException("RandomObjectGenerator cannot be used without namesObjectsGenerator.json configuration file");

            NamesObjectsGenerator? result;

            string json = File.ReadAllText(path);
            result = JsonSerializer.Deserialize<NamesObjectsGenerator>(json);


            if (result == null)
                throw new FileLoadException("namesObjectsGenerator.json is null");
            _properties = result;
        }

        protected override Body CreateBody(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count());
            return new Body()
            {
                Mightiness = _properties.Mightiness[selected]
            };
        }

        protected override Hand CreateHand(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count());
            return new Hand()
            {
                State = _properties.State[selected]
            };
        }

        protected override Leg CreateLeg(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count());
            return new Leg()
            {
                State = _properties.State[selected]
            };
        }
    }
}
