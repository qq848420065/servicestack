using System;
using System.IO;
using System.Linq.Expressions;
using ServiceStack.Common.Reflection;
using ServiceStack.DesignPatterns.Model;

namespace ServiceStack.Common.Utils
{
	public static class IdUtils<T>
	{
		private static readonly Func<T, object> CanGetId;

		static IdUtils()
		{
			var hasIdInterfaces = typeof(T).FindInterfaces(
				(t, critera) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHasId<>), null);

			if (hasIdInterfaces.Length > 0)
			{
				CanGetId = HasId<T>.GetId;
			}
			else if (typeof(T).IsClass
				&& typeof(T).GetProperty(IdUtils.IdField) != null
				&& typeof(T).GetProperty(IdUtils.IdField).GetGetMethod() != null)
			{
				CanGetId = HasPropertyId<T>.GetId;
			}
			else
			{
				CanGetId = x => x.GetHashCode();
			}
		}

		public static object GetId(T entity)
		{
			return CanGetId(entity);
		}
	}

	internal static class HasPropertyId<TEntity>
	{

		private static readonly Func<TEntity, object> GetIdFn;

		static HasPropertyId()
		{
			var pi = typeof(TEntity).GetProperty(IdUtils.IdField);
			GetIdFn = StaticAccessors<TEntity>.ValueUnTypedGetPropertyTypeFn(pi);
		}

		public static object GetId(TEntity entity)
		{
			return GetIdFn(entity);
		}
	}

	internal static class HasId<TEntity>
	{
		private static readonly Func<TEntity, object> GetIdFn;

		static HasId()
		{
			var hasIdInterfaces = typeof(TEntity).FindInterfaces(
				(t, critera) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHasId<>), null);

			var genericArg = hasIdInterfaces[0].GetGenericArguments()[0];
			var genericType = typeof(HasIdGetter<,>).MakeGenericType(typeof(TEntity), genericArg);

			var oInstanceParam = Expression.Parameter(typeof(TEntity), "oInstanceParam");
			var exprCallStaticMethod = Expression.Call
				(
					genericType,
					"GetId",
					new Type[0],
					oInstanceParam
				);
			GetIdFn = Expression.Lambda<Func<TEntity, object>>
				(
					exprCallStaticMethod,
					oInstanceParam
				).Compile();
		}

		public static object GetId(TEntity entity)
		{
			return GetIdFn(entity);
		}
	}

	internal class HasIdGetter<TEntity, TId>
		where TEntity : IHasId<TId>
	{
		public static object GetId(TEntity entity)
		{
			return entity.Id;
		}
	}

	public static class IdUtils
	{
		public const string IdField = "Id";

		public static object GetId<T>(this T entity)
		{
			return IdUtils<T>.GetId(entity);
		}

		public static string CreateUrn<T>(object id)
		{
			return string.Format("urn:{0}:{1}", typeof(T).Name.ToLower(), id);
		}

		public static string CreateUrn<T>(this T entity)
		//			where T : class
		{
			var id = GetId(entity);
			return string.Format("urn:{0}:{1}", typeof(T).Name.ToLower(), id);
		}

		public static string CreateCacheKeyPath<T>(string idValue)
		{
			if (idValue.Length < 4)
			{
				idValue = idValue.PadLeft(4, '0');
			}
			idValue = idValue.Replace(" ", "-");

			var rootDir = typeof(T).Name;
			var dir1 = idValue.Substring(0, 2);
			var dir2 = idValue.Substring(2, 2);

			var path = string.Format("{1}{0}{2}{0}{3}{0}{4}", Path.DirectorySeparatorChar,
				rootDir, dir1, dir2, idValue);

			return path;
		}

	}

}