using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CoinPair.Queries.GetCoinPairQuery;

public class GetCoinPairQueryValidation : AbstractValidator<GetCoinPairQuery>
{
    public GetCoinPairQueryValidation()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Coin pair name is required.");
    }
}
