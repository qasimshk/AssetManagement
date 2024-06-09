namespace assetmgmt.data.Entities
{
    public class Asset : Entity
    {
        private readonly List<Source> _sources = [];

        public string Name { get; private set; } = string.Empty;

        public string Symbol { get; private set; } = string.Empty;

        public string ISIN { get; private set; } = string.Empty;

        public IReadOnlyList<Source> Sources  => _sources.AsReadOnly();

        private Asset() { }

        private Asset(string name, string symbol, string isin)
        {
            Name = name.Trim();
            Symbol = symbol.Trim();
            ISIN = isin.Trim();
        }

        public static Asset Create(string name, string symbol, string isin) => 
            new(name, symbol, isin);

        public static Asset Update(string? name, string? symbol, string? isin, Asset asset)
        {
            if (!string.IsNullOrEmpty(name))
            {
                asset.Name = name;
            }

            if (!string.IsNullOrEmpty(symbol))
            {
                asset.Symbol = symbol;
            }

            if (!string.IsNullOrEmpty(isin))
            {
                asset.ISIN = isin;
            }

            return asset;
        }
    }
}
