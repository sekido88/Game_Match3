public class Dot : BaseDot
{
       public override void ActivateEffect()
    {
        base.ActivateEffect();
        Destroy(this.gameObject);
    }
}