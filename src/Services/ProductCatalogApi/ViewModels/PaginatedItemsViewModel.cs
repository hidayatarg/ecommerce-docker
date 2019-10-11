using System.Collections.Generic;

namespace ProductCatalogApi.ViewModels
{
    public class PaginatedItemsViewModel<TEntity>
    where TEntity: class
    {
        // should be not set from outside
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        // total number of items
        public int Count { get; private set; }
        public IEnumerable<TEntity> Data { get; set; }

        public PaginatedItemsViewModel(int pageIndex, int pageSize, int count, IEnumerable<TEntity> data)
        {
            // set the class variable
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Count = count;
            this.Data = data;
        }
        
    }
}