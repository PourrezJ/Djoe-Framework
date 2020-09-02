using CitizenFX.Core;
using Newtonsoft.Json;
using ProtoBuf;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    public class PlayerSessionData
    {
        public MpGamerHandleDto Gh { get; set; }

        public MpPeerAddressDto PeerAddress { get; set; }

        public int Discriminator { get; set; }

        public int Slot { get; set; } = -1;

        public MpSessionRequestIdDto RequestId { get; set; }

        public PlayerIdSto Id { get; set; }
    }

    public class Main : BaseScript
    {
        public Dictionary<Player, PlayerSessionData> playerDatas { get; set; }

        public int HostIndex { get; set; } = -1;

        public Main()
        {
            Debug.WriteLine("Starting Session Manager .Net");

            EventHandlers["__cfx_internal:pbRlScSession"] += new Action<Player, byte[]>(OnMessage);

            playerDatas = new Dictionary<Player, PlayerSessionData>();

        }

        private void OnMessage([FromSource] Player player, byte[] data)
        {
            RpcMessage message = DeserializeBinary<RpcMessage>(data) as RpcMessage;
            Logger.Debug("OnMessage: " + JsonConvert.SerializeObject(message) + "\n");

            RpcResponseMessage response = null;

            switch (message.Header.MethodName)
            {
                case "InitSession":
                    Logger.Debug("InitSession");

                    response = makeResponse(typeof(InitSessionResponse), new InitSessionResponse { Sesid = new byte[16] });
                    break;

                case "InitPlayer2":
                    Logger.Debug("InitPlayer2 \n");

                    InitPlayer2Parameters ip2 = DeserializeBinary<InitPlayer2Parameters>(message.Content) as InitPlayer2Parameters;

                    playerDatas[player] = new PlayerSessionData
                    {
                        Gh = ip2.Gh,
                        PeerAddress = ip2.peerAddress,
                        Discriminator = ip2.Discriminator,
                        Slot = -1
                    };

                    
                    var result = new InitPlayerResult
                    {
                        Code = 0
                    };

                    response = makeResponse(typeof(InitPlayerResult), result);

                    break;
                case "GetRestrictions":
                    Logger.Debug("GetRestrictions");
                    break;

                case "ConfirmSessionEntered":
                    Logger.Debug("ConfirmSessionEntered");

                    emitMsg(player, new byte[] { });
                    return;

                case "QueueForSession_Seamless":
                    Logger.Debug("QueueForSession_Seamless\n");

                    QueueForSessionSeamlessParameters qsp = DeserializeBinary<QueueForSessionSeamlessParameters>(message.Content) as QueueForSessionSeamlessParameters;

                    playerDatas[player].RequestId = qsp.requestId;
                    playerDatas[player].Id = qsp.requestId.Requestor;
                    playerDatas[player].Slot = AssignSlot();

                    Misc.Delay(50, () =>
                    {
                        emitMsg(player, SerializeBinary(new RpcMessage
                        {
                            Header = new RpcHeader
                            {
                                MethodName = "QueueEntered"
                            },
                            Content = SerializeBinary(new QueueEnteredParameters
                            {
                                queueGroup = 69,
                                requestId = qsp.requestId,
                                optionFlags = qsp.optionFlags
                            })
                        }));


                        if (HostIndex == -1)
                            HostIndex = playerDatas[player].Slot | 0;

                        emitSessionCmds(player, 0, "EnterSession", SerializeBinary(new SessionSubcommandEnterSession
                        {
                            Index = playerDatas[player].Slot | 0,
                            Hindex = HostIndex,
                            sessionFlags = 0,
                            Mode = 0,
                            Size = playerDatas.Count,
                            teamIndex = 0,

                            transitionId = new MpTransitionIdDto
                            {
                                Value = new GuidDto
                                {
                                    A = 0,//2,
                                    B = 0
                                }
                            },

                            sessionManagerType = 0,
                            slotCount = 32
                        }));


                        Misc.Delay(150, () => { 
                        
                            PlayerSessionData meData = playerDatas[player];

                            SessionSubcommandAddPlayer aboutMe = new SessionSubcommandAddPlayer
                            {
                                Id = meData.Id,
                                Gh = meData.Gh,
                                Addr = meData.PeerAddress,
                                Index = playerDatas[player].Slot | 0
                            };
                            
                            foreach(var pData in playerDatas)
                            {
                                if (pData.Key == player || pData.Value.Id == null) 
                                    continue;

                                emitAddPlayer(player, SerializeBinary(new SessionSubcommandAddPlayer
                                {
                                    Id = pData.Value.Id,
                                    Gh = pData.Value.Gh,
                                    Addr = pData.Value.PeerAddress,
                                    Index = pData.Value.Slot | 0
                                }));

                                emitAddPlayer(player, SerializeBinary(aboutMe));
                            }
                        });
                    });
                    
                    response = makeResponse(typeof(QueueForSessionResult), new QueueForSessionResult { Code = 1 });
                    
                    break;
            }

            if (response == null)
                return;

            response.Header.RequestId = message.Header.RequestId;

            emitMsg(player, SerializeBinary(response));
        }

        private RpcResponseMessage makeResponse(Type type, dynamic data)
        {
            if (data == null)
                return null;

            RpcResponseMessage response = new RpcResponseMessage
            {
                Header = new RpcHeader(),
                Container = new RpcResponseContainer
                {
                    Content = SerializeBinary(data)
                }
            };

            return response;
        }

        private static void emitMsg(Player target, Byte[] data)
        {
            Console.WriteLine("Emit msg");
            target.TriggerEvent("__cfx_internal:pbRlScSession", data);
        }

        private static void emitSessionCmds(Player target, uint cmd, string cmdName, byte[] msg)
        {
            Console.WriteLine("Emit SessionCmds");

            emitMsg(target, SerializeBinary(new RpcMessage()
            {
                Header = new RpcHeader()
                {
                    MethodName = "scmds"
                },
                Content = SerializeBinary(new scmdsParameters()
                {
                    Sid = new MpSessionIdDto()
                    {
                        Value = new GuidDto()
                        {
                            A = 2,
                            B = 2
                        }
                    },
                    Ncmds = 1,
                    Cmds = { 
                        new SessionCommand()
                        {
                            Cmd = cmd,
                            Cmdname = cmdName
                        }
                    }
                })
            }));
        }

        public static void emitAddPlayer(Player target, byte[] msg)
        {
            emitSessionCmds(target, 2, "AddPlayer", msg);
        }

        public static void emitRemovePlayer(Player target, byte[] msg)
        {
            emitSessionCmds(target, 3, "RemovePlayer", msg);
        }

        public static void emitHostChanged(Player target, byte[] msg)
        {
            emitSessionCmds(target, 5, "HostChanged", msg);
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

        public static int slotUsed = 0;

        public static int AssignSlot()
        {
            return slotUsed ++;
        }
    }
}
