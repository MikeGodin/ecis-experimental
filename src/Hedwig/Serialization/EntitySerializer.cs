using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Hedwig.Models.Attributes;

namespace Hedwig.Serialization
{
	public class EntitySerializer : IEntitySerializer
	{
		readonly IServiceProvider _serviceProvider;
		readonly IMapper _mapper;
		public EntitySerializer(
			IServiceProvider serviceProvider,
			IMapper mapper
		)
		{
			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public TDest Deserialize<TSource, TDest>(TSource entity)
		{
			var map = _mapper.ConfigurationProvider.FindTypeMapFor(typeof(TSource), typeof(TDest));
			if(map == null)
			{
				throw new Exception($"Missing mapping for <{typeof(TSource)}, {typeof(TDest)}>");
			}

			UnsetReadOnlyProperties(entity);
			var deserialized = _mapper.Map<TSource, TDest>(entity);

			return deserialized;
		}

		public TDest Serialize<TSource, TDest>(TSource entity)
		{
			var map = _mapper.ConfigurationProvider.FindTypeMapFor(typeof(TSource), typeof(TDest));
			if(map == null)
			{
				throw new Exception($"Missing mapping for <{typeof(TSource)}, {typeof(TDest)}>");
			}


			UnsetSelfTypeSubEntities(entity);
			var serialized = _mapper.Map<TSource, TDest>(entity);
			// UnsetSelfTypeSubEntities(serialized);

			return serialized;
		}

		private static void UnsetSelfTypeSubEntities<T>(T entity)
		{
			UnsetTypeSubEntities<T>(entity);
		}
		private static void UnsetTypeSubEntities<T>(object entity)
		{
			if(entity == null || !IsModel(entity))
			{
				return;
			}

			if(entity is ICollection collection && collection != null)
			{
				foreach (var item in collection)
				{
					UnsetTypeSubEntities<T>(item);
				}
				return;
			}

			var properties = entity.GetType().GetProperties();
			foreach (var prop in properties)
			{
				var type = GetEntityType(prop.PropertyType);
				if(type == typeof(T))
				{
					prop.SetValue(entity, null);
					break;
				}

				var propValue = prop.GetValue(entity);
				UnsetTypeSubEntities<T>(propValue);
				// UnsetSelfTypeSubEntities(propValue); doesn't work b/c propValue type is 'RunTimeType'
			}
		}

		private static bool IsModel(object entity)
		{
			return entity.GetType().Namespace.Contains(nameof(Hedwig.Models))
				&& !(entity is Enum);
		}

		private static Type GetEntityType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>)
				? type.GetGenericArguments()[0]
				: type;
		}

		private static void UnsetReadOnlyProperties(object entity)
		{
			if(entity == null || !IsModel(entity)) {
				return;
			}

			if(entity is ICollection collection && collection != null)
			{
				foreach (var item in collection)
				{
					UnsetReadOnlyProperties(item);
				}
				return;
			}

			var properties = entity.GetType().GetProperties();
			foreach(var prop in properties)
			{
				if(ReadOnlyAttribute.IsReadOnly(prop))
				{
					prop.SetValue(entity, null);
					break;
				}

				var propValue = prop.GetValue(entity);
				UnsetReadOnlyProperties(propValue);
			}
		}
	}

	public interface IEntitySerializer
	{
		TDest Serialize<TSource, TDest>(TSource entity);
	}
}
