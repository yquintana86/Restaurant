using Application.Abstractions.Messaging;
using Microsoft.SqlServer.Server;
using SharedLib.Models.Common;
using System.Collections.Generic;

namespace Application.Shifts.Queries.GetShiftByLookup;

public sealed record GetShiftsQuery() : IQuery<List<SelectItem>>;

