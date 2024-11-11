namespace Application.Wrappers
{
    public class PagedResponse<T> : Response<T> where T : class
<<<<<<< HEAD
    {    
        public PagedResponse()
        {

=======
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResponse()
        {
            
        }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalItems)
        {
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
            Message = "Dados recuperados com sucesso.";
            Succeeded = true;
            Errors = null;
        }

        public PagedResponse(List<string> errors, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = null;
            Message = "Ops. Ocorreu um erro ao recuperar os dados.";
            Errors = errors;
            Succeeded = false;
            TotalPages = 0;
            TotalItems = 0;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        }
    }
}
