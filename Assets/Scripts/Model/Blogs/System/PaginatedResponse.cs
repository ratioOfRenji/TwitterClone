using Newtonsoft.Json;
using System.Collections.Generic;

public class PaginatedResponse<T>
{
	[JsonProperty("data")]
	public List<T> Data { get; set; }

	[JsonProperty("pagination")]
	public PaginationInfo Pagination { get; set; }
}
