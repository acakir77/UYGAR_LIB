namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Kar��la�t�rma operat�r�
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
    /// Kar��la�t�rma operat�r�
    /// </summary>
    public enum CompareOperator : int
    {
        /// <summary>
        /// E�it
        /// </summary>
        Equal = 0,

        /// <summary>
        /// B�y�k
        /// </summary>
        Big = 1,

        /// <summary>
        /// K���k
        /// </summary>
        Little = 2,

        /// <summary>
        /// B�y�k e�it
        /// </summary>
        BigEqual = 4,

        /// <summary>
        /// K���k e�it
        /// </summary>
        LittleEqual = 8,

        /// <summary>
        /// ��inde bulunan
        /// </summary>
        Like = 16,

        /// <summary>
        /// �le ba�layan
        /// </summary>
        StartWith = 32,
        /// <summary>
        /// �le Biten
        /// </summary>
        EndWith = 64,

        /// <summary>
        /// E�it Olamayan
        /// </summary>
        NotEqual = 128,
        /// <summary>
        /// B�y�k K���k Harf Duyarl�
        /// </summary>
        BigSmallCharSensitive = 256,
        /// <summary>
        /// Tarih kar��la�t�rma i�in kullan�l�r..
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
