using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class CollectibleSpawner: MonoBehaviour
    {
        [SerializeField]
        private Collectible _collectiblePrefab;
        [SerializeField]
        private float _spawnInterval;
        [SerializeField]
        public Vector3 MinSpawnPosition;
        [SerializeField]
        public Vector3 MaxSpawnPosition;

        private float _spawnTimer;

        private Pool<Collectible> _collectiblePool;

        /// <summary>
        /// Initializes pool for collectibles
        /// </summary>
        void Awake()
        {
            _collectiblePool = new Pool<Collectible>(10, false, _collectiblePrefab,InitCollectible);
        }

        /// <summary>
        /// Initializes the colletctibles to have reference for CollectibleCollected
        /// acion
        /// </summary>
        /// <param name="collectible"> collectible currently initializing </param>
        private void InitCollectible(Collectible collectible)
        {
            collectible.Init(CollectibleCollected);
        }

        // Update is called once per frame
        void Update()
        {
            if(UpdateSpawnTimer())
            {
                SpawnCollectible();
            }
        }

        /// <summary>
        /// Updates the spawntimer for collectibles
        /// </summary>
        /// <returns> true if the collectible can be spawned else returns false</returns>
        private bool UpdateSpawnTimer()
        {
            _spawnTimer += Time.deltaTime;
            if(_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Spawns collectible if the pool had unused collectibles left
        /// </summary>
        private void SpawnCollectible ()
        {
            Collectible collectible = _collectiblePool.GetPooledObject();

            if(collectible != null)
            {
                collectible.transform.position = GetRandomPosition();
            }
        }

        /// <summary>
        /// Randoms position from predetermined positions
        /// </summary>
        /// <returns> Random position between min and max positions</returns>
        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(Mathf.Min(MinSpawnPosition.x, MaxSpawnPosition.x),Mathf.Max(MinSpawnPosition.x, MaxSpawnPosition.x));
            float z = Random.Range(Mathf.Min(MinSpawnPosition.z, MaxSpawnPosition.z), Mathf.Max(MinSpawnPosition.z, MaxSpawnPosition.z));
            return new Vector3(x, 0.5f, z);
        }

        /// <summary>
        /// Draws lines between min and max spawnpositions
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(MinSpawnPosition, new Vector3(MaxSpawnPosition.x, MaxSpawnPosition.y, MinSpawnPosition.z));
            Gizmos.DrawLine(MinSpawnPosition, new Vector3(MinSpawnPosition.x, MaxSpawnPosition.y, MaxSpawnPosition.z));
            Gizmos.DrawLine(MaxSpawnPosition, new Vector3(MaxSpawnPosition.x, MaxSpawnPosition.y, MinSpawnPosition.z));
            Gizmos.DrawLine(MaxSpawnPosition, new Vector3(MinSpawnPosition.x, MaxSpawnPosition.y, MaxSpawnPosition.z));
        }

        /// <summary>
        /// Triggered if collectible is collected by player
        /// </summary>
        /// <param name="collectible"> Reference to the collectible that has been collected,
        /// so it can be returned to the pool </param>
        private void CollectibleCollected ( Collectible collectible )
        {
            if(!_collectiblePool.ReturnObject(collectible))
            {
                Debug.LogError("Could not return collectible back to the pool! ");
            }
        }
    }
}
