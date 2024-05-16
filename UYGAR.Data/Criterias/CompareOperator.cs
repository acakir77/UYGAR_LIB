namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Karþýlaþtýrma operatörü
    /// </summary>
    public enum FlagsOperator
    {
        /// <summary>
        /// ve
        /// </summary>
        And = 0,

        /// <summary>
        /// veya
        /// </summary>
        Or = 1,
    }
    /// <summary>
    /// Karþýlaþtýrma operatörü
    /// </summary>
    public enum CompareOperator : int
    {
        /// <summary>
        /// Eþit
        /// </summary>
        Equal = 0,

        /// <summary>
        /// Büyük
        /// </summary>
        Big = 1,

        /// <summary>
        /// Küçük
        /// </summary>
        Little = 2,

        /// <summary>
        /// Büyük eþit
        /// </summary>
        BigEqual = 4,

        /// <summary>
        /// Küçük eþit
        /// </summary>
        LittleEqual = 8,

        /// <summary>
        /// Ýçinde bulunan
        /// </summary>
        Like = 16,

        /// <summary>
        /// Ýle baþlayan
        /// </summary>
        StartWith = 32,
        /// <summary>
        /// Ýle Biten
        /// </summary>
        EndWith = 64,

        /// <summary>
        /// Eþit Olamayan
        /// </summary>
        NotEqual = 128,
        /// <summary>
        /// Büyük Küçük Harf Duyarlý
        /// </summary>
        BigSmallCharSensitive = 256,
        /// <summary>
        /// Tarih karþýlaþtýrma için kullanýlýr..
        /// </summary>
        DateEqual = 512,
        DateBigEqual = 1024,
        DateLittleEqual = 2048,
        DateBig = 4096,
        DateTimeEqual = 8192,
        DateTimeBigEqual = 16384,
        DateTimeLittleEqual = 32768,
        DateTimeBig = 65536,
        DateLittle = 130072

    }
}
