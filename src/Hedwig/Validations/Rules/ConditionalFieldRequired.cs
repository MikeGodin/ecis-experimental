namespace Hedwig.Validations.Rules
{
	public abstract class ConditionalFieldRequired<T> : IValidationRule<T> where T : INonBlockingValidatableObject
	{
		readonly string _conditionalMessage;
		readonly string _fieldName;
		readonly string _prettyFieldName;

		public ConditionalFieldRequired(
			string conditionalMessage,
			string fieldName,
			string prettyFieldName = null
		)

		{
			_conditionalMessage = conditionalMessage;
			_fieldName = fieldName;
			_prettyFieldName = prettyFieldName;
		}

		protected abstract bool CheckCondition(T entity, NonBlockingValidationContext context);
		public ValidationError Execute(T entity, NonBlockingValidationContext context)
		{
			var prop = typeof(T).GetProperty(_fieldName);
			if (prop != null)
			{
				var value = prop.GetValue(entity);
				if (CheckCondition(entity, context) && (value == null || value as string == ""))
				{
					return new ValidationError(
					field: _fieldName,
					message: $"{(_prettyFieldName != null ? _prettyFieldName : _fieldName)} is required when {_conditionalMessage}"
					);
				}
			}

			return null;
		}
	}
}
