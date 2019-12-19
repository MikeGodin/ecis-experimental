using Hedwig.Models;
using Hedwig.Repositories;
using System.Linq;

namespace Hedwig.Validations.Rules
{
  public class MostRecentDeterminationIsValid: SubObjectIsValid, IValidationRule<Family>
  {
    //TODO: can we get around having to define private base class constructor param?
    public MostRecentDeterminationIsValid(
      INonBlockingValidator validator,
      IFamilyDeterminationRepository determinations
    ) : base(validator)
    {
      _determinations = determinations;
    }

    readonly IFamilyDeterminationRepository _determinations;
    public ValidationError Execute(Family family)
    {
      if(family.Determinations == null)
      {
        _determinations.GetDeterminationsByFamilyId(family.Id);
      }

      if(family.Determinations.Count == 0) return null;

      var determination = family.Determinations
        .OrderByDescending(d => d.DeterminationDate)
        .First();

      ValidateSubObject(determination);

      if(determination.ValidationErrors.Count > 0)
      {
        return new ValidationError(
          field: determination.GetType().Name,
          message: "Most recent determination has validation errors"
        );
      }

      //TODO somehow unload determinations if we added them

      return null;
    }
  }
}