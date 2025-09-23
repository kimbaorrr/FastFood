namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ tải lên tệp.
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Tải lên một ảnh.
        /// </summary>
        /// <param name="formFile">Tệp ảnh cần tải lên.</param>
        /// <param name="savePath">Đường dẫn lưu tệp.</param>
        /// <param name="isRandomName">Có sử dụng tên ngẫu nhiên cho tệp không.</param>
        /// <returns>Kết quả tải lên (thành công/thất bại, thông báo, đường dẫn tệp).</returns>
        Task<(bool, string, string)> ImageUpload(FormFile? formFile, string savePath, bool isRandomName = true);

        /// <summary>
        /// Tải lên nhiều ảnh.
        /// </summary>
        /// <param name="formFiles">Danh sách tệp ảnh cần tải lên.</param>
        /// <param name="savePath">Đường dẫn lưu tệp.</param>
        /// <param name="isRandomName">Có sử dụng tên ngẫu nhiên cho tệp không.</param>
        /// <returns>Kết quả tải lên (thành công/thất bại, thông báo, danh sách đường dẫn tệp).</returns>
        Task<(bool, string, string)> ImagesUpload(List<FormFile?> formFiles, string savePath, bool isRandomName = true);
    }
}