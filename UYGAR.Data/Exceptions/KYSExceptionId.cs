using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    public sealed class KYSExceptionId
    {
        private KYSExceptionId()
        { }
        public const String GENEL_SISTEM_HATASI = "Genel sistem hatas�!";
        public const String KAYIT_BULUNAMADI = "Kay�t bulunamad�!";
        public const String LOGIN_HATALI = "Kullan�c� kodu veya �ifre hatal�!";
        public const String EKSIK_BILGI = "Eksik bilgi!";
        public const String EKSIK_HATALI_BILGI = "Eksik veya hatal� bilgi.";
        public const String MUKERRER_KULLANICI_KODU = "MUKERRER_KULLANICI_KODU";
        public const String KAYIT_DEGISTIRILMIS = "Kay�t ba�ka bir kullan�c� taraf�ndan guncellenmi� L�tfen Kay�tlar� Tekrar Y�kleyiniz!!";
        public const String KAYIT_MUKERRER = "Bu Kay�t �le Ayn� �zellikleri Ta��yan Kay�t Zaten Sistemde Mevcut!!";
        public const String SESSION_EXPIRED = "L�tfen uygulamaya giri� yap�n�z.";
        public const String KAYIT_SILINEMEZ = "Kay�t, ba�ka bir kay�t taraf�ndan kullan�ld���ndan silinemez.!!";
        public const String MUKERRER_ROL_ADI = "Mukerer Rol Ad�";
        public const String NESNE_TANIMLI_UYARISI = "G�venlik Nesnesi tan�ml�. Yeniden tan�mlanamaz.";
        public const String KULLANILAN_KAYIT = " Kay�t kullan�lmaktad�r !";
        public const String MUKERRER_GUVENLIK_NESNESI = "M�kerrer G�venlik Nesnesi Ad�.";
        public const String HAREKET_GORMUS_KAYIT = "Hareket g�ren kay�t silinemez..";
        public const String HAREKET_MIKTAR_BUYUK_OLAMAZ = "Stoktaki miktardan b�y�k olamaz..";

    }
}
