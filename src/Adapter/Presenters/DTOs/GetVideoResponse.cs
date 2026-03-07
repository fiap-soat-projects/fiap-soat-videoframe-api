using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Presenters.DTOs;

public record GetVideoResponse(string Id, string Name, string Path)
{
}
