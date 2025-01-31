namespace UniJWT
{
    public interface IAlgorithm
    {
        string AlgorithmName { get; }
        string CreateSignature(string data, string secret);
    }
}
