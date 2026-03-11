using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Presenters.DTOs;

public record GetVideoEditResponse(string Id, string Recipient, string Type, string Status, string VideoId)
{
}
