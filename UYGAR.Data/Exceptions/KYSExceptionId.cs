using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    public sealed class KYSExceptionId
    {
        private KYSExceptionId()
        { }
        public const String GENEL_SISTEM_HATASI = "Genel sistem hatasý!";
        public const String KAYIT_BULUNAMADI = "Kayýt bulunamadý!";
        public const String LOGIN_HATALI = "Kullanýcý kodu veya þifre hatalý!";
        public const String EKSIK_BILGI = "Eksik bilgi!";
        public const String EKSIK_HATALI_BILGI = "Eksik veya hatalý bilgi.";
        public const String MUKERRER_KULLANICI_KODU = "MUKERRER_KULLANICI_KODU";
        public const String KAYIT_DEGISTIRILMIS = "Kayýt baþka bir kullanýcý tarafýndan guncellenmiþ Lütfen Kayýtlarý Tekrar Yükleyiniz!!";
        public const String KAYIT_MUKERRER = "Bu Kayýt Ýle Ayný Özellikleri Taþýyan Kayýt Zaten Sistemde Mevcut!!";
        public const String SESSION_EXPIRED = "Lütfen uygulamaya giriþ yapýnýz.";
        public const String KAYIT_SILINEMEZ = "Kayýt, baþka bir kayýt tarafýndan kullanýldýðýndan silinemez.!!";
        public const String MUKERRER_ROL_ADI = "Mukerer Rol Adý";
        public const String NESNE_TANIMLI_UYARISI = "Güvenlik Nesnesi tanýmlý. Yeniden tanýmlanamaz.";
        public const String KULLANILAN_KAYIT = " Kayýt kullanýlmaktadýr !";
        public const String MUKERRER_GUVENLIK_NESNESI = "Mükerrer Güvenlik Nesnesi Adý.";
        public const String HAREKET_GORMUS_KAYIT = "Hareket gören kayýt silinemez..";
        public const String HAREKET_MIKTAR_BUYUK_OLAMAZ = "Stoktaki miktardan büyük olamaz..";

    }
}
