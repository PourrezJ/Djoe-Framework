using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using ProtoBuf;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public static Dictionary<Player, PlayerSessionData> playerDatas { get; set; }

        public static int HostIndex { get; set; } = -1;

        public Main()
        {
            Debug.WriteLine("Starting Session Manager .Net");

            EventHandlers["__cfx_internal:pbRlScSession"] += new Action<Player, byte[]>(OnMessage);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            playerDatas = new Dictionary<Player, PlayerSessionData>();
             
            API.RegisterCommand("leavesession", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var pl = new PlayerList();
                var player = pl[source];

                player.TriggerEvent("LeaveSession");

            }), false);

            API.RegisterCommand("changesession", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                    return;

                var pl = new PlayerList();
                var player = pl[source];

                if (int.TryParse(args[0].ToString(), out int id))
                {
                    Logger.Debug("Changement de session pour " + player.Name);
                    player.TriggerEvent("ChangeSession", id);
                }

            }), false);
        }

        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            Debug.WriteLine($"Player {player.Name} dropped (Reason: {reason}).");

            lock(playerDatas)
            {
                PlayerSessionData oData = playerDatas[player];
                playerDatas.Remove(player);

                // Celui qui viens de déco était host
                if (oData != null && HostIndex == oData.Slot)
                {
                    if (playerDatas.Count > 0)
                    {
                        var pda = playerDatas.ElementAt(0);
                        HostIndex = pda.Value.Slot;

                        foreach (var pData in playerDatas)
                        {
                            emitHostChanged(pData.Key, SerializeBinary(new SessionSubcommandHostChanged { Index = HostIndex }));
                        }
                    }
                    else
                        HostIndex = -1;
                }

                if (oData == null)
                    return;

                foreach (var pData in playerDatas)
                {
                    emitRemovePlayer(pData.Key, SerializeBinary(new SessionSubcommandRemovePlayer { Id = oData.Id }));
                }
            }
        }

        private void OnMessage([FromSource] Player player, byte[] data)
        {
            RpcMessage message = DeserializeBinary<RpcMessage>(data) as RpcMessage;

            Logger.Debug("C# message: " + JsonConvert.SerializeObject(message) + "\n");

            RpcResponseMessage response = null;

            switch (message.Header.MethodName)
            {
                case "InitSession":
                    Logger.Debug("InitSession\n");

                    response = makeResponse(typeof(InitSessionResponse), new InitSessionResponse { Sesid = new byte[16] });
                    break;

                case "InitPlayer2":
                    Logger.Debug("InitPlayer2\n");

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
                    Logger.Debug("GetRestrictions\n");

                    response = makeResponse(typeof(GetRestrictionsResult), new GetRestrictionsResult { Data = new GetRestrictionsData() });
                    break;

                case "ConfirmSessionEntered":
                    Logger.Debug("ConfirmSessionEntered\n");

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
                            HostIndex = playerDatas[player].Slot;

                        emitSessionCmds(player, 0, "EnterSession", SerializeBinary(new SessionSubcommandEnterSession
                        {
                            Index = playerDatas[player].Slot,
                            Hindex = HostIndex,
                            sessionFlags = 0,
                            Mode = 0,
                            //Size = playerDatas.Count,
                            Size = 2,
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
                                Index = playerDatas[player].Slot
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
                                    Index = pData.Value.Slot
                                }));

                                emitAddPlayer(pData.Key, SerializeBinary(aboutMe));
                            }
                        });
                    });
                    
                    response = makeResponse(typeof(QueueForSessionResult), new QueueForSessionResult { Code = 1 });
                    
                    break;
            }

            if (response == null)
            {
                Logger.Warn("Session Manager response null");
                return;
            }

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
            Console.WriteLine("C# Emit: " + JsonConvert.SerializeObject(data));
            target.TriggerEvent("__cfx_internal:pbRlScSession", data);
        }

        private static void emitSessionCmds(Player target, uint cmd, string cmdName, byte[] msg)
        {
            Console.WriteLine("Emit SessionCmds: " + cmdName);

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
                            Cmdname = cmdName,
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

        //public static int slotUsed = 0;

        public static int AssignSlot()
        {
            for(int a = 0; a < 32; a++)
            {
                if (!playerDatas.Any(p=>p.Value.Slot == a))
                {
                    return a;
                }
            }

            return 0;
        }
    }
}
