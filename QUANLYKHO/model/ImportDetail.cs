namespace QUANLYKHO.model
{
    public class ImportDetail
    {
        public int Id { get; set; }
        public int ImportId { get; set; }
        public Import Import { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}