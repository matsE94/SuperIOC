namespace SuperIOC.SuperContainer.Test.DummyClasses
{
    internal class BatmanController
    {
        private readonly IBatmanService _batmanService;

        public BatmanController(IBatmanService batmanService)
        {
            _batmanService = batmanService;
        }
    }
}