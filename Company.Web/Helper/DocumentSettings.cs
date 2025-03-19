namespace Company.Web.Helper
{
    public class DocumentSettings
    {

        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "D:\\Route\\Asp.Net C#43\\Projects\\01 C# Basics\\Company.G05\\Company.G05.PL";

            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2. Get File Name And Make It Unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }


        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }



    }
}
