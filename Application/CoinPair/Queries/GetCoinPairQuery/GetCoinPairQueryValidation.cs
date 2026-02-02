namespace Application.CoinPair.Queries.GetCoinPairQuery;

public class GetCoinPairQueryValidation : AbstractValidator<GetCoinPairQuery>
{
    public GetCoinPairQueryValidation()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Coin pair name is required.");
    }
}
