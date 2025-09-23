namespace FastFood.Models
{
    public static class CommonHelper
    {
        /// <summary>
        /// Tách chuỗi hình ảnh thành một mảng các chuỗi dựa trên dấu phẩy.
        /// </summary>
        /// <param name="hinhAnh">Chuỗi chứa danh sách các hình ảnh, các hình ảnh cách nhau bằng dấu phẩy.</param>
        /// <returns>Mảng các chuỗi hình ảnh.</returns>
        public static string[] ImageSplitter(string images)
        {
            if (string.IsNullOrEmpty(images))
                return Array.Empty<string>();

            return images.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Xóa file tại đường dẫn chỉ định.
        /// </summary>
        /// <param name="filePath">Đường dẫn file cần xóa.</param>
        public static void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}