using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using EzyNotes.Features.Notes.Shared;
using EzyNotes.Models;
using EzyNotes.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EzyNotes.Features.Notes.Operations;

public class UpdateNote : IRequest<Result<NoteResponse>>
{
    [FromRoute] public string NoteId { get; set; }
    [FromBody] public UpdateNotePayload Payload { get; set; }
}

public class UpdateNotePayload
{
    public string Content { get; set; }
}

public class UpdateNoteValidator : AbstractValidator<UpdateNote>
{
    public UpdateNoteValidator()
    {
        RuleFor(x => x.Payload.Content).NotEmpty();
    }
}


public class UpdateNoteHandler : IRequestHandler<UpdateNote, Result<NoteResponse>>
{
    readonly IRepository _repository;
    readonly IValidator<UpdateNote> _validator;

    public UpdateNoteHandler(IRepository repository, IValidator<UpdateNote> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<NoteResponse>> Handle(UpdateNote request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);
        if (!validator.IsValid) return Result.Invalid(validator.AsErrors());

        var note = _repository.Get<Note>(request.NoteId, true);
        if (note is null) Result.NotFound("Note does not exist!");

        note.Content = request.Payload.Content;

        await _repository.Notes.Update(x => x.Id == request.NoteId, note, cancellationToken);

        return new NoteResponse(note);
    }
}