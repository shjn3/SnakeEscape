using System;
using Shine.EscapeSnake;
using Shine.EscapeSnake.GamePlay;
using Shine.EscapeSnake.GamePlay.Snake;
using UnityEngine;

public class TestSnake : MonoBehaviour
{
    [SerializeField] SnakeController snake;
    [SerializeField] SnakeData data;
    [SerializeField] SnakeSkinData skinData;

    [SerializeField] float distance;

    [ContextMenu("text")]
    public void Test()
    {
        snake.Activate(data, skinData);
    }

    [ContextMenu("move")]
    public void Move()
    {
        snake.Move(distance);
    }
}
