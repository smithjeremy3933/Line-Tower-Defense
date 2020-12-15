using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static event EventHandler<OnEnemyMovedEventArgs> OnEnemyMoved;
    public class OnEnemyMovedEventArgs : EventArgs
    {
        public EnemyMovement enemyMovement;
        public Node oldNode;
        public Node newNode;
    }

    Unit m_unit;

    EnemyController enemyController;
    readonly float rotateTime = 1f;
    readonly float moveSpeed = 1.25f;
    readonly float delay = 0.5f;
    readonly float distThreshold = 0.01f;
    iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public Unit Unit { get => m_unit; }

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void Init(Unit unit)
    {
        m_unit = unit;
    }

    public IEnumerator FollowPath(List<Node> path)
    {
        //path.RemoveAt(0);
        GetComponent<Animator>().SetBool("walk", true);
        foreach (Node node in path)
        {
            if (gameObject != null)
            {             
                FaceDestination(node);

                if (!enemyController.needsPathRecalc && !enemyController.isMoving)
                {
                    enemyController.isMoving = true;
                    iTween.MoveTo(gameObject, iTween.Hash(
                    "x", node.position.x,
                    "y", node.position.y,
                    "z", node.position.z,
                    "delay", delay,
                    "easetype", easeType,
                    "speed", moveSpeed
                    ));

                    while (Vector3.Distance(node.position, transform.position) > distThreshold)
                    {
                        yield return null;
                    }

                    iTween.Stop(gameObject);
                    OnEnemyMoved?.Invoke(this, new OnEnemyMovedEventArgs { enemyMovement = this, oldNode = enemyController.currentNode, newNode = node });
                    enemyController.currentNode = node;
                    if (enemyController.needsPathRecalc && !enemyController.isAttacking)
                    {
                        StopAllCoroutines();
                        iTween.Stop(gameObject);
                    }
                    enemyController.isMoving = false;
                }
            }
        }
        GetComponent<Animator>().SetBool("walk", false);
    }

    public void FaceDestination(Node node)
    {
        Vector3 relativePos = node.position - transform.position;
        Quaternion newRot = Quaternion.LookRotation(relativePos, Vector3.up);
        float newY = newRot.eulerAngles.y;

        iTween.RotateTo(gameObject, iTween.Hash(
            "y", newY,
            "delay", 0f,
            "easetype", easeType,
            "time", rotateTime
        ));
    } 
}
