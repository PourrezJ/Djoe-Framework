using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using Newtonsoft.Json;
using Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Control = ClientExtented.External.Control;

namespace Client.Scripts
{
    internal class Interactions : BaseScript
    {
        public Interactions()
        {
            EventHandlers["InventoryUseItem"] += new Action<dynamic, dynamic>(InventoryUseItem);
            EventHandlers["EatOrDrink"] += new Action<string, string>(EatOrDrink);
            Tick += OnTick;
        }

        private async void EatOrDrink(string eatordrink, string prop)
        {
            Debug.WriteLine("EatOrDrink");
            bool drink = (eatordrink == "drink");
            Vector3 playerCoords = Game.PlayerPed.Position;

            string dict = "mech_inventory@clothing@bandana";
            string anim = "NECK_2_FACE_RH";
            /*
            if (drink)
            {
                dict = "amb_rest_drunk@world_human_drinking@male_a@idle_a";
                anim = "idle_a";

                if (!API.IsPedMale(Game.PlayerPed.Handle))
                {
                    dict = "amb_rest_drunk@world_human_drinking@female_a@idle_b";
                    anim = "idle_b";
                }
            }*/

             var worldProp = await World.CreateProp(new Model(prop), playerCoords + new Vector3(0,0,2), new Vector3(), true, false, true);

             Debug.WriteLine("prop: " + worldProp.Handle);

             int boneIndex = API.GetEntityBoneIndexByName(API.PlayerPedId(), "SKEL_R_Finger12");

             Debug.WriteLine("boneIndex: " + boneIndex);
             await Delay(1000);

             Debug.WriteLine($"dict: {dict} | anim: {anim}");

            API.RequestAnimDict(dict);
            while (!API.HasAnimDictLoaded(dict))
            {
                await Delay(100);
            }
            //WORLD_HUMAN_COFFEE_DRINK
            //WORLD_HUMAN_DRINKING biere
            //Game.PlayerPed.Tasks.StartScenarioInPlace("WORLD_HUMAN_DRINKING");

            //Game.PlayerPed.Tasks.PlayAnimation(dict, anim, 1f, 5000, AnimationFlags.Loop);

            Function.Call(Hash.TASK_PLAY_ANIM, API.PlayerPedId(), dict, anim, 1.0f, 8.0f, 5000, 31, 0.0f, false, false, false);
            //Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, API.PlayerPedId(), boneIndex, 0.02f, 0.028f, 0.001f, 15.0f, 175.0f, 0.0f, true, true, false, true, 1, true);
            
            await Delay(6000);

            worldProp.Delete();
            Game.PlayerPed.Tasks.ClearSecondary();
        }

        private void InventoryUseItem(dynamic itemID, dynamic Quantity)
        {

        }

        private static Control[] UsedControls =
        {
            Control.SelectItemWheel,
            Control.Loot,
            Control.Loot2,
            Control.Loot3,
            Control.QuickUseItem,
            Control.SpecialAbilityAction,
            Control.FrontendCancel,
            //Control.CreatorMenuToggle,
            //Control.OpenWheelMenu
        };

        private static Task OnTick()
        {
            foreach (Control fi in UsedControls)
            {
                Game.DisableControlThisFrame(0, fi);
                if (Game.IsDisabledControlJustPressed(0, fi))
                {
                    RayCastResult sendResult = null;

                    try
                    {
                        var result = World.CrosshairRaycast(7, IntersectOptions.Everything, Game.PlayerPed);

                        sendResult = new RayCastResult()
                        {
                            DidHit = result.DitHit,
                            DidHitEntity = result.DitHitEntity,
                            HitPosition = result.HitPosition,
                            SurfaceNormal = result.SurfaceNormal,
                            Result = result.Result,
                            IsPed = result.HitEntity.Model.IsPed
                        };
                    }
                    catch { 
                        // Native invocation error, what? idk 
                    }
                    finally
                    {
                        if (UsedControls.Contains(fi))
                            TriggerServerEvent("OnKeyPress", ((uint)fi).ToString(), (sendResult != null) ? JsonConvert.SerializeObject(sendResult) : "");
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
