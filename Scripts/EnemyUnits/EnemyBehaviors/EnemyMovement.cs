using LTD.Map;
using LTD.Towers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LTD.EnemyUnits
{
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
        bool _isSlowed = false;
        float _slowedDelay = 1f;
        float _slowDuration = 5f;
        float _slowTimer = 0f;

        public Unit Unit { get => m_unit; }

        private void Start()
        {
            enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (_isSlowed)
            {
                SlowBehavior();
            }
        }

        public void Init(Unit unit)
        {
            m_unit = unit;
        }

        private void SlowBehavior()
        {
            
            _slowTimer += Time.deltaTime;
            if (_slowTimer >= _slowDuration)
            {
                _isSlowed = false;
                _slowTimer = 0;
            }
        }

        public IEnumerator FollowPath(List<Node> path)
        {
            //path.RemoveAt(0);
            GetComponent<Animator>().SetBool("walk", true);
            foreach (Node node in path)
            {
                if (gameObject != null)
                {
                    float slowedSpeed = moveSpeed * 0.5f;
                    FaceDestination(node);

                    if (!enemyController.needsPathRecalc && !enemyController.isMoving)
                    {
                        enemyController.isMoving = true;
                        iTween.MoveTo(gameObject, iTween.Hash(
                        "x", node.position.x,
                        "y", node.position.y,
                        "z", node.position.z,
                        "delay", _isSlowed ? _slowedDelay : delay,
                        "easetype", easeType,
                        "speed", _isSlowed ? slowedSpeed : moveSpeed
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

        private void OnParticleCollision(GameObject particle)
        {
            BulletsView bulletsView = particle.GetComponent<BulletsView>();
            if (bulletsView != null)
            {
                if (bulletsView.CanSlow && !_isSlowed)
                {
                    _isSlowed = true;
                }
            }
        }
    }

}
