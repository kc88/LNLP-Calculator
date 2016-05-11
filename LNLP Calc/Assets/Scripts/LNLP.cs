using System;
using System.Text.RegularExpressions;


[Serializable]
public class LNLP :IComparable<LNLP>
{ //playing loose and fast with the rules of mathematics //LongNumberLowPrecision, you may wish to refactor this
  //fyi does not support negative numbers - things will crash
    public int exponent;
    public double val;

    private const int precision = 10;
    private const int digitsDisplayed = 3;
    public LNLP(double v) : this(0, v)
    {
    }

    public LNLP(LNLP v) : this(v.exponent, v.val) //deep copy
    {
    }

    public LNLP(int e, double v)
    {
        //if (v < 0) throw new System.ArgumentException("LNLP value cannot be negative", "v");
        exponent = e;
        val = v;
        bool negative = v < 0;
        if (negative) val *= -1;
        while (val > 10)
        {
            val /= 10;
            exponent++;
        }
        while (val < 1)
        {
            val *= 10;
            exponent--;
        }
        //might have rounding errors for numbers that are very far off from the 1-10 scale before clamping
        //above scales value to 1.00 thru 9.99 and changes exponent to fit
        //val = Math.Round(value, precision-1); //round to precision-1 decimal places. docs https://msdn.microsoft.com/en-us/library/75ks3aby(v=vs.110).aspx

        val *= Math.Pow(10, precision - 1);
        val = Math.Round(val);
        val /= Math.Pow(10, precision - 1);

        if (negative) val *= -1;
    }

    public static LNLP Multiply(LNLP a, LNLP b)
    {
        return new LNLP(a.exponent + b.exponent, a.val * b.val); //constructor clamps value between 1 and 9.99 if product of values is higher
    }

    public static LNLP Divide(LNLP a, LNLP b)
    {
        //if (b.exponent - precision > a.exponent) return new LNLP(0, 0); //dividing a small number by a very large number is approximately zero, check value before continuing with arithmetic
        return new LNLP(a.exponent - b.exponent, a.val / b.val); //constructor will scale if the value is less than one
    }

    public static LNLP Add(LNLP a, LNLP b)
    {
        LNLP high = a.exponent > b.exponent ? a : b;
        LNLP low = a.exponent > b.exponent ? b : a;

        if (high.exponent - precision > low.exponent)
            return new LNLP(high.exponent, high.val); //adding a tiny number to a large number is approx the large number

        int magnitude_dif = high.exponent - low.exponent;
        return new LNLP(high.exponent, high.val + low.val / Math.Pow(10, magnitude_dif));
    }
    public static LNLP Subtract(LNLP a, LNLP b)
    {
        return Add(a, -1 * b);
    }
    public static LNLP Pow(LNLP a, double b)
    {
        double decExponent = b * a.exponent; //get the exponent, ill send you the math on this
        int expIpart = (int)Math.Floor(decExponent); //integral part of exponent
        double exponentFpart = decExponent - expIpart; //fractional part of exponent

        return new LNLP(expIpart, Math.Pow((double)a.val, b) * Math.Pow(10, exponentFpart));
    }

    public new string ToString()
    {
        return String.Format("{0:f} * 10 ^ {1:d}", val, exponent);
    }

    public static LNLP FromString(string s)
    {
        var reg = Regex.Match(s, @"(?<v>\A\d\.\d*) \* 10 \^ (?<e>\d*\z)");
        return new LNLP(Int32.Parse(reg.Groups["e"].Value), Double.Parse(reg.Groups["v"].Value));
    }

    public override bool Equals(Object o)
    {
        if (!(o is LNLP))
        {
            return false;
        }
        LNLP b = (LNLP)o;
        return val == b.val && exponent == b.exponent;
    }

    public static LNLP operator +(LNLP a, LNLP b)
    {
        return Add(a, b);
    }
    public static LNLP operator -(LNLP a, LNLP b)
    {
        return Subtract(a, b);
    }
    public static LNLP operator ++(LNLP a)
    {
        return Add(a, 1);
    }
    public static LNLP operator --(LNLP a)
    {
        return Subtract(a, 1);
    }
    public static LNLP operator *(LNLP a, LNLP b)
    {
        return Multiply(a, b);
    }
    public static LNLP operator /(LNLP a, LNLP b)
    {
        return Divide(a, b);
    }
    public static bool operator ==(LNLP a, LNLP b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(LNLP a, LNLP b)
    {
        return !a.Equals(b);
    }

    public int CompareTo(LNLP b)
    {
        int valCompare = val.CompareTo(b.val);
        int expCompare = exponent.CompareTo(b.exponent);
        if (expCompare != 0) return expCompare;
        return valCompare;
    }

    public static bool operator >(LNLP a, LNLP b)
    {
        return a.CompareTo(b) > 0;
    }
    public static bool operator <(LNLP a, LNLP b)
    {
        return a.CompareTo(b) < 0;
    }
    public static bool operator >=(LNLP a, LNLP b)
    {
        return a.CompareTo(b) >= 0;
    }
    public static bool operator <=(LNLP a, LNLP b)
    {
        return a.CompareTo(b) <= 0;
    }

    public override int GetHashCode()
    {
        return exponent.GetHashCode() * val.GetHashCode();
    }

    public static implicit operator LNLP(int v)
    {
        return new LNLP((double)v);
    }
    public static implicit operator LNLP(float v)
    {
        return new LNLP((double)v);
    }
    public static explicit operator int(LNLP a)
    {
        return (int)(a.val * (int)Math.Pow(10, a.exponent));
    }
    public static explicit operator float(LNLP a)
    {
        return (float)a.val * (float)Math.Pow(10, a.exponent);
    }
    public static string[] postfixLookup = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "UDc", "DDc", "TDc", "QaDc", "QiDc", "SxDc", "SpDc",
                                            "OcDc", "NoDc", "Vg", "UVg", "DVg", "TVg", "QaVg", "QiVg", "SxVg", "SpVg", "OcVg", "NoVg", "Tg"};

    public string ToDisplay()
    {
        if (exponent < 0)
        {
            return "[Small number]";
        }
        int lookupIndex = this.exponent / 3;
        int scaler = this.exponent % 3;
        int displayPrecision = exponent - 3 >= 0 ? digitsDisplayed : 0;
        if (this.exponent < postfixLookup.Length * 3)
        {
            return String.Format(String.Concat("{0:f", displayPrecision, "} {1}"), this.val * Math.Pow(10, scaler), postfixLookup[lookupIndex]);
        }
        else
        /*{
            int relExp = lookupIndex + 1 - postfixLookup.Length;
            string postfix = "";
            while (relExp > 0)
            {
                int charIndex = relExp == 26 ? 26 : relExp % 26;
                relExp -= charIndex;
                relExp /= 26;
                //charIndex += charIndex == 26 ? 0 : 1;
                postfix = Convert.ToString(charIndex) + " " + postfix;
            }
            return String.Format("{0:f2} {1}", this.val * Math.Pow(10, scaler), postfix);
        }*/
        {
            return String.Format("{0:f3}e{1:d}", val, exponent);
        }
    }

    public static LNLP Max(LNLP a, LNLP b)
    {
        LNLP high = a > b ? a : b;
        return new LNLP(high);
    }
    public static LNLP Max(LNLP[] lst)
    {
        LNLP high = lst[0];
        for (int max_lst_iter = 0; max_lst_iter < lst.Length; max_lst_iter++)
        {
            if (lst[max_lst_iter] > high) high = lst[max_lst_iter];
        }

        return new LNLP(high);
    }

    public static LNLP Min(LNLP a, LNLP b)
    {
        LNLP low = a < b ? a : b;
        return new LNLP(low);
    }

    public static LNLP Min(LNLP[] lst)
    {
        LNLP low = lst[0];
        for (int min_lst_iter = 0; min_lst_iter < lst.Length; min_lst_iter++)
        {
            if (lst[min_lst_iter] < low) low = lst[min_lst_iter];
        }

        return new LNLP(low);
    }
}