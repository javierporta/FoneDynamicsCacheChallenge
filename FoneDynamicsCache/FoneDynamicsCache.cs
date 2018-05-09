using System.Collections.Generic;

namespace FoneDynamicsCache
{
    /// <summary>
    /// Implementation of ICache
    /// </summary>
    /// <remarks>
    /// Requisites: Time Complexity 0(1) - Thread-safe - Implement eviction policy (given) - Implement ICache interface (given)
    /// </remarks>
    public class FoneDynamicsCache : ICache<string, object>
    {
        /// <summary>
        /// Dictionary where cache keys and values are stored
        /// </summary>
        private readonly Dictionary<string,object> _cachedData = new Dictionary<string, object>();
        /// <summary>
        /// Auxiliar list which handles LRU with time complexity 0(1) operations
        /// </summary>
        private readonly LinkedList<string> _cachedDataKeys = new LinkedList<string>();
        /// <summary>
        /// Max size of cache
        /// </summary>
        private readonly int _cacheMaxSize;
        /// <summary>
        /// Count of cache keys/values
        /// </summary>
        public int SpaceUsed=0;

        /// <summary>
        /// Constructor of FoneDynamicsCache, accepting cache size as a parameter
        /// </summary>
        /// <param name="cacheSize"></param>
        public FoneDynamicsCache(int cacheSize)
        {
            if (cacheSize < 1)
            {
                throw new System.ArgumentException("Cache cannot be less than 1");
            }
            _cacheMaxSize = cacheSize;
        }

        /// <summary>
        /// Implementation of AddOrUpdate
        /// </summary>
        /// <remarks>
        /// Uses dictionary and linked list with lock to satisfy time complexity, eviction policy and safe thread.
        /// </remarks>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        public void AddOrUpdate(string cacheKey, object cacheValue)
        {
            //lock dictionary and linked list
            lock (_cachedData)
            {
                lock (_cachedDataKeys)
                {
                    if (SpaceUsed >= _cacheMaxSize) //if is not enough space
                    {
                        //max size reached, remove LRU
                        var keyToRemoveCache = _cachedDataKeys.First.Value; //get key from the linked list to remove key/value from cache. the first element of this list is the LRU
                        _cachedData.Remove(keyToRemoveCache); //remove key/value from cache
                        _cachedDataKeys.RemoveFirst(); //remove the first of the list
                    }
                    else
                    {
                        // use a new space of cache
                        SpaceUsed++;
                    }
                    object foo= null; //not used obj
                    if(_cachedData.TryGetValue(cacheKey,out foo))
                    {
                        //update action
                        UpdateValueInCache(cacheKey, cacheValue);
                    }
                    else
                    {
                        //add action
                        AddNewKeyValueInCache(cacheKey, cacheValue);
                    }
                }
            }
        }

        /// <summary>
        /// Perform update operation in cache
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        private void UpdateValueInCache(string cacheKey, object cacheValue)
        {
            _cachedData[cacheKey] = cacheValue; //assign new value to old key
            _cachedDataKeys.Remove(cacheKey); //update usage
            _cachedDataKeys.AddLast(cacheKey); //add to the end of the list because was the last used
        }

        /// <summary>
        /// Perform add operation in cache
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        private void AddNewKeyValueInCache(string cacheKey, object cacheValue)
        {
            _cachedData.Add(cacheKey, cacheValue); //add new key/value to cache
            _cachedDataKeys.AddLast(cacheKey); //add to the end of the list because was the last used
        }

        /// <summary>
        /// Implementation of TryGetValue
        /// </summary>
        /// <remarks>
        /// Uses dictionary and linked list with lock to satisfy time complexity, eviction policy and safe thread.
        /// </remarks>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns>
        /// Return value if key exists and null if not.
        /// </returns>
        public bool TryGetValue(string cacheKey, out object cacheValue)
        {
            //lock dictionary and linked list
            lock (_cachedData)
            {
                lock (_cachedDataKeys)
                {
                    if (_cachedData.TryGetValue(cacheKey, out cacheValue))
                    {
                        _cachedDataKeys.Remove(cacheKey); //update usage
                        _cachedDataKeys.AddLast(cacheKey); //add to the end of the list because was the last used
                        return true;

                    }
                    return false;
                }
            }

        }
    }
}
