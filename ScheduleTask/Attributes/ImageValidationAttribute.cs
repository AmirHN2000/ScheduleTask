#nullable enable
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ScheduleTask.Attributes
{
    public class ImageValidationAttribute:ValidationAttribute
    {
        private double MaxSize { get; set; }

        public ImageValidationAttribute(double size)
        {
            MaxSize = size;
            ErrorMessage = $"حجم عکس باید کمتر از {MaxSize} مگابایت باشد";
        }

        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                var image = value as IFormFile;
                var whiteList = new[] {".jpg", ".png", ".jpeg", ".tif", ".tiff", ".JPG"};
                if (!whiteList.Contains(image?.ContentType))
                {
                    ErrorMessage = "نوع فایل ورودی قابل قبول نمی باشد";
                }
                var memory = new MemoryStream();
                image?.CopyTo(memory);
                return memory.ToArray().Length<=(MaxSize*1024*1024);
            }
            return true;
        }
    }
}