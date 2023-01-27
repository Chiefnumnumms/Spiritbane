public class SwingCommand : ICommand
{
    Swinging _swing;

    public SwingCommand(Swinging swing)
    {
        _swing = swing;
    }

    public void Execute()
    {
        _swing.DrawRope();
    }
}
