namespace PV521_BooksShop.BLL.Dtos.Pagination
{
    public class PaginationRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
