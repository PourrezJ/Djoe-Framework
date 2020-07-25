using CitizenFX.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server.Colshape
{
    public abstract class Colshape : IColshape
    {
        #region Fields
        public uint Id { get; }

        public Vector3 Position { get; }

        public float Radius { get; }

        private readonly HashSet<Player> _entities;

        public ICollection<Player> Entities => _entities;

        private ConcurrentDictionary<string, object> _data;
        #endregion

        #region Events
        public event ColshapePlayerEventHandler OnPlayerEnterColshape;
        public event ColshapePlayerEventHandler OnPlayerLeaveColshape;
        public event ColshapePlayerEventHandler OnPlayerInteractInColshape;
        #endregion

        #region Constructor
        public Colshape(uint id, Vector3 position, float radius)
        {
            Id = id;
            Position = position;
            Radius = radius;
            _entities = new HashSet<Player>();
            _data = new ConcurrentDictionary<string, object>();
        }
        #endregion

        #region Public methods
        public void AddEntity(Player entity)
        {
            bool entityAdded;

            lock (_entities)
            {
                entityAdded = _entities.Add(entity);
            }

            if (entityAdded)
                OnPlayerEnterColshape?.Invoke(this, entity);
        }

        public void Delete()
        {
            ColshapeManager.DeleteColshape(this);
        }

        public bool GetData<T>(string key, out T result)
        {
            object value;
            bool res = _data.TryGetValue(key, out value);
            result = (T)value;
            return res;
        }

        public bool IsEntityIn(Player entity)
        {
            lock (_entities)
            {
                return _entities.Contains(entity);
            }
        }

        public abstract bool IsEntityInside(Player entity);

        public abstract bool IsPositionInside(Vector3 position);

        public void PlayerInteractInColshape(Player client)
        {
            OnPlayerInteractInColshape?.Invoke(this, client);
        }

        public void RemoveEntity(Player entity)
        {
            bool entityRemoved;

            lock (_entities)
            {
                entityRemoved = _entities.Remove(entity);
            }

            if (entityRemoved)
            {
                OnPlayerLeaveColshape?.Invoke(this, entity);
            }
        }

        public void SetData(string key, object value)
        {
            _data.AddOrUpdate(key, value, (k, oldValue) => value);
        }
        #endregion
    }
}
