using System.Threading.Tasks;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    public interface IDirectoryTools
    {
        public Task<string[]> GetAllDirectoriesAndSubdirectories(string directoryToCheck);
        public Task<string[]> GetAllFilePathsFromDirectories(string directory, string fileFormat = "");
        public Task<string[]> GetAllFilePathsFromDirectories(string[] directories, string fileFormat = "");
        public Task<string[]> GetAllFilePathsFromDirectory(string directory, string fileFormat = "");
        public Task<string[]> GetAllFilePathsFromDirectory(string directory, string[] fileFormat );
    }
}
