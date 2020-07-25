using CitizenFX.Core;
using ClientExtended.External;
using System.Collections.Generic;

namespace Client.Colshape
{
    public interface IColshape
    {
        #region Properties
        uint Id { get; }

        Vector3 Position { get; }

        float Radius { get; }

        ICollection<Player> Entities { get; }
        #endregion

        #region Events
        event ColshapePlayerEventHandler OnPlayerEnterColshape;
        event ColshapePlayerEventHandler OnPlayerLeaveColshape;
        event ColshapePlayerEventHandler OnPlayerInteractInColshape;
        #endregion

        #region Methods
        void AddEntity(Player entity);

        void Delete();

        bool GetData<T>(string key, out T result);

        bool IsEntityIn(Player entity);

        bool IsEntityInside(Player entity);

        bool IsPositionInside(Vector3 position);

        void PlayerInteractInColshape(Player client);

        void RemoveEntity(Player entity);

        void SetData(string key, object value);
        #endregion
    }
}
