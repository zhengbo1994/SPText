using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPTextProject.Common.EmailHandle
{
    public class CustomAttachment
    {
        public CustomAttachment(Stream data)
        {
            Data = data;
        }
        public CustomAttachment(string fileName, Stream data, string contentType)
        {
            Filename = fileName;
            Data = data;
            ContentType = contentType;
        }

        public string Filename { get; set; }
        public Stream Data { get; set; }
        public string ContentType { get; set; }
    }
}
