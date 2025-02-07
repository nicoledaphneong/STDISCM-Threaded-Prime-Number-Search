public interface IPrimeNumberSearcher
{
    List<int> SearchPrimes(int start, int end, int customThreadId);
    List<int>[] StartPrimeSearch(int numberOfThreads, int upperLimit);
}
