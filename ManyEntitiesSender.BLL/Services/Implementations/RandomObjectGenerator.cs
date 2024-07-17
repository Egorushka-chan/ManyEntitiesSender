using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Entities;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;
using ManyEntitiesSender.BLL.Models;
using System.Net.Http.Headers;

namespace ManyEntitiesSender.BLL.Services.Implementations
{
    /// <summary>
    /// Класс заполняет базу основываясь на namesObjectsGenerator.json в папке InternalConfigs
    /// </summary>
    public class RandomObjectGenerator: AbsObjectGenerator
    {
        NamesObjectsGenerator _properties;
        Random _random = new Random(123);
        public RandomObjectGenerator(IPackageContext context, IOptions<PackageSettings> options, ILogger<AbsObjectGenerator> logger) : base(context, options, logger)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "InternalConfigs", "namesObjectsGenerator.json");
            if (!Path.Exists(path))
                throw new FileNotFoundException("RandomObjectGenerator cannot be used without namesObjectsGenerator.json configuration file");

            NamesObjectsGenerator? result;
            using(var file = File.OpenRead(path))
            {
                result = JsonSerializer.Deserialize<NamesObjectsGenerator>(file);
            }

            if (result == null)
                throw new FileLoadException("namesObjectsGenerator.json is null");
            _properties = result;
        }

        protected override Body CreateBody(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count);
            return new Body()
            {
                Mightiness = _properties.Mightiness[selected]
            };
        }

        protected override Hand CreateHand(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count);
            return new Hand()
            {
                State = _properties.State[selected]
            };
        }

        protected override Leg CreateLeg(int testNo)
        {
            int selected = _random.Next(0, _properties.Mightiness.Count);
            return new Leg()
            {
                State = _properties.State[selected]
            };
        }
    }
}
