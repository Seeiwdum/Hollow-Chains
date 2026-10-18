using System;
using UnityEngine;

namespace HollowChains.Gameplay.Input
{
    /// <summary>What gameplay needs from input. Gameplay depends on this, never on the Input System directly.</summary>
    public interface IPlayerInput
    {
        Vector2 Move { get; }

        Vector2 Look { get; }

        bool SprintHeld { get; }

        event Action InteractPressed;
    }
}
