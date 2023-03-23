using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : StateMachineBehaviour
{
    BossManager bossScript;
    List<GameObject> laserList;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int numberOfLasers = Random.Range(1, 4);
        bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
        List<int> randomList = new List<int>();
        laserList = new List<GameObject>();
        if(bossScript.isFacingLeft)
            laserList.Add(Instantiate(bossScript.laser, bossScript.laserPos.transform.position, Quaternion.Euler(0,0,180)));
        else
            laserList.Add(Instantiate(bossScript.laser, bossScript.laserPos.transform.position, Quaternion.Euler(0, 180, 180)));


        for (int i = 0; i<numberOfLasers; i++)
        {
            // Generate random numbers without duplicates
            int randPos = Random.Range(0, bossScript.laserList.Length);
            while(randomList.Contains(randPos))
            {
                randPos = Random.Range(0, bossScript.laserList.Length);   
            }
            randomList.Add(randPos);

            //Instantiate in Random Position
            laserList.Add(Instantiate(bossScript.laser, bossScript.laserList[randPos].transform.position, Quaternion.Euler(0,0,180)));

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bossScript.GetCurrentHealth() == 0f)
        {
            foreach(GameObject obj in laserList)
            {
                Destroy(obj);
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
