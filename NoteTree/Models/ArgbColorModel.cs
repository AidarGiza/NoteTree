using Avalonia.Media;
using ReactiveUI;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NoteTree.Models
{
    public class ArgbColorModel : ReactiveObject
    {
        public byte A
        {
            get => a;
            set => this.RaiseAndSetIfChanged(ref a, value, nameof(A));
        }
        private byte a;

        public byte R
        {
            get => r;
            set => this.RaiseAndSetIfChanged(ref r, value, nameof(R));
        }
        private byte r;

        public byte G
        {
            get => g;
            set => this.RaiseAndSetIfChanged(ref g, value, nameof(G));
        }
        private byte g;

        public byte B
        {
            get => b;
            set => this.RaiseAndSetIfChanged(ref b, value, nameof(B));
        }
        private byte b;

        public static ArgbColorModel FromUInt32(uint value)
        {
            return new ArgbColorModel
            {
                A = (byte)((value >> 24) & 0xff),
                R = (byte)((value >> 16) & 0xff),
                G = (byte)((value >> 8) & 0xff),
                B = (byte)(value & 0xff),
            };
        }

        public static ArgbColorModel Parse(string s)
        {
            if (s[0] == '#')
            {
                var or = 0u;

                if (s.Length == 7)
                {
                    or = 0xff000000;
                }
                else if (s.Length != 9)
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }

                return FromUInt32(uint.Parse(s.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture) | or);
            }
            else
            {
                var upper = s.ToUpperInvariant();
                var member = typeof(Colors).GetTypeInfo().DeclaredProperties.FirstOrDefault(x => x.Name.ToUpperInvariant() == upper);
                if (member != null)
                {
                    return (ArgbColorModel)member.GetValue(null);
                }
                else
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }
            }
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }

        public string GetRGB()
        {
            string rStr = Convert.ToString(R, 16);
            string gStr = Convert.ToString(G, 16);
            string bStr = Convert.ToString(B, 16);
            if (rStr.Length == 1) rStr = "0" + rStr;
            if (gStr.Length == 1) gStr = "0" + gStr;
            if (bStr.Length == 1) bStr = "0" + bStr;
            return $"#{rStr}{gStr}{bStr}";
        }

        public string GetARGB()
        {
            string aStr = Convert.ToString(A, 16);
            string rStr = Convert.ToString(R, 16);
            string gStr = Convert.ToString(G, 16);
            string bStr = Convert.ToString(B, 16);
            if (aStr.Length == 1) aStr = "0" + aStr;
            if (rStr.Length == 1) rStr = "0" + rStr;
            if (gStr.Length == 1) gStr = "0" + gStr;
            if (bStr.Length == 1) bStr = "0" + bStr;
            return $"#{aStr}{rStr}{gStr}{bStr}";
        }

        public double GetOpacity()
        {
            return (double)A / 255;
        }

        public Brush ToBrush()
        {
            return new SolidColorBrush(ToColor());
        }
    }
}
