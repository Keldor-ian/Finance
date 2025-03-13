namespace FinShark.Queries.Stocks
{
    public class StockQueryObject
    {
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public long? MarketCap { get; set; } 
        public decimal? StockCost { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
