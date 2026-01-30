using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CoinPair.Commands;

public class AddCoinPairCommandValidation : AbstractValidator<AddCoinPairCommand>
{
    public AddCoinPairCommandValidation()
    {
        RuleFor(x => x.CoinPairName)
            .NotEmpty().WithMessage("Coin pair name is required");
    }
}
