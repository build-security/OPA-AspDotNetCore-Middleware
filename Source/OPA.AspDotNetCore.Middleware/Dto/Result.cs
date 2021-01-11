namespace Opa.AspDotNetCore.Middleware.Dto
{
    public class Result
    {
        public bool Allow = false;

        public override string ToString()
        {
            return $"({nameof(Allow)}: {Allow})";
        }
    }
}
