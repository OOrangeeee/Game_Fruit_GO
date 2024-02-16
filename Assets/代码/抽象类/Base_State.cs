
public abstract class Base_State
{
    protected enemy nowEnemy;
    public abstract void OnEnter(enemy enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();
}