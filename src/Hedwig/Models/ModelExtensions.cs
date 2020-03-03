using System;
using System.Collections;
using System.Collections.Generic;

namespace Hedwig.Models
{
	public static class ModelExtensions
	{
		private static Type GetEntityType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>)
				? type.GetGenericArguments()[0]
				: type;
		}
		private static bool IsModel(object entity)
		{
			return entity.GetType().Namespace.Contains(nameof(Hedwig.Models))
				&& !(entity is Enum);
		}

		public static void UnsetSelfTypeSubEntities<T>(this T entity)
		{
			UnsetTypeSubEntities<T>(entity);
		}
		public static void UnsetTypeSubEntities<T>(object entity)
		{
			if(entity == null || !IsModel(entity))
			{
				return;
			}

			var properties = entity.GetType().GetProperties();
			foreach (var prop in properties)
			{
				var type = GetEntityType(prop.PropertyType);
				if(type == typeof(T))
				{
					prop.SetValue(entity, null);
					return;
				}

				var propValue = prop.GetValue(entity);
				if(propValue is ICollection enumerable)
				{
					foreach(var item in enumerable)
					{
						UnsetTypeSubEntities<T>(propValue);
						// propValue.UnsetSelfTypeSubEntities();
					}
				} else {
					UnsetTypeSubEntities<T>(propValue);
					// propValue.UnsetSelfTypeSubEntities();
				}
			}
		}
	}
}

