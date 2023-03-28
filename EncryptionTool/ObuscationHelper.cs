namespace EncryptionTool
{
    public static class ObfuscationHelper
    {
        public static string ObfuscateFileContent(string fileContent, string iv)
            => fileContent + "+++" + iv;

        public static string[] DeObfuscateFileContent(string fileContent)
            => fileContent.Split("+++"); 
    }
}
