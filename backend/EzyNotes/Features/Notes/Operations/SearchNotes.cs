using Ardalis.Result;
using EzyNotes.Features.Notes.Shared;
using EzyNotes.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EzyNotes.Features.Notes.Operations;

public class SearchNotes : IRequest<Result<SearchResponse<NoteResponse>>>
{
    [FromQuery] public int Page { get; set; } = 1;
    [FromQuery] public int Limit { get; set; } = 10;
}

public class SearchResponse<TResult>
{
    public ICollection<TResult> Results { get; set; }

    public long TotalRecords { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}

public class SearchNotesHandler : IRequestHandler<SearchNotes, Result<SearchResponse<NoteResponse>>>
{
    readonly IRepository _repository;

    public SearchNotesHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<SearchResponse<NoteResponse>>> Handle(SearchNotes request, CancellationToken cancellationToken)
    {
        var filter = _repository.Notes.Filter;
        var filters = filter.Eq(x => x.UserId, _repository.GetLoggedInUserId());

        var (result, total) = await _repository.Notes.Search(
            filters,
            _repository.Notes.Sorter.Descending(x => x.CreatedOn),
            request.Page,
            request.Limit
        );

        return new SearchResponse<NoteResponse>
        {
            Results = result.Select(x => new NoteResponse(x)).ToArray(),
            TotalRecords = total,
            Page = request.Page,
            Limit = request.Limit
        };
    }
}