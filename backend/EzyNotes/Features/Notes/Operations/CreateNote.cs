using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using EzyNotes.Features.Notes.Shared;
using EzyNotes.Services;
using FluentValidation;
using MediatR;

namespace EzyNotes.Features.Notes.Operations;

public class CreateNote : IRequest<Result<NoteResponse>>
{
    public string Content { get; set; }
}

public class CreateNoteValidator : AbstractValidator<CreateNote>
{
    public CreateNoteValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Note has missing content!");
    }
}

public class CreateNoteHandler : IRequestHandler<CreateNote, Result<NoteResponse>>
{
    private readonly IRepository _repository;
    readonly IValidator<CreateNote> _validator;

    public CreateNoteHandler(IRepository repository, IValidator<CreateNote> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<NoteResponse>> Handle(CreateNote request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return Result.Invalid(validation.AsErrors());
        }

        var note = await _repository.Notes.Create(new()
        {
            UserId = _repository.GetLoggedInUserId()!,
            Content = request.Content,
            CreatedOn = DateTimeOffset.UtcNow,

        }, cancellationToken);

        return new NoteResponse(note);
    }
}