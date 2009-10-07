using System;
using System.Collections.Generic;

namespace ServiceStack.CacheAccess.Providers
{
	public class CacheManager : ICacheManager
	{
		protected readonly ICacheClient cacheClient;

		public CacheManager(ICacheClient cacheClient)
		{
			this.cacheClient = cacheClient;
		}

		public virtual T Resolve<T>(string cacheKey, Func<T> createCacheFn)
			where T : class
		{
			var result = this.cacheClient.Get<T>(cacheKey);
			if (result != null) return result;

			var cacheValue = createCacheFn();

			this.cacheClient.Set(cacheKey, cacheValue);

			return cacheValue;
		}

		public virtual void Clear(IEnumerable<string> cacheKeys)
		{
			this.cacheClient.RemoveAll(cacheKeys);
		}

		public virtual void Clear(params string[] cacheKeys)
		{
			this.cacheClient.RemoveAll(cacheKeys);
		}
	}

}