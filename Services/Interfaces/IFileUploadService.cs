namespace FastFood.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<(bool, string, string)> ImageUpload(FormFile? formFile, string savePath, bool isRandomName = true);
        Task<(bool, string, string)> ImagesUpload(List<FormFile?> formFiles, string savePath, bool isRandomName = true);
    }
}