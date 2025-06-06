namespace API.Entity.Code.GetEntityDetail
{
    public class UpdateEntityDetailEntity
    {
        public string Entity { get; set; }
        public long EntityId { get; set; }
        public long Id { get; set; }
        public string NewAttribute { get; set; }
        public string NewDescription { get; set; }
        public string SecurityToken { get; set; }
    }
}
