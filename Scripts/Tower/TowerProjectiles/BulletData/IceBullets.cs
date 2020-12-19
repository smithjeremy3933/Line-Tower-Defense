namespace LTD.Towers
{
    public class IceBullets : Bullets
    {
        int _damage = 10;
        bool _canSlow = true;

        public IceBullets(Tower tower) : base(tower)
        {
            this.SetStats(_damage, _canSlow);
        }

        public override void SetStats(int damage, bool canSlow)
        {
            base.SetStats(damage, canSlow);
        }
    }
}
