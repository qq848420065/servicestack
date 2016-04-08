## NOTE: This page has now moved to [IRedisClient on GitHub](https://github.com/ServiceStack/ServiceStack/wiki/IRedisClient). Please update your links. ##



&lt;hr/&gt;


<br /><br />

Details of the IRedisClient API implemented by the [ServiceStack.Redis client](ServiceStackRedis.md)

# Introduction #

This is a friendly, more descriptive API implemented by the [ServiceStack.Redis client](ServiceStackRedis.md) that provides access to key values as strings (or collection of strings for Redis lists and sets).

Use this API if you just need to access values as strings or you want to have control over your own text serialization format.

# IRedisClient API #

```

public interface IRedisClient
	: IBasicPersistenceProvider, ICacheClient
{
	//Basic Redis Connection operations
	int Db { get; set; }
	int DbSize { get; }
	Dictionary<string, string> Info { get; }
	DateTime LastSave { get; }
	string Host { get; }
	int Port { get; }
	int RetryTimeout { get; set; }
	int RetryCount { get; set; }
	int SendTimeout { get; set; }
	string Password { get; set; }
	bool HadExceptions { get; }

	void Save();
	void SaveAsync();
	void Shutdown();
	void RewriteAppendOnlyFileAsync();
	void FlushDb();

	//Basic Redis Connection Info
	string this[string key] { get; set; }

	List<string> GetAllKeys();
	void SetEntry(string key, string value);
	void SetEntry(string key, string value, TimeSpan expireIn);
	bool SetEntryIfNotExists(string key, string value);
	string GetValue(string key);
	string GetAndSetEntry(string key, string value);
	List<string> GetValues(List<string> keys);
	List<T> GetValues<T>(List<string> keys);
	int AppendToValue(string key, string value);
	string GetSubstring(string key, int fromIndex, int toIndex);

	bool ContainsKey(string key);
	bool RemoveEntry(params string[] args);
	int IncrementValue(string key);
	int IncrementValueBy(string key, int count);
	int DecrementValue(string key);
	int DecrementValueBy(string key, int count);
	List<string> SearchKeys(string pattern);

	RedisKeyType GetEntryType(string key);
	string GetRandomKey();
	bool ExpireEntryIn(string key, TimeSpan expireIn);
	bool ExpireEntryAt(string key, DateTime expireAt);
	TimeSpan GetTimeToLive(string key);
	List<string> GetSortedEntryValues(string key, int startingFrom, int endingAt);

	//Useful high-level abstractions
	IRedisTypedClient<T> GetTypedClient<T>();

	IHasNamed<IRedisList> Lists { get; set; }
	IHasNamed<IRedisSet> Sets { get; set; }
	IHasNamed<IRedisSortedSet> SortedSets { get; set; }
	IHasNamed<IRedisHash> Hashes { get; set; }

	IRedisTransaction CreateTransaction();

	IDisposable AcquireLock(string key);
	IDisposable AcquireLock(string key, TimeSpan timeOut);

	#region Redis pubsub

	IRedisSubscription CreateSubscription();
	void PublishMessage(string toChannel, string message);

	#endregion


	#region Set operations

	HashSet<string> GetAllItemsFromSet(string setId);
	void AddItemToSet(string setId, string item);
	void RemoveItemFromSet(string setId, string item);
	string PopItemFromSet(string setId);
	void MoveBetweenSets(string fromSetId, string toSetId, string item);
	int GetSetCount(string setId);
	bool SetContainsItem(string setId, string item);
	HashSet<string> GetIntersectFromSets(params string[] setIds);
	void StoreIntersectFromSets(string intoSetId, params string[] setIds);
	HashSet<string> GetUnionFromSets(params string[] setIds);
	void StoreUnionFromSets(string intoSetId, params string[] setIds);
	HashSet<string> GetDifferencesFromSet(string fromSetId, params string[] withSetIds);
	void StoreDifferencesFromSet(string intoSetId, string fromSetId, params string[] withSetIds);
	string GetRandomItemFromSet(string setId);

	#endregion


	#region List operations

	List<string> GetAllItemsFromList(string listId);
	List<string> GetRangeFromList(string listId, int startingFrom, int endingAt);
	List<string> GetRangeFromSortedList(string listId, int startingFrom, int endingAt);
	void AddItemToList(string listId, string value);
	void PrependItemToList(string listId, string value);
	void RemoveAllFromList(string listId);
	string RemoveStartFromList(string listId);
	string BlockingRemoveStartFromList(string listId, TimeSpan? timeOut);
	string RemoveEndFromList(string listId);
	void TrimList(string listId, int keepStartingFrom, int keepEndingAt);
	int RemoveItemFromList(string listId, string value);
	int RemoveItemFromList(string listId, string value, int noOfMatches);
	int GetListCount(string listId);
	string GetItemFromList(string listId, int listIndex);
	void SetItemInList(string listId, int listIndex, string value);

	//Queue operations
	void EnqueueItemOnList(string listId, string value);
	string DequeueItemFromList(string listId);
	string BlockingDequeueItemFromList(string listId, TimeSpan? timeOut);

	//Stack operations
	void PushItemToList(string listId, string value);
	string PopItemFromList(string listId);
	string BlockingPopItemFromList(string listId, TimeSpan? timeOut);
	string PopAndPushItemBetweenLists(string fromListId, string toListId);

	#endregion


	#region Sorted Set operations

	bool AddItemToSortedSet(string setId, string value);
	bool AddItemToSortedSet(string setId, string value, double score);
	bool RemoveItemFromSortedSet(string setId, string value);
	string PopItemWithLowestScoreFromSortedSet(string setId);
	string PopItemWithHighestScoreFromSortedSet(string setId);
	bool SortedSetContainsItem(string setId, string value);
	double IncrementItemInSortedSet(string setId, string value, double incrementBy);
	int GetItemIndexInSortedSet(string setId, string value);
	int GetItemIndexInSortedSetDesc(string setId, string value);
	List<string> GetAllItemsFromSortedSet(string setId);
	List<string> GetAllItemsFromSortedSetDesc(string setId);
	List<string> GetRangeFromSortedSet(string setId, int fromRank, int toRank);
	List<string> GetRangeFromSortedSetDesc(string setId, int fromRank, int toRank);
	IDictionary<string, double> GetAllWithScoresFromSortedSet(string setId);
	IDictionary<string, double> GetRangeWithScoresFromSortedSet(string setId, int fromRank, int toRank);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetDesc(string setId, int fromRank, int toRank);
	List<string> GetRangeFromSortedSetByLowestScore(string setId, string fromStringScore, string toStringScore);
	List<string> GetRangeFromSortedSetByLowestScore(string setId, string fromStringScore, string toStringScore, int? skip, int? take);
	List<string> GetRangeFromSortedSetByLowestScore(string setId, double fromScore, double toScore);
	List<string> GetRangeFromSortedSetByLowestScore(string setId, double fromScore, double toScore, int? skip, int? take);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(string setId, string fromStringScore, string toStringScore);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(string setId, string fromStringScore, string toStringScore, int? skip, int? take);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(string setId, double fromScore, double toScore);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(string setId, double fromScore, double toScore, int? skip, int? take);
	List<string> GetRangeFromSortedSetByHighestScore(string setId, string fromStringScore, string toStringScore);
	List<string> GetRangeFromSortedSetByHighestScore(string setId, string fromStringScore, string toStringScore, int? skip, int? take);
	List<string> GetRangeFromSortedSetByHighestScore(string setId, double fromScore, double toScore);
	List<string> GetRangeFromSortedSetByHighestScore(string setId, double fromScore, double toScore, int? skip, int? take);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(string setId, string fromStringScore, string toStringScore);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(string setId, string fromStringScore, string toStringScore, int? skip, int? take);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(string setId, double fromScore, double toScore);
	IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(string setId, double fromScore, double toScore, int? skip, int? take);
	int RemoveRangeFromSortedSet(string setId, int minRank, int maxRank);
	int RemoveRangeFromSortedSetByScore(string setId, double fromScore, double toScore);
	int GetSortedSetCount(string setId);
	double GetItemScoreInSortedSet(string setId, string value);
	int StoreIntersectFromSortedSets(string intoSetId, params string[] setIds);
	int StoreUnionFromSortedSets(string intoSetId, params string[] setIds);

	#endregion


	#region Hash operations

	bool HashContainsEntry(string hashId, string key);
	bool SetEntryInHash(string hashId, string key, string value);
	bool SetEntryInHashIfNotExists(string hashId, string key, string value);
	void SetRangeInHash(string hashId, IEnumerable<KeyValuePair<string, string>> keyValuePairs);
	int IncrementValueInHash(string hashId, string key, int incrementBy);
	string GetValueFromHash(string hashId, string key);
	List<string> GetValuesFromHash(string hashId, params string[] keys);
	bool RemoveEntryFromHash(string hashId, string key);
	int GetHashCount(string hashId);
	List<string> GetHashKeys(string hashId);
	List<string> GetHashValues(string hashId);
	Dictionary<string, string> GetAllEntriesFromHash(string hashId);

	#endregion
}

```


# IBasicPersistenceProvider API #
This is a common interface shared by all other ServiceStack persistence providers (OrmLite, db4o, Memchached, In Memory and File System cache providers, etc).

It provides generic operations to get, store and delete any C# POCO type.

```

public interface IBasicPersistenceProvider : IDisposable
{
	T GetById<T>(object id)
		where T : class, new();

	IList<T> GetByIds<T>(ICollection ids) 
		where T : class, new();

	T Store<T>(T entity)
		where T : class, new();

	void StoreAll<TEntity>(IEnumerable<TEntity> entities)
		where TEntity : class, new();

	void Delete<T>(T entity)
		where T : class, new();

	void DeleteById<T>(object id)
		where T : class, new();

	void DeleteByIds<T>(ICollection ids)
		where T : class, new();

	void DeleteAll<TEntity>()
		where TEntity : class, new();
}

```

Generally, if you only have basic persistence needs I would recommend developing against the above common data access API as it is easier for other persistence providers to implement and increases the likely hood that your existing library can be reused as-is to have your POCO types persist against other data stores i.e. against an RDBMS with OrmLite, etc.