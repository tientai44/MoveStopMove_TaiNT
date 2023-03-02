using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(BotController bot)
    {
        Debug.Log("Run");
        bot.SetRandomTargetFollow();
    }

    public void OnExecute(BotController bot)
    {
        bot.FollowTarget();
        if (bot.IsHaveTargetInRange())
        {
            // thuc hien tan cong
            bot.ChangeState(new AttackState());
        }
    }

    public void OnExit(BotController bot)
    {
    }
}
