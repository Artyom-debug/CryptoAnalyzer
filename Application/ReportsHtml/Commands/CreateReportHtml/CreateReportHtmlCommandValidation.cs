namespace Application.ReportsHtml.Commands.CreateReportHtml;

public class CreateReportHtmlCommandValidation : AbstractValidator<CreateReportHtmlCommand>
{
    public CreateReportHtmlCommandValidation() 
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Html content is required.");
    }
}
