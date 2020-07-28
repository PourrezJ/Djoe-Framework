using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtended.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientExtented.External
{
    public class EntityBone
	{
		#region Fields
		protected readonly Entity _owner;
		protected readonly int _index;
		#endregion

		public EntityBone(Entity owner, int boneIndex)
		{
			_owner = owner;
			_index = boneIndex;
		}

		public EntityBone(Entity owner, string boneName)
		{
			_owner = owner;
			_index = API.GetEntityBoneIndexByName(owner.Handle, boneName);
		}

		public int Index { get { return _index; } }

		public Entity Owner { get { return _owner; } }

		public static implicit operator int(EntityBone bone)
		{
			return ReferenceEquals(bone, null) ? -1 : bone.Index;
		}

		public Vector3 Position
		{
			get
			{
				return API.GetWorldPositionOfEntityBone(_owner.Handle, _index);
			}
		}
	}
}
