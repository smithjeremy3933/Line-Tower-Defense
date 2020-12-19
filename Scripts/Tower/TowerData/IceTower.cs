using LTD.Map;

namespace LTD.Towers
{
    public class IceTower : Tower
    {
        int _health = 200;
        float _attackRange = 6f;
        int _damage = 6;
        int _cost = 300;

        public IceTower(Node node) : base(node)
        {
            this.SetStats(_health, _attackRange, _damage, _cost);
        }

        public override void SetStats(int health, float attackRange, int damage, int cost)
        {
            base.SetStats(health, attackRange, damage, cost);
        }
    }
}
