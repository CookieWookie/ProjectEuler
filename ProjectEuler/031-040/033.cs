using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P033()
        {
        }

        private struct Fraction
        {
            private abstract class FractionBase
            {
                public static FractionBase Zero
                {
                    get;
                } = SimpleFraction.Create(0, 1);
                public static FractionBase One
                {
                    get;
                } = SimpleFraction.Create(1, 1);

                protected abstract double ToDouble();

                public static FractionBase operator *(FractionBase left, FractionBase right)
                {
                    return ComplexFraction.Create(
                }
                public static FractionBase operator /(FractionBase left, FractionBase right)
                {
                    return ComplexFraction.Create(left, right);
                }

                private sealed class SimpleFraction : FractionBase
                {
                    private SimpleFraction(long numerator, long denominator)
                    {
                        if (denominator < 0)
                        {
                            numerator = -numerator;
                            denominator = -denominator;
                        }
                        this.Numerator = numerator;
                        this.Denominator = denominator;
                    }

                    private long Numerator
                    {
                        get;
                    }
                    private long Denominator
                    {
                        get;
                    }

                    protected override double ToDouble()
                    {
                        return (double)this.Numerator / this.Denominator;
                    }

                    public static FractionBase Create(long numerator, long denominator)
                    {
                        if (denominator == 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(denominator));
                        }
                        return new SimpleFraction(numerator, denominator);
                    }
                }

                private sealed class ComplexFraction : FractionBase
                {
                    private ComplexFraction(FractionBase numerator, FractionBase denominator)
                    {
                        this.Numerator = numerator;
                        this.Denominator = denominator;
                    }

                    private FractionBase Numerator
                    {
                        get;
                    }
                    private FractionBase Denominator
                    {
                        get;
                    }

                    protected override double ToDouble()
                    {
                        return (this.Numerator / this.Denominator).ToDouble();
                    }

                    public static FractionBase Create(FractionBase numerator, FractionBase denominator)
                    {
                        if (numerator == null)
                        {
                            throw new ArgumentNullException(nameof(numerator));
                        }
                        if (denominator == null)
                        {
                            throw new ArgumentNullException(nameof(denominator));
                        }
                        return new ComplexFraction(numerator, denominator);
                    }
                }
            }
        }
    }
}