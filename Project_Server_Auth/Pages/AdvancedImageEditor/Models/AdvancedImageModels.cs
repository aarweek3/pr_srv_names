using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pr_srv_names.Pages.AdvancedImageEditor.Models
{
    public class SaveImageRequest
    {
        [Required] public string ImageBase64 { get; set; } = string.Empty;

        [Required] public string FileName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public class SaveImageResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class LoadImageResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Оставляем старые модели для совместимости с клиентом
    public class AdvancedImageProcessRequest
    {
        [Required] public string ImageBase64 { get; set; } = string.Empty;

        public float? RotationAngle { get; set; }
        public bool FlipHorizontal { get; set; }
        public bool FlipVertical { get; set; }
        public int? ResizeWidth { get; set; }
        public int? ResizeHeight { get; set; }
        public string ResizeMode { get; set; } = "stretch";
        public string OutputFormat { get; set; } = "png";

        // Фильтры
        public int? Brightness { get; set; }
        public int? Contrast { get; set; }
        public int? Saturation { get; set; }
        public int? Hue { get; set; }
        public float? Gamma { get; set; }
        public bool Sepia { get; set; }
        public bool Grayscale { get; set; }
        public bool Invert { get; set; }
        public int? Blur { get; set; }
        public int? Sharpen { get; set; }

        // Обрезка
        public int? CropX { get; set; }
        public int? CropY { get; set; }
        public int? CropWidth { get; set; }
        public int? CropHeight { get; set; }

        // Водяной знак
        public string? WatermarkText { get; set; }
        public string? WatermarkPosition { get; set; }
        public int? WatermarkOpacity { get; set; }
        public int? WatermarkFontSize { get; set; }
        public string? WatermarkColor { get; set; }
    }

    public class AdvancedImageProcessResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ProcessedImageBase64 { get; set; } = string.Empty;
        public string OutputFormat { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public long ProcessingTimeMs { get; set; }
        public List<string> AppliedOperations { get; set; } = new();
    }

    public class ImageInfoRequest
    {
        [Required] public string ImageBase64 { get; set; } = string.Empty;
    }

    public class ImageInfoResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int ColorDepth { get; set; }
        public bool HasTransparency { get; set; }
    }

    public class ProcessingCapabilitiesResponse
    {
        public string[] SupportedFormats { get; set; } = Array.Empty<string>();
        public long MaxImageSize { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public string[] SupportedFilters { get; set; } = Array.Empty<string>();
        public string[] SupportedWatermarkPositions { get; set; } = Array.Empty<string>();
    }
}