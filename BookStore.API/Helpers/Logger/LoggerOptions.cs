namespace BookStore.API.Helpers.Logger
{
    public class LoggerOptions
    {
        public virtual string FilePath { get; set; }
        public virtual string FolderPath { get; set; }
        public LoggerOptions()
        {
            FolderPath = Directory.GetCurrentDirectory() + "\\Logs";
            FilePath = "log_{date}.log";
        }
    }
}
