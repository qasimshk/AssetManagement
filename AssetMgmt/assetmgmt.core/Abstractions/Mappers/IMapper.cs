namespace assetmgmt.core.Abstractions.Mappers
{
    public interface IMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource from);
    }
}
