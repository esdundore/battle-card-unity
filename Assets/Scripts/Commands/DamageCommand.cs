using UnityEngine;
public class DamageCommand : Command
{
    private Vector3 position;
    private int damage;


    public DamageCommand(Vector3 position, int damage)
    {
        this.position = position;
        this.damage = damage;
    }

    public override void StartCommandExecution()
    {
        DamageEffect.CreateDamageEffect(position, damage, Color.white, true);
        Command.CommandExecutionComplete();
    }
}
