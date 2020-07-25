using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Scripts;
using ClientExtended;
using ClientExtended.External;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Control = ClientExtended.External.Control;

namespace Client.Menus
{
    public class Admin
    {
        public static bool NoClipActived;

        public static void Init(StaffRank staffRank)
        {
            GameMode.RegisterEventHandler("API_SetNoClip", new Action<bool>(SetNoClip));
            GameMode.RegisterTickHandler(OnNoClip);

            if (staffRank >= StaffRank.Animateur)
                SetAdminCommands();
        }

        private static void SetAdminCommands()
        {
            API.RegisterCommand("tp", new Action<int, List<object>, string>((source, args, raw) => {

                if (args.Count != 3)
                {
                    Debug.WriteLine("Erreur dans la saisie de la commande.");
                    return;
                }

                Game.PlayerPed.PositionNoOffset = new Vector3(float.Parse(args[0].ToString()), float.Parse(args[1].ToString()), float.Parse(args[2].ToString()));

            }), false);
        }

        private static void SetNoClip(bool actived)
        {
            Game.PlayerPed.IsPositionFrozen = actived;
            Toast.AddToast("NoClip: " + ((actived) ? "Activé" : "Désactivé"), 1000);
            NoClipActived = actived;
        }

        private static float speed;

        private static Task OnNoClip()
        {
            if (NoClipActived)
            {
                int pedID = Game.Player.Character.IsOnMount ? Game.Player.Character.GetMount.Handle : Game.Player.Character.Handle;

                Vector3 curPosition = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, pedID);
                var camDirection = Misc.GetCamDirection();
                Function.Call(Hash.SET_ENTITY_VELOCITY, pedID, 0.0001f, 0.0001f, 0.0001f);

                if (Game.IsControlPressed(0, ClientExtended.External.Control.Sprint))
                    speed = 2.0f;
                else if (Game.IsControlPressed(0, Control.Duck))
                    speed = 0.5f;
                else
                    speed = 1.0f;

                if (Game.IsControlPressed(0, Control.MoveUpOnly))
                {
                    curPosition.X += speed * camDirection.X;
                    curPosition.Y += speed * camDirection.Y;
                    curPosition.Z += speed * camDirection.Z;
                }
                else if (Game.IsControlPressed(0, Control.MoveDownOnly))
                {
                    curPosition.X -= speed * camDirection.X;
                    curPosition.Y -= speed * camDirection.Y;
                    curPosition.Z -= speed * camDirection.Z;
                }

                Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, pedID, curPosition.X, curPosition.Y, curPosition.Z, false, false, false);
            }

            return Task.FromResult(0);
        }
    }
}
