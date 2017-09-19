namespace TaskManager.RegistryCommon
{
    public class PagingParams
    {
        public string OrderBy { get; set; }
        public bool OrderAsc { get; set; }
        public int Page { get; set; }
        public int? ItemsPerPage { get; set; }
    }
}
