// WARNING: Do not modify! Generated file.
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS)
namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("lSl64H54NLXk3VbWcXMT+hFuI5RLwht/9RIA6fcEuezsKPfsjVUPX0T2dVZEeXJ9XvI88oN5dXV1cXR39nV7dET2dX529nV1dKxYzY0TDuUQqYQ/e/liLGmeQ8+8/RjJTb4XuduUjfDVGBaUO9HtWjGeeIntt8lR+mvdsSPSWy6zjWWB6oGMlIXg/aNGuoq7fBTlFR18QBQwtk6TbAIMeiSQHmwW5gbt+cnz2lmDLSItF5sEizwTZKteWZU11FeB7K+HJwQSgtB4i0Sja7hoRgynbtAjPUoOwzL29uK2R4irfSGKOvf2+fneeS7oXdXIuiBTLDdlJP/tnGGrKg6FY5IMSjzmfvZNg3C2TPhNQ5Yy8xeBplbwf5GIGquR8bQhwXZ3dXR1");
        private static int[] order = new int[] { 9,10,9,10,9,5,11,7,8,12,10,13,12,13,14 };
        private static int key = 116;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif