using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using EzyNotes.Features.Notes.Operations;
using EzyNotes.Features.Notes.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EzyNotes.Features.Notes;

[Authorize]
[Route("[controller]")]
[ApiController]
public class NotesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Ok)]
    public Task<Result<SearchResponse<NoteResponse>>> Search(SearchNotes request) => mediator.Send(request);

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public Task<Result<NoteResponse>> CreateNote([FromBody] CreateNote request)
        => mediator.Send(request);


    [HttpGet("{NoteId}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public Task<Result<NoteResponse>> GetNote(GetNote request) => mediator.Send(request);


    [HttpPut("{NoteId}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public Task<Result<NoteResponse>> UpdateNote(UpdateNote request) => mediator.Send(request);
}