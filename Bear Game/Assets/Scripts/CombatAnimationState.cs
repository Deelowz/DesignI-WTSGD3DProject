// using System.Threading;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CombatAnimationState : MonoBehaviour
// {
//     float timePassed;
//     float clipLength;
//     float clipSpeed;
//     bool attack;

//     public AttackState(Player player, StateMachine stateMachine) : base(character, stateMachine)
//     {
//         character = character;
//         stateMachine = stateMachine;
//     }

//     public override void Enter()
//     {
//         base.Enter();
//         attack = false;
//         character.animator.applyRootMotion = true;
//         timePassed = 0f;
//         character.animator.SetTrigger("attack");
//         character.animator.SetFloat("speed", 0f);
//     }

//     public override void HandleInput()
//     {
//         base.HandleInput();

//         if (attackAction.triggered)
//         {
//             attack = true;
//         }
//     }

//     public override void LogicUpdate()
//     {
//         base.LogicUpdate();

//         timePassed += Time.deltaTime;
//         clipLength = character.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
//         clipSpeed = character.animator.GetCurrentAnimatorClipInfo(1).speed;

//         if (timePassed >= clipLength / clipSpeed && attack)
//         {
//             stateMachine.ChangeState(character.attacking);
//         }
//         if (timePassed >= clipLength / clipSpeed)
//         {
//             stateMachine.ChangeState(character.combatting);
//             character.animator.SetTrigger("move");
//         }
//     }

//     public override void Exit()
//     {
//         base.Exit();
//         character.animator.applyRootMotion = false;
//     }
// }
