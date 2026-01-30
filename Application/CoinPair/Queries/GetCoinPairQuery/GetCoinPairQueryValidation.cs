using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CoinPair.Queries.GetCoinPairQuery;

<<<<<<< HEAD
public class GetCoinPairQueryValidation
{
=======
public class GetCoinPairQueryValidation : AbstractValidator<GetCoinPairQuery>
{
    public GetCoinPairQueryValidation()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Coin pair name is required.");
    }
>>>>>>> 4ec45edcec990bad74f1b7e787932c0bb4abad01
}
