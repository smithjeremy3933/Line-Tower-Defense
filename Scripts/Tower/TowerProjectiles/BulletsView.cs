using UnityEngine;

namespace LTD.Towers
{
    public class BulletsView : MonoBehaviour
    {
        Bullets _bullets;
        public int damage;
        bool _canSlow;

        public Bullets Bullets { get => _bullets; }
        public bool CanSlow { get => _canSlow; }

        public void Init(Bullets bullets)
        {
            _bullets = bullets;
            damage = bullets.Damage;
            _canSlow = bullets.CanSlow;
        }
    }
}
