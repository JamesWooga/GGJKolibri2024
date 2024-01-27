using System.Collections;

namespace Utility.Extensions
{
    public static class CoroutineExtensions
    {
        public static IEnumerator Frames(int frameCount)
        {
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }
}