using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Presenters.DTOs;

public record PaginationRequest(string Skip, string Limit)
{

}
