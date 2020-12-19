using UnityEngine;

namespace LTD.EnemyUnits
{
    public class UnitView : MonoBehaviour
    {
        Unit _unit;

        public void Init(Unit unit)
        {
            gameObject.name = unit.name;
            _unit = unit;
        }
    }
}

