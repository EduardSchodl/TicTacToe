public interface IUnitTestCollection
{
    public IUnitTest[] GetUnitTests();
    public bool IsTestDisabled();
}
