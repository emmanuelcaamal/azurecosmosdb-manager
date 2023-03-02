using System;
using System.Collections.Generic;

namespace AzureCosmosDbManager.Response
{
    public class PagingResponse<TResult> where TResult : class
    {
        public PagingResponse(int pageSize, int total, List<TResult> values, string continuationToken)
        {
            Items = values;
            PageSize = pageSize;
            TotalCount = total;
            ContinuationToken = continuationToken;
        }

        public string ContinuationToken { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<TResult> Items { get; }
    }
}
