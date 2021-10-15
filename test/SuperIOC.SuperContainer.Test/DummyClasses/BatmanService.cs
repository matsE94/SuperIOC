namespace SuperIOC.SuperContainer.Test.DummyClasses
{
    internal interface IBatmanService
    {
        public string Nanananana();
    }

    internal class BatmanService : IBatmanService
    {
        public string Nanananana() => "BATMAN";
    }
}