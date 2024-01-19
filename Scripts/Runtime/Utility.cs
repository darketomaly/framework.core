using System.Collections;

namespace Framework
{
    public static partial class Core
    {
        public static class Utility
        {
            public static IEnumerator WaitFrames(int frames)
            {
                for (int i = 0; i < frames; i++) yield return null;
            }
        }
    }
}
