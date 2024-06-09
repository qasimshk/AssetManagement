namespace assetmgmt.data.Entities
{
    public abstract class Entity
    {
        public int Id { get; }

        public DateTime CreatedOn { get; private set; }

        public DateTime ModifiedOn { get; private set; }
    }
}
