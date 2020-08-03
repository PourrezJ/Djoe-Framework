using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjoeDev
{
    public class Main : BaseScript
    {
        public Main()
        {
            API.RegisterCommand("test", new Action<int, List<object>, string>((source, args, raw) =>
            {
                /*
                sVar8 = "p_cs_catalogue01x_PH_R_HAND";
                if (!bVar7)
                {
                    sVar8 = "mp001_s_mp_catalogue01x_noanim_PH_R_HAND";
                }
                iVar9 = TASK::_GET_SCENARIO_PROPSET_ENTITY(iVar6, sVar8);

                ENTITY::SET_ENTITY_COORDS(uParam0->f_1305, ENTITY::GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS(uParam0->f_1768, 0.3f, 0.25f, -0.005f), true, false, true, true);
                ENTITY::SET_ENTITY_ROTATION(uParam0->f_1305, ENTITY::GET_ENTITY_ROTATION(uParam0->f_1768, 2) + Vector(180f, 90f, 84f), 2, true);
                ENTITY::SET_ENTITY_VISIBLE(Global_34, fals
                */
                // vVar25 = { ENTITY::GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS(uParam1->f_167.f_1768, ENTITY::GET_ENTITY_COORDS(uParam0->f_1769, true, false)) };

                //f_1767 = joaat("MP001_S_MP_CATALOGUE01X_STORE");
                /*	vVar0 = { ENTITY::GET_ENTITY_COORDS(uParam0->f_1768, true, false) };
                vVar3 = { ENTITY::GET_ENTITY_ROTATION(uParam0->f_1768, 2) };
                if (!ENTITY::DOES_ENTITY_EXIST(uParam0->f_1769))
                {
                uParam0->f_4.f_1295 = OBJECT::CREATE_OBJECT_NO_OFFSET(uParam0->f_1767, vVar0, false, true, false, true);
                uParam0->f_1769 = uParam0->f_4.f_1295;
                }*/


            }), false);
        }
    }
}
