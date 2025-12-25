namespace Application.ReportsHtml.Commands.UpdateReportHtml;

public class UpdateReportHtmlCommandValidation : AbstractValidator<UpdateReportHtmlCommand>
{
    public UpdateReportHtmlCommandValidation() 
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Html Content is required");
    }
}
