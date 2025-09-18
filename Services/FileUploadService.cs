using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class FileUploadService : CommonService, IFileUploadService
    {
        public FileUploadService() { }

        public async Task<(bool, string, string)> ImageUpload(FormFile? formFile, string savePath, bool isRandomName = true)
        {
            if (formFile == null)
                return (false, "Chưa chọn tệp tải lên !", string.Empty);

            if (formFile.Length == 0 || formFile.Length > 10000000)
                return (false, "Kích thước các tệp phải khác 0 & <= 10MB !", string.Empty);

            if (!formFile.ContentType.Contains("image/"))
                return (false, "Các tệp tải lên phải là định dạng ảnh !", string.Empty);

            string randomName = Guid.NewGuid().ToString();
            string newUploadFileName = randomName + Path.GetExtension(formFile!.FileName);

            string newUploadFilePath = Path.Combine(savePath, newUploadFileName);

            using (FileStream fileStream = new FileStream(newUploadFilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return (true, string.Empty, newUploadFilePath);

        }

        public async Task<(bool, string, string)> ImagesUpload(List<FormFile?> formFiles, string savePath, bool isRandomName = true)
        {
            List<string> newFilePaths = new List<string>();

            foreach(var formFile in formFiles)
            {
                if (formFile == null)
                    return (false, "Chưa chọn tệp tải lên !", string.Empty);

                if (formFile.Length == 0 || formFile.Length > 10000000)
                    return (false, "Kích thước các tệp phải khác 0 & <= 10MB !", string.Empty);

                if (!formFile.ContentType.Contains("image/"))
                    return (false, "Các tệp tải lên phải là định dạng ảnh !", string.Empty);

                string randomName = Guid.NewGuid().ToString();
                string newUploadFileName = randomName + Path.GetExtension(formFile!.FileName);

                string newUploadFilePath = Path.Combine(savePath, newUploadFileName);

                using (FileStream fileStream = new FileStream(newUploadFilePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                newFilePaths.Add(newUploadFilePath);
            }

            return (true, string.Empty, string.Join(",", newFilePaths));
        }
    }
}
