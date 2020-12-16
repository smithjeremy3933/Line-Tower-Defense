using UnityEngine;

namespace LTD.Towers
{
    public class BulletsView : MonoBehaviour
    {
        Bullets m_bullets;
        public int damage;

        public void Init(Bullets bullets)
        {
            m_bullets = bullets;
            damage = bullets.damage;
        }
    }
}
