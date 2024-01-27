namespace Utility.Extensions
{
    public static class UnityExtensions
    {
        public static bool IsNullOrDestroyed(this object obj)
        {
            return ReferenceEquals(obj, null) || obj.Equals(null);
        }
    }
}