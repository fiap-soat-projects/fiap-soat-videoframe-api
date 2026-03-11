using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Presenters.DTOs;

public record DownloadResponse(string FileName, string ContentType, Stream Content)
{
}
