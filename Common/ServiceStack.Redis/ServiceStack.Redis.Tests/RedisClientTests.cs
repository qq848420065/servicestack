﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using ServiceStack.Common.Extensions;
using ServiceStack.Text;

namespace ServiceStack.Redis.Tests
{
	[TestFixture]
	public class RedisClientTests
		: RedisClientTestsBase
	{
		const string Value = "Value";

		[Test]
		public void Can_Set_and_Get_string()
		{
			Redis.SetEntry("key", Value);
			var valueBytes = Redis.Get("key");
			var valueString = GetString(valueBytes);

			Assert.That(valueString, Is.EqualTo(Value));
		}

		[Test]
		public void Can_Set_and_Get_key_with_space()
		{
			Redis.SetEntry("key with space", Value);
			var valueBytes = Redis.Get("key with space");
			var valueString = GetString(valueBytes);

			Assert.That(valueString, Is.EqualTo(Value));
		}

		[Test]
		public void Can_Set_and_Get_key_with_spaces()
		{
			const string key = "key with spaces";

			Redis.SetEntry(key, Value);
			var valueBytes = Redis.Get(key);
			var valueString = GetString(valueBytes);

			Assert.That(valueString, Is.EqualTo(Value));
		}

		[Test]
		public void Can_Set_and_Get_key_with_all_byte_values()
		{
			const string key = "bytesKey";

			var value = new byte[256];
			for (var i = 0; i < value.Length; i++)
			{
				value[i] = (byte)i;
			}

			Redis.Set(key, value);
			var resultValue = Redis.Get(key);

			Assert.That(resultValue, Is.EquivalentTo(value));
		}

		[Test]
		public void GetKeys_returns_matching_collection()
		{
			Redis.Set("ss-tests:a1", "One");
			Redis.Set("ss-tests:a2", "One");
			Redis.Set("ss-tests:b3", "One");

			var matchingKeys = Redis.SearchKeys("ss-tests:a*");

			Assert.That(matchingKeys.Count, Is.EqualTo(2));
		}

		[Test]
		public void GetKeys_on_non_existent_keys_returns_empty_collection()
		{
			var matchingKeys = Redis.SearchKeys("ss-tests:NOTEXISTS");

			Assert.That(matchingKeys.Count, Is.EqualTo(0));
		}

		[Test]
		public void Can_get_Types()
		{
			Redis.SetEntry("string", "string");
			Redis.AddItemToList("list", "list");
			Redis.AddItemToSet("set", "set");
			Redis.AddItemToSortedSet("sortedset", "sortedset");
			Redis.SetEntryInHash("hash", "key", "val");

			Assert.That(Redis.GetEntryType("nokey"), Is.EqualTo(RedisKeyType.None));
			Assert.That(Redis.GetEntryType("string"), Is.EqualTo(RedisKeyType.String));
			Assert.That(Redis.GetEntryType("list"), Is.EqualTo(RedisKeyType.List));
			Assert.That(Redis.GetEntryType("set"), Is.EqualTo(RedisKeyType.Set));
			Assert.That(Redis.GetEntryType("sortedset"), Is.EqualTo(RedisKeyType.SortedSet));
			Assert.That(Redis.GetEntryType("hash"), Is.EqualTo(RedisKeyType.Hash));
		}

		[Test]
		public void Can_delete_keys()
		{
			Redis.SetEntry("key", "val");

			Assert.That(Redis.ContainsKey("key"), Is.True);

			Redis.Del("key");

			Assert.That(Redis.ContainsKey("key"), Is.False);

			var keysMap = new Dictionary<string, string>();

			10.Times(i => keysMap.Add("key" + i, "val" + i));

			Redis.SetAll(keysMap);

			10.Times(i => Assert.That(Redis.ContainsKey("key" + i), Is.True));

			Redis.Del(keysMap.Keys.ToList<string>().ToArray());

			10.Times(i => Assert.That(Redis.ContainsKey("key" + i), Is.False));
		}

		[Test]
		public void Can_get_RandomKey()
		{
			var keysMap = new Dictionary<string, string>();

			10.Times(i => keysMap.Add("key" + i, "val" + i));

			Redis.SetAll(keysMap);

			var randKey = Redis.RandomKey();

			Assert.That(keysMap.ContainsKey(randKey), Is.True);
		}

		[Test]
		public void Can_RenameKey()
		{
			Redis.SetEntry("oldkey", "val");
			Redis.Rename("oldkey", "newkey");

			Assert.That(Redis.ContainsKey("oldkey"), Is.False);
			Assert.That(Redis.ContainsKey("newkey"), Is.True);
		}

		[Test]
		public void Can_Expire()
		{
			Redis.SetEntry("key", "val");
			Redis.Expire("key", 1);
			Assert.That(Redis.ContainsKey("key"), Is.True);
			Thread.Sleep(2000);
			Assert.That(Redis.ContainsKey("key"), Is.False);
		}

		[Test]
		public void Can_ExpireAt()
		{
			Redis.SetEntry("key", "val");

			var unixNow = DateTime.Now.ToUnixTime();
			var in1Sec = unixNow + 1;

			Redis.ExpireAt("key", in1Sec);

			Assert.That(Redis.ContainsKey("key"), Is.True);
			Thread.Sleep(2000);
			Assert.That(Redis.ContainsKey("key"), Is.False);
		}

		[Test]
		public void Can_GetTimeToLive()
		{
			Redis.SetEntry("key", "val");
			Redis.Expire("key", 10);

			var ttl = Redis.GetTimeToLive("key");
			Assert.That(ttl.TotalSeconds, Is.GreaterThanOrEqualTo(9));
			Thread.Sleep(1700);

			ttl = Redis.GetTimeToLive("key");
			Assert.That(ttl.TotalSeconds, Is.LessThanOrEqualTo(9));
		}

		[Test]
		public void Can_Ping()
		{
			Assert.That(Redis.Ping(), Is.True);
		}

		[Test]
		public void Can_Echo()
		{
			Assert.That(Redis.Echo("Hello"), Is.EqualTo("Hello"));
		}

		[Test]
		public void Can_SlaveOfNoOne()
		{
			Redis.SlaveOfNoOne();			
		}

		[Test]
		public void Can_Save()
		{
			Redis.Save();
		}

		[Test]
		public void Can_BgSave()
		{
			Redis.BgSave();
		}

		[Test]
		public void Can_Quit()
		{
			Redis.Quit();
		}

		[Test]
		public void Can_BgRewriteAof()
		{
			Redis.BgRewriteAof();
		}

		[Test]
		[Ignore("Works too well and shutdown the server")]
		public void Can_Shutdown()
		{
			Redis.Shutdown();
		}

		[Test]
		public void Can_get_Keys_with_pattern()
		{
			5.Times(i => Redis.SetEntry("k1:" + i, "val"));
			5.Times(i => Redis.SetEntry("k2:" + i, "val"));

			var keys = Redis.Keys("k1:*");
			Assert.That(keys.Length, Is.EqualTo(5));
		}

		[Test]
		public void Can_GetAll()
		{
			var keysMap = new Dictionary<string, string>();

			10.Times(i => keysMap.Add("key" + i, "val" + i));

			Redis.SetAll(keysMap);

			var map = Redis.GetAll<string>(keysMap.Keys);
			var mapKeys = Redis.GetValues(keysMap.Keys.ToList<string>());

			foreach (var entry in keysMap)
			{
				Assert.That(map.ContainsKey(entry.Key), Is.True);
				Assert.That(mapKeys.Contains(entry.Value), Is.True);
			}
		}

		[Test]
		public void Can_AcquireLock()
		{
			Redis.IncrementValue("key"); //1

			var asyncResults = 5.TimesAsync(i =>
				IncrementKeyInsideLock(i, new RedisClient(TestConfig.SingleHost)));

			asyncResults.WaitAll(TimeSpan.FromSeconds(5));

			var val = Redis.Get<int>("key");

			Assert.That(val, Is.EqualTo(1 + 5));
		}

		private static void IncrementKeyInsideLock(int clientNo, IRedisClient client)
		{
			using (client.AcquireLock("testlock"))
			{
				Console.WriteLine("client {0} acquired lock", clientNo);
				var val = client.Get<int>("key");

				Thread.Sleep(200);

				client.Set("key", val + 1);
				Console.WriteLine("client {0} released lock", clientNo);
			}
		}

		[Test]
		public void Can_AcquireLock_TimeOut()
		{

			Redis.IncrementValue("key"); //1
			var acquiredLock = Redis.AcquireLock("testlock");
			var waitFor = TimeSpan.FromMilliseconds(1000);
			var now = DateTime.Now;

			try
			{
				using (var client = new RedisClient(TestConfig.SingleHost))
				{
					using (client.AcquireLock("testlock", waitFor))
					{
						Redis.IncrementValue("key"); //2
					}
				}
			}
			catch (TimeoutException tex)
			{
				var val = Redis.Get<int>("key");
				Assert.That(val, Is.EqualTo(1));

				var timeTaken = DateTime.Now - now;
				Assert.That(timeTaken.TotalMilliseconds > waitFor.TotalMilliseconds, Is.True);
				Assert.That(timeTaken.TotalMilliseconds < waitFor.TotalMilliseconds + 1000, Is.True);
				return;
			}
			Assert.Fail("should have Timed out");
		}

		[Test]
		public void Can_Append()
		{
			const string expectedString = "Hello, " + "World!";
			Redis.SetEntry("key", "Hello, ");
			var currentLength = Redis.AppendToValue("key", "World!");

			Assert.That(currentLength, Is.EqualTo(expectedString.Length));

			var val = Redis.GetValue("key");
			Assert.That(val, Is.EqualTo(expectedString));
		}

		[Test]
		public void Can_Substring()
		{
			const string helloWorld = "Hello, World!";
			Redis.SetEntry("key", helloWorld);

			var fromIndex = "Hello, ".Length;
			var toIndex = "Hello, World".Length - 1;

			var expectedString = helloWorld.Substring(fromIndex, toIndex - fromIndex + 1);
			var world = Redis.GetSubstring("key", fromIndex, toIndex);

			Assert.That(world.Length, Is.EqualTo(expectedString.Length));
		}
	}

}
