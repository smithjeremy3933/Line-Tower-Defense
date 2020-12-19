namespace LTD.Towers
{
    public class Bullets
    {
        Tower _tower;
        private int _damage;
        private bool _canSlow;

        public Bullets(Tower tower)
        {
            this._tower = tower;
            this.SetStats(5);
        }

        public int Damage { get => _damage; set => _damage = value; }
        public bool CanSlow { get => _canSlow; }

        public virtual void SetStats(int damage, bool canSlow = false)
        {
            _damage = damage;
            _canSlow = canSlow;
        }
    }
}

