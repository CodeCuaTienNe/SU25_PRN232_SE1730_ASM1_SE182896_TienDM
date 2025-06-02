namespace DNATestingSystem.MVCWebApp.FE.TienDM.Models
{
    public class PaginationResult<T> where T : class
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPages { get; set; }
        public int PageSize { get; set; }
        public T Items { get; set; }
    }
}
