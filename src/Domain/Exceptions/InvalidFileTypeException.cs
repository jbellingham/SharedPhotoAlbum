using System;

namespace SharedPhotoAlbum.Domain.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException(string fileType, Exception ex) : base(
            $"File type: {fileType} is not supported.") { }

    }
}