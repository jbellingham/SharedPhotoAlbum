using System;
using System.Collections.Generic;
using System.Globalization;
using SharedPhotoAlbum.Domain.Common;
using SharedPhotoAlbum.Domain.Enums;
using SharedPhotoAlbum.Domain.Exceptions;

namespace SharedPhotoAlbum.Domain.ValueObjects
{
    public class File : ValueObject
    {
        public static File For(string fileString)
        {
            var file = new File();
            try
            {
                file.FileType = GetFileType(fileString);
                file.DataUrl = fileString;
                file.MimeType = GetMimeType(fileString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return file;
        }

        public string MimeType { get; set; }
        public string DataUrl { get; set; }
        public FileType FileType { get; set; }
        public string PublicId { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FileType;
            yield return DataUrl;
            yield return MimeType;
        }
        
        private static FileType GetFileType(string file)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            string dataType = GetDataType(file);
            FileType fileType;
            try
            {
                fileType = Enum.Parse<FileType>(textInfo.ToTitleCase(dataType));
            }
            catch (Exception ex)
            {
                throw new InvalidFileTypeException(dataType, ex);
            }
            return fileType;
        }

        private static string GetDataType(string file)
        {
            var mimeType = GetMimeType(file);
            var dataType = mimeType.Split('/')[0];
            return dataType;
        }

        private static string GetMimeType(string fileString)
        {
            return fileString.Split(';')[0].Replace("data:", string.Empty);
        }
    }
}