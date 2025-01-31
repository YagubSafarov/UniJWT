namespace UniJWT
{
    public class JWTException : System.Exception
    {
        public JWTException(string message) : base(message)
        {

        }
    }

    public class InvalidSignatureException : JWTException
    {
        public InvalidSignatureException() : base("Invalid token signature")
        {
        }
    }

    public class InvalidTokenException : JWTException
    {
        public InvalidTokenException() : base("Invalid JWT token")
        {
        }
    }

    public class UnsupportedAlgorithmException : JWTException
    {
        public UnsupportedAlgorithmException() : base("Unsupported algorithm")
        {
        }
    }

    public class UnsupportedTypeException : JWTException
    {
        public UnsupportedTypeException() : base("Unsupported type")
        {
        }
    }


}
