namespace CG_2
{
    public class ImageInfo
    {
        public string ImageName { get; set; }

        public int ImageSize { get; set; }

        public int Dpi { get; set; }

        public string PixelFormat { get; set; }

        public string ComressionType { get; set; }

        public ImageInfo(string name, int size, int dpi, string pixelFormat, string comressionType)
        {
            ImageName = name;
            ImageSize = size;
            Dpi = dpi;
            PixelFormat = pixelFormat;
            ComressionType = comressionType;
        }
    }
}