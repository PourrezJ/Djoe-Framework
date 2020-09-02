using CitizenFX.Core;
using Newtonsoft.Json;
using ProtoBuf;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    [ProtoContract]
    public class RpcErrorData
    {
        [ProtoMember(1)]
        public string ErrorCodeString;
        [ProtoMember(2)]
        public int ErrorCode;
        [ProtoMember(3)]
        public string DomainString;
        [ProtoMember(4)]
        public int DomainCode;
        [ProtoMember(5)]
        public byte[] DataEx;
    }

    [ProtoContract]
    public class RpcError
    {
        [ProtoMember(1)]
        public int ErrorCode;
        [ProtoMember(2)]
        public string ErrorMessage;
        [ProtoMember(3)]
        public RpcErrorData Data;
    }

    [ProtoContract]
    public class RpcHeader
    {
        [ProtoMember(1)]
        public string RequestId;
        [ProtoMember(2)]
        public string MethodName;
        [ProtoMember(3)]
        public RpcError Error;
        [ProtoMember(4)]
        public string srcTid;
    }

    [ProtoContract]
    public class RpcMessage
    {  
        [ProtoMember(1)]
        public RpcHeader Header;
        [ProtoMember(2)]
        public byte[] Content;
    }

    [ProtoContract]
    public class RpcResponseContainer
    {
        [ProtoMember(1)]
        public byte[] Content;
    }

    [ProtoContract]
    public class RpcResponseMessage
    {
        [ProtoMember(1)]
        public RpcHeader Header;
        [ProtoMember(2)]
        public RpcResponseContainer Container;
    }

    [ProtoContract]
    public class MpGamerHandleDto
    {
        [ProtoMember(1)]
        public string Gh;
    }

    [ProtoContract]
    public class MpPeerAddressDto
    {
        [ProtoMember(1)]
        public string Addr;
    }

    public class PlayerSession
    {
        public MpGamerHandleDto gh;
        public MpPeerAddressDto peerAddress;
        public int discriminator;
        public int slot;
    }

    public class Main : BaseScript
    {
        public Dictionary<Player, PlayerData> playerDatas;

        public Main()
        {
            Debug.WriteLine("Starting Session Manager .Net");

            EventHandlers["__cfx_internal:pbRlScSession"] += new Action<Player, byte[]>(OnMessage);

            playerDatas = new Dictionary<Player, PlayerData>();

        }

        private void OnMessage([FromSource] Player player, byte[] data)
        {
            RpcMessage message = DeserializeBinary<RpcMessage>(data) as RpcMessage;
            Logger.Info("OnMessage: " + JsonConvert.SerializeObject(message) + "\n");


            switch (message.Header.MethodName)
            {
                case "InitSession":
                    Debug.WriteLine("InitSession");

                    //return makeResponse("InitSessionResponse", new InitSessionResponse() { Sesid = ByteString.CopyFrom((byte)16) });
                    break;

                case "InitPlayer2":
                    Debug.WriteLine("InitPlayer2 \n");
                    /*
                    if (data == null)
                        return null;

                    var req = InitPlayer2_Parameters.Parser.ParseFrom(data);

                    if (req == null)
                        return null;

                    playerDatas[source] = new PlayerData()
                    {
                        gh = req.Gh,
                        peerAddress = req.PeerAddress,
                        discriminator = req.Discriminator,
                        slot = -1
                    };

                    var result = new InitPlayerResult()
                    {
                        Code = 0
                    };

                    return makeResponse("InitPlayerResult", result);*/
                    break;
                case "GetRestrictions":
                    Debug.WriteLine("GetRestrictions");
                    break;

                case "ConfirmSessionEntered":
                    Debug.WriteLine("ConfirmSessionEntered");

                    break;

                case "QueueForSession_Seamless":
                    Debug.WriteLine("QueueForSession_Seamless");
                    break;
            }
        }

        private void emitMsg(Player target, Byte[] data)
        {
            Console.WriteLine("Emit msg");
            target.TriggerEvent("__cfx_internal:pbRlScSession", data);
        }

        public static object DeserializeBinary<T>(byte[] data)
        {
            object output;
            using (var stream = new MemoryStream(data))
            {
                try
                {
                    output = Serializer.Deserialize<T>(stream);
                }
                catch (ProtoException)
                {
                    return null;
                }
            }
            return output;
        }
        public static byte[] SerializeBinary(object data)
        {
            using (var stream = new MemoryStream())
            {
                stream.SetLength(0);
                Serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }
    }
}
