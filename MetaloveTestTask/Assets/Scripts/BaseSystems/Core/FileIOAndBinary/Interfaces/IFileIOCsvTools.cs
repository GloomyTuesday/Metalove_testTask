using System.Threading.Tasks;

namespace Scripts.BaseSystems.FileIOAndBinary
{

    public interface IFileIOCsvTools
    {
        public bool SaveCsvInPersistentDataPath(string directory, string filename, string[][] dataTable);
        public void DeleteCsvFromPersistentDataPath(string directory, string filenam);
        public Task<string[][]> LoadCsvFromPersistentDataPath(string directory, string filename);
        public Task<string[][]> LoadCsvFromPersistentDataPath(string filePathWithName);

        public bool SaveCsv(string directory, string filename, string[][] dataTable);
        public Task<string[][]> LoadCsv(string directory, string filenam);
        public Task<string[][]> LoadCsv(string fullFilePathWithNameAndExtension);

        public void DeleteCsvFromPath(string directory, string filename);
    }
}
