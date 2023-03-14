using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer=0;
    public void OnEnter(BotController bot)
    {
        bot.SetRandomTargetFollow();
    }

    public void OnExecute(BotController bot)
    {
        timer+=Time.deltaTime;
        bot.FollowTarget();
        if (bot.IsHaveTargetInRange() && timer >0.5f)
        {
            // thuc hien tan cong
            bot.ChangeState(new AttackState());
        }
    }

    public void OnExit(BotController bot)
    {
    }
}
