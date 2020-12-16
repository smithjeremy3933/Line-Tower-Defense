using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
