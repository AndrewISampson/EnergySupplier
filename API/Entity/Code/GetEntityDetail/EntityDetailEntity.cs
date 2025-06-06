namespace API.Entity.Code.GetEntityDetail
{
    public class EntityDetailEntity
    {
        public long Id { get; set; }
        public string Attribute { get; set; }
        public string Description { get; set; }
        public string SecurityToken { get; set; }

        public EntityDetailEntity(long id, string attribute, string description)
        {
            Id = id;
            Attribute = attribute;
            Description = description;
        }
    }
}
