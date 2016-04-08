A 'strongly-typed' API to make all Redis Value operations to apply against any c# type

# Introduction #

Below is the strongly-typed API that you have access to after you call `IRedisClient.CreateGenericClient<T>()` e.g.:

```
using (var redisClient = new RedisClient())
{
	var redis = redisClient.CreateGenericClient<MyPocoType>();
}
```

The redis variable now holds a strongly-typed generic client that allows Redis value operations to apply against `MyPocoType`.
The interface below lists all available operations:

# Strongly typed Generic Client API #

```
public interface IRedisGenericClient<T> 
	: IBasicPersistenceProvider<T>
{
	string Host { get; }
	int Port { get; }
	int RetryTimeout { get; set; }
	int RetryCount { get; set; }
	int SendTimeout { get; set; }
	string Password { get; set; }

	IHasNamed<IRedisList<T>> Lists { get; set; }
	IHasNamed<IRedisSet<T>> Sets { get; set; }

	Dictionary<string, string> Info { get; }
	int Db { get; set; }
	int DbSize { get; }
	DateTime LastSave { get; }
	string[] AllKeys { get; }

	T this[string key] { get; set; }
	void Set(string key, T value);
	bool SetIfNotExists(string key, T value);
	T Get(string key);
	T GetAndSet(string key, T value);
	bool ContainsKey(string key);
	bool Remove(string key);
	bool Remove(params string[] args);
	bool Remove(params IHasStringId[] entities);
	int Increment(string key);
	int IncrementBy(string key, int count);
	int Decrement(string key);
	int DecrementBy(string key, int count);
	string SequenceKey { get; set; }
	void SetSequence(int value);
	int GetNextSequence();
	RedisKeyType GetKeyType(string key);
	string NewRandomKey();
	bool ExpireKeyIn(string key, TimeSpan expiresAt);
	bool ExpireKeyAt(string key, DateTime dateTime);
	TimeSpan GetTimeToLive(string key);
	string Save();
	void SaveAsync();
	void Shutdown();
	void FlushDb();
	void FlushAll();
	T[] GetKeys(string pattern);
	List<T> GetKeyValues(List<string> keys);

	List<T> GetRangeFromSortedSet(IRedisSet<T> fromSet, int startingFrom, int endingAt);
	HashSet<T> GetAllFromSet(IRedisSet<T> fromSet);
	void AddToSet(IRedisSet<T> toSet, T value);
	void RemoveFromSet(IRedisSet<T> fromSet, T value);
	T PopFromSet(IRedisSet<T> fromSet);
	void MoveBetweenSets(IRedisSet<T> fromSet, IRedisSet<T> toSet, T value);
	int GetSetCount(IRedisSet<T> set);
	bool SetContainsValue(IRedisSet<T> set, T value);
	HashSet<T> GetIntersectFromSets(params IRedisSet<T>[] sets);
	void StoreIntersectFromSets(IRedisSet<T> intoSet, params IRedisSet<T>[] sets);
	HashSet<T> GetUnionFromSets(params IRedisSet<T>[] sets);
	void StoreUnionFromSets(IRedisSet<T> intoSet, params IRedisSet<T>[] sets);
	HashSet<T> GetDifferencesFromSet(IRedisSet<T> fromSet, params IRedisSet<T>[] withSets);
	void StoreDifferencesFromSet(IRedisSet<T> intoSet, IRedisSet<T> fromSet, params IRedisSet<T>[] withSets);
	T GetRandomEntryFromSet(IRedisSet<T> fromSet);

	List<T> GetAllFromList(IRedisList<T> fromList);
	List<T> GetRangeFromList(IRedisList<T> fromList, int startingFrom, int endingAt);
	List<T> GetRangeFromSortedList(IRedisList<T> fromList, int startingFrom, int endingAt);
	void AddToList(IRedisList<T> fromList, T value);
	void PrependToList(IRedisList<T> fromList, T value);
	void RemoveAllFromList(IRedisList<T> fromList);
	void TrimList(IRedisList<T> fromList, int keepStartingFrom, int keepEndingAt);
	int RemoveValueFromList(IRedisList<T> fromList, T value);
	int RemoveValueFromList(IRedisList<T> fromList, T value, int noOfMatches);
	int GetListCount(IRedisList<T> fromList);
	T GetItemFromList(IRedisList<T> fromList, int listIndex);
	void SetItemInList(IRedisList<T> toList, int listIndex, T value);
	T DequeueFromList(IRedisList<T> fromList);
	T PopFromList(IRedisList<T> fromList);
	void PopAndPushBetweenLists(IRedisList<T> fromList, IRedisList<T> toList);
}
```


# Common data access interface #

Including the above methods, the Generic client also implements Redis non-specific
common data access operations that can be easily implemented by other data persistence providers should you want to swap providers in future.

```
public interface IBasicPersistenceProvider<T>
	: IDisposable
{
	T GetById(string id);

	IList<T> GetByIds(ICollection<string> ids);

	IList<T> GetAll();

	T Store(T entity);

	void StoreAll(IEnumerable<T> entities);

	void Delete(T entity);

	void DeleteById(string id);

	void DeleteByIds(ICollection<string> ids);

	void DeleteAll();
}
```

Generally, if you only have basic persistence needs I would recommend developing against the above common data access API as it is easier for other persistence providers to implement and increases the likely hood that your library can be reused as-is to persist against other data stores i.e. against an RDBMS with OrmLite, etc.