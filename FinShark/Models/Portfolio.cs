namespace FinShark.Models
{
    // Join Table
    // FMPService -> When we add a stock to a portfolio and we haven't created it yet
    // It'll search the API for that stock, if it exists it'll add it to the Stock Table
    // But if another user adds the same stock to their portfolio, it won't add another
    // Entry to the Stock Table (which is what should happen)
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        // Navigation
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }              
    }
}
