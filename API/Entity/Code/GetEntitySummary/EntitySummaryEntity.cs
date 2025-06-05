namespace API.Entity.Code.GetEntitySummary
{
    public class EntitySummaryEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DataTableIdentifier { get; set; }

        public EntitySummaryEntity(long id, string name, string dataTableIdentifier)
        {
            Id = id;
            Name = name;
            DataTableIdentifier = dataTableIdentifier;
        }
    }
}
