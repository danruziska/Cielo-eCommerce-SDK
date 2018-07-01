using FluentValidation;
using FluentValidation.Results;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;

namespace RLabs.Cielo.SDK.Model.Validators
{
    internal abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>
    {
        private readonly ILogger logger;

        protected BaseValidator(ILogger logger)
        {            
            this.logger = logger;
        }

        protected override void EnsureInstanceNotNull(object instanceToValidate)
        {
            if (instanceToValidate == null)
                throw new ArgumentException(string.Format("Parâmetro nulo do tipo {0}", instanceToValidate.GetType().ToString()));
        }

        public virtual bool ValidateField(TRequest field)
        {
            ValidationResult result = this.Validate(field);

            if (!result.IsValid && logger != null)
            {
                LogErrors(result.Errors);
            }

            return result.IsValid;
        }

        private void LogErrors(IList<ValidationFailure> validationErrors)
        {
            IList<ValidationFailure> listaErrosValidacao = validationErrors;
            foreach (var erroValidacao in listaErrosValidacao)
            {
                logger.InfoWithMetadata(new Dictionary<string, string>() { { "ValidationMessage", erroValidacao.ErrorMessage } });
            }
        }

        public abstract void SetRules();
    }
}
