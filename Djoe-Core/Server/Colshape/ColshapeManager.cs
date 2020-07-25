using System.Collections.Generic;
using System;
using CitizenFX.Core;
using System.Threading.Tasks;
using Server;
using System.Linq;

namespace Server.Colshape
{
    #region Delegates
    public delegate void ColshapePlayerEventHandler(IColshape colshape, Player client);
    #endregion

    public class ColshapeManager : BaseScript
    {
        #region Fields
        private static volatile uint _colshapeId = 0;
        private static readonly HashSet<Player> _entitiesToRemove = new HashSet<Player>();
        public static readonly Dictionary<long, IColshape> Colshapes = new Dictionary<long, IColshape>();
        #endregion

        #region Events
        public static event ColshapePlayerEventHandler OnPlayerEnterColshape;
        public static event ColshapePlayerEventHandler OnPlayerLeaveColshape;
        public static event ColshapePlayerEventHandler OnPlayerInteractInColshape;
        #endregion

        #region Init
        public ColshapeManager()
        {
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDisconnect);
        }
        #endregion

        #region Event handlers

        public static void OnPlayerConnect(Player player)
        {
            lock (Colshapes)
            {
                
            }
        }

        public static void OnPlayerDisconnect([FromSource]Player player, string reason)
        {
            lock (Colshapes)
            {
                foreach (IColshape colshape in Colshapes.Values)
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
            IColshape colshape = new CylinderColshape(_colshapeId++, position - new Vector3(0, 0, 1), radius, height);

            lock (Colshapes)
            {
                Colshapes.Add(colshape.Id, colshape);
            }

            return colshape;
        }

        public static IColshape CreateSphereColshape(Vector3 position, float radius)
        {
            IColshape colshape = new SphereColshape(_colshapeId++, position - new Vector3(0, 0, 1), radius);

            lock (Colshapes)
            {
                Colshapes.Add(colshape.Id, colshape);
            }

            return colshape;
        }

        public static void DeleteColshape(IColshape colshape)
        {
            lock (Colshapes)
            {
                Colshapes.Remove(colshape.Id);
            }
        }

        [Tick]
        public Task OnTick()
        {
            lock (Colshapes)
            {
                foreach (IColshape colshape in Colshapes.Values)
                {
                    lock (colshape.Entities)
                    {                        
                        foreach (Player entity in colshape.Entities)
                        {
                            if (!colshape.IsEntityInside(entity))
                            {
                                _entitiesToRemove.Add(entity);

                                OnPlayerLeaveColshape?.Invoke(colshape, entity);

                                entity.TriggerEvent("OnPlayerLeaveColshape", colshape.Id);
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

            lock (Players)
            {
                foreach(var player in Players)
                {
                    lock (Colshapes)
                    {
                        foreach (IColshape colshape in Colshapes.Values)
                        {
                            if (!colshape.IsEntityIn(player) && colshape.IsEntityInside(player))
                            {
                                colshape.AddEntity(player);
                                OnPlayerEnterColshape?.Invoke(colshape, player);
                                player.TriggerEvent("OnPlayerEnterColshape", colshape.Id);
                            }
                        }
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

                lock (Colshapes)
                {
                    IColshape colshape = Colshapes[(long)args[1]];

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
