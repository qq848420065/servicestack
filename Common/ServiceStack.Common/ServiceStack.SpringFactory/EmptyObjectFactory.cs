using System;

namespace ServiceStack.SpringFactory
{
	public class EmptyObjectFactory : IObjectFactory
	{
		public T Create<T>()
		{
			return default(T);
		}

		public object Create(Type type)
		{
			return null;
		}

		public T Create<T>(string objectName)
		{
			return default(T);
		}

		public bool Contains(string objectName)
		{
			return false;
		}
	}
}