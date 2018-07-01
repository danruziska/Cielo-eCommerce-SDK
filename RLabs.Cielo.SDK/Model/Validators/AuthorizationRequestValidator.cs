using FluentValidation;
using FluentValidation.Results;
using NLog;
using RLabs.Cielo.SDK.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;


namespace RLabs.Cielo.SDK.Model.Validators
{
    internal sealed class AuthorizationRequestValidator : BaseValidator<AuthorizationRequest>
    {
        private readonly ILogger logger;

        public AuthorizationRequestValidator(ILogger logger) : base(logger)
        {            
            this.logger = logger;
            SetRules();
        }

        public override void SetRules()
        {
            logger.InfoWithMetadata(new Dictionary<string, string>() { { "Message", "Adicionando regras de autorização" } });
            RuleFor(authRequest => authRequest.Payment.Amount).NotEqual(0).WithMessage("O valor da transação não pode ser zero para esta operação");
        }
    }
}
