using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P033()
        {
            IEnumerable<int> numbers = Enumerable.Range(10, 90);
            BigRational result = 1;
            foreach (var item in numbers.SelectMany(n => numbers.Select(d => new { Numerator = n, Denominator = d })).Where(t => t.Numerator < t.Denominator))
            {
                string n = item.Numerator.ToString();
                string d = item.Denominator.ToString();
                if (d[1] == '0')
                {
                    continue;
                }
                BigRational proper = new BigRational(item.Numerator, item.Denominator);
                if (proper.IsZero)
                {
                    continue;
                }

                BigRational r00 = ReduceFractionIncorrectly(n, d, 0, 0);
                BigRational r01 = ReduceFractionIncorrectly(n, d, 0, 1);
                BigRational r10 = ReduceFractionIncorrectly(n, d, 1, 0);
                BigRational r11 = ReduceFractionIncorrectly(n, d, 1, 1);
                if (proper == r00)
                {
                    result *= r00;
                }
                if (proper == r01)
                {
                    result *= r01;
                }
                if (proper == r10)
                {
                    result *= r10;
                }
                if (proper == r11)
                {
                    result *= r11;
                }
            }
            Console.WriteLine($"P033: {result.Denominator}");
        }
        private static BigRational ReduceFractionIncorrectly(string num, string den, int numIndex, int denIndex)
        {
            if (num[numIndex] == den[denIndex])
            {
                return new BigRational((int)char.GetNumericValue(num[1 - numIndex]), (int)char.GetNumericValue(den[1 - denIndex]));
            }
            return BigRational.Indeterminate;
        }

        [Serializable]
        [ComVisible(false)]
        public struct BigRational : IComparable<BigRational>, IEquatable<BigRational>, IComparable, ISerializable, IDeserializationCallback
        {
            #region Members for internal support

            public const string SOLIDUS = "/";

            private const int DoubleMaxScale = 308;
            private static readonly BigInteger DoublePrecision = BigInteger.Pow(10, DoubleMaxScale);
            private static readonly BigInteger DoubleMinValue = (BigInteger)double.MinValue;
            private static readonly BigInteger DoubleMaxValue = (BigInteger)double.MaxValue;
            [StructLayout(LayoutKind.Explicit)]
            private struct DoubleUlong
            {
                [FieldOffset(0)]
                public double dbl;
                [FieldOffset(0)]
                public ulong uu;
            }

            private const int DecimalScaleMask = 0x00FF0000;
            private const int DecimalSignMask = unchecked((int)0x80000000);
            private const int DecimalMaxScale = 28;
            private static readonly BigInteger DecimalPrecision = BigInteger.Pow(10, DecimalMaxScale);
            private static readonly BigInteger DecimalMinValue = (BigInteger)decimal.MinValue;
            private static readonly BigInteger DecimalMaxValue = (BigInteger)decimal.MaxValue;
            [StructLayout(LayoutKind.Explicit)]
            private struct DecimalUInt32
            {
                [FieldOffset(0)]
                public decimal dec;
                [FieldOffset(0)]
                public int flags;
            }

            #endregion Members for internal support

            private BigInteger _numerator;
            private BigInteger _denominator;

            public BigRational(double value)
            {
                if (double.IsNaN(value))
                {
                    throw new ArgumentException("Specified value is not a number.", nameof(value));
                }
                if (double.IsInfinity(value))
                {
                    throw new ArgumentException("Specified value cannot be infinity.", nameof(value));
                }

                bool isFinite;
                int sign;
                int exponent;
                ulong significand;
                SplitDoubleIntoParts(value, out sign, out exponent, out significand, out isFinite);

                if (significand == 0)
                {
                    this = BigRational.Zero;
                    return;
                }

                this._numerator = significand;
                this._denominator = 1 << 52;

                if (exponent > 0)
                {
                    this._numerator = BigInteger.Pow(this._numerator, exponent);
                }
                else if (exponent < 0)
                {
                    this._denominator = BigInteger.Pow(this._denominator, -exponent);
                }
                if (sign < 0)
                {
                    this._numerator = BigInteger.Negate(this._numerator);
                }
                this.Simplify();

            }
            public BigRational(decimal value)
            {
                if (value == 0)
                {
                    this = BigRational.Zero;
                    return;
                }

                int[] bits = decimal.GetBits(value);
                if (bits == null || bits.Length != 4 || (bits[3] & ~(DecimalSignMask | DecimalScaleMask)) != 0 || (bits[3] & DecimalScaleMask) > (28 << 16))
                {
                    throw new ArgumentException("invalid Decimal", "value");
                }

                // build up the numerator
                ulong ul = (((ulong)(uint)bits[2]) << 32) | (uint)bits[1];            // (hi    << 32) | (mid)
                this._numerator = (new BigInteger(ul) << 32) | (uint)bits[0];         // (hiMid << 32) | (low)

                bool isNegative = (bits[3] & DecimalSignMask) != 0;
                if (isNegative)
                {
                    this._numerator = BigInteger.Negate(this._numerator);
                }

                // build up the denominator
                int scale = (bits[3] & DecimalScaleMask) >> 16;     // 0-28, power of 10 to divide numerator by
                this._denominator = BigInteger.Pow(10, scale);

                this.Simplify();
            }
            public BigRational(BigInteger value) : this(value, BigInteger.One)
            {
            }
            public BigRational(BigInteger numerator, BigInteger denominator)
            {
                if (denominator.IsZero)
                {
                    throw new DivideByZeroException();
                }

                this._numerator = numerator;
                this._denominator = denominator;
                this.Simplify();
            }
            public BigRational(BigInteger whole, BigInteger numerator, BigInteger denominator) : this(whole * denominator + numerator, denominator)
            {
            }
            private BigRational(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException(nameof(info));
                }
                this._numerator = (BigInteger)info.GetValue(nameof(Numerator), typeof(BigInteger));
                this._denominator = (BigInteger)info.GetValue(nameof(Denominator), typeof(BigInteger));
                this.Simplify();
            }

            #region Properties

            public static BigRational Indeterminate
            {
                get;
            } = new BigRational();
            public static BigRational Zero
            {
                get;
            } = new BigRational(0, 1);
            public static BigRational One
            {
                get;
            } = new BigRational(1, 1);
            public static BigRational MinusOne
            {
                get;
            } = new BigRational(-1, 1);

            public BigInteger Numerator
            {
                get
                {
                    return this._numerator;
                }
            }
            public BigInteger Denominator
            {
                get
                {
                    return this._denominator;
                }
            }
            public bool IsIndeterminate
            {
                get
                {
                    return this.Numerator.IsZero && this.Denominator.IsZero;
                }
            }
            public bool IsZero
            {
                get
                {
                    return !this.IsIndeterminate && this.Numerator.IsZero;
                }
            }
            public bool IsOne
            {
                get
                {
                    return this.Numerator.IsOne && this.Denominator.IsOne;
                }
            }
            public bool IsPositive
            {
                get
                {
                    return this.Sign > 0;
                }
            }
            public bool IsNegative
            {
                get
                {
                    return this.Sign < 0;
                }
            }
            public int Sign
            {
                get
                {
                    return this.Numerator.Sign;
                }
            }

            #endregion Properties

            #region Instance methods

            public BigInteger GetWholePart()
            {
                return this.Numerator / this.Denominator;
            }
            public BigRational GetFractionPart()
            {
                return new BigRational(this.Numerator % this.Denominator, this.Denominator);
            }

            public int CompareTo(BigRational other)
            {
                return BigRational.Compare(this, other);
            }
            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return 1;
                }
                else if (obj.GetType() != typeof(BigRational))
                {
                    throw new ArgumentException($"Specified value must be of type {typeof(BigRational).FullName}.", nameof(obj));
                }
                return BigRational.Compare(this, (BigRational)obj);
            }
            public bool Equals(BigRational other)
            {
                return this == other;
            }
            public override bool Equals(object obj)
            {
                if (obj == null || this.GetType() != obj.GetType())
                {
                    return false;
                }
                return this.Equals((BigRational)obj);
            }
            public override int GetHashCode()
            {
                return (this.Numerator / this.Denominator).GetHashCode();
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Numerator.ToString("R", CultureInfo.InvariantCulture));
                sb.Append(BigRational.SOLIDUS);
                sb.Append(this.Denominator.ToString("R", CultureInfo.InvariantCulture));
                return sb.ToString();
            }

            private void Simplify()
            {
                if (this.IsIndeterminate)
                {
                    return;
                }
                if (this.Numerator == BigInteger.Zero)
                {
                    this._denominator = BigInteger.One;
                }
                BigInteger gcd = BigInteger.GreatestCommonDivisor(this.Numerator, this.Denominator);
                if (gcd > BigInteger.One)
                {
                    this._numerator = this.Numerator / gcd;
                    this._denominator = this.Denominator / gcd;
                }
            }

            #endregion Instance methods

            #region Static methods

            public static BigRational Abs(BigRational value)
            {
                return value.Sign < 0 ? new BigRational(BigInteger.Abs(value.Numerator), value.Denominator) : value;
            }
            public static BigRational Add(BigRational left, BigRational right)
            {
                if (left.IsIndeterminate || right.IsIndeterminate)
                {
                    return BigRational.Indeterminate;
                }
                else if (left.IsZero)
                {
                    return right;
                }
                else if (right.IsZero)
                {
                    return left;
                }
                return new BigRational((left.Numerator * right.Denominator) + (right.Numerator * left.Denominator), (left.Denominator * right.Denominator));
            }
            public static BigRational Subtract(BigRational left, BigRational right)
            {
                return Add(left, BigRational.Negate(right));
            }
            public static BigRational Multiply(BigRational left, BigRational right)
            {
                if (left.IsIndeterminate || right.IsIndeterminate)
                {
                    return BigRational.Indeterminate;
                }
                else if (left.IsZero || right.IsZero)
                {
                    return BigRational.Zero;
                }
                else if (left.IsOne)
                {
                    return right;
                }
                else if (right.IsOne)
                {
                    return left;
                }
                return new BigRational(left.Numerator * right.Numerator, left.Denominator * right.Denominator);
            }
            public static BigRational Divide(BigRational left, BigRational right)
            {
                return BigRational.Multiply(left, BigRational.Invert(right));
            }
            public static BigRational Remainder(BigRational left, BigRational right)
            {
                return new BigRational((left.Numerator * right.Denominator) % (left.Denominator * right.Numerator), (left.Denominator * right.Denominator));
            }
            public static BigRational DivRem(BigRational left, BigRational right, out BigRational remainder)
            {
                // a/b / c/d == (ad)/(bc)
                // a/b % c/d == (ad % bc)/(bd)

                BigInteger ad = left.Numerator * right.Denominator;
                BigInteger bc = left.Denominator * right.Numerator;
                BigInteger bd = left.Denominator * right.Denominator;

                remainder = new BigRational(ad % bc, bd);
                return new BigRational(ad, bc);
            }
            public static BigRational Pow(BigRational value, BigInteger exponent)
            {
                if (exponent.IsZero)
                {
                    return One;
                }
                else if (exponent < 0)
                {
                    if (value.IsZero)
                    {
                        throw new ArgumentException("Cannot raise zero to a negative power.", "value");
                    }
                    value = BigRational.Invert(value);
                    exponent = BigInteger.Negate(exponent);
                }

                BigRational result = value;
                while (exponent > BigInteger.One)
                {
                    result = result * value;
                    exponent--;
                }
                return result;
            }
            public static BigRational Invert(BigRational value)
            {
                return new BigRational(value.Denominator, value.Numerator);
            }
            public static BigRational Negate(BigRational value)
            {
                if (value.IsIndeterminate || value.IsZero)
                {
                    return value;
                }
                return new BigRational(BigInteger.Negate(value.Numerator), value.Denominator);
            }
            public static int Compare(BigRational left, BigRational right)
            {
                if (left.IsIndeterminate != right.IsIndeterminate)
                {
                    return left.IsIndeterminate ? -1 : 1;
                }
                return BigInteger.Compare(left.Numerator * right.Denominator, right.Numerator * left.Denominator);
            }
            public static BigInteger LeastCommonDenominator(BigRational left, BigRational right)
            {
                return BigInteger.Abs(left.Denominator / BigInteger.GreatestCommonDivisor(left.Denominator, right.Denominator) * right.Denominator);
            }

            public static BigRational Parse(string value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                string[] parts = value.Split(new[] { BigRational.SOLIDUS }, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    throw new FormatException();
                }
                return new BigRational(BigInteger.Parse(parts[0]), BigInteger.Parse(parts[1]));
            }
            public static bool TryParse(string value, out BigRational result)
            {
                try
                {
                    result = BigRational.Parse(value);
                    return true;
                }
                catch (Exception)
                {
                    result = BigRational.Indeterminate;
                    return false;
                }
            }

            #endregion Static methods

            #region Operator overloads

            public static BigRational operator +(BigRational value)
            {
                return value;
            }
            public static BigRational operator -(BigRational value)
            {
                return BigRational.Negate(value);
            }
            public static BigRational operator ++(BigRational value)
            {
                return value + BigRational.One;
            }
            public static BigRational operator --(BigRational value)
            {
                return value + BigRational.MinusOne;
            }

            public static BigRational operator +(BigRational left, BigRational right)
            {
                return BigRational.Add(left, right);
            }
            public static BigRational operator -(BigRational left, BigRational right)
            {
                return BigRational.Subtract(left, right);
            }
            public static BigRational operator *(BigRational left, BigRational right)
            {
                return BigRational.Multiply(left, right);
            }
            public static BigRational operator /(BigRational left, BigRational right)
            {
                return BigRational.Divide(left, right);
            }
            public static BigRational operator %(BigRational left, BigRational right)
            {
                return BigRational.Remainder(left, right);
            }

            public static bool operator <(BigRational left, BigRational right)
            {
                return BigRational.Compare(left, right) < 0;
            }
            public static bool operator >(BigRational left, BigRational right)
            {
                return BigRational.Compare(left, right) > 0;
            }
            public static bool operator <=(BigRational left, BigRational right)
            {
                return BigRational.Compare(left, right) <= 0;
            }
            public static bool operator >=(BigRational left, BigRational right)
            {
                return BigRational.Compare(left, right) >= 0;
            }
            public static bool operator ==(BigRational left, BigRational right)
            {
                return BigRational.Compare(left, right) == 0;
            }
            public static bool operator !=(BigRational left, BigRational right)
            {
                return !(left == right);
            }

            #endregion Operator overloads

            #region Implicit conversions to BigRational

            public static implicit operator BigRational(sbyte value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(byte value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(short value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(ushort value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(int value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(uint value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(long value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(ulong value)
            {
                return new BigRational((BigInteger)value);
            }
            public static implicit operator BigRational(float value)
            {
                return new BigRational((double)value);
            }
            public static implicit operator BigRational(double value)
            {
                return new BigRational(value);
            }
            public static implicit operator BigRational(decimal value)
            {
                return new BigRational(value);
            }
            public static implicit operator BigRational(BigInteger value)
            {
                return new BigRational(value);
            }

            #endregion Implicit conversions to BigRational

            #region Explicit conversions from BigRational

            public static explicit operator sbyte (BigRational value)
            {
                return (sbyte)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator byte (BigRational value)
            {
                return (byte)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator short (BigRational value)
            {
                return (short)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator ushort (BigRational value)
            {
                return (ushort)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator int (BigRational value)
            {
                return (int)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator uint (BigRational value)
            {
                return (uint)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator long (BigRational value)
            {
                return (long)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator ulong (BigRational value)
            {
                return (ulong)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator BigInteger(BigRational value)
            {
                return (BigInteger)BigInteger.Divide(value.Numerator, value.Denominator);
            }
            public static explicit operator float (BigRational value)
            {
                return (float)(double)value;
            }
            public static explicit operator double (BigRational value)
            {
                if (SafeCastToDouble(value.Numerator) && SafeCastToDouble(value.Denominator))
                {
                    return (double)value.Numerator / (double)value.Denominator;
                }

                // scale the numerator to preseve the fraction part through the integer division
                BigInteger denormalized = (value.Numerator * BigRational.DoublePrecision) / value.Denominator;
                if (denormalized.IsZero)
                {
                    return (value.Sign < 0) ? BitConverter.Int64BitsToDouble(unchecked((long)0x8000000000000000)) : 0d; // underflow to -+0
                }

                double result = 0;
                bool isDouble = false;
                int scale = BigRational.DoubleMaxScale;

                while (scale > 0)
                {
                    if (!isDouble)
                    {
                        if (BigRational.SafeCastToDouble(denormalized))
                        {
                            result = (double)denormalized;
                            isDouble = true;
                        }
                        else
                        {
                            denormalized = denormalized / 10;
                        }
                    }
                    result = result / 10;
                    scale--;
                }

                if (!isDouble)
                {
                    return (value.Sign < 0) ? double.NegativeInfinity : double.PositiveInfinity;
                }
                else
                {
                    return result;
                }
            }
            public static explicit operator decimal (BigRational value)
            {// The Decimal value type represents decimal numbers ranging
             // from +79,228,162,514,264,337,593,543,950,335 to -79,228,162,514,264,337,593,543,950,335
             // the binary representation of a Decimal value is of the form, ((-2^96 to 2^96) / 10^(0 to 28))
                if (BigRational.SafeCastToDecimal(value.Numerator) && BigRational.SafeCastToDecimal(value.Denominator))
                {
                    return (decimal)value.Numerator / (decimal)value.Denominator;
                }

                // scale the numerator to preseve the fraction part through the integer division
                BigInteger denormalized = (value.Numerator * BigRational.DecimalPrecision) / value.Denominator;
                if (denormalized.IsZero)
                {
                    return decimal.Zero; // underflow - fraction is too small to fit in a decimal
                }
                for (int scale = BigRational.DecimalMaxScale; scale >= 0; scale--)
                {
                    if (!BigRational.SafeCastToDecimal(denormalized))
                    {
                        denormalized = denormalized / 10;
                    }
                    else
                    {
                        DecimalUInt32 dec = new DecimalUInt32();
                        dec.dec = (decimal)denormalized;
                        dec.flags = (dec.flags & ~BigRational.DecimalScaleMask) | (scale << 16);
                        return dec.dec;
                    }
                }
                throw new OverflowException("Value was either too large or too small for a Decimal.");

            }

            #endregion Explicit conversions from BigRational

            #region Static helper methods

            private static void SplitDoubleIntoParts(double dbl, out int sign, out int exp, out ulong man, out bool isFinite)
            {
                DoubleUlong du;
                du.uu = 0;
                du.dbl = dbl;

                sign = 1 - ((int)(du.uu >> 62) & 2);
                man = du.uu & 0x000FFFFFFFFFFFFF;
                exp = (int)(du.uu >> 52) & 0x7FF;
                if (exp == 0)
                {
                    // Denormalized number.
                    isFinite = true;
                    if (man != 0)
                        exp = -1074;
                }
                else if (exp == 0x7FF)
                {
                    // NaN or Infinite.
                    isFinite = false;
                    exp = int.MaxValue;
                }
                else
                {
                    isFinite = true;
                    man |= 0x0010000000000000; // mask in the implied leading 53rd significand bit
                    exp -= 1075;
                }
            }
            private static bool SafeCastToDouble(BigInteger value)
            {
                return BigRational.DoubleMinValue <= value && value <= BigRational.DoubleMaxValue;
            }

            private static bool SafeCastToDecimal(BigInteger value)
            {
                return BigRational.DecimalMinValue <= value && value <= BigRational.DecimalMaxValue;
            }
            private static double GetDoubleFromParts(int sign, int exp, ulong man)
            {
                DoubleUlong du;
                du.dbl = 0;

                if (man == 0)
                {
                    du.uu = 0;
                }
                else
                {
                    // Normalize so that 0x0010 0000 0000 0000 is the highest bit set
                    int cbitShift = CbitHighZero(man) - 11;
                    if (cbitShift < 0)
                        man >>= -cbitShift;
                    else
                        man <<= cbitShift;

                    // Move the point to just behind the leading 1: 0x001.0 0000 0000 0000
                    // (52 bits) and skew the exponent (by 0x3FF == 1023)
                    exp += 1075;

                    if (exp >= 0x7FF)
                    {
                        // Infinity
                        du.uu = 0x7FF0000000000000;
                    }
                    else if (exp <= 0)
                    {
                        // Denormalized
                        exp--;
                        if (exp < -52)
                        {
                            // Underflow to zero
                            du.uu = 0;
                        }
                        else
                        {
                            du.uu = man >> -exp;
                        }
                    }
                    else
                    {
                        // Mask off the implicit high bit
                        du.uu = (man & 0x000FFFFFFFFFFFFF) | ((ulong)exp << 52);
                    }
                }

                if (sign < 0)
                {
                    du.uu |= 0x8000000000000000;
                }

                return du.dbl;
            }

            private static int CbitHighZero(ulong uu)
            {
                if ((uu & 0xFFFFFFFF00000000) == 0)
                    return 32 + CbitHighZero((uint)uu);
                return CbitHighZero((uint)(uu >> 32));
            }

            private static int CbitHighZero(uint u)
            {
                if (u == 0)
                    return 32;

                int cbit = 0;
                if ((u & 0xFFFF0000) == 0)
                {
                    cbit += 16;
                    u <<= 16;
                }
                if ((u & 0xFF000000) == 0)
                {
                    cbit += 8;
                    u <<= 8;
                }
                if ((u & 0xF0000000) == 0)
                {
                    cbit += 4;
                    u <<= 4;
                }
                if ((u & 0xC0000000) == 0)
                {
                    cbit += 2;
                    u <<= 2;
                }
                if ((u & 0x80000000) == 0)
                    cbit += 1;
                return cbit;
            }


            #endregion Static helper methods

            #region Serialization methods

            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException(nameof(info));
                }
                info.AddValue(nameof(Numerator), this.Numerator);
                info.AddValue(nameof(Denominator), this.Denominator);
            }

            void IDeserializationCallback.OnDeserialization(object sender)
            {
                try
                {
                    this = new BigRational(this.Numerator, this.Denominator);
                }
                catch (ArgumentException e)
                {
                    throw new SerializationException("Invalid serialization data.", e);
                }
            }

            #endregion Serialization methods
        }
    }
}