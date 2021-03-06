using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Application.Services
{
    public enum RandomCharactersType
    {
        All,
        LowerCaseLetters,
        LowerAndUpperCaseLetters,
        LowerCaseLettersAndNumbers,
        Numbers,
        UpperCaseLetters,
        UpperCaseLettersAndNumbers
    }
    public interface ICaptchaService
    {
        string RandomString(int size, RandomCharactersType randomCharactersType);
        string CreateCaptcha(string text);
    }
    public class CaptchaService : ICaptchaService
    {
        private const string LOWER_CASE_LETTERS = "abcdefghijklmnopqrstuvwyxz";
        private const string NUMBERS = "0123456789";
        private const string UPPER_CASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string GetPattern(RandomCharactersType randomCharactersType)
        {
            var pattern = LOWER_CASE_LETTERS + UPPER_CASE_LETTERS + NUMBERS;
            switch (randomCharactersType)
            {
                case RandomCharactersType.LowerCaseLetters: pattern = LOWER_CASE_LETTERS; break;
                case RandomCharactersType.LowerAndUpperCaseLetters: pattern = LOWER_CASE_LETTERS + UPPER_CASE_LETTERS; break;
                case RandomCharactersType.LowerCaseLettersAndNumbers: pattern = LOWER_CASE_LETTERS + NUMBERS; break;
                case RandomCharactersType.Numbers: pattern = NUMBERS; break;
                case RandomCharactersType.UpperCaseLetters: pattern = UPPER_CASE_LETTERS; break;
                case RandomCharactersType.UpperCaseLettersAndNumbers: pattern = UPPER_CASE_LETTERS + NUMBERS; break;
                default: break;
            }
            return pattern;
        }
        private SizeF GetStringSize(string text, Font stringFont)
        {
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var stringSize = graphics.MeasureString(text, stringFont);

            bitmap.Dispose();
            graphics.Dispose();

            return stringSize;
        }
        public string RandomString(int size, RandomCharactersType randomCharactersType)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            var pattern = GetPattern(randomCharactersType);
            var characters = Enumerable.Range(0, size).Select(x => pattern[rand.Next(0, pattern.Length)]);

            return new string(characters.ToArray());
        }
        public string CreateCaptcha(string text)
        {
            var stringFont = new Font("Tahoma", 24);
            var stringSize = GetStringSize(text, stringFont);
            var bitmap = new Bitmap(Convert.ToInt32(stringSize.Width), Convert.ToInt32(stringSize.Height));
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.LightGray);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, stringFont, new SolidBrush(Color.Black), new PointF(0, 0));
            graphics.DrawLine(new Pen(Color.Black, 2), 0.0F, stringSize.Height, stringSize.Width, 0.0F);

            if (true)
            {
                graphics.DrawLine(new Pen(Color.Black, 2), 0.0F, 0.0F, stringSize.Width, stringSize.Height);
            }
            graphics.Flush();

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();

            return Convert.ToBase64String(byteImage);
        }
    }
}