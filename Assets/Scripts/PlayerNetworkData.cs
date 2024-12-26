using Unity.Collections;
using Unity.Netcode;
using System;
using UnityEngine.SocialPlatforms.Impl;

public struct PlayerObject : INetworkSerializable, IEquatable<PlayerObject>
{
    public ulong ClientId; // Unique identifier for each client
    public FixedString64Bytes PlayerName; // Fixed-size string for player names

    public PlayerObject(ulong clientId, FixedString64Bytes playerName)
    {
        ClientId = clientId;
        PlayerName = playerName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref PlayerName);
    }

    // Implement IEquatable<PlayerObject>
    public bool Equals(PlayerObject other)
    {
        return ClientId == other.ClientId &&
               PlayerName.Equals(other.PlayerName);
    }
}
