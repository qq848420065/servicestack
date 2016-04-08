## NOTE: This page has now moved to [IRedisTypedClient on GitHub](https://github.com/ServiceStack/ServiceStack/wiki/IRedisTypedClient). Please update your links. ##



&lt;hr/&gt;


<br />

A 'strongly-typed' API available on [Service Stack's C# Redis Client](ServiceStackRedis.md) to make all Redis Value operations to apply against any c# type

# Introduction #

Below is the strongly-typed API that you have access to after you call `IRedisClient.GetTypedClient<T>()` e.g.:

```
using (var redisClient = new RedisClient())
{
	var redis = redisClient.GetTypedClient<MyPocoType>();
}
```

The redis variable now holds a strongly-typed generic client that allows Redis value operations to apply against `MyPocoType`.
The interface below lists all available operations:

# Strongly typed Generic Client API #

```

public interface IRedisTypedClient<T>
	: IBasicPersistenceProvider<T>
{
	IHasNamed<IRedisList<T>> Lists { get; set; }
	IHasNamed<IRedisSet<T>> Sets { get; set; }
	IHasNamed<IRedisSortedSet<T>> SortedSets { get; set; }
	IRedisHash<TKey, T> GetHash<TKey>(string hashId);

	IRedisTypedTransaction<T> CreateTransaction();

	IDisposable AcquireLock();
	IDisposable AcquireLock(TimeSpan timeOut);

	int Db { get; set; }
	List<string> GetAllKeys();

	T this[string key] { get; set; }

	string SequenceKey { get; set; }
	void SetSequence(int value);
	int GetNextSequence();
	RedisKeyType GetEntryType(string key);
	string GetRandomKey();

	void SetEntry(string key, T value);
	void SetEntry(string key, T value, TimeSpan expireIn);
	bool SetEntryIfNotExists(string key, T value);
	T GetValue(string key);
	T GetAndSetValue(string key, T value);
	bool ContainsKey(string key);
	bool RemoveEntry(string key);
	bool RemoveEntry(params string[] args);
	bool RemoveEntry(params IHasStringId[] entities);
	int IncrementValue(string key);
	int IncrementValueBy(string key, int count);
	int DecrementValue(string key);
	int DecrementValueBy(string key, int count);
	bool ExpireEntryIn(string key, TimeSpan expiresAt);
	bool ExpireEntryAt(string key, DateTime dateTime);
	TimeSpan GetTimeToLive(string key);
	void Save();
	void SaveAsync();
	void FlushDb();
	void FlushAll();
	T[] SearchKeys(string pattern);
	List<T> GetValues(List<string> keys);
	List<T> GetSortedEntryValues(IRedisSet<T> fromSet, int startingFrom, int endingAt);

	HashSet<T> GetAllItemsFromSet(IRedisSet<T> fromSet);
	void AddItemToSet(IRedisSet<T> toSet, T item);
	void RemoveItemFromSet(IRedisSet<T> fromSet, T item);
	T PopItemFromSet(IRedisSet<T> fromSet);
	void MoveBetweenSets(IRedisSet<T> fromSet, IRedisSet<T> toSet, T item);
	int GetSetCount(IRedisSet<T> set);
	bool SetContainsItem(IRedisSet<T> set, T item);
	HashSet<T> GetIntersectFromSets(params IRedisSet<T>[] sets);
	void StoreIntersectFromSets(IRedisSet<T> intoSet, params IRedisSet<T>[] sets);
	HashSet<T> GetUnionFromSets(params IRedisSet<T>[] sets);
	void StoreUnionFromSets(IRedisSet<T> intoSet, params IRedisSet<T>[] sets);
	HashSet<T> GetDifferencesFromSet(IRedisSet<T> fromSet, params IRedisSet<T>[] withSets);
	void StoreDifferencesFromSet(IRedisSet<T> intoSet, IRedisSet<T> fromSet, params IRedisSet<T>[] withSets);
	T GetRandomItemFromSet(IRedisSet<T> fromSet);

	List<T> GetAllItemsFromList(IRedisList<T> fromList);
	List<T> GetRangeFromList(IRedisList<T> fromList, int startingFrom, int endingAt);
	List<T> SortList(IRedisList<T> fromList, int startingFrom, int endingAt);
	void AddItemToList(IRedisList<T> fromList, T value);
	void PrependItemToList(IRedisList<T> fromList, T value);
	T RemoveStartFromList(IRedisList<T> fromList);
	T BlockingRemoveStartFromList(IRedisList<T> fromList, TimeSpan? timeOut);
	T RemoveEndFromList(IRedisList<T> fromList);
	void RemoveAllFromList(IRedisList<T> fromList);
	void TrimList(IRedisList<T> fromList, int keepStartingFrom, int keepEndingAt);
	int RemoveItemFromList(IRedisList<T> fromList, T value);
	int RemoveItemFromList(IRedisList<T> fromList, T value, int noOfMatches);
	int GetListCount(IRedisList<T> fromList);
	T GetItemFromList(IRedisList<T> fromList, int listIndex);
	void SetItemInList(IRedisList<T> toList, int listIndex, T value);

	//Queue operations
	void EnqueueItemOnList(IRedisList<T> fromList, T item);
	T DequeueItemFromList(IRedisList<T> fromList);
	T BlockingDequeueItemFromList(IRedisList<T> fromList, TimeSpan? timeOut);

	//Stack operations
	void PushItemToList(IRedisList<T> fromList, T item);
	T PopItemFromList(IRedisList<T> fromList);
	T BlockingPopItemFromList(IRedisList<T> fromList, TimeSpan? timeOut);
	T PopAndPushItemBetweenLists(IRedisList<T> fromList, IRedisList<T> toList);

	void AddItemToSortedSet(IRedisSortedSet<T> toSet, T value);
	void AddItemToSortedSet(IRedisSortedSet<T> toSet, T value, double score);
	bool RemoveItemFromSortedSet(IRedisSortedSet<T> fromSet, T value);
	T PopItemWithLowestScoreFromSortedSet(IRedisSortedSet<T> fromSet);
	T PopItemWithHighestScoreFromSortedSet(IRedisSortedSet<T> fromSet);
	bool SortedSetContainsItem(IRedisSortedSet<T> set, T value);
	double IncrementItemInSortedSet(IRedisSortedSet<T> set, T value, double incrementBy);
	int GetItemIndexInSortedSet(IRedisSortedSet<T> set, T value);
	int GetItemIndexInSortedSetDesc(IRedisSortedSet<T> set, T value);
	List<T> GetAllItemsFromSortedSet(IRedisSortedSet<T> set);
	List<T> GetAllItemsFromSortedSetDesc(IRedisSortedSet<T> set);
	List<T> GetRangeFromSortedSet(IRedisSortedSet<T> set, int fromRank, int toRank);
	List<T> GetRangeFromSortedSetDesc(IRedisSortedSet<T> set, int fromRank, int toRank);
	IDictionary<T, double> GetAllWithScoresFromSortedSet(IRedisSortedSet<T> set);
	IDictionary<T, double> GetRangeWithScoresFromSortedSet(IRedisSortedSet<T> set, int fromRank, int toRank);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetDesc(IRedisSortedSet<T> set, int fromRank, int toRank);
	List<T> GetRangeFromSortedSetByLowestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore);
	List<T> GetRangeFromSortedSetByLowestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore, int? skip, int? take);
	List<T> GetRangeFromSortedSetByLowestScore(IRedisSortedSet<T> set, double fromScore, double toScore);
	List<T> GetRangeFromSortedSetByLowestScore(IRedisSortedSet<T> set, double fromScore, double toScore, int? skip, int? take);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByLowestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByLowestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore, int? skip, int? take);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByLowestScore(IRedisSortedSet<T> set, double fromScore, double toScore);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByLowestScore(IRedisSortedSet<T> set, double fromScore, double toScore, int? skip, int? take);
	List<T> GetRangeFromSortedSetByHighestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore);
	List<T> GetRangeFromSortedSetByHighestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore, int? skip, int? take);
	List<T> GetRangeFromSortedSetByHighestScore(IRedisSortedSet<T> set, double fromScore, double toScore);
	List<T> GetRangeFromSortedSetByHighestScore(IRedisSortedSet<T> set, double fromScore, double toScore, int? skip, int? take);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByHighestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByHighestScore(IRedisSortedSet<T> set, string fromStringScore, string toStringScore, int? skip, int? take);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByHighestScore(IRedisSortedSet<T> set, double fromScore, double toScore);
	IDictionary<T, double> GetRangeWithScoresFromSortedSetByHighestScore(IRedisSortedSet<T> set, double fromScore, double toScore, int? skip, int? take);
	int RemoveRangeFromSortedSet(IRedisSortedSet<T> set, int minRank, int maxRank);
	int RemoveRangeFromSortedSetByScore(IRedisSortedSet<T> set, double fromScore, double toScore);
	int GetSortedSetCount(IRedisSortedSet<T> set);
	double GetItemScoreInSortedSet(IRedisSortedSet<T> set, T value);
	int StoreIntersectFromSortedSets(IRedisSortedSet<T> intoSetId, params IRedisSortedSet<T>[] setIds);
	int StoreUnionFromSortedSets(IRedisSortedSet<T> intoSetId, params IRedisSortedSet<T>[] setIds);

	bool HashContainsEntry<TKey>(IRedisHash<TKey, T> hash, TKey key);
	bool SetEntryInHash<TKey>(IRedisHash<TKey, T> hash, TKey key, T value);
	bool SetEntryInHashIfNotExists<TKey>(IRedisHash<TKey, T> hash, TKey key, T value);
	void SetRangeInHash<TKey>(IRedisHash<TKey, T> hash, IEnumerable<KeyValuePair<TKey, T>> keyValuePairs);
	T GetValueFromHash<TKey>(IRedisHash<TKey, T> hash, TKey key);
	bool RemoveEntryFromHash<TKey>(IRedisHash<TKey, T> hash, TKey key);
	int GetHashCount<TKey>(IRedisHash<TKey, T> hash);
	List<TKey> GetHashKeys<TKey>(IRedisHash<TKey, T> hash);
	List<T> GetHashValues<TKey>(IRedisHash<TKey, T> hash);
	Dictionary<TKey, T> GetAllEntriesFromHash<TKey>(IRedisHash<TKey, T> hash);
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