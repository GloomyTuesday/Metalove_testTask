using System.Threading.Tasks;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    public interface IFileIOTools
    {
        public bool SaveBinaryInPersistentDataPath<T>(string directory, string filenameWithExtension, T content);
        public bool SaveBinary<T>(string fullPath, T content);

        public Task<T> LoadBinaryFromPersistentDataPath<T>(string directory, string filenameWithExtension) where T : class;
        public Task<T> LoadBinaryFromPersistentDataPath<T>(string filePathWithNameAndExtension) where T : class;

        public bool SaveBinary<T>(string directory, string filenameWithExtension, T content);
        public Task<T> LoadBinary<T>(string directory, string filenameWithExtension) where T : class;
        public Task<T> LoadBinary<T>(string fullFilePathWithNameAndExtension) where T : class;

        public Task<bool> SaveToJsonInPersistentDataPath<T>(T content, string directory, string filenameWithExtension, string extension=".txt", bool prettyPrint = false);
        public Task<bool> SaveToJson<T>(T content, string directory, string filename, string extension = ".txt", bool prettyPrint = false);
        public Task<bool> SaveToJson<T>(T content, string path, bool prettyPrint = false);

        public Task<T> LoadJSonFromPersistentDataPath<T>(string directory, string filename, string extension = ".txt") where T : class;
        public Task<T> LoadJSon<T>(string fullFilePathWithNameAndExtension) where T : class;
        public Task<T> LoadJSon<T>(string directory, string filename, string extension = ".txt") where T : class;

        public void DeleteFileFromPath(string directory, string filenameWithExtension);
        public void DeleteFileFromPersistentDataPath(string directory, string filenameWithExtension);

    }
}
  