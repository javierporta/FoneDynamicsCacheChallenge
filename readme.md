# Fone Dynamics Cache Challange

This is a practical exam asked for Fone Dynamics as a first step of the interview. I have been asked for create a Cache using C# with the following requisites:

* The cache must implement ICache<TKey, TValue> (below).
* The cache must implement an eviction policy (below).
* The cache must be unit tested.
* All operations, including cache eviction, must have O(1) time complexity.
* The cache must be thread-safe. Your consumers will be using the cache from a variety of threads simultaneously.
* You are writing this for other developers, so please consider their feelings and include an appropriate level of documentation.

### ICache<TKey, TValue> Interface
The cache must implement the following interface.

public interface ICache<TKey, TValue>
{
    /// <summary>
    /// Adds the value to the cache against the specified key.
    /// If the key already exists, its value is updated.
    /// </summary>
    void AddOrUpdate(TKey key, TValue value);
    /// <summary>
    /// Attempts to gets the value from the cache against the specified key
    /// and returns true if the key existed in the cache.
    /// </summary>
    bool TryGetValue(TKey key, out TValue value);
}

### Eviction Policy
When the cache is constructed, it should take as an argument the maximum number of elements stored in the cache.

When an item is added to the cache, a check should be run to see if the cache size exceeds the maximum number of elements permitted. If this is the case, then the least recently added/updated/retrieved item should be evicted from the cache.etting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.


## About the Solution

To ensure satisifying all the requsites I chose to use a Dictionary to save cache key/values and a LinkedList to handle LRU.
I used O(1) operations of those data types. In addition, I use *lock* to ensure thread-safe. I might use ConcurrentDictionary but I prefered to use Dictionaty + LinkedList + Lock for many reasons (faster, more control, etc).
This cache does not implement Remove and Clear operations, as well as a lot of features that a great Cache has to implement. Just does what was required to do.

## Documentation
You may note that code is overcommented. I'm not a irrelevant comments lover, I prefer that my code talks, but, as it is an exam, I decided to comment each method and each line to show what I'm doing in each step and explain why.
XML document generated at building was copied to root of the repo and it is called FoneDynamicsCache.xml.


## Projects of Solution

* FoneDynamicsCache: Cache interface and implementation (.Net Standard 1.4)
* FoneDynamicsCache.Test: Unit Test project for FoneDynamicsCache (.Net Framework 4.6.1)

## Prerequisites

Just build project to download nuget packages.


## Unit Tests

This project uses (FluentAssertion) [https://fluentassertions.com/] package to create some of the assertions.
There is only one class called CacheTests.cs.
For sure, all unit tests written have passed but feel free to run them.

*Code Coverarge Porcentage is: 99%* Tool used: dotCover from Jet Brains (ReSharper 2018.1) Coverage snapshot file is at root of the test project as *CoveCoverageSnapshot.dcvr*. You can open it with dotCover [https://www.jetbrains.com/dotcover/]

## Authors

* **Javier Portaluppi** - (javierporta@hotmail.com)
