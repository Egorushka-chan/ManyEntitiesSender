using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ManyEntitiesSender.DAL
{
    public class TestObjectsGenerator(IPackageContext context, IOptions<PackageSettings> options) : IObjectGenerator
    {
        public async Task EnsurePackageCount()
        {
            int quantity = options.Value.PackageTotal;
            int count = await context.Packages.CountAsync();
            int required = quantity - count;
            int package = options.Value.PackageCount;
            int iterations = required/package;

            if (required > 0) {
                foreach (int iteration in Enumerable.Range(1, iterations + 1)) {
                    List<Package> packages = new List<Package>();

                    foreach (int i in Enumerable.Range(0, package)) {
                        int testNo = i * iteration;
                        packages.Add(new Package() {
                            Key = $"Test Key #{testNo}",
                            Data = $"Test Data #{testNo}"
                        });
                    }

                    await context.Packages.AddRangeAsync(packages);
                    await context.Context.SaveChangesAsync();
                }
            }
        }
    }
}
