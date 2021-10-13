using Bogus;
using ELKApi.Dtos;
using ELKApi.Enumerations;

namespace ELKApi.Tests.Utils
{
    public static class FakerUtils
    {
        public static Faker<LogDto> LogDtoFaker => new Faker<LogDto>()
            .RuleFor(r => r.Fields, f => FieldsFaker.Generate());

        public static Faker<Fields> FieldsFaker => new Faker<Fields>()
            .RuleFor(r => r.Application, f => "unit_test")
            .RuleFor(r => r.Environnement, f => "unit_test")
            .RuleFor(r => r.Level, f => f.PickRandom<LogLevel>().ToString())
            .RuleFor(r => r.Message, f => f.Random.String2(2));
    }
}
