﻿// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: my.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
using ProtoBuf;

namespace Server
{

    [ProtoContract()]
    public partial class RpcErrorData : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        [System.ComponentModel.DefaultValue("")]
        public string ErrorCodeString { get; set; } = "";

        [ProtoMember(2)]
        public int ErrorCode { get; set; }

        [ProtoMember(3)]
        [System.ComponentModel.DefaultValue("")]
        public string DomainString { get; set; } = "";

        [ProtoMember(4)]
        public int DomainCode { get; set; }

        [ProtoMember(5)]
        public byte[] DataEx { get; set; }

    }

    [ProtoContract()]
    public partial class RpcError : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public int ErrorCode { get; set; }

        [ProtoMember(2)]
        [System.ComponentModel.DefaultValue("")]
        public string ErrorMessage { get; set; } = "";

        [ProtoMember(3)]
        public RpcErrorData Data { get; set; }

    }

    [ProtoContract()]
    public partial class RpcHeader : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        [System.ComponentModel.DefaultValue("")]
        public string RequestId { get; set; } = "";

        [ProtoMember(2)]
        [System.ComponentModel.DefaultValue("")]
        public string MethodName { get; set; } = "";

        [ProtoMember(3)]
        public RpcError Error { get; set; }

        [ProtoMember(4)]
        [System.ComponentModel.DefaultValue("")]
        public string srcTid { get; set; } = "";

    }

    [ProtoContract()]
    public partial class RpcMessage : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public RpcHeader Header { get; set; }

        [ProtoMember(2)]
        public byte[] Content { get; set; }

    }

    [ProtoContract()]
    public partial class RpcResponseContainer : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public byte[] Content { get; set; }

    }

    [ProtoContract()]
    public partial class RpcResponseMessage : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public RpcHeader Header { get; set; }

        [ProtoMember(2)]
        public RpcResponseContainer Container { get; set; }

    }

    [ProtoContract()]
    public partial class TokenStuff : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"tkn")]
        [System.ComponentModel.DefaultValue("")]
        public string Tkn { get; set; } = "";

    }

    [ProtoContract()]
    public partial class InitSessionResponse : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"sesid")]
        public byte[] Sesid { get; set; }

        [ProtoMember(2, Name = @"token")]
        public TokenStuff Token { get; set; }

    }

    [ProtoContract()]
    public partial class MpGamerHandleDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"gh")]
        [System.ComponentModel.DefaultValue("")]
        public string Gh { get; set; } = "";

    }

    [ProtoContract()]
    public partial class MpPeerAddressDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"addr")]
        [System.ComponentModel.DefaultValue("")]
        public string Addr { get; set; } = "";

    }

    [ProtoContract(Name = @"InitPlayer2_Parameters")]
    public partial class InitPlayer2Parameters : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"gh")]
        public MpGamerHandleDto Gh { get; set; }

        [ProtoMember(2)]
        public MpPeerAddressDto peerAddress { get; set; }

        [ProtoMember(3, Name = @"discriminator")]
        public int Discriminator { get; set; }

        [ProtoMember(4)]
        public int seamlessType { get; set; }

        [ProtoMember(5)]
        public uint connectionReason { get; set; }

    }

    [ProtoContract()]
    public partial class InitPlayerResult : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"code")]
        public uint Code { get; set; }

    }

    [ProtoContract()]
    public partial class Restriction : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"u1")]
        public int U1 { get; set; }

        [ProtoMember(2, Name = @"u2")]
        public int U2 { get; set; }

        [ProtoMember(3, Name = @"u3")]
        public int U3 { get; set; }

    }

    [ProtoContract()]
    public partial class GetRestrictionsData : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"restriction")]
        public System.Collections.Generic.List<Restriction> Restrictions { get; } = new System.Collections.Generic.List<Restriction>();

        [ProtoMember(2, Name = @"unk2")]
        public System.Collections.Generic.List<string> Unk2s { get; } = new System.Collections.Generic.List<string>();

    }

    [ProtoContract()]
    public partial class GetRestrictionsResult : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"data")]
        public GetRestrictionsData Data { get; set; }

    }

    [ProtoContract()]
    public partial class PlayerIdSto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public int acctId { get; set; }

        [ProtoMember(2)]
        public int platId { get; set; }

    }

    [ProtoContract()]
    public partial class MpSessionRequestIdDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"requestor")]
        public PlayerIdSto Requestor { get; set; }

        [ProtoMember(2, Name = @"index")]
        public int Index { get; set; }

        [ProtoMember(3, Name = @"hash")]
        public int Hash { get; set; }

    }

    [ProtoContract(Name = @"QueueForSession_Seamless_Parameters")]
    public partial class QueueForSessionSeamlessParameters : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public MpSessionRequestIdDto requestId { get; set; }

        [ProtoMember(2)]
        public uint optionFlags { get; set; }

        [ProtoMember(3, Name = @"x")]
        public int X { get; set; }

        [ProtoMember(4, Name = @"y")]
        public int Y { get; set; }

    }

    [ProtoContract()]
    public partial class QueueForSessionResult : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"code")]
        public uint Code { get; set; }

    }

    [ProtoContract(Name = @"QueueEntered_Parameters")]
    public partial class QueueEnteredParameters : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public uint queueGroup { get; set; }

        [ProtoMember(2)]
        public MpSessionRequestIdDto requestId { get; set; }

        [ProtoMember(3)]
        public uint optionFlags { get; set; }

    }

    [ProtoContract()]
    public partial class GuidDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"a", DataFormat = DataFormat.FixedSize)]
        public ulong A { get; set; }

        [ProtoMember(2, Name = @"b", DataFormat = DataFormat.FixedSize)]
        public ulong B { get; set; }

    }

    [ProtoContract()]
    public partial class MpTransitionIdDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"value")]
        public GuidDto Value { get; set; }

    }

    [ProtoContract()]
    public partial class MpSessionIdDto : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"value")]
        public GuidDto Value { get; set; }

    }

    [ProtoContract()]
    public partial class SessionSubcommandEnterSession : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"index")]
        public int Index { get; set; }

        [ProtoMember(2, Name = @"hindex")]
        public int Hindex { get; set; }

        [ProtoMember(3)]
        public uint sessionFlags { get; set; }

        [ProtoMember(4, Name = @"mode")]
        public uint Mode { get; set; }

        [ProtoMember(5, Name = @"size")]
        public int Size { get; set; }

        [ProtoMember(6)]
        public int teamIndex { get; set; }

        [ProtoMember(7)]
        public MpTransitionIdDto transitionId { get; set; }

        [ProtoMember(8)]
        public uint sessionManagerType { get; set; }

        [ProtoMember(9)]
        public int slotCount { get; set; }

    }

    [ProtoContract()]
    public partial class SessionSubcommandLeaveSession : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"reason")]
        public uint Reason { get; set; }

    }

    [ProtoContract()]
    public partial class SessionSubcommandAddPlayer : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"id")]
        public PlayerIdSto Id { get; set; }

        [ProtoMember(2, Name = @"gh")]
        public MpGamerHandleDto Gh { get; set; }

        [ProtoMember(3, Name = @"addr")]
        public MpPeerAddressDto Addr { get; set; }

        [ProtoMember(4, Name = @"index")]
        public int Index { get; set; }

    }

    [ProtoContract()]
    public partial class SessionSubcommandRemovePlayer : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"id")]
        public PlayerIdSto Id { get; set; }

    }

    [ProtoContract()]
    public partial class SessionSubcommandHostChanged : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"index")]
        public int Index { get; set; }

    }

    [ProtoContract()]
    public partial class SessionCommand : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"cmd")]
        public uint Cmd { get; set; }

        [ProtoMember(2, Name = @"cmdname")]
        [System.ComponentModel.DefaultValue("")]
        public string Cmdname { get; set; } = "";

        [ProtoMember(3)]
        public SessionSubcommandEnterSession EnterSession { get; set; }

        [ProtoMember(4)]
        public SessionSubcommandLeaveSession LeaveSession { get; set; }

        [ProtoMember(5)]
        public SessionSubcommandAddPlayer AddPlayer { get; set; }

        [ProtoMember(6)]
        public SessionSubcommandRemovePlayer RemovePlayer { get; set; }

        [ProtoMember(7)]
        public SessionSubcommandHostChanged HostChanged { get; set; }

    }

    [ProtoContract(Name = @"scmds_Parameters")]
    public partial class scmdsParameters : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"sid")]
        public MpSessionIdDto Sid { get; set; }

        [ProtoMember(2, Name = @"ncmds")]
        public int Ncmds { get; set; }

        [ProtoMember(3, Name = @"cmds")]
        public System.Collections.Generic.List<SessionCommand> Cmds { get; } = new System.Collections.Generic.List<SessionCommand>();

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
