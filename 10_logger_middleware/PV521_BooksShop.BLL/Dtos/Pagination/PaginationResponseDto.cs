namespace PV521_BooksShop.BLL.Dtos.Pagination
{
    public class PaginationResponseDto<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int Total { get; set; } = 1;
        public int PageCount { get; set; } = 1;
        public IEnumerable<T> Items { get; set; } = [];
    }
}
