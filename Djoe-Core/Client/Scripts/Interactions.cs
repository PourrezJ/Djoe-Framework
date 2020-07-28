using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Utils.Extensions;
using ClientExtended.External;
using Shared;
using System;
using System.Threading.Tasks;
using Control = ClientExtended.External.Control;

namespace Client.Scripts
{
    internal class Interactions : BaseScript
    {
        private static async void InventoryUseItem(dynamic itemID, dynamic Quantity)
        {

        }

        public Interactions()
        {
            GameMode.RegisterEventHandler("InventoryUseItem", new Action<dynamic, dynamic>(InventoryUseItem));
            Tick += OnTick;
        }

        [Tick]
        private static Task OnTick()
        {
            Game.DisableControlThisFrame(0, Control.SelectItemWheel);
            Game.DisableControlThisFrame(0, Control.Loot);
            Game.DisableControlThisFrame(0, Control.Loot2);
            Game.DisableControlThisFrame(0, Control.Loot3);
            Game.DisableControlThisFrame(0, Control.QuickUseItem);
            Game.DisableControlThisFrame(0, Control.SelectItemWheel);

            foreach (Control fi in (Control[])Enum.GetValues(typeof(Control)))
            {
                if (Game.IsDisabledControlJustPressed(0, fi))
                {
                    switch (fi)
                    {
                        case Control.Loot:
                        case Control.Loot2:
                        case Control.Loot3:
                        case Control.QuickUseItem:
                        case Control.SelectItemWheel:
                            TriggerServerEvent("OnKeyPress", ((uint)fi).ToString());
                            break;
                    }
                }
            }
            
            return Task.FromResult(0);
            /*
            RaycastResult? result = Raycast.CastCapsule(pos, farAway, Game.Player.Character.Handle, -1);

            if (result == null)
                return Task.FromResult(0);

            info = $"Hit: {result?.DitHit.ToString()} \n" +
                               $"Entity: {result?.HitEntity.Handle} \n" +
                               $"Coords: X:{result?.HitPosition.X} Y:{result?.HitPosition.Y} Z:{result?.HitPosition.Z}";
            UIHelper.DrawText(info, 0, 0.5f, 0.5f, 0.2f, 0.2f, Color.FromArgb(255, 255, 255, 255), true);

            return Task.FromResult(0);*/
        }
    }
}
