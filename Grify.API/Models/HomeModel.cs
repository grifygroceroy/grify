namespace Grify.API.Models
{
    public class HomeModel
    {
        public List<itemsModel> Items { get; set; }
        public List<SubItemModel> SubItems { get; set; }
    }
    public class SubItemModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Image { get; set; }
    }
    public class itemsModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public decimal MRP { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int? StorageLife { get; set; }
        public string Image { get; set; }
        public string Remark { get; set; }
    }
}
