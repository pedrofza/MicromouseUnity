using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebugWallOperation
{
    wallSet, wallCleared
}

public enum WallIdentifier
{
    left, right, top, bottom, invalid
}

public class WallStateData
{
    public Vector2Int CellPosition {get; set;}
    public WallIdentifier Wall {get; set;}
    public DebugWallOperation Operation {get; set;}
}

static class DUtils
{
    public static Vector2Int CellPositionFromBytes(byte[] cellBytes)
    {
        Vector2Int cellPosition = new Vector2Int(cellBytes[0], cellBytes[1]);
        return cellPosition;
    }
}

public class WallStateDebuggerMicroSerialAdapter : SimpleSerialAdapter
{
    private const int leftWallBitPosition   = 0;
    private const int rightWallBitPosition  = 1;
    private const int topWallBitPosition    = 2;
    private const int bottomWallBitPosition = 3;

    private const byte leftWallBitMask   = (byte) (1u << leftWallBitPosition);
    private const byte rightWallBitMask  = (byte) (1u << rightWallBitPosition);
    private const byte topWallBitMask    = (byte) (1u << topWallBitPosition);
    private const byte bottomWallBitMask = (byte) (1u << bottomWallBitPosition);

    private const byte wallSetBitMask = (byte) (1u << 0);
    private const byte wallClearedBitMask = (byte) (1u << 1);
    
    private WallIdentifier WallIdentifierFromByte(byte b)
    {
        switch(b)
        {
            case leftWallBitMask:
                return WallIdentifier.left;
                break;
            case rightWallBitMask:
                return WallIdentifier.right;
                break;
            case topWallBitMask:
                return WallIdentifier.top;
                break;
            case bottomWallBitMask:
                return WallIdentifier.bottom;
                break;
            default:
                return WallIdentifier.invalid;
                break;
        }

    }

    public override bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        if (message.Length < 4)
        {
            bytesRead = 0;
            return false;
        }

        Vector2Int cellPosition = DUtils.CellPositionFromBytes(message);
        WallIdentifier wallId = WallIdentifierFromByte(message[2]);
        DebugWallOperation dwo = message[3] == (byte) 0 ? DebugWallOperation.wallSet : DebugWallOperation.wallCleared;

        WallStateData data = new WallStateData {CellPosition = cellPosition, Wall = wallId, Operation = dwo};
        OnNewWallStatus(data);
        
        bytesRead = 4;
        return true;
    }

    public delegate void WallStatusUpdateEventHandler<T, U>(T sender, U eventArgs);
    public event WallStatusUpdateEventHandler<WallStateDebuggerMicroSerialAdapter, WallStateData> NewStatus;

    private void OnNewWallStatus(WallStateData data)
    {
        var handler = NewStatus;
        handler?.Invoke(this, data);
    }
}

public class CellVisitedMicroDebuggerSerialAdapter : SimpleSerialAdapter
{
    public override bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        if (message.Length < 3)
        {
            Debug.Log("The message was too short");
            bytesRead = 0;
            return false;
        }

        Vector2Int cellPosition = DUtils.CellPositionFromBytes(message);
        byte opByte = message[2];

        bytesRead = 3;
        return true;
    }
}