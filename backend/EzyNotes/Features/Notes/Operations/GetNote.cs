using Ardalis.Result;
using EzyNotes.Features.Notes.Shared;
using EzyNotes.Models;
using EzyNotes.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EzyNotes.Features.Notes.Operations;

public class GetNote : IRequest<Result<NoteResponse>>
{
    [FromRoute] public string NoteId { get; set; }
}

public class GetNoteHandler : IRequestHandler<GetNote, Result<NoteResponse>>
{
    readonly IRepository _repository;

    public GetNoteHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<NoteResponse>> Handle(GetNote request, CancellationToken cancellationToken)
    {
        var note = _repository.Get<Note>(request.NoteId, true);

        if (note is null) return Result.NotFound();

        return new NoteResponse(note);
    }
}
