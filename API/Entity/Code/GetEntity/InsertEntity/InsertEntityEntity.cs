namespace API.Entity.Code.GetEntity.InsertEntity
{
    public class InsertEntityEntity
    {
        public string Entity { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Descriptions { get; set; }
        public string SecurityToken { get; set; }
    }
}
