using LTD.Map;
using UnityEngine;

namespace LTD.Towers
{
    public class Tower
    {
        public Vector3 position;
        public Node node;
        public string name;
        private int _health;
        private float _attackRange;
        private int _damage;
        private int _cost;

        public int Health { get => _health; set => _health = value; }
        public float AttackRange { get => _attackRange; set => _attackRange = value; }
        public int Damage { get => _damage; set => _damage = value; }
        public int Cost { get => _cost; set => _cost = value; }

        public Tower(Node node)
        {
            this.node = node;
            this.position = node.position;
            this.SetStats(100, 5f, 5, 200);
        }

        public virtual void SetStats (int health, float attackRange, int damage, int cost)
        {
            _health = health;
            _attackRange = attackRange;
            _damage = damage;
            _cost = cost;
        }
    }
}

