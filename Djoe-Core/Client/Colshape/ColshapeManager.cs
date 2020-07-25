using System.Collections.Generic;
using System;
using CitizenFX.Core;
using ClientExtended.External;
using System.Threading.Tasks;

namespace Client.Colshape
{
    #region Delegates
    public delegate void ColshapePlayerEventHandler(IColshape colshape, Player client);
    #endregion

    public class ColshapeManager : BaseScript
    {
        #region Private static fields
        private static volatile uint _colshapeId = 0;
        private static readonly Dictionary<long, IColshape> _colshapes = new Dictionary<long, IColshape>();
        private static readonly HashSet<Player> _entitiesToRemove = new HashSet<Player>();

        private static List<IColshape> NetworkColshapes = new List<IColshape>();
        #endregion

        #region Events
        public static event ColshapePlayerEventHandler OnPlayerEnterColshape;
        public static event ColshapePlayerEventHandler OnPlayerLeaveColshape;
        public static event ColshapePlayerEventHandler OnPlayerInteractInColshape;
        #endregion

        #region Init
        public ColshapeManager()
        {
            EventHandlers["CreateColshape"] += new Action<Player, int, Vector3, float, int>(CreateColshape);
        }
        #endregion

        #region Event handlers
        private static void CreateColshape(Player player, int id, Vector3 position, float range, int type)
        {
            IColshape colshape = null;

            switch (type)
            {
                case 0:
                    colshape = ColshapeManager.CreateCylinderColshape(position, range, range);
                break;

                case 1:
                    colshape = ColshapeManager.CreateSphereColshape(position, range);
                break;
            }

            NetworkColshapes.Add(colshape);
        }

        public static void OnPlayerDisconnect(Player player, string reason)
        {
            lock (_colshapes)
            {
                foreach (IColshape colshape in _colshapes.Values)
                {
                    if (colshape.IsEntityIn(player))
                    {
                        colshape.RemoveEntity(player);
                        OnPlayerLeaveColshape?.Invoke(colshape, player);
                    }
                }
            }
        }
        #endregion

        #region Public static methods
        public static IColshape CreateCylinderColshape(Vector3 position, float radius, float height)
        {
            IColshape colshape = new CylinderColshape(_colshapeId++, position, radius, height);

            lock (_colshapes)
            {
                _colshapes.Add(colshape.Id, colshape);
            }

            return colshape;
        }

        public static IColshape CreateSphereColshape(Vector3 position, float radius)
        {
            IColshape colshape = new SphereColshape(_colshapeId++, position, radius);

            lock (_colshapes)
            {
                _colshapes.Add(colshape.Id, colshape);
            }

            return colshape;
        }

        public static void DeleteColshape(IColshape colshape)
        {
            lock (_colshapes)
            {
                _colshapes.Remove(colshape.Id);
            }
        }

        [Tick]
        public Task OnTick()
        {
            lock (_colshapes)
            {
                foreach (IColshape colshape in _colshapes.Values)
                {
                    lock (colshape.Entities)
                    {                        
                        foreach (Player entity in colshape.Entities)
                        {
                            if (!colshape.IsEntityInside(entity))
                            {
                                _entitiesToRemove.Add(entity);

                                OnPlayerLeaveColshape?.Invoke(colshape, entity);

                                if (NetworkColshapes.Contains(colshape))
                                    BaseScript.TriggerServerEvent("OnPlayerLeaveColshape", colshape.Id);
                            }
                        }

                        if (_entitiesToRemove.Count > 0)
                        {
                            foreach (Player entity in _entitiesToRemove)
                                colshape.RemoveEntity(entity);

                            _entitiesToRemove.Clear();
                        }
                    }
                }
            }
            
            lock (_colshapes)
            {
                foreach (IColshape colshape in _colshapes.Values)
                {
                    if (!colshape.IsEntityIn(Game.Player) && colshape.IsEntityInside(Game.Player))
                    {
                        colshape.AddEntity(Game.Player);
                        OnPlayerEnterColshape?.Invoke(colshape, Game.Player);
                        if (NetworkColshapes.Contains(colshape))
                            BaseScript.TriggerServerEvent("OnPlayerEnterColshape", colshape.Id);
                    }
                }
            }
            return Task.FromResult(0);
        }

        private static void OnEntityInteractInColshape(Player client, object[] args)
        {
            try
            {
                if (args[0] == null || (long)args[0] != 69)
                {
                    Debug.WriteLine($"[OnEntityInteractInColshape] Error args[0]: {args[0]}");
                        return;
                }

                lock (_colshapes)
                {
                    IColshape colshape = _colshapes[(long)args[1]];

                    if (colshape.IsEntityIn(client))
                    {
                        colshape.PlayerInteractInColshape(client);
                        OnPlayerInteractInColshape?.Invoke(colshape, client);       
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}
