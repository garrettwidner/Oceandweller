using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet 
{
    public PlayerAction Pickup;
    public PlayerAction Pause;
    public PlayerAction Quit;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Move;

    /// <summary>
    /// Returns the absolute value of the axis with the highest magnitude
    /// </summary>
    public float MoveMagnitude
    {
        get
        {
            Vector2 moveVector = new Vector2(Move.X, Move.Y);
            return moveVector.magnitude;
        }
    }

    public PlayerActions()
    {
        Pickup = CreatePlayerAction("PickUp");
        Pause = CreatePlayerAction("Pause");
        Quit = CreatePlayerAction("Quit");
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }

    public static PlayerActions CreateWithDefaultBindings()
    {
        PlayerActions playerActions = new PlayerActions();

        playerActions.Pickup.AddDefaultBinding(InputControlType.Action1);
        playerActions.Pickup.AddDefaultBinding(Key.K);

        playerActions.Pause.AddDefaultBinding(Key.Return);
        playerActions.Pause.AddDefaultBinding(InputControlType.Menu);
        playerActions.Pause.AddDefaultBinding(InputControlType.Pause);
        playerActions.Pause.AddDefaultBinding(InputControlType.Options);
        playerActions.Pause.AddDefaultBinding(InputControlType.Select);
        playerActions.Pause.AddDefaultBinding(InputControlType.Start);
        playerActions.Pause.AddDefaultBinding(InputControlType.Home);

        playerActions.Quit.AddDefaultBinding(Key.Escape);
        playerActions.Quit.AddDefaultBinding(InputControlType.Menu);
        playerActions.Quit.AddDefaultBinding(InputControlType.Pause);
        playerActions.Quit.AddDefaultBinding(InputControlType.Options);
        playerActions.Quit.AddDefaultBinding(InputControlType.Select);
        playerActions.Quit.AddDefaultBinding(InputControlType.Start);
        playerActions.Quit.AddDefaultBinding(InputControlType.Home);

        playerActions.Left.AddDefaultBinding(Key.LeftArrow);
        playerActions.Right.AddDefaultBinding(Key.RightArrow);
        playerActions.Up.AddDefaultBinding(Key.UpArrow);
        playerActions.Down.AddDefaultBinding(Key.DownArrow);

        playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);

        playerActions.Up.AddDefaultBinding(Key.W);
        playerActions.Down.AddDefaultBinding(Key.S);
        playerActions.Left.AddDefaultBinding(Key.A);
        playerActions.Right.AddDefaultBinding(Key.D);

        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 4;

        //playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
        //playerActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
        //playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        //playerActions.ListenOptions.IncludeMouseButtons = true;
        //playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;

        return playerActions;
    }

}
