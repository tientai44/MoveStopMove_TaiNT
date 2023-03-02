using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    float timeDelay = 2f;
    public void OnEnter(BotController bot)
    {
        timer = 0;
        bot.StopMoving();
        bot.Attack();
    }

    public void OnExecute(BotController bot)
    {
        timer+=Time.deltaTime;
        if (timer > timeDelay)
        {
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(BotController bot)
    {
    }
}
