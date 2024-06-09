using assetmgmt.data.Context;
using assetmgmt.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace assetmgmt.tests.TestData
{
    public static class DatabaseSeeding
    {
        public static AssetDbContext GetInMemoryDatabase()
        {
            var builder = new DbContextOptionsBuilder<AssetDbContext>();
            builder.UseInMemoryDatabase(databaseName: "AssetMgmtDbInMemory");

            var dbContextOptions = builder.Options;
            var dbContext = new AssetDbContext(dbContextOptions);

            // Delete existing db before creating a new one
            dbContext.Database.EnsureDeletedAsync().Wait();
            dbContext.Database.EnsureCreatedAsync().Wait();

            AddAssetData(dbContext);
            AddSourceData(dbContext);

            return dbContext;
        }

        private static void AddAssetData(AssetDbContext dbContext)
        {
            var asset = Asset.Create("Microsoft Corporation", "MSFT", "US5949181045");
            dbContext.Assets.Add(asset);
            dbContext.SaveChanges();
        }

        private static void AddSourceData(AssetDbContext dbContext)
        {
            var existingAsset = dbContext.Assets.Single(x => x.Symbol == "MSFT");

            var sources = new List<Source>
            {
                Source.Create("NASDAQ", 425.45m, existingAsset),
                Source.Create("Reuters", 423.85m, existingAsset),
                Source.Create("Appreciate Wealth", 423.85m, existingAsset),
                Source.Create("Yahoo Finance", 423.85m, existingAsset),
                Source.Create("Market CapOf", 423.85m, existingAsset),
                Source.Create("Bloomberg", 423.85m, existingAsset),
            };

            dbContext.Sources.AddRange(sources);
            dbContext.SaveChanges();
        }
    }
}
