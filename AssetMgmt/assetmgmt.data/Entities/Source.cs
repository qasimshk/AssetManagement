namespace assetmgmt.data.Entities
{
    public class Source : Entity
    {
        public string Name { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public int AssetId { get; }

        public Asset Asset { get; }

        private Source() { }

        private Source(string name, decimal price, Asset asset)
        {
            Name = name.Trim();
            Price = price;
            AssetId = asset.Id;
        }

        public static Source Create(string name, decimal price, Asset asset) => 
            new(name, price, asset);

        public static Source Update(decimal price, Source source)
        {
            source.Price = price;

            return source;
        }
    }
}
